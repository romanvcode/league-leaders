import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LandingComponent } from '@features/landing/landing.component';
import { TeamComponent } from '@features/team/team.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [LandingComponent, TeamComponent, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {}
