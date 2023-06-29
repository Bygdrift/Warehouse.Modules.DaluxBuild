namespace Module.Services.Models
{
    public class ProjectCompany
    {
        public string Address { get; set; }
        public GenericId CompanyID { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public string VAT { get; set; }
    }

    public class CompanyShort
    {
        public GenericId CompanyID { get; set; }
        public string Name { get; set; }
        public string VAT { get; set; }
    }
}
