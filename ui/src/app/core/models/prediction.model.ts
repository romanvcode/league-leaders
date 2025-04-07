import { Match } from '@core/models/match.model';

export interface Prediction {
  id: number | null;
  matchId: number;
  homeTeamScore: number;
  awayTeamScore: number;
  match: Match;
}
