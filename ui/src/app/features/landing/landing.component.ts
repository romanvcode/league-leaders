import { Component } from '@angular/core';
import { NextMatchComponent } from '../../shared/next-match/next-match.component';
import { SearchInputComponent } from '../../shared/search-input/search-input.component';
import { StandingTableComponent } from '../../shared/standing-table/standing-table.component';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [StandingTableComponent, NextMatchComponent, SearchInputComponent],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css',
})
export class LandingComponent {
  curretFeature = 'Leaderboard';
}
