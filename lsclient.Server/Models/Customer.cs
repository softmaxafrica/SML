using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lsclient.Server.Models
{
    public class Customer
    {
        [Key]
        [Column("CUSTOMER_ID")]
        [Required]
        public string CustomerID { get; set; }

        [Required]
        [Column("FULL_NAME")]
        public string? FullName { get; set; }

        [EmailAddress]
        [Column("EMAIL")]
        public string? Email { get; set; }

       
        [Column("PHONE")]
        public string? Phone { get; set; }

        [Column("ADDRESS")]
        public string? Address { get; set; }

        // Payment-related fields (nullable)
        [Column("PAYMENT_METHOD")]
        public string? PaymentMethod { get; set; } // e.g., Credit Card, Bank Transfer, Mobile Money, Cash

        [Column("CARD_NUMBER")]
        public string? CardNumber { get; set; }  

        [Column("CARD_TYPE")]
        public string? CardType { get; set; } // Nullable for Credit Card Payment (e.g., Visa, MasterCard)

        [Column("BILLING_ADDRESS")]
        public string? BillingAddress { get; set; } // Nullable for Credit Card Payment

        [Column("EXPIRY_DATE")]
        public string? ExpiryDate { get; set; } // Nullable for Credit Card Payment

        // Bank transfer details (nullable)
        [Column("BANK_NAME")]
        public string? BankName { get; set; } // Nullable for Bank Transfer

        [Column("BANK_ACCOUNT_NUMBER")]
        public string? BankAccountNumber { get; set; } // Nullable for Bank Transfer

        [Column("BANK_ACCOUNT_HOLDER")]
        public string? BankAccountHolder { get; set; } // Nullable for Bank Transfer

        // Fields for Mobile Money Transfer (optional and nullable)
        [Column("MOBILE_NETWORK")]
        public string? MobileNetwork { get; set; } // Nullable (e.g., Tigo Pesa, Mpesa, TTCL)

        [Column("MOBILE_NUMBER")]
        public string? MobileNumber { get; set; } // Nullable for Mobile Money Transfer

        [Column("PROFILE_IMAGE_PATH")]
        public string? ProfileImagePath { get; set; } // Add this property

        // Navigation properties (not required to be nullable, but can be set to null if not relevant)
        //public virtual List<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}
