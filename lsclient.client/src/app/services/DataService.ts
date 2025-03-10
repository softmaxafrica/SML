import { Injectable } from '@angular/core';
 import { ApiResponse } from '../models/api-response.model';
import { SecUser } from '../models/SecUser';
import { LoginPayload } from '../models/LoginPayload';
import { DriverPayload } from '../models/drivers';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';
 import { AppConstants } from '../shared/Constants';
import { ApprovalPayload } from '../models/ApprovalPayload';
import { TrucksPayload } from '../models/TrucksPayload';
import { TruckType } from '../models/TruckTypes';
import { JobRequest } from '../models/JobRequest';
import { JobRequestPayload, RequestWithPrice } from '../models/jobRequestPayload';
import { Customer } from '../models/Customer';
import { PriceAgreement } from '../models/PriceAgreement';
import { Truck } from '../models/Truck';
import { Invoice } from '../models/invoices';
import { Contract } from '../models/contract';
import { Payment } from '../models/payments';


@Injectable({
  providedIn: 'root'
})
export class DataService {
     

  getInvoicePayments(InvoiceNumber: number): Observable<ApiResponse<Payment[]>> {
    return this.http.get<ApiResponse<Payment[]>>(
      `${this.baseUrl}Payment/GetPaymentByInvoiceNumber/${InvoiceNumber}`
    );
  }
   
  getDriverById(DriverId: string): Observable<ApiResponse<DriverPayload>> {
    return this.http.get<ApiResponse<DriverPayload>>(
      `${this.baseUrl}Driver/GetDriverById/${DriverId}`);
  }

  getTruckById(contractID: string): Observable<ApiResponse<Truck>> {
    return this.http.get<ApiResponse<Truck>>(
      `${this.baseUrl}Truck/GetTruckById/${contractID}`);
  }


  DeleteJobRequest(reqID: string) {
    return this.http.delete(`${this.baseUrl}JobRequest/DeleteJobRequest/${reqID}`);
  }   

 
  getPriceDataById(priceAgreementID: string): Observable<ApiResponse<PriceAgreement>> {
    return this.http.get<ApiResponse<PriceAgreement>>(
      `${this.baseUrl}PriceAgreement/GetPriceAgreementById/${priceAgreementID}`);
  }

  deleteTruck(truckID: string) {
      return this.http.delete(`${this.baseUrl}Truck/DeleteTruckByTruckId/${truckID}`);
       }   

 
  deletePayment<T>(paymentID: string): Observable<T>{
    return this.http.delete<T>(`${this.baseUrl}Payment/DeletePayment/${paymentID}`);
     }
 
  getCompanyPayments(companyId: string): Observable<ApiResponse<Payment[]>> {
    return this.http.get<ApiResponse<Payment[]>>(
      `${this.baseUrl}Payment/GetCompanyPayments/${companyId}`
    );
  }
  addPayment<T>(newPayment: Payment): Observable<T>{
    return this.http.post<T>(`${this.baseUrl}Payment/AddPayment`,newPayment);
     }

  
  
  getContractById(contractID: string): Observable<ApiResponse<Contract>> {
    return this.http.get<ApiResponse<Contract>>(
      `${this.baseUrl}Contracts/GetContractById/${contractID}`);
  }
  getCompanyInvoices(companyId: string): Observable<ApiResponse<Invoice[]>> {
    return this.http.get<ApiResponse<Invoice[]>>(
      `${this.baseUrl}Invoice/GetCompanyInvoice/${companyId}`
    );
  }

  getCompanyInvoiceDetails(companyId: string,InvoiceNumber: number): Observable<ApiResponse<Invoice>> {
    return this.http.get<ApiResponse<Invoice>>(
      `${this.baseUrl}Invoice/getCompanyInvoiceDetails/${companyId}/${InvoiceNumber}`
    );
  }

  postCustomer<T>(endpoint: string, customerRegData: Customer): Observable<T>{
 return this.http.post<T>(`${this.baseUrl}Customer/${endpoint}`,customerRegData);
  }

  registerCustomer<T>(endpoint: string, customerRegData: FormData): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}Customer/${endpoint}`, customerRegData);
  }
    //private baseUrl = 'http://softmaxafrica-001-site2.otempurl.com/'; 
    private baseUrl = AppConstants.API_BASE_URL; 

    constructor(private http: HttpClient) {}

    
  postDriverFormData(endpoint: string, formData: FormData): Observable<any> {
    return this.http.post(`${this.baseUrl}Driver/${endpoint}`, formData, {
  
    });
  }
  
  fetchRequestById(jobId: string | null): Observable<ApiResponse<JobRequest[]>> {
    return this.http.get<ApiResponse<JobRequest[]>>(
      `${this.baseUrl}JobRequest/GetJobRequestById/${jobId}`
    );
  }

   
  getAvailableTrucks(type: string, companyId: string): Observable<ApiResponse<Truck[]>> {
    return this.http.get<ApiResponse<Truck[]>>(
      `${this.baseUrl}Truck/GetAvailableTrucksByTruckType/${type}/${companyId}`
    );
  }
 
  
  
  assignTruckTypesToDriver(driverId: string, truckTypes: string[]): Observable<DriverPayload> {
    return this.http.post<DriverPayload>(`${this.baseUrl}Driver/assignTruckTypesToDrive/${driverId}`, truckTypes);
  }
  

  getCustomers(): Observable<ApiResponse<Customer[]>> {
    return this.http.get<ApiResponse<Customer[]>>(
      `${this.baseUrl}Customer/GetAllCustomers/`
    );
  }
  GetCustomersByCompany(companyId: string): Observable<ApiResponse<Customer[]>> {
    return this.http.get<ApiResponse<Customer[]>>(
      `${this.baseUrl}Customer/GetCustomersByCompany/${companyId}`
    );
  }
  
  updateJobRequest( requestUpdates: RequestWithPrice  ) {
  return this.http.put<void>(`${this.baseUrl}JobRequest/UpdateJobRequest/${requestUpdates.jobRequestID}`, requestUpdates);
}
  createJobRequest<T>(endpoint: string, payload: JobRequestPayload): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}JobRequest/${endpoint}`, payload);
  }
  //private baseUrl = 'http://softmaxafrica-001-site2.otempurl.com/'; 
     getCompanyJobs(companyId: string): Observable<ApiResponse<JobRequest[]>> {
      return this.http.get<ApiResponse<JobRequest[]>>(
        `${this.baseUrl}JobRequest/GetCompanyJobRequest/${companyId}`
      );
    }
    getCustomerJobs(customerID: string): Observable<ApiResponse<JobRequest[]>> {
      return this.http.get<ApiResponse<JobRequest[]>>(
        `${this.baseUrl}JobRequest/GetCustomerJobRequest/${customerID}`
      );
    }
  
    
 
  
  GetTruckTypes(): Observable<ApiResponse<TruckType[]>> {
    return this.http.get<ApiResponse<TruckType[]>>(
      `${this.baseUrl}Company/GetAllTruckTypes`
    );
  }
  
   



  post<T>(endpoint: string, payload: T): Observable<T> {
    return this.http.post<T>(`${AppConstants.API_BASE_URL}Company/${endpoint}`, payload);
  }

 
  postDriver<T>(endpoint: string, payload: DriverPayload): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}Driver/${endpoint}`, payload);
  }
 
  postTruck<T>(endpoint: string, payload: TrucksPayload): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}Truck/${endpoint}`, payload);
  }
 

  login(endpoint: string,payload: LoginPayload): Observable<ApiResponse<SecUser>> {
    return this.http.post<ApiResponse<SecUser>>(`${this.baseUrl}SecUser/${endpoint}`, payload);
  }
  
  getCompanyDrivers(companyId: string): Observable<ApiResponse<DriverPayload[]>> {
    return this.http.get<ApiResponse<DriverPayload[]>>(
      `${this.baseUrl}Driver/GetCompanyDrivers/${companyId}`
    );
  }
  getAvilableCompanyDrivers(companyId: string): Observable<ApiResponse<DriverPayload[]>> {
    return this.http.get<ApiResponse<DriverPayload[]>>(
      `${this.baseUrl}Driver/GetAvilableCompanyDrivers/${companyId}`
    );
  }
  
  GetTruckByCompanyId(companyId: string): Observable<ApiResponse<TrucksPayload[]>> {
    return this.http.get<ApiResponse<TrucksPayload[]>>(`${this.baseUrl}Truck/GetTruckByCompanyId/${companyId}`);
  }
  //#region driver Vetting
  
  getDriversAwaitingApproval(companyId: string): Observable<ApiResponse<DriverPayload[]>> {
    return this.http.get<ApiResponse<DriverPayload[]>>(
      `${this.baseUrl}Company/GetDriversAwaitingApprovalByCompanyId/${companyId}`
    );
  }
 
  approveDriver(approvalPayload: ApprovalPayload): Observable<ApiResponse<DriverPayload>> {
    return this.http.post<ApiResponse<DriverPayload>>(
      `${this.baseUrl}Company/ApproveDriver`,
      approvalPayload
    );
  }
//  #endregion
updateDriver(driver: DriverPayload): Observable<void> {
  return this.http.put<void>(`${this.baseUrl}Driver/UpdateDriver/${driver.driverID}`, driver);
}

updateTruckDetails(updatedTruck: TrucksPayload) {
  return this.http.put<void>(`${this.baseUrl}Truck/UpdateTruckByTruckId/${updatedTruck.truckID}`, updatedTruck);
}

deleteDriver(driverId: string): Observable<void> {
  return this.http.delete<void>(`${this.baseUrl}Driver/DeleteDriver/${driverId}`);
}

}
