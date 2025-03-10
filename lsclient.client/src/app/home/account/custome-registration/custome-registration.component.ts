import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { PRIME_COMPONENTS } from '../../../shared-modules';
import { FunctionsService } from '../../../services/functions';
import { DataService } from '../../../services/DataService';
import { Customer } from '../../../models/Customer';
import { Router } from '@angular/router';  

@Component({
  selector: 'app-customer-registration',
  templateUrl: './custome-registration.component.html',
  styleUrl: './custome-registration.component.css',
  imports: [PRIME_COMPONENTS]
})
export class CustomerRegistrationComponent implements OnInit {
  customerForm!: FormGroup;
  paymentMethods: any[] = [
    { label: 'Credit Card', value: 'Credit Card' },
    { label: 'Bank Transfer', value: 'Bank Transfer' },
    { label: 'Mobile Money', value: 'Mobile Money' }
  ];
  cardTypes: any[] = [
    { label: 'Visa', value: 'Visa' },
    { label: 'MasterCard', value: 'MasterCard' }
  ];
  mobileNetworks: any[] = [
    { label: 'Tigo Pesa', value: 'Tigo Pesa' },
    { label: 'Mpesa', value: 'Mpesa' },
    { label: 'TTCL', value: 'TTCL' }
  ];
  ;

  constructor(
    private router: Router ,// Inject the Router service
    private fb: FormBuilder,
    private functions: FunctionsService,
    private dataservices: DataService
  ) {}

  ngOnInit(): void {
    this.customerForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, this.phoneNumberValidator()]],
      address: [''],
      paymentMethod: ['', Validators.required],
      cardNumber: [''],
      cardType: [''],
      billingAddress: [''],
      expiryDate: [''],
      bankName: [''],
      bankAccountNumber: [''],
      bankAccountHolder: [''],
      mobileNetwork: [''],
      mobileNumber: [''],
      profileImage: [null], // Profile image field
      password: ['', [Validators.required, Validators.minLength(8)]], // Password field
      confirmPassword: ['', Validators.required] // Confirm password field
    }, { validators: this.passwordMatchValidator });

    // Listen for changes in the payment method
    this.customerForm.get('paymentMethod')?.valueChanges.subscribe(paymentMethod => {
      this.updatePaymentValidators(paymentMethod);
    });
  }

  // Custom validator for password match
  passwordMatchValidator: ValidatorFn = (control: AbstractControl) => {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  };

  // Custom validator for phone number
  phoneNumberValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const valid = /^\+?[0-9]+$/.test(control.value);
      return valid ? null : { invalidPhoneNumber: { value: control.value } };
    };
  }

  // Handle file upload
  onFileUpload(event: any) {
    const file = event.files[0];
    if (file) {
      this.customerForm.get('profileImage')?.setValue(file); // Update the form control value
      this.functions.displayInfo('Image File Uploaded');
    }
  }

  // Update validators based on payment method
  updatePaymentValidators(paymentMethod: string): void {
    if (paymentMethod === 'Credit Card') {
      this.customerForm.get('cardNumber')?.setValidators([Validators.required]);
      this.customerForm.get('cardType')?.setValidators([Validators.required]);
      this.customerForm.get('billingAddress')?.setValidators([Validators.required]);
      this.customerForm.get('expiryDate')?.setValidators([Validators.required]);
      this.customerForm.get('bankName')?.clearValidators();
      this.customerForm.get('bankAccountNumber')?.clearValidators();
      this.customerForm.get('bankAccountHolder')?.clearValidators();
      this.customerForm.get('mobileNetwork')?.clearValidators();
      this.customerForm.get('mobileNumber')?.clearValidators();
    } else if (paymentMethod === 'Bank Transfer') {
      this.customerForm.get('bankName')?.setValidators([Validators.required]);
      this.customerForm.get('bankAccountNumber')?.setValidators([Validators.required]);
      this.customerForm.get('bankAccountHolder')?.setValidators([Validators.required]);
      this.customerForm.get('cardNumber')?.clearValidators();
      this.customerForm.get('cardType')?.clearValidators();
      this.customerForm.get('billingAddress')?.clearValidators();
      this.customerForm.get('expiryDate')?.clearValidators();
      this.customerForm.get('mobileNetwork')?.clearValidators();
      this.customerForm.get('mobileNumber')?.clearValidators();
    } else if (paymentMethod === 'Mobile Money') {
      this.customerForm.get('mobileNetwork')?.setValidators([Validators.required]);
      this.customerForm.get('mobileNumber')?.setValidators([Validators.required]);
      this.customerForm.get('cardNumber')?.clearValidators();
      this.customerForm.get('cardType')?.clearValidators();
      this.customerForm.get('billingAddress')?.clearValidators();
      this.customerForm.get('expiryDate')?.clearValidators();
      this.customerForm.get('bankName')?.clearValidators();
      this.customerForm.get('bankAccountNumber')?.clearValidators();
      this.customerForm.get('bankAccountHolder')?.clearValidators();
    } else {
      // Reset all validators if no payment method is selected
      this.customerForm.get('cardNumber')?.clearValidators();
      this.customerForm.get('cardType')?.clearValidators();
      this.customerForm.get('billingAddress')?.clearValidators();
      this.customerForm.get('expiryDate')?.clearValidators();
      this.customerForm.get('bankName')?.clearValidators();
      this.customerForm.get('bankAccountNumber')?.clearValidators();
      this.customerForm.get('bankAccountHolder')?.clearValidators();
      this.customerForm.get('mobileNetwork')?.clearValidators();
      this.customerForm.get('mobileNumber')?.clearValidators();
    }

    // Update the validity of the form controls
    this.customerForm.get('cardNumber')?.updateValueAndValidity();
    this.customerForm.get('cardType')?.updateValueAndValidity();
    this.customerForm.get('billingAddress')?.updateValueAndValidity();
    this.customerForm.get('expiryDate')?.updateValueAndValidity();
    this.customerForm.get('bankName')?.updateValueAndValidity();
    this.customerForm.get('bankAccountNumber')?.updateValueAndValidity();
    this.customerForm.get('bankAccountHolder')?.updateValueAndValidity();
    this.customerForm.get('mobileNetwork')?.updateValueAndValidity();
    this.customerForm.get('mobileNumber')?.updateValueAndValidity();
  }

  onSubmit(): void {
    if (this.customerForm.valid) {
      // Prepare the form data
      const formData = new FormData();
      formData.append('FullName', this.customerForm.value.fullName);
      formData.append('Email', this.customerForm.value.email);
      formData.append('Phone', this.customerForm.value.phone);
      formData.append('Address', this.customerForm.value.address);
      formData.append('PaymentMethod', this.customerForm.value.paymentMethod);
      formData.append('CardNumber', this.customerForm.value.cardNumber);
      formData.append('CardType', this.customerForm.value.cardType);
      formData.append('BillingAddress', this.customerForm.value.billingAddress);
      formData.append('ExpiryDate', this.customerForm.value.expiryDate);
      formData.append('BankName', this.customerForm.value.bankName);
      formData.append('BankAccountNumber', this.customerForm.value.bankAccountNumber);
      formData.append('BankAccountHolder', this.customerForm.value.bankAccountHolder);
      formData.append('MobileNetwork', this.customerForm.value.mobileNetwork);
      formData.append('MobileNumber', this.customerForm.value.mobileNumber);
      formData.append('Password', this.customerForm.value.password);
      formData.append('ConfirmPassword', this.customerForm.value.confirmPassword);
  
      // Append the profile image file
      if (this.customerForm.value.profileImage) {
        formData.append('ProfileImage', this.customerForm.value.profileImage);
      }
  
      // Append company IDs (if any)
      if (this.customerForm.value.companies) {
        this.customerForm.value.companies.forEach((companyId: string) => {
          formData.append('Companies', companyId);
        });
      }
  
      // Send the form data to the backend
      this.dataservices.registerCustomer<Customer>('RegisterCustomer', formData).subscribe({
        next: (response) => {
          console.log('Customer registered successfully:', response);
          this.router.navigate(['/account/login']);  
                  this.functions.displaySuccess('Customer registered successfully! \n Please Login To Continue');
        },
        error: (error) => {
          console.error('Error registering customer:', error);
          this.functions.displayError('Failed to register customer. Please try again.');
        }
      });
    } else {
      this.functions.displayError('Please fill out the form correctly.');
    }
  }
}