import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrippinService } from '../trippin.service';
import {
  Observable,
  catchError,
  debounceTime,
  distinctUntilChanged,
  of,
  shareReplay,
  startWith,
  switchMap,
  tap,
} from 'rxjs';
import { Person } from '../models/TrippinModel';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-people-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './people-list.component.html',
  styleUrls: ['./people-list.component.scss'],
})
export class PeopleListComponent implements OnInit {
  public nameFilter = new FormControl('');
  public data$?: Observable<{ value: Person[] }>;
  public httpError?: Error;

  constructor(private trippin: TrippinService) {}

  ngOnInit() {
    this.data$ = this.nameFilter.valueChanges.pipe(
      tap(() => delete this.httpError),
      startWith(''),
      debounceTime(250),
      distinctUntilChanged(),
      switchMap((nameFilter) => this.trippin.getPeople(nameFilter ?? '')),
      shareReplay(1),
      catchError((err: Error) => {
        this.httpError = err;
        return of({ value: [] });
      })
    );
  }
}
