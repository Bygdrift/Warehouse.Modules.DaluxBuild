using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ModuleTests.ManipulateData
{
    [TestClass]
    public class LoadFromFiles : BaseTests
    {
        [TestMethod]
        public async Task TestRunModule()
        {
            var saveToDataLake = false;
            var saveToDb = true;
            var projects = GetDataFromTestFile<Project>();

            await new Module.Refines.ProjectsRefine(App, saveToDataLake, saveToDb).Refine(projects);

            var projectsContracts = GetDataFromTestFileAsProjectDictionary<ProjectContract>();
            await new Module.Refines.ProjectsContractsRefine(App, saveToDataLake, saveToDb).Refine(projectsContracts);

            var projectsChecklists = GetDataFromTestFile<ProjectChecklist>();
            await new Module.Refines.ProjectsChecklistsRefine(App, saveToDataLake, saveToDb).Refine(projectsChecklists);

            var projectsUsers = GetDataFromTestFileAsProjectDictionary<ProjectUser>();
            await new Module.Refines.ProjectsUsersRefine(App, saveToDataLake, saveToDb).Refine(projectsUsers);

            var projectsApprovals = GetDataFromTestFile<ProjectApproval>();
            await new Module.Refines.ProjectsApprovalsRefine(App, saveToDataLake, saveToDb).Refine(projectsApprovals);

            var projectsCompanies = GetDataFromTestFileAsProjectDictionary<ProjectCompany>();
            await new Module.Refines.ProjectsCompaniesRefine(App, saveToDataLake, saveToDb).Refine(projectsCompanies);

            await new Module.Refines.Log(App, saveToDataLake, saveToDb).WriteLog();

            Assert.IsFalse(App.Log.HasErrorsOrCriticals());
        }

        private Dictionary<string, List<T>> GetDataFromTestFileAsProjectDictionary<T>()
        {
            var fileName = typeof(T).Name + ".json";
            var path = Path.Combine(BasePath, "Files", "In", fileName);
            if (!File.Exists(path))
                throw new Exception("File is missing");

            var json = File.ReadAllText(path, Encoding.UTF8);
            return JsonConvert.DeserializeObject<Dictionary<string, List<T>>>(json);
        }

        private List<T> GetDataFromTestFile<T>()
        {
            var fileName = typeof(T).Name + ".json";
            var path = Path.Combine(BasePath, "Files", "In", fileName);
            if (!File.Exists(path))
                throw new Exception("File is missing");

            var json = File.ReadAllText(path, Encoding.UTF8);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}