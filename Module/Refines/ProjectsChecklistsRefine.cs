using Bygdrift.Tools.CsvTool;
using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Warehouse;
using Module.Services.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Refines
{
    public class ProjectsChecklistsRefine : BaseRefine
    {
        public ProjectsChecklistsRefine(AppBase<Settings> app, bool saveToDataLake, bool saveToDatabase) : base(app, saveToDataLake, saveToDatabase)
        {
        }

        public async Task Refine(List<ProjectChecklist> projectsApprovals)
        {
            App.Log.LogInformation("Refining projectChecklists...");
            var csv = CreateCsv(projectsApprovals);

            if (SaveToDataLake)
                await App.DataLake.SaveCsvAsync(csv, "Refined", "ProjectChecklists.csv", FolderStructure.DatePath);

            if (SaveToDatabase)
                App.Mssql.InsertCsv(csv, "ProjectChecklists", true, false);
        }

        private Csv CreateCsv(List<ProjectChecklist> projectsChecklists)
        {
            App.CsvConfig.FormatKind = FormatKind.TimeOffsetDST;
            var csv = new Csv("ChecklistId, Closed, CreatedByUserMail, CreatedByUserID, CreatedByCompanyName, CreatedByCompanyID, CreatedByDateTime," +
                "IsDeleted, LastModifiedByUserMail, LastModifiedByUserID, LastModifiedByCompanyName, LastModifiedByCompanyID, LastModifiedDateTime, ProjectID, ProjectName, ProjectNumber, Safety, BuildingObjectInfo, LocationList, ExtensionsDataList, ViewPointImageList");

            int r = 1;
            foreach (var checklist in projectsChecklists)
                csv.AddRecord(r, 1, checklist.ChecklistID?.ID)
                   .AddRecord(r, 2, checklist.Closed)
                   .AddRecord(r, 3, checklist.CreatedByUser.EmailAddress)
                   .AddRecord(r, 4, checklist.CreatedByUser.UserID?.ID)
                   .AddRecord(r, 5, checklist.CreatedByUser.Company?.Name)
                   .AddRecord(r, 6, checklist.CreatedByUser.Company?.CompanyID?.ID)
                   .AddRecord(r, 7, checklist.CreatedDateTime)
                   .AddRecord(r, 8, checklist.IsDeleted)
                   .AddRecord(r, 9, checklist.LastModifiedByUser.EmailAddress)
                   .AddRecord(r, 10, checklist.LastModifiedByUser.UserID?.ID)
                   .AddRecord(r, 11, checklist.LastModifiedByUser.Company?.Name)
                   .AddRecord(r, 12, checklist.LastModifiedByUser.Company?.CompanyID?.ID)
                   .AddRecord(r, 13, checklist.LastModifiedDateTime)
                   .AddRecord(r, 14, checklist.Project.ProjectID?.ID)
                   .AddRecord(r, 15, checklist.Project.Name)
                   .AddRecord(r, 16, checklist.Project.Number)
                   .AddRecord(r, 17, checklist.Safety)
                   .AddRecord(r, 18, JsonConvert.SerializeObject(checklist.BuildingObjectInfo))
                   .AddRecord(r, 19, JsonConvert.SerializeObject(checklist.LocationList))
                   .AddRecord(r, 20, JsonConvert.SerializeObject(checklist.ExtensionsDataList))
                   .AddRecord(r++, 21, JsonConvert.SerializeObject(checklist.ViewPointImageList));   

            return csv;
        }
    }
}