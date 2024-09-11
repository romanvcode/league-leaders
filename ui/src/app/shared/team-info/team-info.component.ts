import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { Team } from '@core/models/team.model';
import { ApiService } from '@core/services/api.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-team-info',
  standalone: true,
  imports: [MatCardModule, MatListModule],
  templateUrl: './team-info.component.html',
  styleUrl: './team-info.component.css',
})
export class TeamInfoComponent {
  private destroy$ = new Subject<void>();

  @Input({ required: true }) teamId!: number;

  public team: Team | null = null;
  public error: string | null = null;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.apiService
      .getTeam(this.teamId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (team) => {
          this.team = team;
        },
        error: (error) => {
          console.error('Failed to load team', error);
          this.error = 'An error occurred while fetching the team';
        },
      });
  }
}
