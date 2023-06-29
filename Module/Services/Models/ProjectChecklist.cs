using System;
using System.Collections.Generic;

namespace Module.Services.Models
{
    public class ProjectChecklistRoot
    {
        public ProjectChecklist[] Checklists { get; set; }
        public string NextBookmark { get; set; }
    }

    public class ProjectChecklist
    {
        public Buildingobjectinfo BuildingObjectInfo { get; set; }
        public GenericId ChecklistID { get; set; }
        public string ChecklistName { get; set; }
        public string ChecklistNumber { get; set; }
        public bool Closed { get; set; }
        public UserShort CreatedByUser { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public Extensionsdatalist[] ExtensionsDataList { get; set; }
        public bool IsDeleted { get; set; }
        public UserShort LastModifiedByUser { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public Locationlist[] LocationList { get; set; }
        public ProjectShort Project { get; set; }
        public bool Safety { get; set; }
        public Viewpointimagelist[] ViewPointImageList { get; set; }
    }

    public class Buildingobjectinfo
    {
        public string ClassificationID { get; set; }
        public string ClassificationName { get; set; }
        public string ComponentTypeID { get; set; }
        public string ComponentTypeName { get; set; }
        public string ModelID { get; set; }
        public DateTime ModelUploadedDateTime { get; set; }
        public string ObjectID { get; set; }
        public string ObjectName { get; set; }
    }

    public class Extensionsdatalist
    {
        public GenericId[] AttachedIssues { get; set; }
        public string Comment { get; set; }
        public Medialist[] MediaList { get; set; }
        public string Name { get; set; }
        public Repeatablefield[] RepeatableFields { get; set; }
        public string Value { get; set; }
    }

    public class Repeatablefield
    {
        public object[] AttachedIssues { get; set; }
        public string Comment { get; set; }
        public object[] MediaList { get; set; }
        public string Name { get; set; }
        public object RepeatableFields { get; set; }
        public string Value { get; set; }
    }

    public class Viewpointimagelist
    {
        public DateTime? CreatedDateTime { get; set; }
        public string Description { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string ReferenceURL { get; set; }
    }
}
