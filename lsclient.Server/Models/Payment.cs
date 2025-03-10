using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models
{
    public class Payment
    {
        [Key]
        [Column("PAYMENT_ID")]
        public string PaymentID { get; set; }

        [Required]
        [Column("INVOICE_NUMBER")]
        public int InvoiceNumber { get; set; }

        [Required]
        [Column("AMOUNT_PAID")]
        public double AmountPaid { get; set; }

        [Required]
        [Column("PAYMENT_DATE")]
        public DateTime PaymentDate { get; set; }

        [Column("PAYMENT_METHOD")]
        public string PaymentMethod { get; set; } // e.g., "Credit Card", "Bank Transfer"

        [Column("REFERENCE_NUMBER")]
        public string? ReferenceNumber { get; set; } // Optional

        [Column("CURRENCY")]
        public string? Currency { get; set; } // Optional

        // Navigation properties
        [ForeignKey("InvoiceNumber")]
        public virtual Invoice? Invoice { get; set; }
    }
}
