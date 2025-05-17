import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Match } from '@core/models/match.model';
import { Prediction } from '@core/models/prediction.model';
import { Team } from '@core/models/team.model';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { Standing } from '../models/standing.model';
import { TeamStat } from '@core/models/team-stat.model';

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
  
  getTeamsStats(matchId: number): Observable<TeamStat[]> {
    return this.httpClient.get<TeamStat[]>(
      `${this.apiUrl}/matches/${matchId}/teams-stats`
    );
  }

  getTeamsBySearchTerm(searchTerm: string): Observable<Team[]> {
    return this.httpClient.get<Team[]>(
      `${this.apiUrl}/teams?searchTerm=${searchTerm}`
    );
  }

  getPredictions(): Observable<Prediction[]> {
    return this.httpClient.get<Prediction[]>(`${this.apiUrl}/predictions`);
  }
  
  createPrediction(
    matchId: number,
    homeTeamScore: number,
    awayTeamScore: number,
    predicted: boolean,
  ): Observable<Prediction> {
    return this.httpClient.post<Prediction>(`${this.apiUrl}/predictions`, {
      matchId,
      homeTeamScore,
      awayTeamScore,
      predicted
    });
  }
  
  deletePrediction(predictionId: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/predictions/${predictionId}`);
  }
}
