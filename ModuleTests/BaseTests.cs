using Bygdrift.Tools.CsvTool;
using Bygdrift.Warehouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module;
using System;
using System.Diagnostics;
using System.IO;

namespace ModuleTests
{
    public class BaseTests
    {
        public readonly string BasePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
        public readonly Config CsvConfig;
        public readonly AppBase<Settings> App = new();

        public BaseTests()
        {
            CsvConfig = new Config(App.CultureInfo, App.TimeZoneInfo, FormatKind.TimeOffsetDST);

            if (App.Mssql.ConnectionString.Contains("localdb", StringComparison.OrdinalIgnoreCase))  //Then it's local
                Assert.IsNull(App.Mssql.DeleteAllTables());
        }

        public static string MethodName
        {
            get
            {
                var methodInfo = new StackTrace().GetFrame(1).GetMethod();
                return methodInfo.Name;
            }
        }
    }
}
