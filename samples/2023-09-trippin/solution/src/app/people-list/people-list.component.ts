import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrippinService } from '../trippin.service';
import { switchMap } from 'rxjs';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { delayedSearch, extractError } from '../rxjsHelpers';

@Component({
  selector: 'app-people-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './people-list.component.html',
  styleUrls: ['./people-list.component.scss'],
})
export class PeopleListComponent {
  public nameFilter = new FormControl('');

  public data$? = this.nameFilter.valueChanges.pipe(
    delayedSearch(),
    switchMap((nameFilter: string | null) => this.trippin.getPeople(nameFilter ?? ''))
  );

  public httpError$? = this.data$?.pipe(extractError());

  constructor(private trippin: TrippinService) {}
}
