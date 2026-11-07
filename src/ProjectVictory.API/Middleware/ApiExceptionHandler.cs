using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.API.Session;
using ProjectVictory.Application.Session;

namespace ProjectVictory.API.Middleware;

public sealed class ApiExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ApiExceptionHandler> _logger;


    public ApiExceptionHandler(
        ILogger<ApiExceptionHandler> logger)
    {
        _logger = logger;
    }


    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // PV-SEC-002 (R-018): identidade ausente → 401, com log de aviso (não de erro), para não
        // inflar o log de erros com requisições não autenticadas.
        if (exception is PlayerIdentityUnavailableException)
        {
            _logger.LogWarning(
                "Request without a usable account identity.");

            httpContext.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await httpContext.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized"
                },
                cancellationToken);

            return true;
        }

        _logger.LogError(
            exception,
            "Unhandled exception occurred.");

        if (exception.Message == "Invalid credentials.")
        {
            httpContext.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await httpContext.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Detail = "Invalid credentials."
                },
                cancellationToken);

            return true;
        }


        // PV-SUPPLY-010: configuração de suprimentos inválida → 400.
        // PV-RUNTIME-013 (R-051/R-041): área de Hunt inexistente ou acima do tier → 400.
        if (exception is ProjectVictory.Application.Supply.InvalidSupplyPolicyException
            or ProjectVictory.Application.Hunt.InvalidHuntAreaException
            // PV-ADMIN-010: XP rate fora de (0, máximo] → 400.
            or ProjectVictory.Application.Admin.InvalidExperienceRateException
            // PV-CHAR-013: nome fora do formato, classe/gênero/aparência desconhecidos → 400.
            or ProjectVictory.Application.Character.InvalidCharacterRequestException)
        {
            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = exception.Message
                },
                cancellationToken);

            return true;
        }


        // Revisão do PV-HUNT-011 (ressalva 4): Hunt já encerrada → 409.
        // PV-HUNT-013 (DEC-20261106-46): segunda Hunt com outra em andamento → 409 (a anterior continua).
        if (exception is ProjectVictory.Application.Hunt.HuntAlreadyFinishedException
            or ProjectVictory.Application.Hunt.HuntAlreadyRunningException
            // Revisão do PV-QA-007 (ressalva 1): `automaticReturn` enviado pelo cliente estando deslogado → 409.
            or ProjectVictory.Application.Hunt.AutomaticReturnCancelledException
            // PV-CHAR-013: sem slot livre, classe repetida, nome em uso, exclusão já pendente/não pendente → 409.
            or ProjectVictory.Application.Character.CharacterCreationConflictException
            or ProjectVictory.Application.Character.CharacterDeletionConflictException)
        {
            httpContext.Response.StatusCode =
                StatusCodes.Status409Conflict;

            await httpContext.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = exception.Message
                },
                cancellationToken);

            return true;
        }


        // PV-SEC-001: recurso de outra conta → 403, sem expor detalhes do recurso.
        // PV-ADMIN-010: conta sem papel de administrador (checagem do caso de uso) → 403.
        if (exception is OwnershipViolationException
            or ProjectVictory.Application.Admin.AdministratorRequiredException)
        {
            httpContext.Response.StatusCode =
                StatusCodes.Status403Forbidden;

            await httpContext.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Forbidden"
                },
                cancellationToken);

            return true;
        }


        httpContext.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error"
            },
            cancellationToken);

        return true;
    }
}