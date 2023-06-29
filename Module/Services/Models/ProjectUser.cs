namespace Module.Services.Models
{
    public class ProjectUser
    {
        public GenericId UserID { get; set; }
        public CompanyShort Company { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UserShort
    {
        public CompanyShort Company { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenericId UserID { get; set; }
    }
}
