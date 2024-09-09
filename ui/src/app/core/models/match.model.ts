import { Team } from './team.model';

export interface Match {
  id: number;
  date: string;
  homeTeam: Team;
  awayTeam: Team;
  homeTeamScore: number;
  awayTeamScore: number;
}
