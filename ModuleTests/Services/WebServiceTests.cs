using Bygdrift.Warehouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module.Services;
using Module.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ModuleTests.Refines
{
    [TestClass]
    public class WebServiceTests
    {
        public static readonly string BasePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
        private readonly WebService service;

        public WebServiceTests() => service = new WebService(new AppBase<Module.Settings>(), false);

        [TestMethod]
        public async Task GetProjects()
        {
            var projects = await service.GetProjectsAsync();
            Assert.IsFalse(service.App.Log.HasErrorsOrCriticals());
            SaveToFile(projects, typeof(Project).Name);
        }

        [TestMethod]
        public async Task GetWebServiceContent()
        {
            var projects = await service.GetProjectsAsync();
            Assert.IsFalse(service.App.Log.HasErrorsOrCriticals());
            SaveToFile(projects, typeof(Project).Name);

            var projectsChecklists = await service.GetProjectsCheckListsAsync(projects);
            Assert.IsFalse(service.App.Log.HasErrorsOrCriticals());
            SaveToFile(projectsChecklists, typeof(ProjectChecklist).Name);

            var projectUsers = await service.GetProjectsUsersAsync(projects);
            Assert.IsFalse(service.App.Log.HasErrorsOrCriticals());
            SaveToFile(projectUsers, typeof(ProjectUser).Name);

            var projectsApprovals = await service.GetProjectsApprovalsAsync(projects);
            Assert.IsFalse(service.App.Log.HasErrorsOrCriticals());
            SaveToFile(projectsApprovals, typeof(ProjectApproval).Name);
            
            var projectsCompanies = await service.GetProjectsCompaniesAsync(projects);
            Assert.IsFalse(service.App.Log.HasErrorsOrCriticals());
            SaveToFile(projectsCompanies, typeof(ProjectCompany).Name);

            var projectsContracts = await service.GetProjectsContractsAsync(projects);
            Assert.IsFalse(service.App.Log.HasErrorsOrCriticals());
            SaveToFile(projectsContracts, typeof(ProjectContract).Name);
        }

        private static void SaveToFile<T>(IEnumerable<T> data, string fileName)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            fileName = fileName + ".json";
            var filePath = Path.Combine(BasePath, "Files", "In", fileName);
            File.WriteAllText(filePath, json);
        }
    }
}
