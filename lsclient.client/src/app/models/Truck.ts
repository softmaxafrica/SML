import { CompanyPayload } from "./CompanyPayload";
 import { TruckType } from "./TruckTypes";
import { DriverPayload } from "./drivers";
import { JobRequest } from "./JobRequest";

export interface Truck {
    truckID: string;
    truckNumber: string;
    model: string;
    companyID: string;
    truckTypeID: string;
    company: CompanyPayload;
    driver:DriverPayload | null;
    truckType: TruckType;
    isActive:boolean;
    isTruckAvilableForBooking: boolean;
    jobRequests: JobRequest[] | null;  
    locations: any[] | null;  
  }
  