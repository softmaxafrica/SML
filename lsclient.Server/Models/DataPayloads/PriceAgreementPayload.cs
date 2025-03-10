using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models.DataPayloads
{
    public class PriceAgreementPayload
    {

        public string? PriceAgreementID { get; set; }
        public string? CompanyID { get; set; }
        public string? CustomerID { get; set; }
        public string? JobRequestID { get; set; }
        public double? CompanyPrice { get; set; }
        public double? CustomerPrice { get; set; }
        public double? AgreedPrice { get; set; }
    }

    public class RequestWithPaymentPayload
    {
        public string? JobRequestID { get; set; }
        public string? PickupLocation { get; set; }
        public string? DeliveryLocation { get; set; }
        public string? CargoDescription { get; set; }
        public string? ContainerNumber { get; set; }
        public string? Status { get; set; }
        public string? PriceAgreementID { get; set; }
        public string? TruckType { get; set; }
        public string? TruckID { get; set; }
        public string? DriverID { get; set; }
        public string? RequestType { get; set; } //Truck, Driver, Both
        public string? CustomerID { get; set; }
        public double? RequestedPrice { get; set; }
        public double? CustomerPrice { get; set; }
        public double? AcceptedPrice { get; set; }
        public string? CompanyID { get; set; }
        public string? ContractId { get; set; }
        public double? CompanyAdvanceAmountRequred { get; set; }
        public double? FirstDepositAmount { get; set; }

    }

}
