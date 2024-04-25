import { Routes } from '@angular/router';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { CustomerDetailsComponent } from './customer-details/customer-details.component';
import { CustomerDetailsWithInputComponent } from './customer-details-with-input/customer-details-with-input.component';

export const routes: Routes = [
  { path: 'customers', component: CustomerListComponent },
  { path: 'customers/:lastName', component: CustomerDetailsComponent },
  { path: 'customers-with-input/:lastName', component: CustomerDetailsWithInputComponent },
];
