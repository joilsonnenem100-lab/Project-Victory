using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-STATE-001: o RuntimeId vem do cliente em POST api/runtime/create. Um id já em uso por
// personagem de outra conta é recusado com 403 antes de qualquer gravação; o runtime
// original continua do dono.
[Collection("Api")]
public sealed class RuntimeIdReuseHttpTests
{
    private readonly ApiFactory _factory;

    public RuntimeIdReuseHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    private async Task<(HttpClient Client, Guid CharacterId)> NewPlayer(string prefix)
    {
        var client = _factory.CreateClient();
        var token = await _factory.RegisterAndLoginAsync(client, $"{prefix}-{Guid.NewGuid():N}@test.com");
        ApiFactory.WithToken(client, token);

        var created = await client.PostAsJsonAsync(
            "/api/character",
            new { name = TestNames.Unique("S"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();

        return (client, await created.Content.ReadFromJsonAsync<Guid>());
    }

    [Fact]
    public async Task ReusingAnotherAccountsRuntimeId_Returns403()
    {
        var (owner, ownerCharacter) = await NewPlayer("owner");
        var (other, otherCharacter) = await NewPlayer("other");

        using (owner)
        using (other)
        {
            var runtimeId = Guid.NewGuid();

            var first = await owner.PostAsJsonAsync("/api/runtime/create", new { runtimeId, characterId = ownerCharacter });
            Assert.Equal(HttpStatusCode.OK, first.StatusCode);

            var hijack = await other.PostAsJsonAsync("/api/runtime/create", new { runtimeId, characterId = otherCharacter });
            Assert.Equal(HttpStatusCode.Forbidden, hijack.StatusCode);

            // O runtime continua do dono: atualizar a atividade com o personagem do dono funciona.
            var activity = await owner.PostAsJsonAsync("/api/runtime/activity", new { runtimeId, activity = "Exploring" });
            Assert.Equal(HttpStatusCode.OK, activity.StatusCode);
        }
    }
}
