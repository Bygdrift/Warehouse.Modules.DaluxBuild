using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Warehouse;
using Module.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Module.Services
{
    public class WebService
    {
        private DateTime lastDownloadOfAccessToken;
        private HttpClient _client;
        private readonly string baseUrl = "https://field.dalux.com/service/APIv2/FieldRestService.svc/v2.2/";
        private readonly bool saveJsonToDataLake;

        public AppBase<Settings> App { get; }

        public WebService(AppBase<Settings> app, bool saveJsonToDataLake)
        {
            App = app;
            this.saveJsonToDataLake = saveJsonToDataLake;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            App.Log.LogInformation("Fething projects...");
            var response = await Client.GetAsync("projects");
            var content = await GetContent(response);
            if (content == null)
                return null;

            await SaveJsonToDataLake(content, "projects.json");
            return JsonConvert.DeserializeObject<List<Project>>(content);
        }

        public async Task<IEnumerable<ProjectApproval>> GetProjectsApprovalsAsync(List<Project> projects)
        {
            App.Log.LogInformation("Fething projectApprovals...");
            var projectIds = projects?.Select(o => o.ProjectID.ID)?.ToArray();
            if (!projectIds.Any())
                return default;

            var tasks = new List<Task<ProjectApprovalRoot>>();
            foreach (var projectId in projectIds)
                tasks.Add(GetProjectPackageAsync<ProjectApprovalRoot>($"projects/{projectId}/approvals", $"project {projectId}_approvals.json"));

            await Task.WhenAll(tasks);

            return tasks.SelectMany(o => o.Result.ApprovalsList);
        }

        public async Task<IEnumerable<ProjectChecklist>> GetProjectsCheckListsAsync(List<Project> projects)
        {
            App.Log.LogInformation("Fething projectChecklists...");
            var projectIds = projects?.Select(o => o.ProjectID.ID)?.ToArray();
            if (!projectIds.Any())
                return default;

            var tasks = new List<Task<ProjectChecklistRoot>>();
            foreach (var projectId in projectIds)
                tasks.Add(GetProjectPackageAsync<ProjectChecklistRoot>($"projects/{projectId}/checklists", $"project {projectId}_checklists.json"));

            await Task.WhenAll(tasks);

            return tasks.SelectMany(o => o.Result.Checklists);
        }

        public async Task<Dictionary<string, List<ProjectCompany>>> GetProjectsCompaniesAsync(List<Project> projects)
        {
            App.Log.LogInformation("Fething projectCompanies...");
            return await GetProjectsPackagesAsync<ProjectCompany>(projects, "companies");
        }

        public async Task<Dictionary<string, List<ProjectContract>>> GetProjectsContractsAsync(List<Project> projects)
        {
            App.Log.LogInformation("Fething projectContracts...");
            return await GetProjectsPackagesAsync<ProjectContract>(projects, "contracts");
        }

        public async Task<Dictionary<string, List<ProjectUser>>> GetProjectsUsersAsync(List<Project> projects)
        {
            App.Log.LogInformation("Fething projectUsers...");
            return await GetProjectsPackagesAsync<ProjectUser>(projects, "users");
        }

        private async Task<Dictionary<string, List<T>>> GetProjectsPackagesAsync<T>(List<Project> projects, string urlPostfix) where T : class
        {
            if (!projects.Any())
                return default;

            var tasks = new List<KeyValuePair<string, Task<List<T>>>>();
            foreach (var project in projects)
                tasks.Add(new KeyValuePair<string, Task<List<T>>>(project.ProjectID.ID, GetProjectPackagesAsync<T>($"projects/{project.ProjectID.ID}/{urlPostfix}", $"project {project.ProjectID.ID}_{urlPostfix}.json")));

            await Task.WhenAll(tasks.Select(o=> o.Value));
            return tasks.ToDictionary(o => o.Key, o => o.Value.Result);
        }

        private async Task<T> GetProjectPackageAsync<T>(string url, string dataLakeFileName) where T : class
        {
            var response = await Client.GetAsync(url);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                App.Log.LogError($"The webservice '{url}' failed while trying to fetch from url {url}. Error: {response.ReasonPhrase}.");
                return default;
            }
            var content = await GetContent(response);
            if (content == null)
                return null;

            await SaveJsonToDataLake(content, dataLakeFileName);
            return JsonConvert.DeserializeObject<T>(content);
        }

        private async Task<List<T>> GetProjectPackagesAsync<T>(string url, string dataLakeFileName) where T : class
        {
            var response = await Client.GetAsync(url);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                App.Log.LogError($"The webservice '{url}' failed while trying to fetch from url {url}. Error: {response.ReasonPhrase}.");
                return default;
            }
            var content = await GetContent(response);
            if (content == null)
                return null;

            await SaveJsonToDataLake(content, dataLakeFileName);
            return JsonConvert.DeserializeObject<List<T>>(content);
        }

        private HttpClient Client
        {
            get
            {
                if (_client == null || lastDownloadOfAccessToken.AddHours(1) < DateTime.Now)
                {
                    _client = GetHttpClient();
                    lastDownloadOfAccessToken = DateTime.Now;
                }

                return _client;
            }
        }

        /// <summary>
        /// Gets an access token that can be used for up to one hour - then it has to be revoked.
        /// </summary>
        private HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler();
            var client = new HttpClient(handler);
            client.Timeout = new TimeSpan(10, 0, 0);
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", "apikey " + App.Settings.ApiKey);
            return client;
        }

        private async Task<string> GetContent(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                App.Log.LogCritical("The webservice {Url} failed. Error: {ReasonPhrase}.", response.RequestMessage.RequestUri, response.ReasonPhrase);
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }

        private async Task SaveJsonToDataLake(string content, string dataLakeFileName)
        {
            if (saveJsonToDataLake)
                await App.DataLake.SaveStringAsync(content, "Raw", dataLakeFileName, FolderStructure.DatePath);
        }
    }
}