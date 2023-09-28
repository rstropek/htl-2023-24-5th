import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SixteenSegmentCharComponent } from '../sixteen-segment-char/sixteen-segment-char.component';

@Component({
  selector: 'app-sixteen-segment-text',
  standalone: true,
  imports: [CommonModule, SixteenSegmentCharComponent],
  templateUrl: './sixteen-segment-text.component.html',
  styleUrls: ['./sixteen-segment-text.component.scss']
})
export class SixteenSegmentTextComponent {
  public _text = signal('');

  @Input() public set text(value: string) {
    this._text.set(value);
  }
}
