namespace BlogSystem.BLL.GlobalExceptions.ModelStateErros
{
    public class ModelStateErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
        public ModelStateErrorResponse()
        {
            StatusCode = 400;
            Message = "Bad Request";
            Errors = [];
        }
    }
}
