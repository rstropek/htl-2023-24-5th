import { Routes } from '@angular/router';
import { PeopleListComponent } from './people-list/people-list.component';
import { TripListComponent } from './trip-list/trip-list.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'people' },
  { path: 'people', component: PeopleListComponent },
  { path: 'people/:userName/trips', component: TripListComponent },
];
