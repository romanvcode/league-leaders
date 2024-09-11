import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { Team } from '@core/models/team.model';

@Component({
  selector: 'app-team-info',
  standalone: true,
  imports: [MatCardModule, MatListModule],
  templateUrl: './team-info.component.html',
  styleUrl: './team-info.component.css',
})
export class TeamInfoComponent {
  @Input({ required: true }) team!: Team;
}
