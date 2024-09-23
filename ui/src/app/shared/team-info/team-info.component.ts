import { DatePipe } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatListModule } from '@angular/material/list';
import { Match } from '@core/models/match.model';
import { Team } from '@core/models/team.model';
import { ApiService } from '@core/services/api.service';
import { Subject, takeUntil } from 'rxjs';
import { MatchInfoComponent } from '../match-info/match-info.component';

@Component({
  selector: 'app-team-info',
  standalone: true,
  imports: [
    MatCardModule,
    MatListModule,
    MatGridListModule,
    DatePipe,
    MatchInfoComponent,
  ],
  templateUrl: './team-info.component.html',
  styleUrl: './team-info.component.css',
})
export class TeamInfoComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  @Input({ required: true }) teamId!: number;

  team: Team | null = null;
  matches: Match[] = [];
  error: string | null = null;
  isError = false;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.apiService
      .getTeam(this.teamId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (team) => {
          this.team = team;

          this.apiService.getTeamMatches(this.team.id).subscribe({
            next: (matches) => {
              this.matches = matches;
            },
            error: (error) => {
              console.error('Failed to load team matches', error);
              this.error = 'An error occurred while fetching the team matches';
              this.isError = true;
            },
          });
        },
        error: (error) => {
          console.error('Failed to load team', error);
          this.error = 'An error occurred while fetching the team';
          this.isError = true;
        },
      });
  }

  ngOnChanges(): void {
    this.ngOnInit();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
