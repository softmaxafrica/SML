// src/app/app-routing.module.ts

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Import your components here
import { DashboardComponent } from './home/dashboard/dashboard.component';
import { RequestComponent } from './home/request/request.component';
import { LoginComponent } from './home/account/login/login.component';
import { LandingComponent } from './home/landing/landing.component';
import { AuthService } from './auth/auth.service';
import { RegistrationComponent } from './home/account/registration/registration.component';
import { InvoiceComponent } from './home/billing/invoice/invoice.component';
// import { LiveTrackingComponent } from './home/live_track/live-tracking.component';
 import { DriversComponent } from './home/drivers/drivers.component';
// import { DriversRegistrationComponent } from './home/drivers/registration/drivers-registration.component';
// import { DriversVettingComponent } from './home/drivers/vetting/drivers-vetting.component';
// import { CustomersComponent } from './home/customers/customers.component';
// import { TrucksComponent } from './home/trucks/trucks.component';
// import { MessagesComponent } from './messages/messages.component';
// import { SettingsComponent } from './settings/settings.component';
 import { PaymentsComponent } from './home/billing/payments/payments.component';
import { TrucksComponent } from './home/trucks/trucks.component';
import { CustomerRegistrationComponent } from './home/account/custome-registration/custome-registration.component';
import { CustomerRequestComponent } from './home/customer/request/customer-request.component';
// import { LoginComponent } from './account/login/login.component';
// import { RegistrationComponent } from './account/registration/registration.component';
  

export const routes: Routes = [
  // { path: '', redirectTo: '/home/dashboard', pathMatch: 'full' },
  { path: '', redirectTo: 'home/landing', pathMatch: 'full' },
  { path: 'home/landing', component: LandingComponent, canActivate: [AuthService] },
   { path: 'home/dashboard',component: DashboardComponent, canActivate: [AuthService], data: { breadcrumb: 'Dashboard' } },
  { path: 'home/request', component: RequestComponent, canActivate: [AuthService], data: { breadcrumb: 'Requests' } },
  { path: 'home/customer/requests', component: CustomerRequestComponent, canActivate: [AuthService], data: { breadcrumb: 'Requests' } },

  { path: 'home/trucks', component: TrucksComponent, canActivate: [AuthService], data: { breadcrumb: 'Trucks' } },
  { path: 'home/drivers', component: DriversComponent, canActivate: [AuthService], data: { breadcrumb: 'Drivers' } },

  { path: 'home/billing/invoices', component: InvoiceComponent, canActivate: [AuthService], data: { breadcrumb: 'Invoices' } },
  { path: 'home/billing/payments', component: PaymentsComponent, canActivate: [AuthService], data: { breadcrumb: 'Payments' } },


  { path: 'account/login', component: LoginComponent },
  { path: 'account/registration', component: RegistrationComponent },
  { path: 'account/registration', component: RegistrationComponent },

  { path: 'account/customer-registration', component: CustomerRegistrationComponent }

];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}

