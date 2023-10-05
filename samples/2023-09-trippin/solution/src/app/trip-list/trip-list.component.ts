import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TrippinService } from '../trippin.service';
import { Trip } from '../models/TrippinModel';
import { Observable, firstValueFrom } from 'rxjs';
import { extractError } from '../rxjsHelpers';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-trip-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './trip-list.component.html',
  styleUrls: ['./trip-list.component.scss'],
})
export class TripListComponent implements AfterViewInit {
  public data$?: Observable<{ value: Trip[] }>;
  public httpError$?: Observable<Error>;
  public tripForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    budget: new FormControl(1000, [Validators.required]),
    description: new FormControl('', [Validators.required]),
    tags: new FormControl(''),
    startsAt: new FormControl('', [Validators.required]),
    endsAt: new FormControl('', [Validators.required]),
  });

  constructor(private trippin: TrippinService) {}

  @Input() public userName?: string;

  ngAfterViewInit(): void {
    this.refresh();
  }

  refresh(): void {
    if (this.userName) {
      this.data$ = this.trippin.getTrips(this.userName);
      this.httpError$ = this.data$?.pipe(extractError());
    }
  }

  async addTrip() {
    if (this.userName) {
      const newTrip: Trip = {
        TripId: 9000 + Math.floor(Math.random() * 1000),
        ShareId: Guid.create().toString(),
        Name: this.tripForm.value.name ?? '',
        Budget: this.tripForm.value.budget ?? 0,
        Description: this.tripForm.value.description ?? '',
        Tags: (this.tripForm.value.tags ?? '').split(','),
        StartsAt: new Date(this.tripForm.value.startsAt ?? '').toISOString(),
        EndsAt: new Date(this.tripForm.value.endsAt ?? '').toISOString(),
      };
      for (let i = 0; i < 5; i++) {
        try {
          await firstValueFrom(this.trippin.addTrip(this.userName, newTrip));
          break;
        } catch (e) {
          newTrip.TripId = 9000 + Math.floor(Math.random() * 1000);
        }
      }

      this.refresh();
      this.tripForm.reset();
    }
  }
}
