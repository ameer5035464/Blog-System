using BlogSystem.BLL.GlobalExceptions.ExceptionModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogSystem.BLL.GlobalExceptions
{
    public class CustomExcpetionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = context.Exception switch
            {
                CustomBadRequest => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                RegisterAccountException => StatusCodes.Status400BadRequest,
                CustomConflictException => StatusCodes.Status409Conflict,
                CustomForbiddenException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Result = new ObjectResult(new ExceptionFormat()
            {
                StatusCode = statusCode,
                Message = context.Exception.Message
            })
            { StatusCode = statusCode };

        }
    }
}
