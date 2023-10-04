import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TrippinService } from '../trippin.service';

@Component({
  selector: 'app-trip-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './trip-list.component.html',
  styleUrls: ['./trip-list.component.scss'],
})
export class TripListComponent implements AfterViewInit {
  constructor(private trippin: TrippinService) {}

  @Input() public userName?: string;

  ngAfterViewInit(): void {
    if (this.userName) {
      this.trippin.getTrips(this.userName).subscribe((trips) => {
        console.log(trips);
      });
    }
  }
}
