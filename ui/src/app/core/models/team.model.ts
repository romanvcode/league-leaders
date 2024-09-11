import { Player } from "./player.model";

export interface Team {
  id: number;
  name: string;
  abbreviation: string;
  country: string;
  stadium: string;
  manager: string;
  players: Player[];
}
