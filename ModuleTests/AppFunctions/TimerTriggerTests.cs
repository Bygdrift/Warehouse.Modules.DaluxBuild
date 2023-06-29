using Bygdrift.Tools.DataLakeTool;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module.AppFunctions;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace ModuleTests.Refines
{
    /// To run this test, then first add an Azure environmed, as decribed here: https://github.com/Bygdrift/Warehouse
    /// Then fetch the Azure App config connections tring and paste it to this project's User Secret like: {"ConnectionStrings:AppConfig": "the connectionstring to app config"}.
    [TestClass]
    public class AppFunctionsTest:BaseTests
    {
        private readonly bool saveToDataLake = false;
        private readonly bool saveToDatabase = true;
        private readonly Mock<ILogger<TimerTrigger>> loggerMock = new();
        private readonly TimerTrigger function;

        public AppFunctionsTest() => function = new TimerTrigger(loggerMock.Object, saveToDataLake, saveToDatabase);

        [TestMethod]
        public async Task TimerTrigger()
        {
            if (saveToDataLake)  //Clear the data in the warehouse for this module:
                await function.App.DataLake.DeleteDirectoryAsync("", FolderStructure.Path);

            //Run the function:
            await function.TimerTriggerAsync(default);

            //There should come no errors
            var errors = function.App.Log.GetErrorsAndCriticals().ToList();
            Assert.AreEqual(0, errors.Count);

            if (saveToDataLake)  //Is data uploaded to datalake?:
            {
                Assert.IsTrue(function.App.DataLake.FileExist("Refined", "Projects.csv", FolderStructure.DatePath));
                Assert.IsTrue(function.App.DataLake.FileExist("Refined", "ProjectsUsers.csv", FolderStructure.DatePath));
            }

            if (saveToDatabase)  //Is data uploaded to database?:
            {
                var csvFromDb = function.App.Mssql.GetAsCsv("Projects");
                Assert.IsTrue(csvFromDb.RowCount > 1);
            }
        }
    }
}
