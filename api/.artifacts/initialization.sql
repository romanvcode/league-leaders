CREATE DATABASE ChampionsLeagueDB;
GO

USE ChampionsLeagueDB;
GO

CREATE TABLE Competitions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Region NVARCHAR(100),
    SportradarId INT UNIQUE
);

CREATE TABLE Seasons (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    CompetitionId INT NOT NULL,
    StartAt DATE,
    EndAt DATE,
    SportradarId INT UNIQUE,
    FOREIGN KEY (CompetitionId) REFERENCES Competitions(Id)
);

CREATE TABLE Stages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    SeasonId INT NOT NULL,
    Type NVARCHAR(50),
    Order INT UNIQUE,
    FOREIGN KEY (SeasonId) REFERENCES Seasons(Id)
);

CREATE TABLE Teams (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Abbreviation NVARCHAR(10),
    Country NVARCHAR(100),
    Stadium NVARCHAR(100),
    Manager NVARCHAR(100),
    SportradarId INT UNIQUE
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
    SportradarId INT UNIQUE,
    FOREIGN KEY (TeamId) REFERENCES Teams(Id)
);

CREATE TABLE Venues (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    City NVARCHAR(100),
    Country NVARCHAR(100),
    Capacity INT,
    SportradarId INT UNIQUE
);

CREATE TABLE Referees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Nationality NVARCHAR(50),
    SportradarId INT UNIQUE
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
    SportradarId INT UNIQUE,
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

INSERT INTO Competitions (Name, Region, SportradarId)
VALUES 
('UEFA Champions League', 'Europe', 1);

INSERT INTO Seasons (Name, CompetitionId, StartAt, EndAt, SportradarId)
VALUES 
('2024/2025', 1, '2024-07-09', '2025-05-31', 1);

INSERT INTO Stages (Name, SeasonId, Type, Order)
VALUES 
('Group Stage', 1, 'Group', 1),
('Round of 16', 1, 'Knockout', 2),
('Quarter-finals', 1, 'Knockout', 3),
('Semi-finals', 1, 'Knockout', 4),
('Final', 1, 'Knockout', 5);

INSERT INTO Teams (Name, Abbreviation, Country, Stadium, Manager, SportradarId)
VALUES 
('FC Barcelona', 'BAR', 'Spain', 'Camp Nou', 'Xavi Hernandez', 1),
('Real Madrid', 'RM', 'Spain', 'Santiago Bernabeu', 'Carlo Ancelotti', 2),
('Manchester City', 'MCI', 'England', 'Etihad Stadium', 'Pep Guardiola', 3),
('Bayern Munich', 'BM', 'Germany', 'Allianz Arena', 'Thomas Tuchel', 4);

INSERT INTO Players (Name, TeamId, Position, Number, Height, Nationality, DateOfBirth, SportradarId)
VALUES 
('Lionel Messi', 1, 'Forward', 10, 170, 'Argentina', '1987-06-24', 1),
('Karim Benzema', 2, 'Forward', 9, 185, 'France', '1987-12-19', 2),
('Kevin De Bruyne', 3, 'Midfielder', 17, 181, 'Belgium', '1991-06-28', 3),
('Robert Lewandowski', 4, 'Forward', 9, 185, 'Poland', '1988-08-21', 4);

INSERT INTO Venues (Name, City, Country, Capacity, SportradarId)
VALUES 
('Camp Nou', 'Barcelona', 'Spain', 99354, 1),
('Santiago Bernabeu', 'Madrid', 'Spain', 81044, 2),
('Etihad Stadium', 'Manchester', 'England', 55097, 3),
('Allianz Arena', 'Munich', 'Germany', 75000, 4);

INSERT INTO Referees (Name, Nationality, SportradarId)
VALUES 
('Daniele Orsato', 'Italy', 1),
('Anthony Taylor', 'England', 2),
('Clement Turpin', 'France', 3),
('Felix Brych', 'Germany', 4);

INSERT INTO Matches (StageId, HomeTeamId, AwayTeamId, Date, VenueId, RefereeId, HomeTeamScore, AwayTeamScore, SportradarId)
VALUES 
(1, 1, 2, '2024-08-10 20:00:00', 1, 1, 3, 2, 1),
(1, 3, 4, '2024-08-11 20:00:00', 3, 2, 2, 2, 2),
(1, 2, 3, '2024-08-12 20:00:00', 2, 3, 1, 2, 3),
(1, 4, 1, '2024-08-13 20:00:00', 4, 4, 2, 1, 4);,
(1, 2, 1, '2024-10-10 20:00:00', 2, 1, 3, 3, 5),
(1, 4, 3, '2024-10-11 20:00:00', 4, 2, 1, 0, 6),
(1, 3, 2, '2024-10-12 20:00:00', 3, 3, 2, 0, 7),
(1, 1, 4, '2024-10-13 20:00:00', 1, 4, 0, 1, 8);

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
