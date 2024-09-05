import { Component, DestroyRef, inject, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { ApiService } from '@core/services/api.service';
import { Standing } from '@core/models/standing.model';

@Component({
  selector: 'app-standing-table',
  standalone: true,
  imports: [MatTableModule, MatCardModule, MatProgressSpinner],
  templateUrl: './standing-table.component.html',
  styleUrl: './standing-table.component.css',
})
export class StandingTableComponent {
  standings: Standing[] = [];
  isFething = signal(false);
  error = signal('');

  private apiService = inject(ApiService);
  private destroyRef = inject(DestroyRef);

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

    const subscription = this.apiService.getStandings().subscribe({
      next: (standings) => {
        this.standings = standings;
      },
      error: (error) => {
        console.error(error);
        this.error.set('An error occurred while fetching standings');
      },
      complete: () => {
        this.isFething.set(false);
      },
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
