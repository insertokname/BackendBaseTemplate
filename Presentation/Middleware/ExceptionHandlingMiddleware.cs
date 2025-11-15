namespace Presentation.Middleware
{
    public static class ExceptionHandlingMiddleware
    {
        public static void ConfigureExceptionHandling(this WebApplication app)
        {
            app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                    var exception = exceptionHandlerFeature?.Error;

                    var responseMessage = app.Environment.IsDevelopment()
                        ? exception?.Message
                        : "An unexpected error occurred. Please try again later.";

                    await context.Response.WriteAsync("{\"error\": \"" + responseMessage + "\"}");
                }));
        }
    }
}
