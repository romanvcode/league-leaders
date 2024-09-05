import { HttpClient } from '@angular/common/http';
import { Component, DestroyRef, inject } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-standing-table',
  standalone: true,
  imports: [MatTableModule, MatCardModule],
  templateUrl: './standing-table.component.html',
  styleUrl: './standing-table.component.css',
})
export class StandingTableComponent {
  standings: any[] = [];
  displayedColumns: string[] = ['team', 'points', 'matchesPlayed'];

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.apiService.getStandings().subscribe((data) => {
      this.standings = data;
    });
  }
}
