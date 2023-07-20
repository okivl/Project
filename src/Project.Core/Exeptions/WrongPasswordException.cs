namespace Project.Core.Exeptions
{
    public class WrongPasswordException : Exception
    {
        public WrongPasswordException() : base("Wrong password") { }
    }
}
