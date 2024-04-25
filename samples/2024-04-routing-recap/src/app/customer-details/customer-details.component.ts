import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Customer } from '../model';
import { CustomersService } from '../customers.service';

@Component({
  selector: 'app-customer-details',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './customer-details.component.html',
  styleUrl: './customer-details.component.css'
})
export class CustomerDetailsComponent {
  private customerLastName: string;

  public customer: Customer;

  constructor(private route: ActivatedRoute, private router: Router, private customers: CustomersService) {
    this.customerLastName = route.snapshot.params['lastName'];
    this.customer = customers.customers.find(c => c.lastName === this.customerLastName) ?? {} as Customer;
  }

  public onBack(): void {
    this.router.navigateByUrl('/customers');
  }
}
