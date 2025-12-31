using AutoDrive.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace AutoDrive.WebAPI.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerFeature>()?.Error;

                context.Response.ContentType = "application/json";

                var statusCode = HttpStatusCode.InternalServerError;
                object response;

                switch (exception)
                {
                    case NotFoundException ex:
                        statusCode = HttpStatusCode.NotFound;
                        response = new
                        {
                            message = ex.Message
                        };
                        break;

                    case ValidationException ex:
                        statusCode = HttpStatusCode.BadRequest;
                        response = new
                        {
                            message = "Validation failed",
                            errors = ex.Errors
                                .GroupBy(e => e.PropertyName)
                                .ToDictionary(
                                    g => g.Key,
                                    g => g.Select(e => e.ErrorMessage).ToArray()
                                )
                        };
                        break;

                    case UnauthorizedAccessException:
                        statusCode = HttpStatusCode.Unauthorized;
                        response = new
                        {
                            message = "Unauthorized access"
                        };
                        break;

                    default:
                        app.Logger.LogError(exception, "Unhandled exception");

                        response = new
                        {
                            message = "Internal server error"
                        };
                        break;
                }

                context.Response.StatusCode = (int)statusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            });
        });
    }
}
