import { Component } from '@angular/core';
import { StandingTableComponent } from '../../shared/standing-table/standing-table.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NextMatchComponent } from '../../shared/next-match/next-match.component';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [
    StandingTableComponent,
    MatGridListModule,
    MatToolbarModule,
    NextMatchComponent,
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css',
})
export class LandingComponent {}
