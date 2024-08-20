CREATE DATABASE ChampionsLeagueDB;
GO

USE ChampionsLeagueDB;
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
    StartAt DATE,
    EndAt DATE,
    FOREIGN KEY (CompetitionId) REFERENCES Competitions(Id)
);

CREATE TABLE Stages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    SeasonId INT NOT NULL,
    Type NVARCHAR(50),
    FOREIGN KEY (SeasonId) REFERENCES Seasons(Id)
);

CREATE TABLE Teams (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Abbreviation NVARCHAR(10),
    Country NVARCHAR(100),
    Stadium NVARCHAR(100),
    Manager NVARCHAR(100)
);

CREATE TABLE Players (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    TeamId INT NOT NULL,
    Position NVARCHAR(50),
    Number INT,
    Height INT,
    Nationality NVARCHAR(50),
    DateOfBirth DATE,
    FOREIGN KEY (TeamId) REFERENCES Teams(Id)
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
    FOREIGN KEY (StageId) REFERENCES Stages(Id),
    FOREIGN KEY (HomeTeamId) REFERENCES Teams(Id),
    FOREIGN KEY (AwayTeamId) REFERENCES Teams(Id),
    FOREIGN KEY (VenueId) REFERENCES Venues(Id),
    FOREIGN KEY (RefereeId) REFERENCES Referees(Id)
);

CREATE TABLE PlayerStats (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MatchId INT NOT NULL,
    PlayerId INT NOT NULL,
    TeamId INT NOT NULL,
    Goals INT,
    Assists INT,
    RedCards INT,
    YellowCards INT,
    Shots INT,
    ShotsOnTarget INT,
    FOREIGN KEY (MatchId) REFERENCES Matches(Id),
    FOREIGN KEY (PlayerId) REFERENCES Players(Id),
    FOREIGN KEY (TeamId) REFERENCES Teams(Id)
);

CREATE TABLE TeamStats (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MatchId INT NOT NULL,
    TeamId INT NOT NULL,
    Possession INT,
    RedCards INT,
    YellowCards INT,
    Corners INT,
    Offsides INT,
    Fouls INT,
    Shots INT,
    ShotsOnTarget INT,
    FOREIGN KEY (MatchId) REFERENCES Matches(Id),
    FOREIGN KEY (TeamId) REFERENCES Teams(Id)
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
    FOREIGN KEY (StageId) REFERENCES Stages(Id),
    FOREIGN KEY (TeamId) REFERENCES Teams(Id)
);
GO

INSERT INTO Competitions (Name, Region)
VALUES 
('UEFA Champions League', 'Europe');

INSERT INTO Seasons (Name, CompetitionId, StartAt, EndAt)
VALUES 
('2023/2024', 1, '2023-09-01', '2024-06-01');

INSERT INTO Stages (Name, SeasonId, Type)
VALUES 
('Group Stage', 1, 'Group'),
('Round of 16', 1, 'Knockout'),
('Quarter-finals', 1, 'Knockout'),
('Semi-finals', 1, 'Knockout'),
('Final', 1, 'Knockout');

INSERT INTO Teams (Name, Abbreviation, Country, Stadium, Manager)
VALUES 
('FC Barcelona', 'BAR', 'Spain', 'Camp Nou', 'Xavi Hernandez'),
('Real Madrid', 'RM', 'Spain', 'Santiago Bernabeu', 'Carlo Ancelotti'),
('Manchester City', 'MCI', 'England', 'Etihad Stadium', 'Pep Guardiola'),
('Bayern Munich', 'BM', 'Germany', 'Allianz Arena', 'Thomas Tuchel');

INSERT INTO Players (Name, TeamId, Position, Number, Height, Nationality, DateOfBirth)
VALUES 
('Lionel Messi', 1, 'Forward', 10, 170, 'Argentina', '1987-06-24'),
('Karim Benzema', 2, 'Forward', 9, 185, 'France', '1987-12-19'),
('Kevin De Bruyne', 3, 'Midfielder', 17, 181, 'Belgium', '1991-06-28'),
('Robert Lewandowski', 4, 'Forward', 9, 185, 'Poland', '1988-08-21');

INSERT INTO Venues (Name, City, Country, Capacity)
VALUES 
('Camp Nou', 'Barcelona', 'Spain', 99354),
('Santiago Bernabeu', 'Madrid', 'Spain', 81044),
('Etihad Stadium', 'Manchester', 'England', 55097),
('Allianz Arena', 'Munich', 'Germany', 75000);

INSERT INTO Referees (Name, Nationality)
VALUES 
('Daniele Orsato', 'Italy'),
('Anthony Taylor', 'England'),
('Clement Turpin', 'France'),
('Felix Brych', 'Germany');

INSERT INTO Matches (StageId, HomeTeamId, AwayTeamId, Date, VenueId, RefereeId, HomeTeamScore, AwayTeamScore)
VALUES 
(1, 1, 2, '2023-09-10 20:00:00', 1, 1, 3, 2),
(1, 3, 4, '2023-09-11 20:00:00', 3, 2, 2, 2),
(2, 2, 3, '2024-03-05 20:00:00', 2, 3, 1, 2),
(2, 4, 1, '2024-03-06 20:00:00', 4, 4, 2, 1);

INSERT INTO PlayerStats (MatchId, PlayerId, TeamId, Goals, Assists, RedCards, YellowCards, Shots, ShotsOnTarget)
VALUES 
(1, 1, 1, 2, 0, 0, 1, 5, 3),
(1, 2, 2, 1, 1, 0, 0, 4, 2),
(2, 3, 3, 1, 1, 0, 1, 6, 4),
(2, 4, 4, 2, 0, 0, 0, 7, 5);

INSERT INTO TeamStats (MatchId, TeamId, Possession, RedCards, YellowCards, Corners, Offsides, Fouls, Shots, ShotsOnTarget)
VALUES 
(1, 1, 55, 0, 2, 5, 3, 12, 10, 7),
(1, 2, 45, 0, 1, 3, 4, 13, 12, 5),
(2, 3, 60, 0, 1, 7, 1, 10, 8, 6),
(2, 4, 40, 0, 1, 4, 2, 11, 9, 5);

INSERT INTO Standings (StageId, TeamId, Points, Place, MatchesPlayed, Wins, Draws, Losses, GoalsFor, GoalsAgainst)
VALUES 
(1, 1, 3, 1, 1, 1, 0, 0, 3, 2),
(1, 2, 0, 2, 1, 0, 0, 1, 2, 3),
(1, 3, 1, 3, 1, 0, 1, 0, 2, 2),
(1, 4, 1, 4, 1, 0, 1, 0, 2, 2);
GO
