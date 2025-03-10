using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models
{
    public class CustomerPayload
    {
        // All properties are now nullable
        public string? CustomerID { get; set; }

        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        // Payment-related fields (nullable)
        public string? PaymentMethod { get; set; } // e.g., Credit Card, Bank Transfer, Mobile Money, Cash

        public string? CardNumber { get; set; } // Nullable for Credit Card Payment
        public string? CardType { get; set; } // Nullable for Credit Card Payment (e.g., Visa, MasterCard)
        public string? BillingAddress { get; set; } // Nullable for Credit Card Payment
        public string? ExpiryDate { get; set; } // Nullable for Credit Card Payment

        // Bank transfer details (nullable)
        public string? BankName { get; set; } // Nullable for Bank Transfer
        public string? BankAccountNumber { get; set; } // Nullable for Bank Transfer
        public string? BankAccountHolder { get; set; } // Nullable for Bank Transfer

        // Fields for Mobile Money Transfer (optional and nullable)
        public string? MobileNetwork { get; set; } // Nullable (e.g., Tigo Pesa, Mpesa, TTCL)
        public string? MobileNumber { get; set; } // Nullable for Mobile Money Transfer

        public List<string> Companies { get; set; } = new List<string>(); // List of company IDs

    }

    public class CustomerRegistrationModel
    {
        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        // Payment-related fields (nullable)
        public string? PaymentMethod { get; set; } // e.g., Credit Card, Bank Transfer, Mobile Money, Cash

        public string? CardNumber { get; set; } // Nullable for Credit Card Payment
        public string? CardType { get; set; } // Nullable for Credit Card Payment (e.g., Visa, MasterCard)
        public string? BillingAddress { get; set; } // Nullable for Credit Card Payment
        public string? ExpiryDate { get; set; } // Nullable for Credit Card Payment

        // Bank transfer details (nullable)
        public string? BankName { get; set; } // Nullable for Bank Transfer
        public string? BankAccountNumber { get; set; } // Nullable for Bank Transfer
        public string? BankAccountHolder { get; set; } // Nullable for Bank Transfer

        // Fields for Mobile Money Transfer (optional and nullable)
        public string? MobileNetwork { get; set; } // Nullable (e.g., Tigo Pesa, Mpesa, TTCL)
        public string? MobileNumber { get; set; } // Nullable for Mobile Money Transfer

        public List<string> Companies { get; set; } = new List<string>(); // List of company IDs

        public IFormFile? ProfileImage { get; set; } // Profile image file

        [Required]
        public string Password { get; set; } // Password supplied by the customer

        [Required]
        public string ConfirmPassword { get; set; } // Confirm password supplied by the customer
    }


}
