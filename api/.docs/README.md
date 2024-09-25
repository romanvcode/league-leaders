# LeagueLeaders API Data Mapping

## API Base URL

`https://api.sportradar.com/soccer/trial/v4/`

Make sure to provide your API key as query parameter: `?api_key=YOUR_API_KEY`.

---

## Mapping Overview

| **LeagueLeaders Table** | **API Entity**               |
| ----------------------- | ---------------------------- |
| Competition             | competition_with_category    |
| Season                  | season                       |
| Stage                   | season_stage                 |
| Match                   | sport_event                  |
| Team                    | competitor                   |
| Player                  | player                       |
| Venue                   | venue                        |
| Referee                 | referee                      |
| TeamStat                | season_competitor_statistics |
| PlayerStat              | season_player_statistics     |
| Standing                | standing                     |

---

## Detailed Mapping

### 1. **Competitions**

- **API Data**: Represents football competitions (e.g., Champions League).
- **Mapped Table**: `Competitions` table in the LeagueLeaders database.

#### API Fields → DB Fields:

| **API Field**           | **Database Field** |
| ----------------------- | ------------------ |
| `id`                    | `CompetitionId`    |
| `name`                  | `Name`             |
| `category.country_code` | `Region`           |
