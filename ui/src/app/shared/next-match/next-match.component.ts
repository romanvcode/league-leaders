import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-next-match',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './next-match.component.html',
  styleUrl: './next-match.component.css',
})
export class NextMatchComponent {}
