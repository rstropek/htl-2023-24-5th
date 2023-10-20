import { Component, Input, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegularDigitComponent } from '../regular-digit/regular-digit.component';

@Component({
  selector: 'app-regular-number',
  standalone: true,
  imports: [CommonModule, RegularDigitComponent],
  templateUrl: './regular-number.component.html',
  styleUrls: ['./regular-number.component.scss']
})
export class RegularNumberComponent {
  _number = signal(0);
  @Input() set number(value: number) {
    this._number.set(value);
  }

  private getDigit(index: number): number {
    if (this._number() / Math.pow(10, index) < 1) {
      return -1;
    }
    
    return Math.floor(this._number() / Math.pow(10, index)) % 10;
  }

  first = computed(() => this._number() % 10);
  second = computed(() => this.getDigit(1));
  third = computed(() => this.getDigit(2));
  fourth = computed(() => this.getDigit(3));
}
