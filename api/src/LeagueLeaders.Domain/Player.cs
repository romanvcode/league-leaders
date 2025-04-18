﻿namespace LeagueLeaders.Domain;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TeamId { get; set; }
    public string? Position { get; set; }
    public int? Number { get; set; }
    public int? Height { get; set; }
    public string? Nationality { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public int SportradarId { get; set; }

    public Team Team { get; set; } = null!;
}
