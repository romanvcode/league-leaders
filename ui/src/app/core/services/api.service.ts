import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Match } from '@core/models/match.model';
import { Prediction } from '@core/models/prediction.model';
import { Team } from '@core/models/team.model';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { Standing } from '../models/standing.model';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  getStandings(): Observable<Standing[]> {
    return this.httpClient.get<Standing[]>(
      `${this.apiUrl}/leaderboard/standings`
    );
  }

  getMatches(): Observable<Match[]> {
    return this.httpClient.get<Match[]>(`${this.apiUrl}/schedule/matches`);
  }

  getTeam(teamId: number): Observable<Team> {
    return this.httpClient.get<Team>(`${this.apiUrl}/teams/${teamId}`);
  }

  getTeamMatches(teamId: number): Observable<Match[]> {
    return this.httpClient.get<Match[]>(
      `${this.apiUrl}/teams/${teamId}/matches`
    );
  }

  getTeamsBySearchTerm(searchTerm: string): Observable<Team[]> {
    return this.httpClient.get<Team[]>(
      `${this.apiUrl}/teams?searchTerm=${searchTerm}`
    );
  }

  createPrediction(
    matchId: number,
    homeTeamScore: number,
    awayTeamScore: number
  ): Observable<Prediction> {
    return this.httpClient.post<Prediction>(`${this.apiUrl}/predictions`, {
      matchId,
      homeTeamScore,
      awayTeamScore,
    });
  }
}
