import { Routes } from '@angular/router';
import { TeamComponent } from '@features/team/team.component';
import { LandingComponent } from '@features/landing/landing.component';
import { PredictionsComponent } from '@features/predictions/predictions.component';

export const routes: Routes = [
  { path: '', component: LandingComponent, title: 'Leaderboard' },
  { path: 'team', component: TeamComponent, title: 'Team' },
  { path: 'predictions', component: PredictionsComponent, title: 'Predictions' },
];
