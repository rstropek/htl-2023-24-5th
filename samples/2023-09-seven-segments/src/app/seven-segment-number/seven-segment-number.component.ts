import { Component, Input, Signal, computed, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { SevenSegmentDigitComponent } from '../seven-segment-digit/seven-segment-digit.component';
import { FormControl, ReactiveFormsModule, RequiredValidator, Validators } from '@angular/forms';

@Component({
  selector: 'app-seven-segment-number',
  standalone: true,
  imports: [CommonModule, SevenSegmentDigitComponent, ReactiveFormsModule],
  templateUrl: './seven-segment-number.component.html',
  styleUrls: ['./seven-segment-number.component.scss'],
})
export class SevenSegmentNumberComponent {
  numberInput = new FormControl(0, { nonNullable: true });

  constructor() {
    this._number = toSignal(this.numberInput.valueChanges, { initialValue: 0 });
  }

  _number: Signal<number>;
  first = computed(() => this._number() % 10);
  second = computed(() => this._number() / 10 < 1 ? -1 : Math.floor(this._number() / 10) % 10);
  third = computed(() => this._number() / 100 < 1 ? -1 : Math.floor(this._number() / 100) % 10);
  fourth = computed(() => this._number() / 1000 < 1 ? -1 : Math.floor(this._number() / 1000) % 10);
}
