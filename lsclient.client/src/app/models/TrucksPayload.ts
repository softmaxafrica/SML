export interface TrucksPayload {
    truckID: string;
    truckNumber: string;
    model: string;
    truckTypeID: string;
    companyID: string;
    isActive: boolean;
    isTruckAvilableForBooking: boolean;
    driverID: string;
    chasisNo?: string;
    brand?: string;
    engineCapacity?: string;
    fuelType?: string;
    cabinType?: string;
    category?: string;
    drive?: string;
  }
  