import { Routes } from '@angular/router';
import { NewPaymentComponent } from './new-payment/new-payment.component';
import { PaymentsWithDetectionsComponent } from './payments-with-detections/payments-with-detections.component';
import { PaymentsComponent } from './payments/payments.component';
import { WelcomeComponent } from './welcome/welcome.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', component: WelcomeComponent },
  { path: 'payments', component: PaymentsComponent },
  { path: 'new-payment', component: NewPaymentComponent },
  { path: 'payments-with-detections', component: PaymentsWithDetectionsComponent },
];
