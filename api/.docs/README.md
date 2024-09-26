# The Soccer API Data Mapping

## API Base URL

`https://api.sportradar.com/soccer/trial/v4/`

Make sure to provide your API key as query parameter: `?api_key=YOUR_API_KEY`.

---

## Mapping Overview

| **LeagueLeaders Table** | **API Entity**               |
| ----------------------- | ---------------------------- |
| Competitions            | Competition_With_Category    |
| Seasons                 | Season                       |
| Stages                  | Season_Stage                 |
| Matches                 | Sport_Event_Status           |
| Teams                   | Competitor                   |
| Players                 | Player                       |
| Venues                  | Venue                        |
| Referees                | Referee                      |
| TeamStats               | Season_Competitor_Statistics |
| PlayerStats             | Season_Player_Statistics     |
| Standings               | Standing                     |

---

## Detailed Mapping

### 1. **Competitions**

- Take data from `/competitions`

| **LeagueLeaders Field** | **API Field**           |
| ----------------------- | ----------------------- |
| `Id`                    | `id`                    |
| `Name`                  | `name`                  |
| `Region`                | `category.country_code` |

---

### 2. **Seasons**

- Take data from `/competitions/{urn_competition}/seasons`

| **LeagueLeaders Field** | **API Field**    |
| ----------------------- | ---------------- |
| `Id`                    | `id`             |
| `Name`                  | `name`           |
| `StartAt`               | `start_date`     |
| `EndAt`                 | `end_date`       |
| `CompetitionId`         | `competition_Id` |

---

### 3. **Stages**

- Take data from `/seasons/{urn_season}/stages_groups_cup_rounds`

| **LeagueLeaders Field** | **API Field**  |
| ----------------------- | -------------- |
| `Id`                    | `stages.order` |
| `Name`                  | `phase`        |
| `Type`                  | `type`         |
| `SeasonId`              | `{urn_season}` |

---

### 4. **Matches**

- Take data from `/seasons/{urn_season}/summaries`

| **LeagueLeaders Field** | **API Field**                                                                |
| ----------------------- | ---------------------------------------------------------------------------- |
| `Id`                    | `sport_event.id`                                                             |
| `StageId`               | `sport_event.sport_event_context.stage.order`                                |
| `HomeTeamId`            | `sport_event.competitors.single(qualifier == 'home').id`                     |
| `AwayTeamId`            | `sport_event.competitors.single(qualifier == 'away').id`                     |
| `Date`                  | `sport_event.start_time`                                                     |
| `VenueId`               | `sport_event.venue.id`                                                       |
| `Referee`               | `sport_event.sport_event_conditions.referees.single(type == 'main_referee')` |
| `HomeTeamScore`         | `sport_event.sport_event_status.home_score`                                  |
| `AwayTeamScore`         | `sport_event.sport_event_status.away_score`                                  |

---

### 5. **Teams**

- Take data from `/seasons/{urn_season}/competitors`

| **LeagueLeaders Field** | **API Field**  |
| ----------------------- | -------------- |
| `Id`                    | `id`           |
| `Name`                  | `name`         |
| `Abbreviation`          | `abbreviation` |

- For each Competitor from previous request take data from `/competitors/{urn_competitor}/profile`

| **LeagueLeaders Field** | **API Field**             |
| ----------------------- | ------------------------- |
| `Country`               | `competitor.country_code` |
| `Stadium`               | `venue.name`              |
| `Manager`               | `manager.name`            |

---

### 6. **Players**

- Take data from `/seasons/{urn_season}/competitor_players`

| **LeagueLeaders Field** | **API Field** |
| ----------------------- | ------------- |
| `TeamId`                | `id`          |

- For each Player in `players`

| **LeagueLeaders Field** | **API Field**   |
| ----------------------- | --------------- |
| `Id`                    | `id`            |
| `Name`                  | `name`          |
| `Position`              | `type`          |
| `Number`                | `jersey_number` |
| `Height`                | `height`        |
| `Natianality`           | `nationality`   |
| `DateOfBirth`           | `date_of_birth` |

---

### 7. **Venues**

- Take data from `/seasons/{urn_season}/summaries` for each `sport_event.venue`

| **LeagueLeaders Field** | **API Field**  |
| ----------------------- | -------------- |
| `Id`                    | `id`           |
| `Name`                  | `name`         |
| `City`                  | `city_name`    |
| `Country`               | `country_code` |
| `Capacity`              | `capacity`     |

---

### 8. **Referees**

- Take data from `/seasons/{urn_season}/summaries` for each `sport_event.sport_event_conditions.referees.single(type == 'main_referee')`

| **LeagueLeaders Field** | **API Field** |
| ----------------------- | ------------- |
| `Id`                    | `id`          |
| `Name`                  | `name`        |
| `City`                  | `nationality` |

---

### 9. **TeamStats**

- Take data from `/sport_events/{urn_sport_event}/summary` for each `statistics` take for each in `totals.competitors`

| **LeagueLeaders Field** | **API Field**          |
| ----------------------- | ---------------------- |
| `Id`                    | `{id}_{urn_sport_event}` |
| `TeamId`                | `id`                   |
| `MatchId`               | `{urn_sport_event}`    |

- Next, take `statistics`

| **LeagueLeaders Field** | **API Field**     |
| ----------------------- | ----------------- |
| `Possession`            | `ball_possession` |
| `RedCards`              | `red_cards`       |
| `YellowCards`           | `yellow_cards`    |
| `Corners`               | `corner_kicks`    |
| `Offsides`              | `offsides`        |
| `Fouls`                 | `fouls`           |
| `Shots`                 | `shots_total`     |
| `ShotsOnTarget`         | `shots_on_target` |

---

### 10. **PlayerStats**

- Take data from `/sport_events/{urn_sport_event}/summary` for each `statistics` take for each in `totals.competitors`

| **LeagueLeaders Field** | **API Field** |
| ----------------------- | ------------- |
| `TeamId`                | `id`          |

- Next, take for each in `players`

| **LeagueLeaders Field** | **API Field**          |
| ----------------------- | ---------------------- |
| `Id`                    | `{id}_{urn_sport_event}` |
| `PlayerId`              | `id`                   |
| `MatchId`               | `{urn_sport_event}`    |

- Next, take `statistics`

| **LeagueLeaders Field** | **API Field**                          |
| ----------------------- | -------------------------------------- |
| `Goals`                 | `goals_scored`                         |
| `Assists`               | `assists`                              |
| `RedCards`              | `red_cards`                            |
| `YellowCards`           | `yellow_cards`                         |
| `Shots`                 | `shots_on_target` + `shots_off_target` |
| `ShotsOnTarget`         | `shots_on_target`                      |

---

### 11. **Standings**

- Take data from `seasons/{urn_season}/standings` for each in `groups`

| **LeagueLeaders Field** | **API Field**                  |
| ----------------------- | ------------------------------ |
| `Id`                    | `+ {stage.order}_{urn_season}` |
| `StageId`               | `stage.order`                  |

- Next, for each in `standings`

| **LeagueLeaders Field** | **API Field**        |
| ----------------------- | -------------------- |
| `Id`                    | `{competitor_id}_ +` |
| `TeamId`                | `competitor.id`      |
| `Points`                | `points`             |
| `Place`                 | `rank`               |
| `MatchesPlayed`         | `played`             |
| `Wins`                  | `win`                |
| `Draws`                 | `draw`               |
| `Losses`                | `loss`               |
| `GoalsFor`              | `goals_for`          |
| `GoalsAgainst`          | `goals_against`      |
