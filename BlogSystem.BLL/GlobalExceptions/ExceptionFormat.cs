namespace BlogSystem.BLL.GlobalExceptions
{
    public class ExceptionFormat
    {
        public int? StatusCode { get; set; }
        public string? Message { get; set; }

        public ExceptionFormat(int? statusCode = 0, string? message = "")
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
