using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BlogSystem.BLL.GlobalExceptions.Forbidden_Exception
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RoleRequirementHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            var user = context.User;

            if (!requirement.Roles.Any(role => context.User.IsInRole(role)))
            {
                _contextAccessor.HttpContext!.Response.StatusCode = StatusCodes.Status403Forbidden;
                _contextAccessor.HttpContext.Response.ContentType = "application/json";
                _contextAccessor.HttpContext.Response.WriteAsync("{\"message\": \" you have no access to this context\"}");

                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
