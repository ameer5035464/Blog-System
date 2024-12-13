namespace BlogSystem.BLL.GlobalExceptions.ExceptionModels
{
    public class CustomForbiddenException : Exception
    {
        public CustomForbiddenException(string message) : base(message)
        {
        }
    }
}
