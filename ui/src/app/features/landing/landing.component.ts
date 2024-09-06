import { Component } from '@angular/core';
import { StandingTableComponent } from '../../shared/standing-table/standing-table.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NextMatchComponent } from '../../shared/next-match/next-match.component';
import { SearchInputComponent } from "../../shared/search-input/search-input.component";

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [
    StandingTableComponent,
    MatGridListModule,
    MatToolbarModule,
    NextMatchComponent,
    SearchInputComponent
],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css',
})
export class LandingComponent {
  curretFeature = 'Leaderboard';
}
