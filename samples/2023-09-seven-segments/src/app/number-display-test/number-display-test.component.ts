import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RegularNumberComponent } from '../regular-number/regular-number.component';
import { SevenSegmentNumberComponent } from '../seven-segment-number/seven-segment-number.component';
import { SevenSegmentNumberComponentLevel2 } from '../seven-segment-number-level2/seven-segment-number-level2.component';
import { SevenSegmentNumberComponentLevel3 } from '../seven-segment-number-level3/seven-segment-number-level3.component';

/**
 * Component used to test the RegularNumberComponent and the SevenSegmentNumberComponent.
 */
@Component({
  selector: 'app-number-display-test',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RegularNumberComponent,
    SevenSegmentNumberComponent,
    SevenSegmentNumberComponentLevel2,
    SevenSegmentNumberComponentLevel3
  ],
  templateUrl: './number-display-test.component.html',
  styleUrls: ['./number-display-test.component.scss'],
})
export class NumberDisplayTestComponent {
  // FormControl is a fundamental building block of Angular forms. It tracks the value and
  // validation status of an individual form control, such as an input field. It can be used
  // standalone (as shown here) or as part of a larger form structure like FormGroup or FormArray.
  // With the FormControl, you can set default values, listen for changes (as shown here),
  // and apply validation rules.
  //
  // Note that we need to import ReactiveFormsModule in order to have FormControl available.
  //
  // For details see https://angular.io/api/forms/FormControl.
  numberInput = new FormControl(0);
  numberOfDigitsInput = new FormControl(4);
  precision = new FormControl(2);
}
