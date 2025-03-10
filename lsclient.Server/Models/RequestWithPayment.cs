using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace lsclient.Server.Models
{
    public class RequestWithPayment
    {
        [Key]
        [Column("PRICE_AGREEMENT_ID")]
        public string PriceAgreementID { get; set; }
        
        [Column("COMPANY_ID")]
        public string? CompanyID { get; set; }
        
        [Column("REQUEST_ID")]
        public string? JobRequestID { get; set; }

        [Column("CUSTOMER_ID")]
        public string? CustomerID { get; set; }

        [Column("COMPANY_PRICE")]
        public double? CompanyPrice { get; set; }
        [Column("CUSTOMER_PRICE")]
        public double? CustomerPrice { get; set; }

        [Column("AGREED_PRICE")]
        public double? AgreedPrice { get; set; }
    }

}
