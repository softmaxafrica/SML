using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models.DataPayloads
{
    public class JobRequestPayload
    {
         
            public string? JobRequestID { get; set; }
            public string? PickupLocation { get; set; }
            public string? DeliveryLocation { get; set; }
            public string? CargoDescription { get; set; }
            public string? ContainerNumber { get; set; } 

            public string? Status { get; set; }  
            public string? PriceAgreementID { get; set; }
         
            public string? TruckID { get; set; }
            public string? CustomerID { get; set; }


         
    }
}
