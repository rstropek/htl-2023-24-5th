import { Component, Input, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SevenSegmentDigitComponent } from '../seven-segment-digit/seven-segment-digit.component';
import { ReactiveFormsModule } from '@angular/forms';

/**
 * Component used to display a number between 0 and 9999 using four {@link SevenSegmentDigitComponent}.
 */
@Component({
  selector: 'app-seven-segment-number',
  standalone: true,
  imports: [CommonModule, SevenSegmentDigitComponent, ReactiveFormsModule],
  templateUrl: './seven-segment-number.component.html',
  styleUrls: ['./seven-segment-number.component.scss'],
})
export class SevenSegmentNumberComponent {
  _number = signal(0);
  @Input() set number(value: number) {
    this._number.set(value);
  }

  private getDigit(index: number): number {
    return this._number() / Math.pow(10, index) < 1 ? -1 : Math.floor(this._number() / Math.pow(10, index)) % 10;
  }

  first = computed(() => this._number() % 10);
  second = computed(() => this.getDigit(1));
  third = computed(() => this.getDigit(2));
  fourth = computed(() => this.getDigit(3));
}
