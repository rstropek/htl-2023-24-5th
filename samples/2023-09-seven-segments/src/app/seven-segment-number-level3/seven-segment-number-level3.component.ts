import { Component, Input, Signal, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SevenSegmentDigitComponent } from '../seven-segment-digit/seven-segment-digit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SevenSegmentNumberComponentLevel2 } from '../seven-segment-number-level2/seven-segment-number-level2.component';

/**
 * Component used to display a number using {@link SevenSegmentDigitComponent}.
 *
 * Note that this is the advanced version of {@link SevenSegmentNumberComponentLevel2}.
 * It offers a precision setting with which the number of digits after the
 * comma can be controlled.
 *
 * NOTE that this time we extend the level 2 version to demonstrate the possibility
 * to derive one Angular component from another. The derived components inherits the
 * functionality of the base component. In our case, this is exactly what we want
 * because we only want to add functionalty.
 */
@Component({
  selector: 'app-seven-segment-number-level3',
  standalone: true,
  imports: [CommonModule, SevenSegmentDigitComponent, ReactiveFormsModule],
  templateUrl: './seven-segment-number-level3.component.html',
  styleUrls: ['./seven-segment-number-level3.component.scss'],
})
export class SevenSegmentNumberComponentLevel3 extends SevenSegmentNumberComponentLevel2 {
  // The following signal controls the precision (i.e. the
  // number of digits after the comma).
  _precision = signal(2);

  @Input() set precision(value: number) {
    this._precision.set(value);
  }

  public override getDigit(index: number): Signal<number> {
    return computed(() => {
      let digit: number;
      if (index < this._precision()) {
        // Digits behind the comma
        digit = this._number() * Math.pow(10, this._precision() - index);
      }
      else
      {
        // Digits in front of the comma
        digit = this._number() / Math.pow(10, index - this._precision());
      }

      if (index > this._precision() && digit < 1) {
        // We display a zero only if we were asked for the
        // first digit. This is necessary to avoid printing "0001"
        // when the value is 1.

        return -1;
      }

      return Math.floor(digit) % 10;
    });
  }
}
