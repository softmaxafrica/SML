import { Component } from '@angular/core';
import { CompanyPayload } from '../../../models/CompanyPayload';
import { DataService } from '../../../services/DataService';  
import { Router } from '@angular/router';
import { PRIME_COMPONENTS } from '../../../shared-modules';
import { FunctionsService } from '../../../services/functions';
@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  imports: [PRIME_COMPONENTS],
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  companyPayload: CompanyPayload = new CompanyPayload();
isSmallScreen: any;
;

  constructor(
    private dataService: DataService,
     private router: Router,
     private functions: FunctionsService) {}

  onRegister(): void {
    if (this.isFormValid()) {
      this.dataService.post<CompanyPayload>('RegisterCompany', this.companyPayload)
        .subscribe({
          next: (response: any) => {
            this.functions.displaySuccess('Registration Succsessfully ! \n Please Login to proceed');
            console.log('Company registered successfully', response);
            this.router.navigate(['/account/login']);  // Navigate to the appropriate page after successful registration
          },
          error: (err: any) => {
            console.error('Registration failed', err);
          }
        });
    } else {
      console.warn('Please fill out all required fields');
    }
  }

  isFormValid(): boolean {
    return this.companyPayload.companyName != null && 
           this.companyPayload.adminEmail != null && 
           this.companyPayload.adminFullName != null && 
           this.companyPayload.adminContact != null && 
           this.companyPayload.companyAddress != null;
  }
}
