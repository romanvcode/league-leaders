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

  constructor(
    private apiService: ApiService,
    public dialogRef: MatDialogRef<PredictionFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Match
  ) {
    this.matchId = data.id;
    this.homeTeamScore = data.homeTeamScore;
    this.awayTeamScore = data.awayTeamScore;
  }

  onSubmit() {
    if (this.isValidForm()) {
      this.apiService
        .createPrediction(this.matchId, this.homeTeamScore, this.awayTeamScore)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.dialogRef.close();
          },
          error: (error) => {
            this.errorMessage = error.error;
            this.isError = true;
          },
        });
    }
  }

  isValidForm(): boolean {
    return this.homeTeamScore >= 0 && this.awayTeamScore >= 0;
  }
}
