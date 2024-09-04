import { Component } from '@angular/core';
import { MatTableModule } from '@angular/material/table';

export interface Standing {
  position: number; 
  team: string;
  played: number; 
  won: number;
  drawn: number;
  lost: number;
  points: number;
}

const STANDING_DATA: Standing[] = [
  {position: 1, team: 'Liverpool', played: 38, won: 32, drawn: 3, lost: 3, points: 99},
  {position: 2, team: 'Manchester City', played: 38, won: 26, drawn: 3, lost: 9, points: 81},
  {position: 3, team: 'Manchester United', played: 38, won: 18, drawn: 12, lost: 8, points: 66},
  {position: 4, team: 'Chelsea', played: 38, won: 20, drawn: 6, lost: 12, points: 66},
  {position: 5, team: 'Leicester City', played: 38, won: 18, drawn: 8, lost: 12, points: 62},
  {position: 6, team: 'Tottenham Hotspur', played: 38, won: 16, drawn: 11, lost: 11, points: 59},
  {position: 7, team: 'Wolverhampton Wanderers', played: 38, won: 15, drawn: 14, lost: 9, points: 59},
  {position: 8, team: 'Arsenal', played: 38, won: 14, drawn: 14, lost: 10, points: 56},
]

@Component({
  selector: 'app-standing-table',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './standing-table.component.html',
  styleUrl: './standing-table.component.css'
})
export class StandingTableComponent {
  displayedColumns: string[] = ['position', 'team', 'played', 'won', 'drawn', 'lost', 'points'];
  dataSource = STANDING_DATA;
}
