using ProjectVictory.Application.Session;

namespace ProjectVictory.SmokeTests;

public sealed class TestCurrentPlayerContext : ICurrentPlayerContext
{
    private Guid _accountId;

    public Guid AccountId => _accountId;

    public void SetAccount(Guid accountId)
    {
        _accountId = accountId;
    }
}
