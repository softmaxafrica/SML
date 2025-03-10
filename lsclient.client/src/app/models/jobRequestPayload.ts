import { TrucksPayload } from "./TrucksPayload";

export interface JobRequestPayload {
  jobRequestID: string;            
  pickupLocation: string;         
  deliveryLocation: string;       
  cargoDescription: string;      
  containerNumber: string; 
  status: string;                 
  truckID: string;                
  customerID: string;
  contractId:string;

}

export interface RequestWithPrice {
invoiceNumber?: number;
  jobRequestID: string;            
  pickupLocation: string;         
  deliveryLocation: string;       
  cargoDescription: string;      
  containerNumber: string; 
  status: string;                 
  priceAgreementID: string;
  truckType: string;                        
  truckID: string;  
  driverID: string;                              
  requestType: string;                
  customerID: string;
  contractId:string;
  firstDepositAmount?: number;
  companyAdvanceAmountRequred?:  number;
  // Properties that may need default values for binding
  requestedPrice?: number;  // Use `?` to make it optional initially
  acceptedPrice?: number;
  customerPrice?: number;
  companyID : string;

}
