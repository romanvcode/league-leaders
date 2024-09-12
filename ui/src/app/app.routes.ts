import { Routes } from '@angular/router';
import { TeamComponent } from '@features/team/team.component';
import { LandingComponent } from './features/landing/landing.component';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'team/:id', component: TeamComponent },
];
