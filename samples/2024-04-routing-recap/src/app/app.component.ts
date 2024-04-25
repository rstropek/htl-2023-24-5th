import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';

type Customer = {
  name: string;
  age: number;
};

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'NewSyntax';

  numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
  customers = [
    { name: 'John Doe', age: 30 },
    { name: 'Jane Doe', age: 25 },
    { name: 'Jim Doe', age: 40 }
  ];
}
