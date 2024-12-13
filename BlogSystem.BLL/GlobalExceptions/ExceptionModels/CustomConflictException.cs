namespace BlogSystem.BLL.GlobalExceptions.ExceptionModels
{
    public class CustomConflictException : Exception
    {
        public CustomConflictException(string message):base(message)
        {
            
        }
    }
}
