using Bygdrift.Tools.CsvTool;
using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Warehouse;
using Module.Services.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Refines
{
    public class ProjectsRefine:BaseRefine
    {
        public ProjectsRefine(AppBase<Settings> app, bool saveToDataLake, bool saveToDatabase) : base(app, saveToDataLake, saveToDatabase)
        {
        }

        public async Task Refine(List<Project> projects)
        {
            App.Log.LogInformation("Refining project...");
            var csv = CreateCsv(projects);

            if (SaveToDataLake)
                await App.DataLake.SaveCsvAsync(csv, "Refined", "Projects.csv", FolderStructure.DatePath);

            if (SaveToDatabase)
                App.Mssql.InsertCsv(csv, "Projects", true, false);
        }

        private Csv CreateCsv(List<Project> projects)
        {
            App.CsvConfig.FormatKind = FormatKind.TimeOffsetDST;
            var csv = new Csv(App.CsvConfig, "ProjectID, Address, BoxType, Created, Name, Number");
            int r = 1;
            foreach (var project in projects)
            {
                csv.AddRecord(r, 1, project.ProjectID.ID)
                   .AddRecord(r, 2, project.Address)
                   .AddRecord(r, 3, project.BoxType)
                   .AddRecord(r, 4, project.CreationDateTime)
                   .AddRecord(r, 5, project.Name)
                   .AddRecord(r, 6, project.Number);

                foreach (var metaData in project.ProjectMetaData)
                    csv.AddRecord(r, metaData.Name, metaData.Value);

                r++;
            }
            return csv;
        }
    }
}