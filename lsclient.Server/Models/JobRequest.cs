using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models
{
    public class JobRequest
    {
        [Key]
        [Column("JOB_REQUEST_ID")]
        public string JobRequestID { get; set; }
        [Column("ASSIGNED_COMPANY")]
        public string? AssignedCompany { get; set; }


        [Column("PICKUP_LOCATION")]
        public string? PickupLocation { get; set; }

         
        [Column("DELIVERY_LOCATION")]
        public string? DeliveryLocation { get; set; }

        [Column("CARGO_DESCRIPTION")]
        public string? CargoDescription { get; set; }

        [Column("CONTAINER_NUMBER")]
        public string? ContainerNumber { get; set; } // Optional

        [Column("REQUEST_TYPE")]
        public string? RequestType{ get; set; } //Truck, Driver, Both
         
        [Column("STATUS")]
        public string? Status { get; set; } // e.g., "Pending", "In Progress", "Completed"

        [Column("PRICE_AGREEMENT_ID")]
        public string? PriceAgreementID { get; set; }

        //[ForeignKey("PriceAgreementID")]
        [ForeignKey("JobRequestID, CompanyID")]
        public  RequestWithPayment PriceAgreement { get; set; }

        public List<RequestWithPayment>? Negotiations { get; set; }


        [Column("TRUCK_TYPE")]
        public string? TruckType { get; set; }
       
        
        [ForeignKey("TruckID")]
        public string? TruckID { get; set; }

        [Column("CREATED_DATE")]
        public DateTime? Cdate{ get; set; }


        [Column("LAST_UPDATE_TIME")]
        public DateTime? Udate { get; set; }
        [Column("FIRST_DEPOSIT_AMOUNT")]
        public double? FirstDepositAmount { get; set; }

        [Column("COMPANY_ADVANCE_AMOUNT_REQUIRED")]
        public double? CompanyAdvanceAmountRequred { get; set; }

        [Column("CONTRACT_ID")]
        public string? ContractId { get; set; }

        [Column("INVOICE_NUMBER")]
        public int? InvoiceNumber { get; set; }


        // Navigation properties
        public virtual Truck Truck { get; set; }

        [ForeignKey("DriverID")]
        public string? DriverID { get; set; }
        public virtual Driver Driver { get; set; }

        [ForeignKey("CustomerID")]
        public string? CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        [ForeignKey("InvoiceNumber")]

        public virtual  Invoice? InvoiceDetails { get; set; }
    }

}
