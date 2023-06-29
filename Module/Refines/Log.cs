using Bygdrift.Tools.CsvTool;
using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Warehouse;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Refines
{
    /// <summary>
    /// A log describing last run and if there were any errors
    /// </summary>
    public class Log : BaseRefine
    {
        public Log(AppBase<Settings> app, bool saveToDataLake, bool saveToDatabase) : base(app, saveToDataLake, saveToDatabase)
        {
        }

        public async Task WriteLog()
        {
            App.Log.LogInformation("Writing log...");
            App.CsvConfig.FormatKind = FormatKind.TimeOffsetDST;
            var errors = App.Log.GetErrorsAndCriticals();
            var errorsDetails = string.Empty;
            if (errors.Any())
                errorsDetails = string.Join(',', App.Log.GetErrorsAndCriticals());

            var csv = new Csv(App.CsvConfig, "TimeStamp, Errors, ErrorDetails")
                .AddRow(App.CsvConfig.DateHelper.Now(), errors.Count(), errorsDetails);

            if(SaveToDataLake)
                await App.DataLake.SaveCsvAsync(csv, "Refined", "Log.csv", FolderStructure.DatePath);

            if (SaveToDatabase)
                App.Mssql.MergeCsv(csv, "Log", "TimeStamp");
        }
    }
}
