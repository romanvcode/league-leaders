import { Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { Prediction } from '@core/models/prediction.model';
import { MatCardModule } from '@angular/material/card';
import { NgClass } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIcon } from '@angular/material/icon';
import { ApiService } from '@core/services/api.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-prediction-info',
  standalone: true,
  imports: [MatCardModule, MatTooltipModule, NgClass, MatIcon],
  templateUrl: './prediction-info.component.html',
  styleUrl: './prediction-info.component.css',
})
export class PredictionInfoComponent implements OnDestroy {
  private destroy$ = new Subject<void>();
  
  @Input({ required: true }) prediction!: Prediction;
  
  @Output() predictionDeleted = new EventEmitter<void>();
  
  constructor(private service: ApiService) {}

  deletePrediction() {
    this.service
      .deletePrediction(this.prediction.id!)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.predictionDeleted.emit());
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
