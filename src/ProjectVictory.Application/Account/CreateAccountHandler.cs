using ProjectVictory.Domain.Account;

namespace ProjectVictory.Application.Account;

public sealed class CreateAccountHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateAccountHandler(
        IAccountRepository accountRepository,
        IPasswordHasher passwordHasher)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(
        CreateAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        var email = new Email(command.Email);

        var passwordHash = _passwordHasher.Hash(command.Password);

        var account = ProjectVictory.Domain.Account.Account.Create(
            AccountId.New(),
            email,
            passwordHash);

        await _accountRepository.AddAsync(
            account,
            cancellationToken);

        return account.Id;
    }
}