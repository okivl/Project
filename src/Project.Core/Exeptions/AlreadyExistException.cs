namespace Project.Core.Exeptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException() : base("Already exist") { }
    }
}
