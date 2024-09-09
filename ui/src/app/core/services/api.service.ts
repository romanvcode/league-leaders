import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Match } from '@core/models/match.model';
import { environment } from 'environment';
import { Observable } from 'rxjs';
import { Standing } from '../models/standing.model';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  private userStandings = signal<Standing[]>([]);

  loadedStandings = this.userStandings.asReadonly();

  getStandings(): Observable<Standing[]> {
    return this.httpClient.get<Standing[]>(
      `${this.apiUrl}/leaderboard/standings`
    );
  }

  getMathces(): Observable<Match[]> {
    return this.httpClient.get<Match[]>(`${this.apiUrl}/schedule/matches`);
  }
}
