import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Match } from '@core/models/match.model';
import { MatchDetailsModalComponent } from '@shared/match-details-modal/match-details-modal.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-match-info',
  standalone: true,
  imports: [MatCardModule, DatePipe],
  templateUrl: './match-info.component.html',
  styleUrl: './match-info.component.css',
})
export class MatchInfoComponent {
  @Input({ required: true }) match!: Match;
  
  constructor(private dialog: MatDialog) {
  }

  openMatchDetails(match: Match) {
    this.dialog.open(MatchDetailsModalComponent, {
      data: match,
      panelClass: 'rounded-lg',
      maxWidth: '90vw',
      width: '600px'
    });
  }
}
