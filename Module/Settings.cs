using Bygdrift.Warehouse.Attributes;

namespace Module
{
    /// <summary>
    /// The AppBase is initialized in 'Module.AppFunctions.TimerTrigger' with this class. Then the warehouse knows it should load all AppSettings to this class and you can reach them through appBase.Settings.
    /// </summary>
    public class Settings
    {
        [ConfigSecret(NotSet = NotSet.ShowLogError)]
        public string ApiKey { get; set; }
    }
}
