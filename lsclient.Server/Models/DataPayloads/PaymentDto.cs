using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models.DataPayloads
{
    public class PaymentDto
    {
     public string PaymentID { get; set; }
     public int InvoiceNumber { get; set; }
     public double AmountPaid { get; set; }
     public DateTime PaymentDate { get; set; }
     public string PaymentMethod { get; set; } // e.g., "Credit Card", "Bank Transfer"
     public string? ReferenceNumber { get; set; } // Optional
     public string? Currency { get; set; } // Optional
    }


}
