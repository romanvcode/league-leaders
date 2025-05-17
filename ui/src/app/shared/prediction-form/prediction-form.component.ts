import { NgIf } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Match } from '@core/models/match.model';
import { ApiService } from '@core/services/api.service';
import { Subject, takeUntil } from 'rxjs';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-prediction-form',
  standalone: true,
  imports: [
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatLabel,
    MatButtonModule,
    FormsModule,
    NgIf,
    MatProgressSpinner,
  ],
  templateUrl: './prediction-form.component.html',
  styleUrl: './prediction-form.component.css',
})
export class PredictionFormComponent {
  private destroy$ = new Subject<void>();

  matchId: number;
  homeTeamScore: number;
  awayTeamScore: number;

  errorMessage: string | null = null;
  isError = false;
  isGenerating = false;

  constructor(
    private apiService: ApiService,
    private snackbarService: MatSnackBar,
    private router: Router,
    public dialogRef: MatDialogRef<PredictionFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Match
  ) {
    this.matchId = data.id;
    this.homeTeamScore = data.homeTeamScore;
    this.awayTeamScore = data.awayTeamScore;
  }

  onSubmit(predicted = true): void {
    if (!this.isValidForm()) return;

    this.isGenerating = true;

    this.apiService
      .createPrediction(this.matchId, this.homeTeamScore, this.awayTeamScore, predicted)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.isGenerating = false;
          this.dialogRef.close();
          this.router.navigate(['/predictions']).then(() => {
            this.snackbarService.open('Prediction saved', 'Close', {
              duration: 2000,
              panelClass: ['success-snackbar'],
              verticalPosition: 'bottom',
              horizontalPosition: 'right',
              data: {
                message: 'Prediction saved',
              },
            })
          })
        },
        error: (error) => {
          this.isGenerating = false;
          this.errorMessage = error.error;
          this.isError = true;
        },
      });
  }

  isValidForm(): boolean {
    return this.homeTeamScore >= 0 && this.awayTeamScore >= 0;
  }

  generateWithAI(): void {
    this.onSubmit(false);
  }
}
