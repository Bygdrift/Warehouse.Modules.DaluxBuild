using Bygdrift.Tools.CsvTool;
using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Warehouse;
using Module.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Refines
{
    public class ProjectsContractsRefine : BaseRefine
    {
        public ProjectsContractsRefine(AppBase<Settings> app, bool saveToDataLake, bool saveToDatabase) : base(app, saveToDataLake, saveToDatabase)
        {
        }

        public async Task Refine(Dictionary<string, List<ProjectContract>> projectsContracts)
        {
            App.Log.LogInformation("Refining projectUsers...");
            var csv = CreateCsv(projectsContracts);

            if (SaveToDataLake)
                await App.DataLake.SaveCsvAsync(csv, "Refined", "ProjectsContracts.csv", FolderStructure.DatePath);

            if (SaveToDatabase)
                App.Mssql.InsertCsv(csv, "ProjectsContracts", true, false);
        }

        private Csv CreateCsv(Dictionary<string, List<ProjectContract>> projectsContracts)
        {
            App.CsvConfig.FormatKind = FormatKind.TimeOffsetDST;
            var csv = new Csv("ProjectID, ContractID, CompanyID, Name, Code");
            int r = 1;
            foreach (var item in projectsContracts)
                foreach (var contract in item.Value)
                    csv.AddRecord(r, 1, item.Key)
                       .AddRecord(r, 2, contract.ContractID?.ID)
                       .AddRecord(r, 3, contract.CompanyID?.ID)
                       .AddRecord(r, 4, contract.Name)
                       .AddRecord(r++, 5, contract.Code);

            return csv;
        }
    }
}