using System.Security.Cryptography;
using System.Text;
using ProjectVictory.Domain.Account;

namespace ProjectVictory.Infrastructure.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    public PasswordHash Hash(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);

        return new PasswordHash(
            Convert.ToBase64String(hash));
    }

    public bool Verify(
        string password,
        PasswordHash passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        ArgumentNullException.ThrowIfNull(passwordHash);

        var generated = Hash(password);

        return generated.Value == passwordHash.Value;
    }
}