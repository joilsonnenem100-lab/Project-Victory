using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Loot;
using ProjectVictory.Infrastructure.Content;

namespace ProjectVictory.Infrastructure.Loot;

// PV-CONTENT-011: XP, ouro e drops vêm do catálogo da Spider Cave (dados PROVISÓRIOS).
public sealed class MonsterLootDefinitionProvider : ILootDefinitionProvider
{
    public MonsterLootDefinition? GetByOpponentId(
        Guid opponentId)
    {
        return SpiderCaveCatalog.Find(opponentId)?.Loot;
    }
}
