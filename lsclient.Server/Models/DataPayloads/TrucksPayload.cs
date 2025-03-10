using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models.DataPayloads
{
    public class TrucksPayload
    {
            public string? TruckID { get; set; }
            public string? TruckNumber { get; set; }
            public string? Model { get; set; }
            public string? CompanyID { get; set; }
            public string? DriverID { get; set; }

            public string? TruckTypeID { get; set; }
            public bool? IsActive { get; set; }
           public bool? IsTruckAvilableForBooking { get; set; }

           public string? chasisNo { get; set; }
           public string? Brand { get; set; }
           public string? EngineCapacity { get; set; }
           public string? FuelType { get; set; }
           public string? CabinType { get; set; }
           public string? Category { get; set; }
           public string? Drive { get; set; }



    }
}
