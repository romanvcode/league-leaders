import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Standing } from '@core/models/standing.model';
import { ApiService } from '@core/services/api.service';
import { Subject, takeUntil } from 'rxjs';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-standing-table',
  standalone: true,
  imports: [MatTableModule, MatCardModule, MatProgressSpinner, RouterLink],
  templateUrl: './standing-table.component.html',
  styleUrl: './standing-table.component.css',
})
export class StandingTableComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  dataSource = new MatTableDataSource<Standing>();

  isFetching = false;

  error: string | null = null;
  isError = false;

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {}

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
    this.isFetching = true;

    this.apiService
      .getStandings()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (standings) => {
          this.dataSource.data = standings;
        },
        error: (error) => {
          console.error('Failed to load standings', error);
          this.error = 'An error occurred while fetching standings';
          this.isError = true;
        },
        complete: () => {
          this.isFetching = false;
        },
      });
  }

  onSelect(id: number): void {
    this.router.navigate(['/team'], { queryParams: { id } });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
