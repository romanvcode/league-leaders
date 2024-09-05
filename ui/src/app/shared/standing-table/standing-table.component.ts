import { Component } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { ApiService } from '../../core/services/api.service';
import { map } from 'rxjs';

@Component({
  selector: 'app-standing-table',
  standalone: true,
  imports: [MatTableModule, MatCardModule],
  templateUrl: './standing-table.component.html',
  styleUrl: './standing-table.component.css',
})
export class StandingTableComponent {
  standings: any[] = [];
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

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.apiService.getStandings().subscribe({
      next: (standings) => {
        this.standings = standings;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }
}
