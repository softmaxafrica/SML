import { ChangeDetectorRef, Component } from '@angular/core';
import { Invoice } from '../../../models/invoices';
import { Router } from '@angular/router';
import { AuthService } from '../../../auth/auth.service';
 import { DataService } from '../../../services/DataService';
 import { MenuItem } from 'primeng/api';
import { FunctionsService } from '../../../services/functions';
import { GlobalColumnControlService } from '../../../services/GlobalColumnControlService';
import { PRIME_COMPONENTS } from '../../../shared-modules';
 import { LoadingService } from '../../../services/loading.service';
import { Payment } from '../../../models/payments';
import { Customer } from '../../../models/Customer';
import { jsPDF } from 'jspdf';
import html2canvas from 'html2canvas'; // Optional, for better rendering

 @Component({
  selector: 'app-invoices',
  templateUrl: './invoice.component.html',
  styleUrls: ['../../../app.component.css','./invoice.component.css'],
  imports: [PRIME_COMPONENTS]
})
export class InvoiceComponent {
  showPaymentFormDetails: boolean= false;
  isRecordPayment:boolean= false;
  newPayment: boolean= false;
  displayInvoicePaymentDialog: boolean= false;

  amountInvalid: boolean = true;
  currencyInvalid: boolean = true;  

 searchTerm: any;
 filteredInvoices: Invoice[] = [];
 globalSearchService: any;
 selectedPayment: any= null;
 sourceColumns: any[] = [];
  targetColumns: any[] = [];
 paymentsColumns: any[]=[];


//#region Payment
payments: Payment[] = [];
filteredPayments: Payment[]=[];

//#region Defaults Objects
defaultCustomerDetails: Customer  = {
 customerID: '',
 fullName: '',
 email: '',
 phone: '',
 address: '',

   cardNumber: '',
   cardType: '',
   paymentMethod: '',
   billingAddress: '',
   expiryDate: '',
   bankName: '',
   bankAccountNumber: '',
   bankAccountHolder: '',
   mobileNetwork: '',
   mobileNumber: '',
 
 companies: []
};

defaultInvoice: Invoice = {
 invoiceNumber: 0,
 companyID: '',
 customerID: '',
 jobRequestID: '',
 paymentId: null,
 totalAmount: 0,
 totalPaidAmount: 0,
 owedAmount: 0,
 serviceCharge: 0,
 operationalCharge: 0,
 issueDate: new Date().toISOString(),
 dueDate: null,
 status: '',
 customerDetails: this.defaultCustomerDetails,
};

//#endregion
payment: Payment = {
 paymentID: '',
 invoiceNumber: 0,
 amountPaid: 0,
 paymentDate: new Date(),
 paymentMethod: '',
 referenceNumber: null,
 currency: '',
 invoice: this.defaultInvoice,
}; // Holds payment details during creation/edit

invoiceDetails: Invoice|null=null;
companyId!: string;
currencies =[
 {label:'Tanzania Shillings (TZs)',value: 'TZS'},
 {label:'US DOLLERS (USD)',value: 'USD'}
]
paymentMethods = [
 { label: 'Credit Card', value: 'Credit Card' },
 { label: 'Bank Transfer', value: 'Bank Transfer' },
 { label: 'Cash', value: 'Cash' },
 { label: 'Mobile Payment', value: 'Mobile Payment' }
];
   isSaveButtonDisabled: boolean =false;
 

// savePayment() {
//   if (this.payment) {
//     this.payment.invoiceNumber= this.selectedInvoice.invoiceNumber;
//       this.dataService.addPayment(this.payment).subscribe({
//           next: (response: any) => {
//               if (response && response.success) {
//                   this.loadPayments(this.companyId);
//                   this.displayDialog = false;
//                   this.closePaymentDialog();
//                   this.functions.displaySuccess('Payment added successfully');
//               } else {
//                   const errorMessage = response?.message || 'Failed to add payment';
//                   this.functions.displayError(errorMessage);
//               }
//           },
//           error: (error: any) => {
//               const errorMessage =
//                   error.error?.message ||
//                   (typeof error.error === 'string' ? error.error : null) ||
//                   error.message ||
//                   'An unknown error occurred';
//                   this.closePaymentDialog();
//                   this.functions.displayError(errorMessage);

//           },
//       });
//   }
//   // this.reloadPage(); 

// }

validateCurrency() {
  this.currencyInvalid = !this.payment.currency || this.payment.currency === ''; 
  this.updateButtonState();
}

validateAmount() {
  if (!this.payment.amountPaid || this.payment.amountPaid > this.selectedInvoice?.owedAmount || this.payment.amountPaid <= 0) {
    this.amountInvalid = true;
    this.functions.displayError(`Amount must be between 1 and ${this.selectedInvoice?.owedAmount}`);
  } else {
    this.amountInvalid = false;
  }
  this.updateButtonState();
}

updateButtonState() {
  // This ensures the button gets enabled only when both amount and currency are valid
  this.isSaveButtonDisabled = this.amountInvalid || this.currencyInvalid;
}



savePayment() {
  if (this.amountInvalid) {
    this.functions.displayError("Fix errors before saving.");
    return;
  }

  if (this.payment) {
    this.payment.invoiceNumber = this.selectedInvoice.invoiceNumber;
    this.dataService.addPayment(this.payment).subscribe({
      next: (response: any) => {
        if (response?.success) {
          this.loadPayments(this.companyId);
          this.functions.displaySuccess('Payment added successfully');
          this.closePaymentDialog(); // Hide form
        } else {
          this.functions.displayError(response?.message || 'Failed to add payment');
        }
      },
      error: (error: any) => {
        this.functions.displayError(error.error?.message || error.message || 'An unknown error occurred');
        this.closePaymentDialog(); // Hide form
      }
    });
  }
}
onAddPayment() {
  this.newPayment = true;
  this.displayInvoicePaymentDialog = true;
   this.isRecordPayment = true; // Show the form
}
   columnDialogVisible: boolean = false;

   getNestedValue(rowData: any, field: string): any {
     return field.split('.').reduce((acc, part) => acc && acc[part], rowData) || null;
   }

  
   closePaymentDialog() {
    this.selectedPayment = null;
    this.showPaymentFormDetails = false; // Hide the form
    this.newPayment = false;
    this.payment = { // Reset the payment object
      paymentID: '',
      invoiceNumber: 0,
      amountPaid: 0,
      paymentDate: new Date(),
      paymentMethod: '',
      referenceNumber: null,
      currency: '',
      invoice: this.defaultInvoice
    };
  }

loadCompanyInvoicesDetails(InvoiceNumber: number): void {
  this.dataService.getCompanyInvoiceDetails(this.companyId,InvoiceNumber).subscribe(
    (response: any) => {
      if (response && response.data) {
         this.invoiceDetails = response.data;
         this.payment.amountPaid = this.invoiceDetails?.owedAmount || 0;
      } else {
         this.invoiceDetails = response || [];
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
 

loadPayments(companyId: string): void {
  this.dataService.getCompanyPayments(companyId).subscribe(
    (response: any) => {
      if (response && response.data) {
         this.payments = response.data;
         this.filteredPayments = this.payments;
 
      } else {
         this.payments = response || [];
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
 

//#endregion

 

  
   invoiceList: Invoice[] = [];
  actionMenu: MenuItem[] | null | undefined;
  generateInvoiceVisible: boolean = false;
  layout: string = 'list';


  constructor(
    public functions: FunctionsService,
    private authServices: AuthService,
    public globalColumnControlService: GlobalColumnControlService,
    private dataService: DataService,
    private changeDetector: ChangeDetectorRef,
    private router: Router,
    private loadingService: LoadingService,

  ) { }


   ngOnInit() {
    this.companyId = this.authServices.getCompanyId() || '';
    this.loadCompanyInvoices(this.companyId);
    this.loadColumns();
    this.loadActionMenu();
    this.displayInvoicePaymentDialog=false;
    this.showPaymentFormDetails=false;
  }
  loadColumns() {
    // Initialize target columns with default values (these will be the visible columns in the table initially)
    this.targetColumns = [
      { field: 'invoiceNumber', header: 'Invoice Number' },
      { field: 'customerID', header: 'Customer ID' }, // Added missing field
      { field: 'totalAmount', header: 'Total Amount' },
      { field: 'status', header: 'Status' },
      { field: 'owedAmount', header: 'Owed Amount' }, // Added missing field
      { field: 'totalPaidAmount', header: 'Total Paid Amount' }, // Added missing field

    ];

    // Source columns include optional and less frequently displayed fields
    this.sourceColumns = [
      { field: 'companyID', header: 'Company ID' }, // Added missing field
      { field: 'jobRequestID', header: 'Job Request ID' }, // Added missing field
      //{ field: 'paymentId', header: 'Payment ID' },
       { field: 'serviceCharge', header: 'Service Charge' },
      { field: 'operationalCharge', header: 'Operational Charge' },
      { field: 'issueDate', header: 'Issue Date' },
      { field: 'dueDate', header: 'Due Date' }, // Optional field
    ];


    this.paymentsColumns = [
      { field: 'invoiceNumber', header: 'Invoice Number' },
      { field: 'paymentDate', header: 'Payment Date' },
      { field: 'amountPaid', header: 'Amount Paid' },
      //{ field: 'invoice.owedAmount', header: 'Owed Amount' },
      //{ field: 'paymentMethod', header: 'Payment Method' },
      { field: 'currency', header: 'Currency' },
      // Invoice Details

      // { field: 'invoice.status', header: 'Invoice Status'},
       
      // Customer Details
      { field: 'invoice.customerDetails.fullName', header: 'Customer Name' },
      { field: 'invoice.customerDetails.phone', header: 'Customer Phone' },

    ];
  }



  loadCompanyInvoices(companyId: string): void {
    this.loadingService.show();
    this.dataService.getCompanyInvoices(companyId).subscribe(
      (response: any) => {
        if (response && response.data) {
          this.invoiceList = response.data;
          this.filteredInvoices = this.invoiceList;
        } else {
          this.invoiceList = response || [];
          this.functions.displayInfo('Received an unexpected response format');
        }
        this.loadingService.hide(); // Ensure hide is always called
      },
      (error) => {
        const errorMessage =
          error.error?.message ||
          (typeof error.error === 'string' ? error.error : null) ||
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.loadingService.hide(); // Ensure hide is always called
        this.functions.displayError(errorMessage);
      }
    );
  }
  

  displayDialog = false;
  selectedInvoice: any = null;

   async showInvoiceDetails(invoice: Invoice) {
     this.loadingService.show();
    await this.loadInvoicePayments(invoice.invoiceNumber);
    this.loadCompanyInvoicesDetails(invoice.invoiceNumber);
    this.selectedInvoice = invoice;
     this.displayDialog = true;
     this.loadingService.hide();
   }


   loadInvoicePayments(InvoiceNumber: number): void {
    this.loadingService.show();
     this.dataService.getInvoicePayments(InvoiceNumber).subscribe(
       (response: any) => {
         if (response && response.data) {
           this.filteredPayments = response.data;
         }
       },
       (error: any) => {
        this.loadingService.hide();
         const errorMessage =
           error.error?.message ||
           (typeof error.error === 'string' ? error.error : null) ||
           error.message || // General HTTP error message
           'An unknown error occurred';
         this.functions.displayError(errorMessage);
       }
     );
   }


  

  //#region Menu
  loadActionMenu() {
    this.actionMenu = [
      {
        label: 'add',
        icon: 'assets/images/icons/add.png',
        command: () => {
          this.NewInvoiceDialog();
        }
      },
      {
        label: 'edit',
        icon: 'assets/images/icons/edit.png',
        command: () => {
          console.log('Edit action triggered');
          // this.router.navigate(['/edit-item']);
        }
      },
      {
        label: 'delete',
        icon: 'assets/images/icons/delete.png',
        command: () => {
          console.log('Delete action triggered');
          // this.confirmDelete();
        }
      },
      {
        label: 'info',
        icon: 'assets/images/icons/more-info.png',
        command: () => {
          console.log('Info action triggered');
          // this.router.navigate(['/info']);
        }
      }
    ];

  }

  NewInvoiceDialog() {
    this.generateInvoiceVisible = true;
  }
  //#endregion


  //#region  PageActions
  closeJobDialog() {
    this.reloadPage();
  }

  reloadPage() {
    window.location.reload();
  }
  //#endregion

  //#region filters
  onSearch() {
    // Extract fields from targetColumns dynamically
    const fieldsToSearch = this.targetColumns.map(column => column.field);

    // Filter the data based on the fields
    this.filteredInvoices = this.globalColumnControlService.filterData(this.invoiceList, this.searchTerm, fieldsToSearch);
  }


  //#endregion

  printContract() {
    const printContent = document.getElementById('invoiceDocument');
    const WindowPrt = window.open('', '', 'width=900,height=700');

    if (WindowPrt && printContent) {
      WindowPrt.document.write('<html><head><title>Invoice</title></head><body>');
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.write('</body></html>');
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }
  }

}
