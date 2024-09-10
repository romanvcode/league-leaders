import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { Standing } from '@core/models/standing.model';
import { ApiService } from '@core/services/api.service';
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
  isFetching = signal(false);
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
    this.isFetching.set(true);

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
          this.isFetching.set(false);
        },
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription()?.unsubscribe();
  }
}
