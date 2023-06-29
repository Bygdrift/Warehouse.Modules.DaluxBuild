using System;

namespace Module.Services.Models
{
    public class GenericId
    {
        public string ID { get; set; }
    }

    public class Medialist
    {
        public DateTime? CreatedDateTime { get; set; }
        public object Description { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string ReferenceURL { get; set; }
    }

    public class Locationlist
    {

        public object Apartment { get; set; }
        public string BuildingName { get; set; }
        public Coordinatelist[] CoordinateList { get; set; }
        public int CoordinatesType { get; set; }
        public object DrawingDiscipline { get; set; }
        public string DrawingID { get; set; }
        public string DrawingName { get; set; }
        public object DrawingSubdiscipline { get; set; }
        public DateTime? DrawingUploadedDateTime { get; set; }
        public object Entrance { get; set; }
        public string Floor { get; set; }
        public GenericId LocationID { get; set; }
        public string LocationText { get; set; }
        public object ModelDiscipline { get; set; }
        public object ModelID { get; set; }
        public object ModelSubdiscipline { get; set; }
        public object ModelUploadedDateTime { get; set; }
        public object Room { get; set; }
        public string Zone { get; set; }
    }

    public class Coordinatelist
    {
        public Coordinates2d Coordinates2D { get; set; }
        public object Coordinates3D { get; set; }
        public object LatitudeLongitude { get; set; }
    }

    public class Coordinates2d
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
