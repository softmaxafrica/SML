 import { CompanyPayload } from "./CompanyPayload";
import { Customer } from "./Customer";
import { JobRequest } from "./JobRequest"; 

export interface Contract {
    contractID: string;
    requestID?: string;
    companyID?: string;
    customerID?: string;
    contractDate?: Date;
    termsAndConditions?: string;
    agreedPrice?: number;
    advancePayment?: number;
  advancePaymentDate?: Date;
  expirationDate?: Date;
    status?: string;
  paymentDue?: Date;
  lateFee?: Date;
    jobRequest?: JobRequest;
    company?: CompanyPayload;
    customer?: Customer;
  }
