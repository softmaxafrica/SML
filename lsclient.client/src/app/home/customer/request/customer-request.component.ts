import { Component, OnInit } from '@angular/core';
import { JobRequest } from '../../../models/JobRequest';
import { PRIME_COMPONENTS } from '../../../shared-modules';
import { TableColumn } from '../../../models/components/table-columns';
import { GlobalColumnControlService } from '../../../services/GlobalColumnControlService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PriceAgreement } from '../../../models/PriceAgreement'; 
import { TruckType } from '../../../models/TruckType';
import { RequestWithPrice } from '../../../models/RequestWithPrice';
import { FunctionsService } from '../../../services/functions';
import { MessageService } from 'primeng/api'; // import the service
import { Customer } from '../../../models/Customer';
import { AppConstants } from '../../../shared/Constants';
import { Contract } from '../../../models/contract';
 import { DriverPayload } from '../../../models/drivers';
import { Truck } from '../../../models/Truck';
import { AuthService } from '../../../auth/auth.service';
import { LoadingService } from '../../../services/loading.service';
import { DataService } from '../../../services/DataService';
import { jsPDF } from 'jspdf';
import html2canvas from 'html2canvas'; // Optional, for better rendering

@Component({
  selector: 'app-request',
  imports: [PRIME_COMPONENTS,],
  templateUrl: './customer-request.component.html',
  styleUrls: ['./customer-request.component.css']
})


export class CustomerRequestComponent implements OnInit {
  searchQuery: string = "";
 
  
drivers: any;
  showTruckContent: boolean = false;
    contractDetails: Contract | null= null;
  TruckDetails: Truck|null= null;
  DriverDetails: any;
  avilableDriverLists: DriverPayload[]=[];

  avilableTruckLists: Truck[] = [];
 


  RequestToDelete: any;




  deleteDialogVisible: boolean = false;

  activeCustomerPrice: any;
  activeCustomerAdvanceAmount: any;
  activeCompanyAdvanceAmount: any;
  activeFinalRequestPrice: any;
  activeReqPrice: any;

  isFormChanged: boolean = false;

  

   SelectedTruckavilableTruck: any;
   truckCompanyDriverFullName: any;
  truckCompanyName: any;
  truckCompanyAdminName: any;
  truckCompanyAdminContact: any;
  truckCompanyDriverContact: any;
   requestType: any = [];
   jobDialogVisible: boolean = false;
   isOnProcessRequest: boolean= false;
   IsshowRequestDetaisl: boolean= false;
   showRequestDetails: any;
  selectedJobRequest: JobRequest | null = null;
   RequestOnProcess: JobRequest | null= null;

   public baseUrl = AppConstants.API_SERVER_URL;
 
 
  onTruckTypeChange() {
    const selectedTruckType = this.truckTypes.find(truck => truck.truckTypeID === this.selectedTruckTypeID);
   this.selectedTrucTypeImage  = selectedTruckType!.sampleImageUrl;
    if (selectedTruckType) {
     this.selectedTruckDescription = selectedTruckType.description;
   } else {
     this.selectedTruckDescription = null;
   }
 
    // this.showTruckContent=true;
 }
 responsiveOptions = [
  {
    breakpoint: '1024px',
    numVisible: 3,
    numScroll: 3
  },
  {
    breakpoint: '768px',
    numVisible: 2,
    numScroll: 2
  },
  {
    breakpoint: '560px',
    numVisible: 1,
    numScroll: 1
  }
];
  saveJob(job: RequestWithPrice) {
    this.loadingService.show();
    this.newJobRequest = { ...job };
    this.newJobRequest.customerID = this.customerID;
    this.newJobRequest.requestedPrice = this.newJobRequest.requestedPrice || 0.0;
    this.newJobRequest.acceptedPrice = this.newJobRequest.acceptedPrice || 0.0;
    this.newJobRequest.customerPrice = this.newJobRequest.customerPrice || 0.0;

    if (this.newJobRequest) {
      if (this.newJobRequest.jobRequestID) {
        // Update existing job

      } else {
        // Create new 
        //request type
        this.newJobRequest.jobRequestID = 'JOBREQID';
        this.newJobRequest.status = 'CREATED';
        this.newJobRequest.requestedPrice = 0.00;
        this.newJobRequest.customerPrice = 0.00;
        this.newJobRequest.truckType = this.selectedTruckTypeID;
        // this.newJobRequest.truckID=this.SelectedTruckavilableTruck;
        this.newJobRequest.truckID = this.SelectedTruckavilableTruck;
        this.dataService.createJobRequest('CreateJobRequest', this.newJobRequest).subscribe(() => {
          this.loadJobs(this.customerID); // Refresh job list
          this.jobDialogVisible = false;

          this.loadingService.hide();

          this.functions.displayInsertSuccess('request_created');
        }, (error: any): void => {
          this.loadingService.hide();
          const errorMessage = error.error || error.message || 'An unknown error occurred';
          this.functions.displayError(errorMessage);
        });
      }
    }
    this.hideDialog();
  }
  updateRequest() {
    this.loadingService.show();
    this.activeRequest.customerID = this.customerID;
  
    this.dataService.updateJobRequest(this.activeRequest!).subscribe(
      (response: any) => {
        this.loadingService.hide();
        this.jobDialogVisible = false;
        this.ngOnInit();

        // Show success message
        const successMessage = response.message || 'Job request updated successfully';
        this.functions.displaySuccess(successMessage);
  
        // Refresh job list
      },
      (error: any) => {
        this.loadingService.hide();
        const errorMessage = error.error || error.message || 'An unknown error occurred';
        this.functions.displayError(errorMessage);
      }
    );
  }
  

  closeJobDialog() {
    this.activeRequest = this.emptyReq;
    this.requestDetailsVisible = false;
    this.isFormChanged = false;
    this.ngOnInit();
 
  }


 
 
  newJobRequest: RequestWithPrice = {
    jobRequestID: '',            
    pickupLocation: '',         
    deliveryLocation: '',       
    cargoDescription: '',      
    containerNumber: '', 
    status: '',                 
    priceAgreementID: '',        
    truckID: '',                
    customerID: '', 
    requestedPrice: 0,      
    acceptedPrice: 0,       
    customerPrice: 0,
    truckType: '',                        
    driverID: '',                              
    requestType: '',
    companyID:'',
    contractId:'',
    invoiceNumber:undefined     
  };


 
 

  addNewReq() {
    this.showNewReqDialog = true;
  }

  // Close the dialog
  hideDialog() {
    // this.ngOnInit();
    this.showNewReqDialog = false;
  }

  // Toggle maximize state
  toggleMaximize() {
    this.maximized = !this.maximized;
  }
 
 


searchTerm: string = '';
customerID!: string;
customersList: Customer[] = [];
request: JobRequest|undefined;
rows: number|undefined;
first: number|null|undefined;


  sourceColumns: TableColumn[] = []; // Available columns
  targetColumns: TableColumn[] = []; // Selected columns
  jobRequests: JobRequest[] = [];
    columnDialogVisible: boolean= false;
  showNewReqDialog: boolean= false;
  maximized: boolean= false;
  requestForm: FormGroup;

  truckTypes: TruckType[] = [];
  selectedTruckTypeID: any;
  selectedTruckDescription: any;
 selectedTrucTypeImage: string='';
   requestDetailsVisible: boolean =false;
   inspectRequestVisible: boolean= false;
   ActiveCustomerName: string | undefined;
   isLoading: boolean= false;
   showDriverContent: boolean= false;
    showPriceContent: boolean= true;
    showPriceLabels: boolean=false;
 
   filteredJobs: JobRequest[]=[];
   PriceDetails: PriceAgreement | null = null;
    payment: any;
  emptyReq: RequestWithPrice = {
    jobRequestID: '',
    pickupLocation: '',
    deliveryLocation: '',
    cargoDescription: '',
    containerNumber: '',
    status: '',
    priceAgreementID: '',
    truckType: '',
    truckID: '',
    driverID: '',
    requestType: '',
    customerID: '',
    requestedPrice: undefined,  // Set as undefined
    acceptedPrice: undefined,
    customerPrice: undefined,
    companyID: '',
    contractId: '',
    firstDepositAmount: undefined
  };
 activeRequest: RequestWithPrice = {
  jobRequestID: '',
  pickupLocation: '',
  deliveryLocation: '',
  cargoDescription: '',
  containerNumber: '',
  status: '',
  priceAgreementID: '',
  truckType: '',
  truckID: '',
  driverID: '',
  requestType: '',
  customerID: '',
  requestedPrice: undefined,  // Set as undefined
  acceptedPrice: undefined,
  customerPrice: undefined,
  companyID:'',
  contractId:'',
  firstDepositAmount: undefined
};
 

  constructor(
    public globalColumnControlService: GlobalColumnControlService,
    public functions: FunctionsService,
    private dataService: DataService,
    private fb: FormBuilder,
    private authServices: AuthService,
    private loadingService: LoadingService,
  ) {

    this.requestForm = this.fb.group({
      customer: [null, Validators.required],
      requestType: [null, Validators.required],
      description: ['', Validators.required],
      priority: ['Medium', Validators.required],
      dueDate: [null, Validators.required],
      attachments: [null]
    });
  }

  ngOnInit() {      
    
     this.loadColumns();
    this.customerID = this.authServices.getUserId() || '';
    this.loadJobs(this.customerID);
    this.loadColumns();
    this.loadCustomers();
    this.getTruckTypes();
     this.preLoads();
     

    this.filteredJobs = this.jobRequests;


  }

  preLoads() {
    this.requestType = [
      
      { label: 'TRUCK WITH DRIVER', value: 'TRUCK WITH DRIVER' },
      { label: 'DRIVER ONLY', value: 'DRIVER' },

    ];
  }


  // Fetch available truck types
  public getTruckTypes(): void {
    this.loadingService.show();
    this.dataService.GetTruckTypes().subscribe(
      (response) => {
        this.loadingService.hide();

        this.truckTypes = response.data;
      },
      (error) => {
        this.loadingService.hide();

        const errorMessage =
          error.error?.message ||
          (typeof error.error === 'string' ? error.error : null) ||
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.functions.displayInfo(errorMessage);
      }
    );
  }
  loadJobs(customerID: string): void {
    this.loadingService.show();
    this.dataService.getCustomerJobs(customerID).subscribe(
      (response) => {
         if (response.data) {
          this.jobRequests = response.data;
          this.filteredJobs = this.jobRequests;

          console.log(response.data);
          this.loadingService.hide();
        } else {
          this.loadingService.hide();
          this.functions.displayError('No job requests found.');
        }
      },
      (error) => {
        this.loadingService.hide();

        const errorMessage =
          error.error?.message ||
          (typeof error.error === 'string' ? error.error : null) ||
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.functions.displayError(errorMessage);
      }
    );
    this.loadingService.hide();
  }

  loadCustomers() {
    this.loadingService.show();
    this.dataService.GetCustomersByCompany(this.customerID).subscribe(
      (response) => this.customersList = response.data,
      (error) => {
        this.loadingService.hide();
        const errorMessage =
          error.error?.message ||
          (typeof error.error === 'string' ? error.error : null) ||
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.functions.displayError(errorMessage);
      });
      this.loadingService.hide();
  }

  getCustomerName(customerID: string | undefined): string {
    const customerOnRequest = this.customersList.find(cust => customerID === customerID); // Ensure to use a property like 'id' for comparison
    this.ActiveCustomerName = customerOnRequest ? customerOnRequest.fullName : undefined; // Assuming 'name' is the property you want
    return this.ActiveCustomerName || 'N/A'; // Return the customer's name or 'N/A'
  }
  
  confirmDelete(): void {

    this.dataService.DeleteJobRequest(this.RequestToDelete).subscribe(
      (response: any) => {
        if (response && response.success) {
          // this.payments = this.payments.filter((p) => p.paymentID !== paymentID);
          this.functions.displaySuccess('Job Request deleted successfully');
          this.functions.reloadPage();
          this.functions.displayDeleteSuccess();
        } else {
          this.functions.displayInfo('Received an unexpected response format');
        }
      },
      (error: any) => {
        const errorMessage =
          error.error?.message ||
          (typeof error.error === 'string' ? error.error : null) ||
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.functions.displayError(errorMessage);
      }
    );
  }
  resetForm() {
    this.contractDetails = null;
    this.activeRequest = {} as RequestWithPrice;  // Reset to an empty object
    this.showTruckContent = false;
    this.showDriverContent = false;
    this.showDriverContent = false;
    this.showPriceContent = true;
    this.showPriceLabels = false;
    this.requestDetailsVisible=false;
    
  }

    showRequestOnProcess(req: JobRequest){
      this.RequestOnProcess=req;

    }
    showRequestDetaisl(req: JobRequest){

    this.RequestOnProcess=req; 
       this.IsshowRequestDetaisl = true;


    }

  editRequest(event: JobRequest) {
    this.loadingService.show();
    this.resetForm();
     this.showTruckContent = false;
    this.showDriverContent = false;

    const selectedJobRequest  = event;
    this.activeReqPrice = event.priceDetails?.companyPrice;
    const ActiveReq: RequestWithPrice = {
      jobRequestID: selectedJobRequest.jobRequestID,
      pickupLocation: selectedJobRequest.pickupLocation,
      deliveryLocation: selectedJobRequest.deliveryLocation,
      cargoDescription: selectedJobRequest.cargoDescription,
      containerNumber: selectedJobRequest.containerNumber,
      status: selectedJobRequest.status,
      priceAgreementID: selectedJobRequest.priceAgreementID,
      truckType: selectedJobRequest.truckType || '',
      truckID: selectedJobRequest.truckID || '',
      driverID: selectedJobRequest.driverID || '',  // Set this if you can get driverID from the JobRequest
      requestType: selectedJobRequest.requestType || '',  // Set this if available
      customerID: selectedJobRequest.customerID,
      requestedPrice: selectedJobRequest.priceDetails?.companyPrice || 0,  // Use defaults or extract from a relevant property
      acceptedPrice: selectedJobRequest.priceDetails?.agreedPrice || 0,   // Default or extracted value
      customerPrice: selectedJobRequest.priceDetails?.customerPrice || 0,
      companyID: '',
      contractId: selectedJobRequest.contractId || 'N/A',
      companyAdvanceAmountRequred: selectedJobRequest.companyAdvanceAmountRequred || 0,
      firstDepositAmount: selectedJobRequest.firstDepositAmount || 0

    };
    // Set active request
    this.activeRequest = { ...ActiveReq };
    if (ActiveReq.requestType.includes(AppConstants.TRUCK_REQUEST_TYPE) &&
      ((ActiveReq.status == "READY FOR INVOICE") || (ActiveReq.status == "READY TO SERVE") || (ActiveReq.status == "TRUCK DRIVER ASSIGNMENT"))) {
      
      this.showTruckContent = true;
    }
   if (ActiveReq.contractId != null) {
      this.loadContractDetails(ActiveReq.contractId);
    }

    if (ActiveReq.truckID != null) {
      this.loadTruckDetails(ActiveReq.truckID);
    }

    if (ActiveReq.driverID != null) {
      this.loadDriverDetails(ActiveReq.driverID);
    }
 
    if ((ActiveReq.status == "CREATED") || (ActiveReq.status == "ON AGREEMENT") || (ActiveReq.status == 'DRAFT') || (ActiveReq.status == 'INCOMPLETE ADVANCE PAYMENT')) {
      this.showDriverContent = false;
      this.showTruckContent = false;
    }
    if ((ActiveReq.status == "READY FOR INVOICE") || (ActiveReq.status == "TRUCK DRIVER ASSIGNMENT") || (ActiveReq.status == "INVOICE PARTIAL READY TO SERVE") || (ActiveReq.status == "INVOICE PAID READY TO SERVE") || (ActiveReq.status == "READY TO SERVE") ||
      (ActiveReq.status == "CANCELLED") || (ActiveReq.status.includes("PENDING")) ||
      (ActiveReq.status.includes("DRAFT")) || (ActiveReq.status.includes("INCOMPLETE ADVANCE PAYMENT")) || (ActiveReq.status.includes("ONGOING INVOICE GENERATION"))) {
      this.loadPriceDetails(ActiveReq.priceAgreementID);
      this.showPriceContent = false;
      this.showPriceLabels = true;
    }
    this.loadingService.hide();


    this.requestDetailsVisible = true;

  }


  openNewJobDialog() {
    this.jobDialogVisible = true;
  }

  


  DeleteRequest(request: JobRequest): void {
    this.selectedJobRequest = request;
    this.deleteDialogVisible = true;
    this.RequestToDelete = request.jobRequestID;
  }

  

  loadColumns() {
    this.sourceColumns = [
      { field: 'cdate', header: 'Requested Time' },
      { field: 'udate', header: 'Last Update' },
  
      { field: 'pickupLocation', header: 'Pickup Location' },
      { field: 'deliveryLocation', header: 'Delivery Location' },
    
  
      // TruckDetails (Truck)
      { field: 'truck.model', header: 'Truck Model' },
      { field: 'truck.isTruckAvilableForBooking', header: 'Is Truck Available' },
      { field: 'truckType', header: 'Truck Type' },
      { field: 'truck.truckNumber', header: 'Truck Number' },
  
      // CustomerDetails (Customer)

      { field: 'customer.email', header: 'Customer Email' },
      { field: 'customer.phone', header: 'Customer Phone' },
      { field: 'customer.address', header: 'Customer Address' },
      { field: 'customer.fullName', header: 'Customer Name' },
      { field: 'containerNumber', header: 'Reference Number' },
      { field: 'priceAgreement.priceAgreementID', header: 'Agreement ID' },

    ];
  
    // Initialize selected columns with default values
    this.targetColumns = [
       { field: 'jobRequestID', header: 'Job Request ID' },
      { field: 'companyID', header: 'Company Id' },
      { field: 'customer.customerID', header: 'Customer Id' },
      { field: 'cargoDescription', header: 'Cargo Description' },
      { field: 'invoiceNumber', header: 'Invoice Number'},
      { field: 'priceAgreement.agreedPrice', header: 'Accepted Price' },
      { field: 'priceAgreement.companyPrice', header: 'Last Company Reply Price' },
      { field: 'priceAgreement.customerPrice', header: 'Your  Price'},
      { field: 'status', header: 'Status' },
    ];
  }
 
  
  stockSeverity(req: JobRequest) {
    if (req.status === "PENDING") return 'danger';
    else if (req.status =="PAID PARTIAL READY TO SERVE" ) return 'warn';
    else return 'success';
}
  getNestedValue(rowData: any, field: string): any {
    return field.split('.').reduce((acc, part) => acc && acc[part], rowData) || null;
  }


  isFormValid(): boolean {
    return this.newJobRequest.requestType && 
           this.selectedTruckTypeID && 
           this.newJobRequest.customerID && 
           this.newJobRequest.pickupLocation && 
           this.newJobRequest.deliveryLocation;
  }

  onSearch() {
    // Extract fields from targetColumns dynamically
    const fieldsToSearch = this.targetColumns.map(column => column.field);
  
    // Filter the data based on the fields
    this.filteredJobs = this.globalColumnControlService.filterData(this.jobRequests, this.searchTerm, fieldsToSearch);
  }
 

  loadContractDetails(ContrId: string) {
    if (ContrId) {
      this.loadingService.show();
      this.dataService.getContractById(this.activeRequest.contractId).subscribe(
        (response)  => this.contractDetails = response.data,
        (error) => {
          this.loadingService.hide();
          const errorMessage =
          error.error?.message ||  
          (typeof error.error === 'string' ? error.error : null) ||  
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.functions.displayError(errorMessage);
        }     
      );
      this.loadingService.hide();
    }
  }
  loadTruckDetails(TruckId: string) {
    if (TruckId) {
      this.loadingService.show();
      this.dataService.getTruckById(this.activeRequest.truckID).subscribe(
        (response)  => this.TruckDetails = response.data,
        (error) => {
          this.loadingService.hide();
          const errorMessage =
          error.error?.message ||  
          (typeof error.error === 'string' ? error.error : null) ||  
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.functions.displayError(errorMessage);
        }       
        );
        this.loadingService.hide();
    }
  }
  
  loadDriverDetails(DriverId: string) {
    if (DriverId) {
      this.loadingService.show();
      this.dataService.getDriverById(this.activeRequest.driverID).subscribe(
        (response)  => this.DriverDetails = response.data,
        (error) => {
          this.loadingService.hide();
          const errorMessage =
          error.error?.message ||  
          (typeof error.error === 'string' ? error.error : null) ||  
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.functions.displayError(errorMessage);
        }    
           );
           this.loadingService.hide();
    }
  }
  
  loadPriceDetails(priceAgreementID: string) {
    if (priceAgreementID) {
      this.loadingService.show();
      this.dataService.getPriceDataById(this.activeRequest.priceAgreementID).subscribe(
        (response)  => this.PriceDetails = response.data,
        (error) => {
          this.loadingService.hide();
          const errorMessage =
          error.error?.message ||  
          (typeof error.error === 'string' ? error.error : null) ||  
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.functions.displayError(errorMessage);
        }
      );
      this.loadingService.hide();
    }
  }


 
   
 
  
 
  //#region printing to pdf

  exportToPDF() {
    const doc = new jsPDF();
    const content = document.getElementById('contractDocument');

    if (content) {
      doc.text(content.innerText, 10, 10);
      doc.save('Contract_Agreement.pdf');
    }
  }

  printContract() {
    const printContent = document.getElementById('contractDocument');
    const WindowPrt = window.open('', '', 'width=900,height=700');

    if (WindowPrt && printContent) {
      WindowPrt.document.write('<html><head><title>Contract</title></head><body>');
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.write('</body></html>');
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }
  }

 // #endregion


 searchJobs() {
  if (!this.searchTerm.trim()) {
    // If the search is cleared, reset to all job requests
    this.filteredJobs = [...this.jobRequests];
  } else {
    this.filteredJobs = this.jobRequests.filter(job =>
      job.pickupLocation.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      job.deliveryLocation.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      job.status.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }
}

onInputChange() {
  this.isFormChanged = true;
  if (!this.searchTerm.trim()) {
    this.filteredJobs = [...this.jobRequests];
  }
}
filterRequests() {
  if (this.searchTerm) {
    this.filteredJobs = this.jobRequests.filter(job =>
      Object.values(job).some(value =>
        value !== null &&
        value !== undefined &&
        value.toString().toLowerCase().includes(this.searchTerm.toLowerCase())
      )
    );
  } else {
    // Reload all jobs when clearing the search
    this.loadJobs(this.customerID);
  }
}


}
