import { Routes } from '@angular/router';
import { TeamComponent } from '@features/team/team.component';
import { LandingComponent } from '@features/landing/landing.component';
import { PredictionsComponent } from '@features/predictions/predictions.component';
import { MatchDetailsModalComponent } from '@shared/match-details-modal/match-details-modal.component';

export const routes: Routes = [
  { path: '', component: LandingComponent, title: 'Leaderboard' },
  { path: 'team', component: TeamComponent, title: 'Team' },
  { path: 'predictions', component: PredictionsComponent, title: 'Predictions' },
];
