import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Component used to display a single digit between 0 and 9.
 *
 * Note that this component is just here to prepare us for the more complex seven segments display.
 */
@Component({
  selector: 'app-regular-digit',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './regular-digit.component.html',
  styleUrls: ['./regular-digit.component.scss']
})
export class RegularDigitComponent {
  /** Signal for digit; -1 means that the signal should not be displayed */
  _digit = signal<number>(-1);

  @Input() set digit(value: number) {
    // Whenever the input of the component changes, we update the signal.
    // Note that this will become easier in the future.
    // See also https://github.com/angular/angular/discussions/49682.
    this._digit.set(value);
  }
}
