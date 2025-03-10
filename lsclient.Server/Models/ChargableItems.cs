using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
 
namespace lsclient.Server.Models
{
    public class ChargableItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ITEM_ID")]
        public int ItemId { get; set; }

        [Column("JOB_REQUEST_ID")]
        public string?  JobRequestID { get; set; }
         
        [Column("PRICE_AGREEMENT_ID")]
        public string?  PriceAgreementID { get; set; }
         
        [Column("STATUS")]
        public string? Status { get; set; }

        [Column("INVOICE_NUMBER")]
        public int? InvoiceNumber { get; set; }

         [Column("CUSTOMER_ID")]
        public string? CustomerID { get; set; }

        [Column("ITEM_DESCRRIPTION")]
        public string? ItemDescription { get; set; }

        [Column("AMOUNT")]
        public double? Amount { get; set; }

        [Column("ISSUE_DATE")]
        public DateTime? IssueDate { get; set; }

        // Navigation properties

        [ForeignKey("JobRequestID")]
        public virtual JobRequest? JobRequest { get; set; }
 


    }

}
