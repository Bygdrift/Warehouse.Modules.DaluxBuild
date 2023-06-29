using Bygdrift.Tools.CsvTool;
using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Warehouse;
using Module.Services.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Refines
{
    public class ProjectsCompaniesRefine : BaseRefine
    {
        public ProjectsCompaniesRefine(AppBase<Settings> app, bool saveToDataLake, bool saveToDatabase) : base(app, saveToDataLake, saveToDatabase)
        {
        }

        public async Task Refine(Dictionary<string, List<ProjectCompany>> projectsCompanies)
        {
            App.Log.LogInformation("Refining projectsCompanies...");
            var csv = CreateCsv(projectsCompanies);

            if (SaveToDataLake)
                await App.DataLake.SaveCsvAsync(csv, "Refined", "ProjectsCompanies.csv", FolderStructure.DatePath);

            if (SaveToDatabase)
                App.Mssql.InsertCsv(csv, "ProjectsCompanies", true, false);
        }

        private Csv CreateCsv(Dictionary<string, List<ProjectCompany>> projectsCompanies)
        {
            App.CsvConfig.FormatKind = FormatKind.TimeOffsetDST;
            var csv = new Csv("ProjectID, CompanyID, Address, CountryCode, Name, VAT");
            int r = 1;
            foreach (var item in projectsCompanies)
                foreach (var company in item.Value)
                    csv.AddRecord(r, 1, item.Key)
                       .AddRecord(r, 2, company.CompanyID?.ID)
                       .AddRecord(r, 3, company.Address)
                       .AddRecord(r, 4, company.CountryCode)
                       .AddRecord(r, 5, company.Name)
                       .AddRecord(r++, 6, company.VAT);

            return csv;
        }
    }
}