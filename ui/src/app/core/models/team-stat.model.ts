export interface TeamStat {
  id: number;
  matchId: number;
  teamId: number;
  possession: number;
  redCards: number;
  yellowCards: number;
  corners: number;
  offsides: number;
  fouls: number;
  shots: number;
  shotsOnTarget: number;
}
