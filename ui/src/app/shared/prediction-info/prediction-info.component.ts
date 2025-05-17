import { Component, Input } from '@angular/core';
import { Prediction } from '@core/models/prediction.model';
import { MatCardModule } from '@angular/material/card';
import { NgClass } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-prediction-info',
  standalone: true,
  imports: [MatCardModule, MatTooltipModule, NgClass],
  templateUrl: './prediction-info.component.html',
  styleUrl: './prediction-info.component.css',
})
export class PredictionInfoComponent {
  @Input({ required: true }) prediction!: Prediction;
}
