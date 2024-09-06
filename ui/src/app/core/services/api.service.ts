import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Standing } from '../models/standing.model';
import { environment } from 'environment';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private apiUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);

  private userStandings = signal<Standing[]>([]);

  loadedStandings = this.userStandings.asReadonly();

  getStandings() {
    return this.httpClient.get<Standing[]>(
      `${this.apiUrl}/leaderboard/standings`
    );
  }
}
