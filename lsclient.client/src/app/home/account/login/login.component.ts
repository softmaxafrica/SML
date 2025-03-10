import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../auth/auth.service'; 
import { PRIME_COMPONENTS } from '../../../shared-modules';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { LoadingService } from '../../../services/loading.service';
import { CardModule } from 'primeng/card';
import { FunctionsService } from '../../../services/functions';

@Component({
  selector: 'app-login',
  imports: [PRIME_COMPONENTS, ProgressSpinnerModule, CardModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  isLoading: boolean = false;
  chooseAccountDialog: boolean = false;
  email = '';
  password = '';
  messages: { severity: string; summary: string; detail: string }[] = [];

  constructor(
    private router: Router,
    private authService: AuthService,
    private loadingService: LoadingService,
    public functions: FunctionsService,
  ) {}

  onLogin(): void {
    if (!this.email || !this.password) {
      this.messages = [{
        severity: 'warn',
        summary: 'Missing Fields',
        detail: 'Email and password are required'
      }];
      return;
    }

    this.isLoading = true;  // Start loading animation
    this.loadingService.show();

    this.authService.login(this.email, this.password).subscribe({
      next: () => {
        this.messages = [{
          severity: 'success',
          summary: 'Success',
          detail: 'Welcome again!'
        }];

        setTimeout(() => {
          this.isLoading = false;  // Stop loading animation
          this.loadingService.hide();
          this.router.navigate(['/home/dashboard']); // Navigate to dashboard
        }, 1500);
      },
      error: (error) => {
        this.isLoading = false;  // Stop loading animation on error
        this.loadingService.hide();
        
        const errorMessage =
          error.error?.message ||
          (typeof error.error === 'string' ? error.error : null) ||
          error.message || // General HTTP error message
          'An unknown error occurred';
        this.functions.displayInfo(errorMessage);
      }
    });
  }
  isFormValid(): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;  
  
    return emailRegex.test(this.email.trim()) && 
           this.password.trim() !== '';
  }
  openChooseAccountDialog() {
    this.chooseAccountDialog = true;
  }
}