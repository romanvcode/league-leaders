import { Component } from '@angular/core';
import { NextMatchComponent } from '@shared/next-match/next-match.component';
import { SearchInputComponent } from '@shared/search-input/search-input.component';
import { TeamInfoComponent } from '@shared/team-info/team-info.component';
import { PredictionsResultsComponent } from '@shared/predictions-results/predictions-results.component';
import { StandingTableComponent } from '@shared/standing-table/standing-table.component';

@Component({
  selector: 'app-predictions',
  standalone: true,
  imports: [
    NextMatchComponent,
    SearchInputComponent,
    TeamInfoComponent,
    PredictionsResultsComponent,
    StandingTableComponent,
  ],
  templateUrl: './predictions.component.html',
  styleUrl: './predictions.component.css',
})
export class PredictionsComponent {
  currentFeature = 'Прогнози';
}
