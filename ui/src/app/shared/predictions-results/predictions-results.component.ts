import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ApiService } from '@core/services/api.service';
import { Prediction } from '@core/models/prediction.model';
import { PredictionInfoComponent } from '@shared/prediction-info/prediction-info.component';

@Component({
  selector: 'app-predictions-results',
  standalone: true,
  imports: [PredictionInfoComponent],
  templateUrl: './predictions-results.component.html',
  styleUrl: './predictions-results.component.css',
})
export class PredictionsResultsComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  predictions: Prediction[] = [];
  error: string | null = null;
  isError = false;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.apiService
      .getPredictions()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (prediction) => {
          this.predictions = prediction;
        },
        error: (err) => {
          this.error = err;
          this.isError = true;
        },
      });
  }

  onPredictionDeleted() {
    this.ngOnInit();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
