import { Component, Input, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegularDigitComponent } from '../regular-digit/regular-digit.component';
import { ReactiveFormsModule } from '@angular/forms';

/**
 * Component used to display a number between 0 and 9999 using four {@link RegularDigitComponent}.
 *
 * Note that this component is just here to prepare us for the more complex seven segments display.
 */
@Component({
  selector: 'app-regular-number',
  standalone: true,
  imports: [CommonModule, RegularDigitComponent, ReactiveFormsModule],
  templateUrl: './regular-number.component.html',
  styleUrls: ['./regular-number.component.scss']
})
export class RegularNumberComponent {
  _number = signal(0);
  @Input() set number(value: number) {
    // Whenever the input of the component changes, we update the signal.
    // Note that this will become easier in the future.
    // See also https://github.com/angular/angular/discussions/49682.
    this._number.set(value);
  }

  /**
   * Gets the digit on the given index (zero-based) from {@link _number}
   *
   * @returns -1 if the digit is not available (e.g. digit with index 2 of 12)
   */
  private getDigit(index: number): number {
    return this._number() / Math.pow(10, index) < 1 ? -1 : Math.floor(this._number() / Math.pow(10, index)) % 10;
  }

  first = computed(() => this._number() % 10);
  second = computed(() => this.getDigit(1));
  third = computed(() => this.getDigit(2));
  fourth = computed(() => this.getDigit(3));
}
