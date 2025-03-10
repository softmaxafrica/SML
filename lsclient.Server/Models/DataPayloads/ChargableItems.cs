 
namespace lsclient.Server.Models
{
    public class ChargableItemPayload
    {
        public int? ItemId { get; set; }
        public string? JobRequestID { get; set; }
        public string? PriceAgreementID { get; set; }
        public string? Status { get; set; }
        public int? InvoiceNumber { get; set; }
        public string? CustomerID { get; set; }
        public string ItemDescription { get; set; }
        public double? Amount { get; set; }
        public DateTime? IssueDate { get; set; }
     }
}
