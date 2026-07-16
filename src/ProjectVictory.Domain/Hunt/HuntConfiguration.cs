using System;

namespace ProjectVictory.Domain.Hunt;

public sealed class HuntConfiguration
{
    public Guid EnemyId { get; }

    public HuntConfiguration(Guid enemyId)
    {
        if (enemyId == Guid.Empty)
        {
            throw new ArgumentException(
                "EnemyId is required.",
                nameof(enemyId));
        }

        EnemyId = enemyId;
    }
}