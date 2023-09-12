import { Component, Input, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: '[app-seven-segment-digit]',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './seven-segment-digit.component.html',
  styleUrls: ['./seven-segment-digit.component.scss']
})
export class SevenSegmentDigitComponent {
  _digit = signal<number>(-1);
  a = computed(() => [0, 2, 3, 5, 6, 7, 8, 9].includes(this._digit()));
  b = computed(() => [0, 1, 2, 3, 4, 7, 8, 9].includes(this._digit()));
  c = computed(() => [0, 1, 3, 4, 5, 6, 7, 8, 9].includes(this._digit()));
  d = computed(() => [0, 2, 3, 5, 6, 8, 9].includes(this._digit()));
  e = computed(() => [0, 2, 6, 8].includes(this._digit()));
  f = computed(() => [0, 4, 5, 6, 8, 9].includes(this._digit()));
  g = computed(() => [2, 3, 4, 5, 6, 8, 9].includes(this._digit()));

  @Input() set digit(value: number) {
    // Note that this will become easier in the future.
    // See also https://github.com/angular/angular/discussions/49682.
    this._digit.set(value);
  }
}
