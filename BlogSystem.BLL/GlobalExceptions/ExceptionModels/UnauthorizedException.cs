namespace BlogSystem.BLL.GlobalExceptions.ExceptionModels
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message):base(message)
        {
            
        }
    }
}
