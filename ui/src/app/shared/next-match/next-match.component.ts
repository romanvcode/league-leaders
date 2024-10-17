import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { Match } from '@core/models/match.model';
import { ApiService } from '@core/services/api.service';
import { PredictionFormComponent } from '@shared/prediction-form/prediction-form.component';
import { interval, map, Observable, Subject, takeUntil } from 'rxjs';
import { NextMatchCountdown } from './next-match-countdown.model';

@Component({
  selector: 'app-next-match',
  standalone: true,
  imports: [MatCardModule, CommonModule],
  templateUrl: './next-match.component.html',
  styleUrl: './next-match.component.css',
})
export class NextMatchComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  nextMatch: Match | null = null;
  nextMatch$: Subject<Match> = new Subject<Match>();
  timeLeft$: Observable<string> = new Observable<string>();

  error: string | null = null;
  isError = false;

  constructor(
    private apiService: ApiService,
    private dialog: MatDialog
  ) {}

  onNextMatchClick(): void {
    this.dialog.open(PredictionFormComponent, {
      data: this.nextMatch,
    });
  }

  ngOnInit(): void {
    this.apiService
      .getMatches()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (matches) => {
          const nextMatch = matches[0];
          this.nextMatch$.next(nextMatch);
          this.nextMatch = nextMatch;

          const matchDateEpoch = new Date(nextMatch.date).getTime();

          this.timeLeft$ = interval(1000).pipe(
            map(() => {
              const countdown = this.tickTock(matchDateEpoch);
              return this.formatTimeLeft(countdown);
            })
          );
        },
        error: (error) => {
          console.error('Failed to load upcoming match', error);
          this.error = 'An error occurred while fetching the next match';
          this.isError = true;
        },
      });
  }

  private tickTock(nextMatchInEpoch: number): NextMatchCountdown {
    const diff = nextMatchInEpoch - Date.now();

    const daysToDday = Math.floor(diff / (1000 * 60 * 60 * 24));
    const hoursToDday = Math.floor((diff / (1000 * 60 * 60)) % 24);
    const minutesToDday = Math.floor((diff / (1000 * 60)) % 60);
    const secondsToDday = Math.floor(diff / 1000) % 60;

    return {
      seconds: secondsToDday,
      minutes: minutesToDday,
      hours: hoursToDday,
      days: daysToDday,
    };
  }

  private formatTimeLeft(countdown: NextMatchCountdown): string {
    const days = String(countdown.days);
    const hours = String(countdown.hours).padStart(2, '0');
    const minutes = String(countdown.minutes).padStart(2, '0');
    const seconds = String(countdown.seconds).padStart(2, '0');

    if (countdown.days > 1) {
      return `${days} days to go`;
    }

    return `${hours}h ${minutes}m ${seconds}s`;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
