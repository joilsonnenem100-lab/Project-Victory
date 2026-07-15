using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Account;

public sealed class Account : AggregateRoot
{
    public Account(Guid id)
        : base(id)
    {
    }
}