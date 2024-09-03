namespace LeagueLeaders.Application.Exceptions
{
    public class MatchesNotFoundException : Exception
    {
        public MatchesNotFoundException() { }

        public MatchesNotFoundException(string message)
            : base(message) { }

        public MatchesNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
