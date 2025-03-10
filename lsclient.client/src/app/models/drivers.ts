// export interface DriverPayload {
//   driverID: string;
//   fullName: string;
//   email: string;
//   phone: string;
//   licenseNumber: string;
//   status: string | "ACTIVE";
//   registrationComment: string;
//   companyID: string;
//   licenseClasses: string;                
//   licenseExpireDate: Date;               
//   isAvilableForBooking: boolean;       
//   imageUrl?: string;                     
// }
export interface DriverPayload {
  driverID: string;
  fullName: string;
  email: string;
  phone: string;
  licenseNumber: string;
  status: string | "ACTIVE";
  registrationComment: string;
  companyID: string;
  licenseClasses: string;                
  licenseExpireDate: Date;               
  isAvilableForBooking: boolean;       
  imageUrl?: string;     
  truckTypes?: TruckType[];  // ✅ Optional, avoids errors during registration
}

// Define the TruckType interface
export interface TruckType {
  truckTypeID: string;
  typeName: string;
  description: string;
  sampleImageUrl: string;
}
