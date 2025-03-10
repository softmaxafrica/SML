import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
     import { MenuItem } from 'primeng/api';
import { Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { DriverPayload } from '../../models/drivers';
import { TruckType } from '../../models/TruckType';
import { DataService } from '../../services/DataService';
import { FunctionsService } from '../../services/functions';
import { GlobalColumnControlService } from '../../services/GlobalColumnControlService';
import { AppConstants } from '../../shared/Constants';
import { PRIME_COMPONENTS } from '../../shared-modules';
import { LoadingService } from '../../services/loading.service';
import { ApprovalPayload } from '../../models/ApprovalPayload';
// import { ReactiveInputGroupComponent } from '../../models/components/reactiveinputsgroup/reactiveinputsgroup.component';
 
@Component({
  selector: 'app-driver-vetting',
  templateUrl: './drivers.component.html',
  styleUrls: ['./drivers.component.css','../../app.component.css'],
  imports:[PRIME_COMPONENTS],
  providers: [DataService]
})
export class DriversComponent implements OnInit {

 
  drivers: DriverPayload[] = [];
  ShowNewDriverDialog: boolean=false;
  companyId!: string;
  driverForm!: FormGroup;
  displayDialog: boolean = false;
  selectedDriver: DriverPayload | null = null;
  action: any;
  columnDialogVisible: boolean = false;
  first: number = 0;
  rows: number = 10;
  editDialogVisible: boolean = false;
  deleteDialogVisible: boolean = false;
  actionMenu: MenuItem[] | null | undefined;
  public baseUrl = AppConstants.API_SERVER_URL;

  assignTruckDialogVisible: boolean = false;
  assignTruckForm!: FormGroup;
  availableTruckTypes: { label: string, value: string }[] = [];
  selectedDriverForAssignment: DriverPayload | null = null;
  fiteredDrivers: DriverPayload[] =[];
  searchTerm: string='';
  

  constructor(
    private dataService: DataService,
    private authService: AuthService,
    private fb: FormBuilder,
    public globalColumnControlService: GlobalColumnControlService,
    public functions: FunctionsService,
    private router: Router,
    private loadingService: LoadingService,
    
  ) {}
  statusOptions = [
     { label: 'Approve', value: 'APPROVED' },
    { label: 'Reject', value: 'REJECTED' },
   ];
  ngOnInit(): void {
    this.initializeColumns();
    this.companyId = this.authService.getCompanyId() || '';
    this.getCompanyDrivers();
     this.initializeAssignTruckForm();

    this.editDialogVisible = false;

    
    // Initialize the form with validations
    this.driverForm = this.fb.group({
      fullName: ['', Validators.required],
      licenseNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      companyID: [{ value: '', disabled: true }, Validators.required],
      status: [{ value: '', disabled: false }, Validators.required],
      licenseClasses: [''],
      licenseExpireDate: ['', Validators.required], // License Expire Date should not be empty
      isAvilableForBooking: [false],
      registrationComment: ['']
    });
  }

  // selectedFile: File[]=[];
selectedFile: File | null = null;

driverPayload: DriverPayload = {
  status: 'PENDING', 
  companyID: '',
  driverID : '',              
  fullName : '',              
  email : '',                
  phone : '',                
  licenseNumber : '',         
   registrationComment : '', 
   licenseClasses: '',                
isAvilableForBooking: true,     
licenseExpireDate: new Date,  
imageUrl: '',  
   
 };  

  get licenseExpireDateControl(): FormControl {
    return this.driverForm.get('licenseExpireDate') as FormControl;
  }
  get isAvailableForBookingControl(): FormControl {
    return this.driverForm.get('isAvilableForBooking') as FormControl;
  }

  onSearch() {
    const fieldsToSearch = this.globalColumnControlService.getTargetColumns().map(column => column.field);
    this.fiteredDrivers = this.globalColumnControlService.filterData(this.drivers, this.searchTerm, fieldsToSearch);
  }
  
  truckColumns = [
    { field: 'sampleImageUrl', header: 'Image' },
    { field: 'truckTypeID', header: 'Truck Type Id' },
    { field: 'typeName', header: 'Truck Type' },
    { field: 'description', header: 'Description' }
  ];

  
  initializeColumns() {
    this.globalColumnControlService.setSourceColumns([
      { field: 'companyID', header: 'Company ID' },
      { field: 'driverID', header: 'Driver ID' },
      { field: 'licenseNumber', header: 'License Number' },
      { field: 'licenseClasses', header: 'License Classes' },
      { field: 'isAvilableForBooking', header: 'Is Driver Available' }
    ]);

    this.globalColumnControlService.setTargetColumns([
      { field: 'imageUrl', header: 'Picture' },
      { field: 'fullName', header: 'Full Name' },
      { field: 'email', header: 'Email' },
      { field: 'status', header: 'Status' },
      { field: 'licenseExpireDate', header: 'License Expire Date' },
      { field: 'phone', header: 'Phone' }
    ]);
  }

  
  initializeAssignTruckForm(): void {
    this.assignTruckForm = this.fb.group({
      truckTypes: [[], Validators.required],
    });
  }
  // loadActionMenu() {
  //   this.actionMenu = [
  //     {
  //       label: 'All',
  //       icon: 'assets/images/icons/test-drive.png',
  //       command: () => {
  //         this.openNewJobDialog();
  //         this.router.navigate(['/home/drivers/listing']);
  //       }
  //     },
  //     {
  //       label: 'Add',
  //       icon: 'assets/images/icons/add.png',
  //       command: () => {
  //         this.openNewJobDialog();
  //         this.router.navigate(['/home/drivers/registration']);
  //       }
  //     },
  //     {
  //       label: 'Vetting',
  //       icon: 'assets/images/icons/endorsement.png',
  //       command: () => {
  //         this.openNewJobDialog();
  //         this.router.navigate(['/home/drivers/vetting']);
  //       }
  //     },

  //   ];
  // }
 

  onImageSelected(event: any): void {
    this.selectedFile = event.files[0];  // PrimeNG file upload returns an array of files
  }

  populateForm(driver: DriverPayload): void {
    this.driverForm.setValue({
      fullName: driver.fullName,
      licenseNumber: driver.licenseNumber,
      email: driver.email,
      phone: driver.phone,
      companyID: driver.companyID,
      status: driver.status,
      licenseClasses: driver.licenseClasses,
      // Properly format the date for the form
      licenseExpireDate: driver.licenseExpireDate
        ? new Date(driver.licenseExpireDate).toISOString().split('T')[0]
        : '',
      isAvilableForBooking: driver.isAvilableForBooking,
      registrationComment: '' // Reset the comment field
    });
  }

  isLastPage(): boolean {
    return this.drivers.length
      ? this.globalColumnControlService.first >=
          this.drivers.length - this.globalColumnControlService.rows
      : true;
  }

  onCancel(): void {
    this.displayDialog = false;
    this.functions.displayCancelSuccess(); // Use FunctionsService for cancellation notification
  }

  editDriver(driver: DriverPayload): void {
    this.selectedDriver = driver;
    console.log(driver);
    this.populateForm(driver);
    this.openAssignTruckDialog(driver);
    this.editDialogVisible = true;
  }

  saveDriver(): void {
    if (this.driverForm.valid) {
      const updatedDriver = { ...this.selectedDriver, ...this.driverForm.value };

    const licenseExpireDate = this.driverForm.get('licenseExpireDate')?.value;
    updatedDriver.licenseExpireDate = this.functions.getFormatApiDateOnly(licenseExpireDate);

      updatedDriver.registrationComment = 'NO COMMENT'; // Set default comment


      this.dataService.updateDriver(updatedDriver).subscribe({
        next: (response: any) => {
          this.functions.displayUpdateSuccess();
            
        },
        error: (err) => {
          this.functions.displayError('Failed to update truck status: ' + err.message);
        }
      });
       this.ngOnInit();
    }
 
    }
   



  confirmDelete(): void {
    this.deleteDialogVisible = true;
  }

  deleteDriver(): void {
    if (this.selectedDriver) {
      this.dataService.deleteDriver(this.selectedDriver.driverID).subscribe(
        () => {
          this.ngOnInit();
          this.functions.displayDeleteSuccess(); 
          this.deleteDialogVisible = false;
        },
        (error) => {
          const errorMessage = error.error || error.message || 'An unknown error occurred';
          this.functions.displayError(errorMessage); // Use FunctionsService for error notification
        }
      );
    }
  }

  getCompanyDrivers(): void {
    this.loadingService.show();
    this.dataService.getCompanyDrivers(this.companyId).subscribe(
      (response) => {
        this.loadingService.hide();
        this.drivers = response.data || [];
        this.fiteredDrivers= this.drivers;
      },
      (error) => {
        this.loadingService.hide();
        const errorMessage = error.error || error.message || 'An unknown error occurred';
        this.functions.displayError(errorMessage); // Use FunctionsService for error notification
      }
    );
  }
   
  assignTruckTypes(): void {
    this.loadingService.show();
    if (this.assignTruckForm.valid && this.selectedDriverForAssignment) {
      const selectedTruckTypes = this.assignTruckForm.value.truckTypes;
  
   
      if (!selectedTruckTypes || selectedTruckTypes.length === 0) {
        this.functions.displayError("No truck types selected.");
        return;
      }
  
      const assignedTruckTypes = selectedTruckTypes.map((truckType: any) => truckType.value || truckType);
  
      console.log("Mapped truck types to send:", assignedTruckTypes);
  
      const driverId = this.selectedDriverForAssignment.driverID;
 
      this.dataService.assignTruckTypesToDriver(driverId, assignedTruckTypes).subscribe(
        () => {
          this.loadingService.hide();
          this.assignTruckDialogVisible = false;
       this.ngOnInit();
          this.functions.displaySuccess("Truck types assigned successfully.");
          this.getCompanyDrivers(); // Refresh list
        },
        (error) => {
          this.loadingService.hide();
          const errorMessage = error.error?.message || error.message || 'An unknown error occurred';
          this.functions.displayError(errorMessage);
        }
      );
    }
  }
  
  
  
  

  fetchTruckTypes(): void {

    this.dataService.GetTruckTypes().subscribe(
      (response) => {
        this.availableTruckTypes = response.data.map((type: TruckType) => ({
          label: type.typeName, // Display text
          value: type.truckTypeID // Value submitted
      }));      },
      (error) => {
        const errorMessage =
        error.error?.message ||  
        (typeof error.error === 'string' ? error.error : null) ||  
        error.message || // General HTTP error message
        'An unknown error occurred';
      this.functions.displayInfo(errorMessage);
      }
    );
}

 
openAssignTruckDialog(driver: DriverPayload): void {
  this.selectedDriverForAssignment = driver;
  this.fetchTruckTypes(); // Ensure this is called before showing the dialog
  this.assignTruckForm.reset();
  // this.assignTruckDialogVisible = true;
}

 
showTruckAssignmentToDriver(): void {
  this.assignTruckDialogVisible = true;
}


ResendDriverRegistration() {
  if (this.driverForm.valid) {
    const driverToReRegister = { ...this.selectedDriver, ...this.driverForm.value };

  const licenseExpireDate = this.driverForm.get('licenseExpireDate')?.value;
  driverToReRegister.licenseExpireDate = this.functions.getFormatApiDateOnly(licenseExpireDate);

  driverToReRegister.registrationComment = this.selectedDriver?.registrationComment ?? 'RE-REGISTRATION'; // Set default comment
  driverToReRegister.status='PENDING';

    this.dataService.updateDriver(driverToReRegister).subscribe({
      next: (response: any) => {
        this.functions.displaySuccess(response.message);
          
      },
      error: (err) => {
        this.functions.displayError('Failed to update truck status: ' + err.message);
      }
    });
     this.functions.reloadPage();
  }

}


//#region NEW DRIVER REGION
leftFields = [
  { key: 'fullName', label: 'full_name', type: 'text' },
  { key: 'email', label: 'email', type: 'email' },
  { key: 'phone', label: 'phone', type: 'text' },
];

rightFields = [
  { key: 'licenseNumber', label: 'license_number', type: 'text' },
  { key: 'licenseExpireDate', label: 'license_expire_date', type: 'datepicker' },
  { key: 'isAvailableForBooking', label: 'is_available_for_booking', type: 'toggle' },
  { key: 'profileImage', label: 'profile_image', type: 'file' }, // Image upload control
];

// Buttons for the footer
buttons = [
  { label: 'cancel', icon: 'pi pi-times', class: 'p-button-secondary', action: 'cancel' },
  { label: 'save', icon: 'pi pi-check', class: 'p-button-primary', action: 'save' },
];


openDriverDialog() {
  this.ShowNewDriverDialog = true;
}

// Handle button actions
onButtonClick(action: string) {
  if (action === 'save') {
    this.saveDriver();
  } else if (action === 'cancel') {
    this.displayDialog = false;
  }
}



// Handle file upload
onFileUpload(event: any) {
  const file = event.file;
  this.driverForm.get('profileImage')?.setValue(file); // Update form control with the file
  this.functions.displayInfo('Profile image uploaded successfully.');
}

// Save the driver data
saveNewDriver() {
  if (this.driverForm.valid) {
    console.log('Driver Data:', this.driverForm.value);
    this.functions.displayInsertSuccess('Driver registered successfully!');
 
    this.displayDialog = false;  
  } else {
    this.functions.displayError('Please fill all required fields.!'); 
  }
}


onRegister() {
  if (this.isFormValid()) {
    const formData = new FormData();

    formData.append('FullName', this.driverPayload.fullName);
    formData.append('Email', this.driverPayload.email);
    formData.append('Phone', this.driverPayload.phone);
    formData.append('LicenseNumber', this.driverPayload.licenseNumber);
    formData.append('CompanyID', this.companyId);
    formData.append('LicenseExpireDate', this.driverPayload.licenseExpireDate ? new Date(this.driverPayload.licenseExpireDate).toISOString().split('T')[0] : '');
    formData.append('LicenseClasses', this.driverPayload.licenseClasses || '');
    formData.append('RegstrationComment', this.driverPayload.registrationComment || 'N/A');
    formData.append('Status', this.driverPayload.status || '');
    formData.append('isAvilableForBooking', this.driverPayload.isAvilableForBooking.toString());

    // Add the selected file to FormData if it exists
    if (this.selectedFile) {
      formData.append('file', this.selectedFile, this.selectedFile.name);
    }
    console.log(this.driverPayload);

    // Send the FormData object
    this.dataService.postDriverFormData('RegisterDriver', formData)
      .subscribe({
        next: (response: any) => {
          if (response.success) {
            this.functions.displayInsertSuccess('Driver registered successfully');

            
            // this.router.navigate(['/home/dashboard']);
          } else {
            this.functions.displayInsertSuccess('Registration failed. Please try again.');
          }
        },
        error: (err: any) => {
          this.functions.displayError(err.error);
          
        }
      });
  } else {
    this.functions.displayError('Please fill out all required fields');
   }
}
isFormValid(): boolean {
  // Check if all required fields are non-empty and valid
  return (
    this.driverPayload.fullName?.trim() !== '' &&
    this.driverPayload.email?.trim() !== '' &&
    this.driverPayload.phone?.trim() !== '' &&
    this.driverPayload.licenseNumber?.trim() !== '' &&
     // Add any additional validations as needed
    this.driverPayload.licenseExpireDate instanceof Date && // Checks if it's a valid Date
    this.driverPayload.licenseClasses?.trim() !== '' // Optional but checking if it's filled
  );
}
//#endregion


//#region DriverVetting
approveDriver(driver: DriverPayload): void {
  this.selectedDriver = driver;
  this.action = 'approve';
  this.populateVettingForm(driver);
  this.displayDialog = true;
}

rejectDriver(driver: DriverPayload): void {
  this.selectedDriver = driver;
  this.action = 'reject';
  this.populateVettingForm(driver);
  this.displayDialog = true;
}

populateVettingForm(driver: DriverPayload): void {
  this.driverForm.setValue({
    fullName: driver.fullName,
    licenseNumber: driver.licenseNumber,
    email: driver.email,
    phone: driver.phone,
    registrationComment: '', // Reset the comment field
    companyID: this.companyId
  });
  this.driverForm.get('registrationComment')?.enable(); // Enable remarks field
}

onSubmit(): void {
  // Ensure the form is valid before proceeding
  if (this.driverForm.invalid) {
    this.functions.displaySuccess('Validation Error\n Please complete all required fields.');
    return;
  }

  // Extract the status and registrationComment from the form
  const status = this.driverForm.value.status;
    const registrationComment = this.driverForm.value.registrationComment || 'N/A';
  this.functions.displaySuccess('Status is: ' + status);

  // Ensure the selectedDriver has a valid driverID
  if (!this.selectedDriver?.driverID) {
    this.functions.displayError('Driver ID is missing. Cannot proceed with approval/rejection.');
    return;
  }

  // Prepare the payload for the API call
  const approvalPayload: ApprovalPayload = {
    userID: this.selectedDriver.driverID, // Use the driverID from selectedDriver
    status: status, // Use the status from the form
    registrationComment: registrationComment // Use the registrationComment from the form
  };

  // Call the API to approve/reject the driver
  this.dataService.approveDriver(approvalPayload).subscribe(
    (response) => {
      // Refresh the data after successful approval/rejection
      this.ngOnInit();

      // Display a success message based on the action
      const successMessage = status === 'REJECTED' ? 'Rejected Successfully' : 'Approved Successfully';
      this.functions.displaySuccess(successMessage);
    },
    (error: any) => {
      // Display an error message if the API call fails
      this.functions.displayError('Error approving/rejecting driver:\n' + error.error);
    }
  );
}

// New method to set action value
    setAction(action: string): void {
      this.action = action;
    }


//#endregion

}