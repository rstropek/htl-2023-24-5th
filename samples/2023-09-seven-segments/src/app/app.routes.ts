import { Routes } from '@angular/router';
import { SevenSegmentNumberComponent } from './seven-segment-number/seven-segment-number.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', component: SevenSegmentNumberComponent },
];
