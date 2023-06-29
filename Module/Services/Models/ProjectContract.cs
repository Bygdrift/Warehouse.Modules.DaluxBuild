using Newtonsoft.Json;

namespace Module.Services.Models
{
    public class ProjectContract
    {
        public string Code { get; set; }
        public GenericId CompanyID { get; set; }
        public GenericId ContractID { get; set; }
        public GenericId ProjectID { get; set; }
        public string Name { get; set; }
    }

    public class ContractShort
    {
        public string Code { get; set; }
        public GenericId ContractID { get; set; }
        public string Name { get; set; }
    }

}
