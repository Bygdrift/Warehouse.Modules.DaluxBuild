using System;

namespace Module.Services.Models
{
    public class ProjectApprovalRoot
    {
        public ProjectApproval[] ApprovalsList { get; set; }
        public string NextBookmark { get; set; }
    }

    public class ProjectApproval
    {
        public UserShort CreatedByUser { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public object[] ExtensionsDataList { get; set; }
        public string InspectionType { get; set; }
        public bool IsDeleted { get; set; }
        public Locationlist[] LocationList { get; set; }
        public Project Project { get; set; }
        public string RoleName { get; set; }
        public GenericId ApprovalID { get; set; }
        public string ApprovalNumber { get; set; }
        public Approvalrevisionlist[] ApprovalRevisionList { get; set; }
    }

    public class Approvalrevisionlist
    {
        public DateTime? AssignedDateTime { get; set; }
        public CompanyShort AssignedToCompany { get; set; }
        public UserShort AssignedToUser { get; set; }
        public object BuildingObjectInfo { get; set; }
        public ContractShort Contract { get; set; }
        public string Description { get; set; }
        public string Discipline { get; set; }
        public object DueDateTime { get; set; }
        public int LocalStatus { get; set; }
        public GenericId LocationID { get; set; }
        public int MainStatus { get; set; }
        public Medialist[] MediaList { get; set; }
        public UserShort ModifiedByUser { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public int RevisionNumber { get; set; }
        public string Title { get; set; }
        public object[] ViewPointImageList { get; set; }
    }
}
