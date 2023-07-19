namespace Project.Core.Exeptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException() : base("Пользователь не авторизован") { }
    }
}
