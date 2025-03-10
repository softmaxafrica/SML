export interface Customer {
    customerID: string;
    fullName: string;
    email: string;
    phone: string;
    address?: string;
    companies: string[];
    cardNumber?: string;
    cardType?: string; // e.g., Visa, MasterCard
    billingAddress?: string;
    paymentMethod?: string; // e.g., Credit Card, Bank Transfer, Tigo Pesa, Mpesa, TTCL
    expiryDate?: string;
    
    bankName?: string;
    bankAccountNumber?: string;
    bankAccountHolder?: string;

    // Fields for Mobile Money Transfer (optional, shown if paymentMethod is Mobile Transfer)
    mobileNetwork?:  string;
    mobileNumber?: string; // Add paymentInfo as an optional field in Customer
    
 }
 