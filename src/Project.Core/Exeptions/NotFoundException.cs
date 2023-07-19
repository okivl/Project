namespace Project.Core.Exeptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Element not found") { }
    }
}
