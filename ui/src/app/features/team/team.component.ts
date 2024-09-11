import { Component } from '@angular/core';
import { NextMatchComponent } from '../../shared/next-match/next-match.component';
import { SearchInputComponent } from '../../shared/search-input/search-input.component';
import { TeamInfoComponent } from '../../shared/team-info/team-info.component';

@Component({
  selector: 'app-team',
  standalone: true,
  imports: [NextMatchComponent, SearchInputComponent, TeamInfoComponent],
  templateUrl: './team.component.html',
  styleUrl: './team.component.css',
})
export class TeamComponent {
  curretFeature = 'Team';

  currentTeamId = 1;

  team = {
    id: 1,
    name: 'Real Madrid',
    abbreviation: 'RMD',
    country: 'Spain',
    stadium: 'Santiago Bernabeu',
    manager: 'Carlo Ancelotti',
    players: [
      {
        id: 1,
        name: 'Karim Benzema',
        position: 'Forward',
        number: 9,
      },
      {
        id: 2,
        name: 'Thibaut Courtois',
        position: 'Goalkeeper',
        number: 1,
      },
      {
        id: 3,
        name: 'Luka Modric',
        position: 'Midfielder',
        number: 10,
      },
      {
        id: 4,
        name: 'Sergio Ramos',
        position: 'Defender',
        number: 4,
      },
      {
        id: 5,
        name: 'Eden Hazard',
        position: 'Forward',
        number: 7,
      },
      {
        id: 6,
        name: 'Casemiro',
        position: 'Midfielder',
        number: 14,
      },
      {
        id: 7,
        name: 'Raphael Varane',
        position: 'Defender',
        number: 5,
      },
      {
        id: 8,
        name: 'Vinicius Jr',
        position: 'Forward',
        number: 20,
      },
      {
        id: 9,
        name: 'Toni Kroos',
        position: 'Midfielder',
        number: 8,
      },
      {
        id: 10,
        name: 'Dani Carvajal',
        position: 'Defender',
        number: 2,
      },
      {
        id: 11,
        name: 'Ferland Mendy',
        position: 'Defender',
        number: 23,
      },
    ],
  };
}
