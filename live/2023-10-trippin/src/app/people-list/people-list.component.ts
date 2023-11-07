import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, startWith, switchMap, tap } from 'rxjs';
import { TrippinService } from '../trippin.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-people-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './people-list.component.html',
  styleUrls: ['./people-list.component.scss']
})
export class PeopleListComponent {
  public nameFilter = new FormControl('');

  public input$ = this.nameFilter.valueChanges.pipe(
    startWith(''),
    debounceTime(1000),
    distinctUntilChanged(),
    switchMap(name => this.trippin.getPeople(name ?? ''))
  );

  constructor(private trippin: TrippinService) { }
}
