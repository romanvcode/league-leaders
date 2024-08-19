CREATE DATABASE ChampionsLeagueDatabase;
GO

USE ChampionsLeagueDatabase;
GO

CREATE TABLE Competitions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Region NVARCHAR(100)
);

CREATE TABLE Seasons (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    CompetitionId INT NOT NULL,
    StartDate DATE,
    EndDate DATE,
    FOREIGN KEY (CompetitionId) REFERENCES Competition(Id)
);

CREATE TABLE Stages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    SeasonId INT NOT NULL,
    Type NVARCHAR(50),
    FOREIGN KEY (SeasonId) REFERENCES Season(Id)
);

CREATE TABLE Teams (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Country NVARCHAR(100),
    Stadium NVARCHAR(100),
    Coach NVARCHAR(100)
);

CREATE TABLE Players (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Position NVARCHAR(50),
    Nationality NVARCHAR(50),
    DateOfBirth DATE,
    TeamId INT NOT NULL,
    FOREIGN KEY (TeamId) REFERENCES Team(Id)
);

CREATE TABLE Venues (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    City NVARCHAR(100),
    Country NVARCHAR(100),
    Capacity INT
);

CREATE TABLE Referees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Nationality NVARCHAR(50)
);

CREATE TABLE Matches (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StageId INT NOT NULL,
    HomeTeamId INT NOT NULL,
    AwayTeamId INT NOT NULL,
    Date DATETIME,
    VenueId INT NOT NULL,
    RefereeId INT NOT NULL,
    HomeTeamScore INT,
    AwayTeamScore INT,
    FOREIGN KEY (StageId) REFERENCES Stage(Id),
    FOREIGN KEY (HomeTeamId) REFERENCES Team(Id),
    FOREIGN KEY (AwayTeamId) REFERENCES Team(Id),
    FOREIGN KEY (VenueId) REFERENCES Venue(Id),
    FOREIGN KEY (RefereeId) REFERENCES Referee(Id)
);

CREATE TABLE MatchEvents (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MatchId INT NOT NULL,
    Type NVARCHAR(50),
    PlayerId INT NOT NULL,
    TeamId INT NOT NULL,
    Time INT,
    FOREIGN KEY (MatchId) REFERENCES Match(Id),
    FOREIGN KEY (PlayerId) REFERENCES Player(Id),
    FOREIGN KEY (TeamId) REFERENCES Team(Id)
);

CREATE TABLE Standings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StageId INT NOT NULL,
    TeamId INT NOT NULL,
    Points INT,
    Place INT,
    MatchesPlayed INT,
    Wins INT,
    Draws INT,
    Losses INT,
    GoalsFor INT,
    GoalsAgainst INT,
    GoalDifference INT,
    FOREIGN KEY (StageId) REFERENCES Stage(Id),
    FOREIGN KEY (TeamId) REFERENCES Team(Id)
);

GO

INSERT INTO Competition (Name, Region) VALUES 
('UEFA Champions League', 'Europe');

INSERT INTO Season (Name, CompetitionId, StartDate, EndDate) VALUES 
('2023/2024', 1, '2023-09-14', '2024-06-01');

INSERT INTO Stage (Name, SeasonId, Type) VALUES 
('Group Stage', 1, 'Group'),
('Round of 16', 1, 'Knockout'),
('Quarter-Finals', 1, 'Knockout'),
('Semi-Finals', 1, 'Knockout'),
('Final', 1, 'Knockout');

INSERT INTO Team (Name, Country, Stadium) VALUES 
('Real Madrid', 'Spain', 'Santiago Bernabéu', 'Carlo Ancelotti'),
('Manchester City', 'England', 'Etihad Stadium', 'Pep Guardiola'),
('Bayern Munich', 'Germany', 'Allianz Arena', 'Thomas Tuchel'),
('Paris Saint-Germain', 'France', 'Parc des Princes', 'Luis Enrique');

INSERT INTO Player (Name, Position, Nationality, DateOfBirth, TeamId) VALUES 
('Karim Benzema', 'Forward', 'France', '1987-12-19', 1),
('Vinícius Júnior', 'Forward', 'Brazil', '2000-07-12', 1),
('Kevin De Bruyne', 'Midfielder', 'Belgium', '1991-06-28', 2),
('Erling Haaland', 'Forward', 'Norway', '2000-07-21', 2),
('Thomas Müller', 'Forward', 'Germany', '1989-09-13', 3),
('Kylian Mbappé', 'Forward', 'France', '1998-12-20', 4);

INSERT INTO Venue (Name, City, Country, Capacity) VALUES 
('Santiago Bernabéu', 'Madrid', 'Spain', 81044),
('Etihad Stadium', 'Manchester', 'England', 53400),
('Allianz Arena', 'Munich', 'Germany', 75000),
('Parc des Princes', 'Paris', 'France', 47929);

INSERT INTO Referee (Name, Nationality) VALUES 
('Björn Kuipers', 'Netherlands'),
('Damir Skomina', 'Slovenia');

INSERT INTO Match (StageId, HomeTeamId, AwayTeamId, Date, VenueId, RefereeId, HomeTeamScore, AwayTeamScore) VALUES 
(1, 1, 2, '2023-10-15 20:45:00', 1, 1, 3, 2),
(1, 3, 4, '2023-10-16 20:45:00', 3, 2, 1, 1);

INSERT INTO MatchEvent (MatchId, Type, PlayerId, TeamId, Time) VALUES 
(1, 'Goal', 1, 1, 25),
(1, 'Goal', 3, 2, 40),
(1, 'Goal', 4, 2, 65),
(1, 'Goal', 2, 1, 88),
(2, 'Goal', 6, 4, 15),
(2, 'Goal', 5, 3, 55);

INSERT INTO Standings (StageId, TeamId, Points, Position, MatchesPlayed, Wins, Draws, Losses, GoalsFor, GoalsAgainst, GoalDifference) VALUES 
(1, 1, 3, 1, 1, 1, 0, 0, 3, 2, 1),
(1, 2, 0, 2, 1, 0, 0, 1, 2, 3, -1),
(1, 3, 1, 3, 1, 0, 1, 0, 1, 1, 0),
(1, 4, 1, 4, 1, 0, 1, 0, 1, 1, 0);
