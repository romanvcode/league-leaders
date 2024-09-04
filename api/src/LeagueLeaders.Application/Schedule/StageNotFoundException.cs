﻿namespace LeagueLeaders.Application.Schedule;

public class StageNotFoundException : Exception
{
    public StageNotFoundException() { }

    public StageNotFoundException(string message)
        : base(message) { }

    public StageNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
