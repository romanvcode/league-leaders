import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NextMatchComponent } from '@shared/next-match/next-match.component';
import { SearchInputComponent } from '@shared/search-input/search-input.component';
import { TeamInfoComponent } from '@shared/team-info/team-info.component';
import { StandingTableComponent } from '@shared/standing-table/standing-table.component';

@Component({
  selector: 'app-team',
  standalone: true,
  imports: [
    NextMatchComponent,
    SearchInputComponent,
    TeamInfoComponent,
    StandingTableComponent,
  ],
  templateUrl: './team.component.html',
  styleUrl: './team.component.css',
})
export class TeamComponent implements OnInit {
  currentFeature = 'Team';

  teamId!: number;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.teamId = params['id'];
    });
  }
}
