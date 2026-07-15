namespace ProjectVictory.Domain.Account;

public interface IPasswordHasher
{
    PasswordHash Hash(string password);

    bool Verify(string password, PasswordHash passwordHash);
}