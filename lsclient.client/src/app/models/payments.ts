import { Invoice } from "./invoices";

export interface Payment {
    paymentID: string;
    invoiceNumber: number;
    amountPaid: number;
    paymentDate: Date; //ISO Date string
    paymentMethod: string;
    referenceNumber: string | null;
    currency: string;
    invoice?: Invoice;
  }
  