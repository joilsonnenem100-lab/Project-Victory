// src/ProjectVictory.Domain/Loot/ILootDefinitionProvider.cs
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Domain.Loot;

public interface ILootDefinitionProvider
{
    MonsterLootDefinition? GetByOpponentId(
        Guid opponentId);
}