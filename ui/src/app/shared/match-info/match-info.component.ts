import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Match } from '@core/models/match.model';

@Component({
  selector: 'app-match-info',
  standalone: true,
  imports: [MatCardModule, DatePipe],
  templateUrl: './match-info.component.html',
  styleUrl: './match-info.component.css',
})
export class MatchInfoComponent {
  @Input({ required: true }) match!: Match;
}
