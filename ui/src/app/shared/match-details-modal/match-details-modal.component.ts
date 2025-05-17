import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Match } from '@core/models/match.model';
import { ApiService } from '@core/services/api.service';
import { Subject, takeUntil } from 'rxjs';
import { TeamStat } from '@core/models/team-stat.model';
import { DatePipe } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-match-details-modal',
  standalone: true,
  imports: [
    DatePipe,
    MatDialogModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './match-details-modal.component.html',
  styleUrl: './match-details-modal.component.css',
})
export class MatchDetailsModalComponent implements OnInit, OnDestroy {
  stats: TeamStat[] = [];
  error: string | null = null;
  isError = false;
  private destroy$ = new Subject<void>();

  constructor(
    private apiService: ApiService,
    public dialogRef: MatDialogRef<MatchDetailsModalComponent>,
    @Inject(MAT_DIALOG_DATA) public match: Match
  ) {}

  ngOnInit(): void {
    this.apiService
      .getTeamsStats(this.match.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (teamsStats) => {
          this.stats = teamsStats;
        },
        error: (err) => {
          this.error = err;
          this.isError = true;
        },
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}

