using Bygdrift.Tools.CsvTool;
using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Warehouse;
using Module.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Refines
{
    public class ProjectsUsersRefine : BaseRefine
    {
        public ProjectsUsersRefine(AppBase<Settings> app, bool saveToDataLake, bool saveToDatabase) : base(app, saveToDataLake, saveToDatabase)
        {
        }

        public async Task Refine(Dictionary<string, List<ProjectUser>> projectsUsers)
        {
            App.Log.LogInformation("Refining projectUsers...");
            var csv = CreateCsv(projectsUsers);

            if (SaveToDataLake)
                await App.DataLake.SaveCsvAsync(csv, "Refined", "ProjectsUsers.csv", FolderStructure.DatePath);

            if (SaveToDatabase)
                App.Mssql.InsertCsv(csv, "ProjectsUsers", true, false);
        }

        private Csv CreateCsv(Dictionary<string, List<ProjectUser>> projectsUsers)
        {
            App.CsvConfig.FormatKind = FormatKind.TimeOffsetDST;
            var csv = new Csv("ProjectID, UserID, Email, Firstname, Lastname, CompanyID, CompanyName, CompanyVAT");
            var r = 1;
            foreach (var item in projectsUsers)
                foreach (var user in item.Value)
                    csv.AddRecord(r, 1, item.Key)
                       .AddRecord(r, 2, user.UserID?.ID)
                       .AddRecord(r, 3, user.EmailAddress)
                       .AddRecord(r, 4, user.FirstName)
                       .AddRecord(r, 5, user.LastName)
                       .AddRecord(r, 6, user.Company?.CompanyID?.ID)
                       .AddRecord(r, 7, user.Company?.Name)
                       .AddRecord(r++, 8, user.Company?.VAT);

            return csv;
        }
    }
}