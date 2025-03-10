using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models.DataPayloads
{
    public class CompanyPayload
    {
        public string? CompanyID { get; set; }

        public string? CompanyTinNumber { get; set; }
             public string? CompanyName { get; set; }
             public string? AdminFullName { get; set; }

             public string? AdminEmail { get; set; }
             public string? AdminContact { get; set; }
             public string? CompanyAddress { get; set; }
             public string? CompanyDescription { get; set; }
             public double? CompanyLatitude { get; set; }
             public double? CompanyLongitude { get; set; }
          

    }
}
