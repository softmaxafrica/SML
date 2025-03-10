using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models.DataPayloads
{
    public class DriverPayload
    {
             public string? DriverID { get; set; }
             public string? FullName { get; set; }
             public string? Email { get; set; }

             public string? Phone { get; set; }
             public string? LicenseNumber { get; set; }
             public string? Status { get; set; }
             public string? RegstrationComment { get; set; }

             public string? LicenseClasses { get; set; }
             public DateOnly? LicenseExpireDate { get; set; }
             public bool? isAvilableForBooking { get; set; }
             public string? ImageUrl { get; set; }
             public string? CompanyID { get; set; }
        

    }
}
