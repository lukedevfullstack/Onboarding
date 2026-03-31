using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace OnboardingCreateAccount.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Erro capturado pelo Middleware: {Message}", exception.Message);

        var statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            KeyNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

        if (exception.Message.Contains("não encontrada")) statusCode = HttpStatusCode.NotFound;
        if (exception.Message.Contains("Já existe")) statusCode = HttpStatusCode.BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = statusCode == HttpStatusCode.BadRequest ? "Erro de Validação" : "Erro no Servidor",
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        if (exception is ValidationException fluentException)
        {
            problemDetails.Extensions.Add("errors", fluentException.Errors
                .Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }));
        }

        httpContext.Response.StatusCode = (int)statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}