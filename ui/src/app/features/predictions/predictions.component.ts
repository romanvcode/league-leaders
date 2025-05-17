import { Component } from '@angular/core';
import { NextMatchComponent } from '@shared/next-match/next-match.component';
import { SearchInputComponent } from '@shared/search-input/search-input.component';
import { PredictionsResultsComponent } from '@shared/predictions-results/predictions-results.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-predictions',
  standalone: true,
  imports: [
    NextMatchComponent,
    SearchInputComponent,
    PredictionsResultsComponent,
    RouterLink,
  ],
  templateUrl: './predictions.component.html',
  styleUrl: './predictions.component.css',
})
export class PredictionsComponent {
  currentFeature = 'Predictions';
}
