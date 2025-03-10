using lsclient.Server.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogiSync.Models
{
    public class Contract
    {
        [Key]
        [Column("CONTRACT_ID")]
        public string ContractID { get; set; } // Example: "CON12345678"


        [Column("REQUEST_ID")]
        public string? RequestID { get; set; } // Reference to the Job Request


        [Column("COMPANY_ID")]
        public string? CompanyID { get; set; } // Reference to the Company


        [Column("CUSTOMER_ID")]
        public string? CustomerID { get; set; } // Reference to the Customer


        [Column("CONTRACT_DATE")]
        public DateTime? ContractDate { get; set; } // Date when the contract is created


        [Column("TERMS_AND_CONDITIONS")]
        public string? TermsAndConditions { get; set; } // Terms and agreements in text format

        [Column("AGREED_PRICE")]
        public double? AgreedPrice { get; set; } // Total price agreed upon for the request

        [Column("ADVANCE_PAYMENT")]
        public double? AdvancePayment { get; set; } // Initial deposit made by the customer

        [Column("ADVANCE_PAYMENT_DATE")]
        public DateTime? AdvancePaymentDate { get; set; } // Date when the advance payment was made

        [Column("STATUS")]
        public string? Status { get; set; } // Contract status (e.g., PENDING, ACTIVE, COMPLETED, CANCELLED)

        // Navigation Properties
        public virtual JobRequest JobRequest { get; set; }
        public virtual Company Company { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
