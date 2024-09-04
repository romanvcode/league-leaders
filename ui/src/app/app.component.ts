import { Component } from '@angular/core';
import { LandingComponent } from "./features/landing/landing.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [LandingComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {}
