import { Routes } from '@angular/router';
import { TeamComponent } from '@features/team/team.component';
import { LandingComponent } from './features/landing/landing.component';

export const routes: Routes = [
  { path: '', redirectTo: 'leaderboard', pathMatch: 'full' },
  { path: 'leaderboard', component: LandingComponent, title: 'Leaderboard' },
  { path: 'team', component: TeamComponent, title: 'Team' },
];
