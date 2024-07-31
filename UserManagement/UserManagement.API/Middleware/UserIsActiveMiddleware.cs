using UserManagement.Service.Services;
using UserManagement.API.Middlewares;


namespace UserManagement.API.Middlewares
{
    public class UserIsActiveMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserIsActiveMiddleware> _logger;

        public UserIsActiveMiddleware(RequestDelegate next, ILogger<UserIsActiveMiddleware> logger)
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
                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("Attempt to access inactive or non-existent user with ID {UserId}.", id);
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Access denied. User is inactive or does not exist.");
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
        return builder.UseMiddleware<UserIsActiveMiddleware>();
    }
}
