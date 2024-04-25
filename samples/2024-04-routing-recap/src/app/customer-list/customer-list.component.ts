import { Component } from '@angular/core';
import { CustomersService } from '../customers.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.css'
})
export class CustomerListComponent {
  constructor(public customers: CustomersService) {}
}
