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
  ],
  templateUrl: './prediction-form.component.html',
  styleUrl: './prediction-form.component.css',
})
export class PredictionFormComponent {
  homeTeamScore: number;
  awayTeamScore: number;

  constructor(
    public dialogRef: MatDialogRef<PredictionFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Match
  ) {
    this.homeTeamScore = data.homeTeamScore;
    this.awayTeamScore = data.awayTeamScore;
  }

  onSubmit() {
    const prediction = {
      matchId: this.data.id,
      homeTeamScore: this.homeTeamScore,
      awayTeamScore: this.awayTeamScore,
    };
    // Call backend service to save prediction
    console.log('Prediction Submitted: ', prediction);
    this.dialogRef.close();
  }
}
