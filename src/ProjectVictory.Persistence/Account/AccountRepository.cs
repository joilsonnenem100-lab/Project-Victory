using Microsoft.EntityFrameworkCore;
using ProjectVictory.Domain.Account;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.Account;

public sealed class AccountRepository : IAccountRepository
{
    private readonly ProjectVictoryDbContext _context;


    public AccountRepository(
        ProjectVictoryDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(
        Domain.Account.Account account,
        CancellationToken cancellationToken = default)
    {
        var state =
            new PersistentAccountState
            {
                Id = account.Id,

                Email = account.Email.Value,

                PasswordHash = account.PasswordHash.Value
            };


        await _context.Accounts.AddAsync(
            state,
            cancellationToken);


        await _context.SaveChangesAsync(
            cancellationToken);
    }


    public async Task<Domain.Account.Account?> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        var state =
            await _context.Accounts
                .FirstOrDefaultAsync(
                    x => x.Email == email.Value,
                    cancellationToken);


        if (state is null)
        {
            return null;
        }


        return Domain.Account.Account.Create(
            new AccountId(state.Id),
            new Email(state.Email),
            new PasswordHash(state.PasswordHash));
    }
}