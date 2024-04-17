import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { PaymentAddDto, PaymentType } from '../model';
import { FormsModule } from '@angular/forms';
import { BASE_URL } from '../app.config';

// Todo: Complete the component logi

@Component({
  standalone: true,
  selector: 'app-new-payment',
  templateUrl: './new-payment.component.html',
  styleUrls: ['./new-payment.component.css'],
  imports: [HttpClientModule, FormsModule],
})
export class NewPaymentComponent {
  payment: PaymentAddDto = {
    licensePlate: '',
    paidAmount: 0,
    payingPerson: '',
    paymentType: PaymentType.Cash,
  };

  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(BASE_URL) private apiBaseUrl: string
  ) {}
}
