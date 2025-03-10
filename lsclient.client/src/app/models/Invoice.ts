import { Customer } from "./Customer";

export interface Invoice {
    invoiceNumber: number;
    companyID: string;
    customerID: string;
    jobRequestID: string;
    paymentId: string | null;
    totalAmount: number;
    totalPaidAmount: number;
    owedAmount: number;
    serviceCharge: number;
    operationalCharge: number;
    issueDate: string; // ISO Date string
    dueDate: string | null; // ISO Date string
    status: string;
    customerDetails: Customer;
  }
  