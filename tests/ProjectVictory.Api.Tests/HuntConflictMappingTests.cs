using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using ProjectVictory.API.Middleware;
using ProjectVictory.Application.Hunt;
using Xunit;

namespace ProjectVictory.Api.Tests;

// Revisão do PV-HUNT-012 (ressalva 1): a Hunt encerrada ENQUANTO o comando esperava o lock responde
// 409 (a Hunt já fora da memória responde 403, como recurso desconhecido).
public sealed class HuntConflictMappingTests
{
    [Fact]
    public async Task HuntAlreadyFinished_MapsTo409()
    {
        var handler = new ApiExceptionHandler(NullLogger<ApiExceptionHandler>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var handled = await handler.TryHandleAsync(context, new HuntAlreadyFinishedException(), CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);
    }
}
