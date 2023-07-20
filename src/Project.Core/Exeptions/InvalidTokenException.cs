namespace Project.Core.Exeptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base("Invalid token") { }
    }
}
