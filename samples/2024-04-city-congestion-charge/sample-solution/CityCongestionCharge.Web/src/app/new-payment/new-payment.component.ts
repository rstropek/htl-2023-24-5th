import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { PaymentAddDto, PaymentType } from '../model';
import { FormsModule } from '@angular/forms';
import { BASE_URL } from '../app.config';

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

  save() {
    if (this.payment.licensePlate) {
      // Note: Students need to show that they know how to post data to RESTful api.
      this.http
        .post(`${this.apiBaseUrl}/api/payments`, this.payment)
        .subscribe(() => this.router.navigateByUrl('/payments'));
    }
  }
}
