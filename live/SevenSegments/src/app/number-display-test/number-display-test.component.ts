import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegularNumberComponent } from '../regular-number/regular-number.component';
import { SevenSegmentNumberComponent } from '../seven-segment-number/seven-segment-number.component';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-number-display-test',
  standalone: true,
  imports: [CommonModule, RegularNumberComponent, SevenSegmentNumberComponent, ReactiveFormsModule],
  templateUrl: './number-display-test.component.html',
  styleUrls: ['./number-display-test.component.scss']
})
export class NumberDisplayTestComponent {
  numberInput = new FormControl(0);

}
