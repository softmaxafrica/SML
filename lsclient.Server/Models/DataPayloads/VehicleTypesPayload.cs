using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models.DataPayloads
{
    public class VehicleTypesPayload
    {
             public string? TruckTypeID { get; set; }

             public string? TypeName { get; set; }
         
            public string? Description { get; set; }
            public string? SampleImageUrl { get; set; }

    }
}
