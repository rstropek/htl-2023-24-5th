import { HttpClient, HttpClientModule, HttpParams } from "@angular/common/http";
import { Component, Inject, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { PaymentResultDto, PaymentType } from "../model";
import { BASE_URL } from "../app.config";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";

@Component({
  standalone: true,
  selector: "app-payments",
  templateUrl: "./payments.component.html",
  styleUrls: ["./payments.component.css"],
  imports: [HttpClientModule, FormsModule, CommonModule]
})
export class PaymentsComponent implements OnInit {
  paymentType?: PaymentType = undefined;
  onlyFuturePayments: boolean = false;
  onlyAnonymous: boolean = false;

  payments?: Observable<PaymentResultDto[]>;

  constructor(private http: HttpClient, @Inject(BASE_URL) private apiBaseUrl: string) {}

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    let params = new HttpParams();
    if (this.paymentType !== undefined) {
      params = params.append("type", this.paymentType);
    }

    if (this.onlyFuturePayments) {
      params = params.append("future", this.onlyFuturePayments);
    }

    if (this.onlyAnonymous) {
      params = params.append("anonym", this.onlyAnonymous);
    }

    const paramsString = params.toString();

    this.payments = this.http.get<PaymentResultDto[]>(`${this.apiBaseUrl}/api/payments${paramsString ? '?' : ''}${paramsString}`);
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
