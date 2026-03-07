namespace StoreInventorySystem.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Time: {DateTime.Now}");
            Console.WriteLine($"Method: {context.Request.Method}");
            Console.WriteLine($"Path: {context.Request.Path}");

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception");
                Console.WriteLine($"{ex.ToString()}\n");
                throw;
            }

            Console.WriteLine($"Status code: {context.Response.StatusCode}\n");
        }
    }

    public static class LogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
