import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { CustomersService } from '../customers.service';
import { Customer } from '../model';

@Component({
  selector: 'app-customer-details-with-input',
  standalone: true,
  imports: [],
  templateUrl: './customer-details-with-input.component.html',
  styleUrl: './customer-details-with-input.component.css'
})
export class CustomerDetailsWithInputComponent implements OnInit {
  @Input('lastName') public customerLastName: string = '';
  public customer?: Customer;

  constructor(private customers: CustomersService) {}

  ngOnInit(): void {
    this.customer = this.customers.customers.find(c => c.lastName === this.customerLastName);
  }
}
