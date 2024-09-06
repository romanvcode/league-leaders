import {
  Component,
  DestroyRef,
  inject,
  OnDestroy,
  OnInit,
  signal,
} from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { ApiService } from '@core/services/api.service';
import { Standing } from '@core/models/standing.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-standing-table',
  standalone: true,
  imports: [MatTableModule, MatCardModule, MatProgressSpinner],
  templateUrl: './standing-table.component.html',
  styleUrl: './standing-table.component.css',
})
export class StandingTableComponent implements OnInit, OnDestroy {
  standings: Standing[] = [];
  isFething = signal(false);
  error = signal('');
  subscription = signal<Subscription | undefined>(undefined);

  constructor(private apiService: ApiService) {}

  displayedColumns: string[] = [
    'place',
    'team',
    'matchesPlayed',
    'wins',
    'draws',
    'losses',
    'goalsFor',
    'goalsAgainst',
    'points',
  ];

  ngOnInit(): void {
    this.isFething.set(true);

    this.subscription.set(
      this.apiService.getStandings().subscribe({
        next: (standings) => {
          this.standings = standings;
          console.log(standings);
        },
        error: (error) => {
          console.error(error);
          this.error.set('An error occurred while fetching standings');
        },
        complete: () => {
          this.isFething.set(false);
        },
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription()?.unsubscribe();
  }
}
