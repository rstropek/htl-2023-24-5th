import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { PaymentWithDetectionDto, PaymentType } from '../model';
import { BASE_URL } from '../app.config';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-payments-with-detections',
  templateUrl: './payments-with-detections.component.html',
  styleUrls: ['./payments-with-detections.component.css'],
  imports: [HttpClientModule, CommonModule]
})
export class PaymentsWithDetectionsComponent implements OnInit {
  payments?: Observable<PaymentWithDetectionDto[]>;

  constructor(private http: HttpClient, @Inject(BASE_URL) private apiBaseUrl: string) { }

  ngOnInit(): void {
    this.payments = this.http.get<PaymentWithDetectionDto[]>(`${this.apiBaseUrl}/api/payments/with-detections`);
  }

  getPaymentTypeDescription(type: PaymentType): string {
    switch (type) {
      case PaymentType.Cash:
        return "Cash";
      case PaymentType.BankTransfer:
        return "Bank Transfer";
      case PaymentType.CreditCard:
        return "Creditcard";
      case PaymentType.DebitCard:
        return "Debitcard";
      default:
        return "unknown";
    }
  }
}
