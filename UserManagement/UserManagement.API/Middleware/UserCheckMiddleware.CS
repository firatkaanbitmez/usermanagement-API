using UserManagement.Service.Services;
using UserManagement.API.Middlewares;


namespace UserManagement.API.Middlewares
{
    public class UserCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserCheckMiddleware> _logger;

        public UserCheckMiddleware(RequestDelegate next, ILogger<UserCheckMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, UserService userService)
        {
            var userId = context.Request.RouteValues["id"] as string;

            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out var id))
            {
                var user = await userService.GetUserByIdAsync(id);
                if (user == null )
                {
                    _logger.LogWarning("Attempt to access non-existent user with ID {UserId}.", id);
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Access denied. User is does not exist.");
                    return;
                }
              
            }

            await _next(context);
        }
    }

}

public static class UserIsActiveMiddlewareExtensions
{
    public static IApplicationBuilder UseUserIsActiveMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserCheckMiddleware>();
    }
}
