import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem } from 'primeng/api';
import { DataService } from '../../services/DataService';
import { TrucksPayload } from '../../models/TrucksPayload';
 import { AuthService } from '../../auth/auth.service';
import { Router } from '@angular/router';
import { Sidebar } from 'primeng/sidebar';
import { TruckType } from '../../models/TruckTypes';
import { Truck } from '../../models/Truck';
import { DriverPayload } from '../../models/drivers';
import { DropdownChangeEvent } from 'primeng/dropdown';
import { CarouselResponsiveOptions } from 'primeng/carousel';
import { FunctionsService } from '../../services/functions';
import { PRIME_COMPONENTS } from '../../shared-modules';
import { LoadingService } from '../../services/loading.service';
import { AppConstants } from '../../shared/Constants';
 
@Component({
  selector: 'app-trucks',
  templateUrl: './trucks.component.html',
    imports: [PRIME_COMPONENTS],
  styleUrls: ['./trucks.component.css']
})
export class TrucksComponent implements OnInit {
  responsiveOptions: any[] | undefined;
  searchQuery: string = '';
  filteredTrucks: any[] = [];
  public baseUrl = AppConstants.API_SERVER_URL;

onSubmit() {
   console.log(this.truckPayload);
}

  currentDriverName: string = ''; // Updated to default to empty string
  isTruckTypeSelected: boolean = false;
  filteredTruckTypes: TruckType[] = [];
  cabinType: any[] = []; // Initialize your dropdown options
  category: any[] = [];
  fuelType: any[] = [];
  drive: any[] = [];
  selectedDriver: DriverPayload | null = null;
  selectedTruckType: TruckType | null = null;
  companyId: string = ''; // Default to empty string

  truckPayload: TrucksPayload = {
    truckID: '',   
    truckNumber: '',
    model: '',
    truckTypeID: '',
    companyID: '',
    isActive: true,
    isTruckAvilableForBooking: true,
    driverID: '',
    chasisNo: '',
    brand: '',
    engineCapacity: '',
    fuelType: '',
    cabinType: '',
    category: '',
    drive: ''
  };
   
  trucks: Truck[] = [];
  truckTypes: TruckType[] = [];
  companyDrivers: DriverPayload[] = [];
  
  newTruckVisibility: boolean = false;
  truckForm: FormGroup;
  truckmenu: MenuItem[] = [];
  sidebarVisible: boolean = false;
  editTruckDialog: boolean= false;
 

  constructor(
    private dataService: DataService,
    public functions: FunctionsService,
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private loadingService: LoadingService,
  ) {
    this.truckForm = this.fb.group({
      truckNumber: ['', Validators.required],
      model: ['', Validators.required],
      chasisNo: ['', Validators.nullValidator],   
      brand: ['', Validators.nullValidator],
      engineCapacity: ['', Validators.nullValidator],
      fuelType: ['', Validators.nullValidator],
      cabinType: ['', Validators.nullValidator],
      category: ['', Validators.nullValidator],
      drive: ['', Validators.nullValidator],
    });
  }

  ngOnInit(): void {
    this.companyId = this.authService.getCompanyId() || '';
    this.truckPayload.companyID = this.companyId;
     this.getTruckTypes();
    this.loadDropdownOptions();
    this.GetTruckByCompanyId();
    this.getCompanyDrivers();

    this.responsiveOptions = [
      {
          breakpoint: '1199px',
          numVisible: 1,
          numScroll: 1
      },
      {
          breakpoint: '991px',
          numVisible: 2,
          numScroll: 1
      },
      {
          breakpoint: '767px',
          numVisible: 1,
          numScroll: 1
      }
  ];
  }
  filterTrucks() {
    const query = this.searchQuery.toLowerCase();

    this.filteredTrucks = this.trucks.filter(truck => {
        return Object.values({
            truckID: truck.truckID,
            truckNumber: truck.truckNumber,
            model: truck.model,
            companyID: truck.companyID,
            truckTypeID: truck.truckTypeID,
            companyName: truck.company?.companyName, 
            driverName: truck.driver?.fullName,
            truckTypeName: truck.truckType?.typeName,
            isActive: truck.isActive.toString(),
            isTruckAvailableForBooking: truck.isTruckAvilableForBooking.toString()
        }).some(value => value?.toLowerCase().includes(query));
    });
}

  getCompanyDrivers() {
    this.loadingService.show();
    this.dataService.getCompanyDrivers(this.companyId).subscribe(
      (response) => {
        this.companyDrivers = response.data;
      },
      (error) => {
        const errorMessage = error.error || error.message || 'An unknown error occurred';
        this.functions.displayError(errorMessage);
      }
    );
    this.loadingService.hide();
  }

  loadDropdownOptions(): void {
    // Example data, replace with actual data retrieval logic
    this.cabinType = [
      { label: 'SINGLE CABIN', value: 'SINGLE CABIN' },
      { label: 'DOUBLE CABIN', value: 'DOUBLE CABIN' }
    ];
    this.category = [
      { label: 'HORSE', value: 'HORSE' },
      { label: 'TRAILER', value: 'TRAILER' }
    ];
    this.fuelType = [
      { label: 'DIESEL', value: 'DIESEL' },
      { label: 'PETROL', value: 'PETROL' },
      { label: 'ELECTRICAL', value: 'ELECTRICAL' }

    ];
    this.drive = [
      { label: 'LEFT HANDED DRIVE', value: 'LEFT HANDED DRIVE' },
      { label: 'RIGHT HANDED DRIVE', value: 'RIGHT HANDED DRIVE' }
    ];
  }

  public getTruckTypes(): void {
    this.dataService.GetTruckTypes().subscribe(
      (response) => {
        this.truckTypes = response.data;
      },
      (error) => {
        const errorMessage = error.error || error.message || 'An unknown error occurred';
        this.functions.displayError(errorMessage);
      }
    );
  }

  updateTruckDetails(updatedTruck: TrucksPayload, isActive: boolean) {
    updatedTruck.isActive = isActive;
    updatedTruck.isTruckAvilableForBooking = updatedTruck.isActive ? updatedTruck.isTruckAvilableForBooking : false;
    this.truckPayload = { ...updatedTruck };
    this.dataService.updateTruckDetails(updatedTruck).subscribe({
      next: (response: any) => {
        this.functions.displayInfo('Truck number: ' + updatedTruck.truckNumber + ' is now ' + (isActive ? 'ACTIVE' : 'INACTIVE'));
      },
      error: (err) => {
        this.functions.displayError('Failed to update truck status: ' + err.message);
      }
    });
    this.ngOnInit();
    this.editTruckDialog=false;
  }

  updateFullTruckDetails(updatedTruck: TrucksPayload, newDriverId?: string) {
    updatedTruck.isTruckAvilableForBooking = updatedTruck.isActive ? updatedTruck.isTruckAvilableForBooking : false;
    this.truckPayload = { ...updatedTruck };
    this.truckPayload.driverID = newDriverId ? newDriverId : this.truckPayload.driverID || ''; // Provide default value

    this.dataService.updateTruckDetails(updatedTruck).subscribe({
      next: (response: any) => {
        this.functions.displayUpdateSuccess();
      },
      error: (err) => {
        this.functions.displayError('Failed to update truck status: ' + err.message);
      }
    });
    this.editTruckDialog=false;
    this.ngOnInit();
  }

  GetTruckByCompanyId(): void {
    this.loadingService.show();
    this.dataService.GetTruckByCompanyId(this.companyId).subscribe(
      (response) => {
        this.loadingService.hide();
        if (response && response.data) {
          this.trucks = response.data;
          this.filteredTrucks = [...this.trucks]; 
          this.trucks.forEach(truck => {
            this.isTruckTypeSelected = truck.isActive; // Adjusted to use correct boolean
          });
        }
      },
      (error) => {
        this.loadingService.hide();
        const errorMessage = error.error || error.message || 'An unknown error occurred';
        this.functions.displayError(errorMessage);
      }
    );
  }

  onAddTruck() {
    if (this.isFormValid()) {
      this.truckPayload.truckTypeID = this.selectedTruckType?.truckTypeID || '';
      this.truckPayload.isActive = true;
      this.truckPayload.isTruckAvilableForBooking = true;

      // // Map the form values to truckPayload
      // this.truckPayload.chasisNo = this.truckForm.value.chasisNo  ; 
      // this.truckPayload.brand = this.truckForm.value.brand  ; 
      // this.truckPayload.engineCapacity = this.truckForm.value.engineCapacity  ; 
      // this.truckPayload.fuelType = this.truckForm.value.fuelType  ; 
      // this.truckPayload.cabinType = this.truckForm.value.cabinType  ; 
      // this.truckPayload.category = this.truckForm.value.category; 
      // this.truckPayload.drive = this.truckForm.value.drive; 
      this.loadingService.show();
      this.dataService.postTruck<TrucksPayload>('AddTruck', this.truckPayload)
        .subscribe({
          next: (response: any) => {
            this.loadingService.hide();
            if (response.success) {
              this.newTruckVisibility = false;
              this.ngOnInit();
              this.functions.displayInsertSuccess();
            } else {
              this.newTruckVisibility = false;
              this.functions.displayError(response.message);
            }
          },
          error: (err: any) => {
            this.loadingService.hide();
            this.newTruckVisibility = false;
            const errorMessage = err.error || 'unknown_error';
            this.functions.displayError('data_insertion_failed \n' + errorMessage);
          }
        });
    } else {
      this.functions.displayInfo('please_fill_all_the_required_fields');
    }
  }

  isFormValid(): boolean {
    return this.truckPayload.truckNumber.trim() !== '' &&
           this.truckPayload.model.trim() !== '' &&
           this.truckPayload.truckTypeID.trim() !== '';
  }

  onTruckTypeChange(event: any): void {
    this.selectedTruckType = event.value;
    this.truckPayload.truckTypeID = this.selectedTruckType?.truckTypeID || ''; // Handle undefined

    this.isTruckTypeSelected = true;
    if (this.selectedTruckType) {
      this.filteredTruckTypes = this.truckTypes.filter(truckType => truckType.truckTypeID === this.selectedTruckType!.truckTypeID);
    } else {
      this.filteredTruckTypes = [];  
    }
  }

  onCabinTypeChange(event: any) {
    console.log('Cabin Type Event:', event); // Log event details
    this.truckPayload.cabinType = event.value; // Assign selected value to truckPayload
    console.log('Selected Cabin Type:', this.truckPayload.cabinType);
}


  onCategoryChange(event: any) {
    this.truckPayload.category = event.value; // Assign selected value to truckPayload
    console.log('Selected Category:', this.truckPayload.category);
  }

  onFuelTypeChange(event: any) {
    this.truckPayload.fuelType = event.value; // Assign selected value to truckPayload
    console.log('Selected Fuel Type:', this.truckPayload.fuelType);
  }

  onDriveChange(event: any) {
    this.truckPayload.drive = event.value;  
    console.log('Selected Drive Type:', this.truckPayload.drive);
  }

  onEditTruck(truck: TrucksPayload) {
     
    this.truckPayload = { ...truck };
    const driver = this.companyDrivers.find(driver => driver.driverID === this.truckPayload.driverID);
    
    this.currentDriverName = driver ? driver.fullName : 'N/A';
    
    // Populate the form with existing truck details
    this.truckForm.setValue({
      truckNumber: this.truckPayload.truckNumber,
      model: this.truckPayload.model,
      chasisNo: this.truckPayload.chasisNo  ,
      brand: this.truckPayload.brand  ,
      engineCapacity: this.truckPayload.engineCapacity  ,
      fuelType: this.truckPayload.fuelType  ,
      cabinType: this.truckPayload.cabinType  ,
      category: this.truckPayload.category  ,
      drive: this.truckPayload.drive  ,
    });
    
    this.editTruckDialog = true;
  }
  onChangeTruckDriver(event: DropdownChangeEvent) {
    this.selectedDriver = event.value; 
     this.truckPayload.driverID = this.selectedDriver?.driverID ?? ''; // Fallback to empty string if undefined

  }
  onDeleteTruck(truckID: string) {
    this.loadingService.show();
       this.dataService.deleteTruck(truckID).subscribe(
        (response: any) => {
          this.loadingService.hide();
          if (response && response.success) {
            this.reloadPage();
            this.functions.displaySuccess('Truck deleted successfully');
            // this.reloadPage();
            this.functions.displayDeleteSuccess();
           } else {
            this.functions.displayInfo('Received an unexpected response format');
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
    }
  
  editTruck() {
   this.newTruckVisibility = true;
        
  }
  reloadPage() {
    window.location.reload();
  }

  
}
