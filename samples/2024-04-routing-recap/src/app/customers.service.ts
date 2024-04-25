import { Injectable } from '@angular/core';
import { Customer } from './model';

@Injectable({
  providedIn: 'root'
})
export class CustomersService {
  public customers: Customer[] = [
    { firstName: 'John', lastName: 'Doe', company: 'Acme' },
    { firstName: 'Jane', lastName: 'Smith', company: 'Acme' },
    { firstName: 'Jim', lastName: 'Clarkson', company: 'Acme' }
  ];

  constructor() { }
}
