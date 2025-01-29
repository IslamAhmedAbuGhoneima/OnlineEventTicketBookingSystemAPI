using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;

namespace OnlineEventTicketBookingSystemAPI.Extenstions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (context is not null)
                {
                    context.Response.StatusCode = contextFeature?.Error switch
                    {
                        _ => StatusCodes.Status500InternalServerError
                    };

                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        Message = contextFeature?.Error.Message,
                        StatusCode = context.Response.StatusCode
                    }.ToString());
                }
            });
        });
    }
}
