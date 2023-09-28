import { Component, Input, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sixteen-segment-char',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sixteen-segment-char.component.html',
  styleUrls: ['./sixteen-segment-char.component.scss']
})
export class SixteenSegmentCharComponent {
  private _char = signal('0');

  @Input() set char(char: string) {
    this._char.set(char);
  }
  
  // The 16 segments of a sixteen segments display:
  // https://en.wikipedia.org/wiki/Sixteen-segment_display#/media/File:16-segmente.png
  public a1 = computed(() => ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'I', 'O', 'P', 'Q', 'R', 'S', 'T', 'Z', 'g', 'i', 'p', 'q', '0', '2', '3', '5', '6', '7', '8', '9', '@'].includes(this._char()));
  public a2 = computed(() => ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'I', 'O', 'P', 'Q', 'R', 'S', 'T', 'Z', 'f', '0', '2', '3', '5', '6', '7', '8', '9', '@'].includes(this._char()));
  public b = computed(() => ['A', 'B', 'D', 'H', 'J', 'M', 'N', 'O', 'P', 'Q', 'R', 'U', 'W', '0', '2', '3', '4', '7', '8', '9', '@', '#'].includes(this._char()));
  public c = computed(() => ['A', 'B', 'D', 'G', 'H', 'J', 'M', 'N', 'O', 'Q', 'S', 'U', 'W', 'm', 'w', '0', '3', '4', '5', '6', '7', '8', '9', '#', '%'].includes(this._char()));
  public d1 = computed(() => ['B', 'C', 'D', 'E', 'G', 'I', 'J', 'L', 'O', 'Q', 'S', 'U', 'Z', 'a', 'b', 'c', 'd', 'e', 'g', 'j', 'l', 'o', 's', 't', 'u', 'y', 'z', '0', '2', '3', '5', '6', '8', '9', '@', '#', '_', '='].includes(this._char()));
  public d2 = computed(() => ['B', 'C', 'D', 'E', 'G', 'I', 'J', 'L', 'O', 'Q', 'S', 'U', 'Z', 'a', 's', '0', '2', '3', '5', '6', '8', '9', '@', '#', '_', '='].includes(this._char()));
  public e = computed(() => ['A', 'C', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'U', 'V', 'W', 'a', 'b', 'c', 'd', 'e', 'h', 'k', 'l', 'm', 'n', 'o', 'p', 'r', 't', 'u', 'v', 'w', '0', '2', '6', '8', '@'].includes(this._char()));
  public f = computed(() => ['A', 'C', 'E', 'F', 'G', 'H', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'U', 'V', 'W', 'b', 'g', 'h', 'k', 'l', 'p', 'q', 't', 'y', '0', '4', '5', '6', '8', '9', '@', '%', '"'].includes(this._char()));
  public g1 = computed(() => ['A', 'E', 'F', 'H', 'K', 'P', 'R', 'S', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'k', 'm', 'n', 'o', 'p', 'q', 'r', 't', 'y', 'z', '2', '4', '5', '6', '8', '9', '#', '%', '-', '=', '+'].includes(this._char()));
  public g2 = computed(() => ['A', 'B', 'G', 'H', 'P', 'R', 'S', 'f', 'k', 'm', 's', '2', '3', '4', '5', '6', '8', '9', '@', '#', '%', '-', '=', '+'].includes(this._char()));
  public h = computed(() => ['M', 'N', 'S', 'X', 'Y', 'x', '%', '*', ')'].includes(this._char()));
  public i = computed(() => ['B', 'D', 'I', 'T', 'd', 'f', 'g', 'j', 'p', 'q', 'y', '1', '@', '#', '*', '+', '\'', '"'].includes(this._char()));
  public j = computed(() => ['K', 'M', 'V', 'X', 'Y', 'Z', 'x', '0', '%', '*', '(', '/'].includes(this._char()));
  public k = computed(() => ['V', 'W', 'X', 'Z', 'e', 'v', 'w', 'x', 'z', '0', '%', '*', ')', ',', '/'].includes(this._char()));
  public l = computed(() => ['B', 'D', 'I', 'T', 'Y', 'a', 'b', 'd', 'f', 'g', 'h', 'i', 'j', 'm', 'n', 'o', 'q', 'u', 'y', '1', '#', '*', '+', '.'].includes(this._char()));
  public m = computed(() => ['K', 'N', 'Q', 'R', 'S', 'W', 'X', 'k', 's', 'w', 'x', '%', '*', '('].includes(this._char()));
}
