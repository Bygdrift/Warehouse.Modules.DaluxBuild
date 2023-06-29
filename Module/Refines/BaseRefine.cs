using Bygdrift.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Refines
{
    public class BaseRefine
    {
        public readonly AppBase<Settings> App;
        public readonly bool SaveToDataLake;
        public readonly bool SaveToDatabase;

        public BaseRefine(AppBase<Settings> app, bool saveToDataLake, bool saveToDatabase)
        {
            App = app;
            SaveToDataLake = saveToDataLake;
            SaveToDatabase = saveToDatabase;
        }
    }
}
