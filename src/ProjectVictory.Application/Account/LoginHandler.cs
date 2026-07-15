using ProjectVictory.Domain.Account;

namespace ProjectVictory.Application.Account;

public sealed class LoginHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginHandler(
        IAccountRepository accountRepository,
        IPasswordHasher passwordHasher)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        var email = new Email(command.Email);

        var account = await _accountRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (account is null)
        {
            throw new InvalidOperationException(
                "Invalid credentials.");
        }

        var validPassword = _passwordHasher.Verify(
            command.Password,
            account.PasswordHash);

        if (!validPassword)
        {
            throw new InvalidOperationException(
                "Invalid credentials.");
        }

        return account.Id;
    }
}