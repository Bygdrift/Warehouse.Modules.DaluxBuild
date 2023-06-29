using Bygdrift.Tools.CsvTool;
using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Warehouse;
using Module.Services.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Refines
{
    public class ProjectsApprovalsRefine : BaseRefine
    {
        public ProjectsApprovalsRefine(AppBase<Settings> app, bool saveToDataLake, bool saveToDatabase) : base(app, saveToDataLake, saveToDatabase)
        {
        }

        public async Task Refine(List<ProjectApproval> projectsApprovals)
        {
            App.Log.LogInformation("Refining projectApprovals...");
            var csv = CreateCsv(projectsApprovals);

            if (SaveToDataLake)
                await App.DataLake.SaveCsvAsync(csv, "Refined", "ProjectsApprovals.csv", FolderStructure.DatePath);

            if (SaveToDatabase)
                App.Mssql.InsertCsv(csv, "ProjectsApprovals", true, false);
        }

        private Csv CreateCsv(List<ProjectApproval> projectsApprovals)
        {
            App.CsvConfig.FormatKind = FormatKind.TimeOffsetDST;
            var csv = new Csv("ApprovalID, Number, CreatedByUserMail, CreatedByUserID, CreatedByCompanyName, CreatedByCompanyId, Created," +
                "InspectionType, IsDeleted, ProjectID, ProjectName, ProjectNumber, RoleName, RevisionList, LocationList");

            int r = 1;
            foreach (var project in projectsApprovals)
                csv.AddRecord(r, 1, project.ApprovalID?.ID)
                   .AddRecord(r, 2, project.ApprovalNumber)
                   .AddRecord(r, 3, project.CreatedByUser.EmailAddress)
                   .AddRecord(r, 4, project.CreatedByUser.UserID?.ID)
                   .AddRecord(r, 5, project.CreatedByUser.Company?.Name)
                   .AddRecord(r, 6, project.CreatedByUser.Company?.CompanyID?.ID)
                   .AddRecord(r, 7, project.CreatedDateTime)
                   .AddRecord(r, 8, project.InspectionType)
                   .AddRecord(r, 9, project.IsDeleted)
                   .AddRecord(r, 10,project.Project.ProjectID?.ID)
                   .AddRecord(r, 11,project.Project.Name)
                   .AddRecord(r, 12,project.Project.Number)
                   .AddRecord(r, 13,project.RoleName)
                   .AddRecord(r, 14, JsonConvert.SerializeObject(project.ApprovalRevisionList))
                   .AddRecord(r++, 15, JsonConvert.SerializeObject(project.LocationList));

            return csv;
        }
    }
}