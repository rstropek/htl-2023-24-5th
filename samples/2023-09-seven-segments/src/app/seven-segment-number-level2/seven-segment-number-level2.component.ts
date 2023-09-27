import { Component, Input, Signal, computed, effect, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SevenSegmentDigitComponent } from '../seven-segment-digit/seven-segment-digit.component';
import { ReactiveFormsModule } from '@angular/forms';

/**
 * Component used to display a number using {@link SevenSegmentDigitComponent}.
 *
 * Note that this is the advanced version of {@link SevenSegmentNumberComponent}.
 * It is not limited to displaying four digits. Instead, the number
 * of digits can be configured.
 */
@Component({
  selector: 'app-seven-segment-number-level2',
  standalone: true,
  imports: [CommonModule, SevenSegmentDigitComponent, ReactiveFormsModule],
  templateUrl: './seven-segment-number-level2.component.html',
  styleUrls: ['./seven-segment-number-level2.component.scss'],
})
export class SevenSegmentNumberComponentLevel2 {
  _number = signal(0);

  // We build a signal that returns an array with the length
  // corresponding to the currently configured number of digits.
  // We use the type `unknown` as the content of each array
  // element does not matter.
  _digits = signal<unknown[]>([0, 0, 0, 0]);

  // The following is just a helper to make our template
  // easier to read.
  _numberOfDigits = computed(() => this._digits().length);

  @Input() set number(value: number) {
    this._number.set(value);
  }

  @Input() set numberOfDigits(value: number) {
    // Build an array with the corresponding number of elements.
    let newDigits: unknown[] = [];
    for (let i = 0; i < value; i++) { newDigits.push(0); }

    // Set the signal
    this._digits.set(newDigits);
  }

  constructor() {
    // This is just for demonstration purposes. Sometime, you
    // need debug output whenever a signal changes. This is how
    // you implement that.
    effect(() => console.log(this._digits()));
  }

  public getDigit(index: number): Signal<number> {
    return computed(() => {
      // We have to compute the digit on the given number.
      let digit = this._number() / Math.pow(10, index);
      if (index > 0 && digit < 1) { return -1; }
      return Math.floor(digit) % 10;
    });
  }
}
