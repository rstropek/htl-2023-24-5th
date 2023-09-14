import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferingsService } from '../offerings.service';
import { SevenSegmentNumberComponent } from '../seven-segment-number/seven-segment-number.component';

@Component({
  selector: 'app-cash-register',
  standalone: true,
  imports: [CommonModule, SevenSegmentNumberComponent],
  templateUrl: './cash-register.component.html',
  styleUrls: ['./cash-register.component.scss']
})
export class CashRegisterComponent {
  constructor(public offerings: OfferingsService) {}
}
