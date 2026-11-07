using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-CHAR-013 pela API real: 401 sem token; 403 em personagem de outra conta; 400 formato; 409 conflito.
[Collection("Api")]
public sealed class CharacterAccountRulesHttpTests
{
    private readonly ApiFactory _factory;

    public CharacterAccountRulesHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    private async Task<HttpClient> NewClient()
    {
        var client = _factory.CreateClient();
        ApiFactory.WithToken(client, await _factory.RegisterAndLoginAsync(client, $"chars-{Guid.NewGuid():N}@test.com"));
        return client;
    }

    [Fact]
    public async Task WithoutToken_Is401()
    {
        using var anonymous = _factory.CreateClient();
        var id = Guid.NewGuid();

        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.GetAsync("/api/character/slots")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.PostAsync($"/api/character/{id}/delete", null)).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.PostAsync($"/api/character/{id}/cancel-deletion", null)).StatusCode);
    }

    [Fact]
    public async Task InvalidName_Is400_SecondCharacterOfTheSameClass_Is409()
    {
        using var client = await NewClient();

        Assert.Equal(HttpStatusCode.BadRequest,
            (await client.PostAsJsonAsync("/api/character", new { name = "Hero1", characterClass = "Knight" })).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest,
            (await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("Ou"), characterClass = "Knight", outfit = "Dragon" })).StatusCode);

        (await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("Kn"), characterClass = "Knight", gender = "Female", outfit = "Guard" })).EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Conflict,
            (await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("Kn"), characterClass = "Knight" })).StatusCode);

        var character = Assert.Single((await client.GetFromJsonAsync<JsonElement>("/api/character")).EnumerateArray());
        Assert.Equal("Female", character.GetProperty("gender").GetString());
        Assert.Equal("Guard", character.GetProperty("outfit").GetString());
    }

    [Fact]
    public async Task Slots_FiveWithTwoLocked_FourthCharacter_Is409()
    {
        using var client = await NewClient();

        foreach (var characterClass in new[] { "Knight", "Paladin", "Sorcerer" })
        {
            (await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("Sl"), characterClass })).EnsureSuccessStatusCode();
        }

        Assert.Equal(HttpStatusCode.Conflict,
            (await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("Sl"), characterClass = "Druid" })).StatusCode);

        var slots = await client.GetFromJsonAsync<JsonElement>("/api/character/slots");
        var states = slots.GetProperty("slots").EnumerateArray().Select(s => s.GetProperty("state").GetString()).ToList();
        Assert.Equal(new[] { "Occupied", "Occupied", "Occupied", "Locked", "Locked" }, states);
        Assert.Contains("Female", slots.GetProperty("genders").EnumerateArray().Select(g => g.GetString()));
    }

    [Fact]
    public async Task Deletion_PendingWithDate_Cancel_AndOtherAccount_Is403()
    {
        using var owner = await NewClient();
        using var stranger = await NewClient();
        var created = await owner.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("Dl"), characterClass = "Monk" });
        created.EnsureSuccessStatusCode();
        var id = await created.Content.ReadFromJsonAsync<Guid>();

        Assert.Equal(HttpStatusCode.Forbidden, (await stranger.PostAsync($"/api/character/{id}/delete", null)).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await stranger.PostAsync($"/api/character/{Guid.NewGuid()}/delete", null)).StatusCode);

        var deletion = await owner.PostAsync($"/api/character/{id}/delete", null);
        Assert.Equal(HttpStatusCode.OK, deletion.StatusCode);
        var scheduled = (await deletion.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("deletionScheduledFor").GetDateTimeOffset();
        Assert.InRange(scheduled - DateTimeOffset.UtcNow, TimeSpan.FromDays(7) - TimeSpan.FromMinutes(1), TimeSpan.FromDays(7));
        Assert.Equal(HttpStatusCode.Conflict, (await owner.PostAsync($"/api/character/{id}/delete", null)).StatusCode);

        var listed = Assert.Single((await owner.GetFromJsonAsync<JsonElement>("/api/character")).EnumerateArray());
        Assert.Equal(JsonValueKind.String, listed.GetProperty("deletionScheduledFor").ValueKind);

        Assert.Equal(HttpStatusCode.Forbidden, (await stranger.PostAsync($"/api/character/{id}/cancel-deletion", null)).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await owner.PostAsync($"/api/character/{id}/cancel-deletion", null)).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, (await owner.PostAsync($"/api/character/{id}/cancel-deletion", null)).StatusCode);
    }
}
