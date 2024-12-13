namespace BlogSystem.BLL.GlobalExceptions.ExceptionModels
{
    public class RegisterAccountException : Exception
    {
        public string[]? Errors { get; set; }
        public RegisterAccountException(string message):base(message)
        {
            Errors = [];
        }
    }   
}
