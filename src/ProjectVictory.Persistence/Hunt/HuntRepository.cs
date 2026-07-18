using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Persistence.Hunt;

public sealed class HuntRepository : IHuntRepository
{
    private readonly List<Domain.Hunt.Hunt> _hunts = [];

    public Task AddAsync(
        Domain.Hunt.Hunt hunt,
        CancellationToken cancellationToken = default)
    {
        _hunts.Add(hunt);

        return Task.CompletedTask;
    }

    public Task<Domain.Hunt.Hunt?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var hunt = _hunts
            .FirstOrDefault(x => x.Id == id);

        return Task.FromResult(hunt);
    }
}