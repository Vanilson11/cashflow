using CashFlow.Domain.Security.Criptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security.Criptography;
internal class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        var passwordHash = BC.HashPassword(password);

        return passwordHash;
    }

    public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
    //o Verify verfica se é possível que essa password gere um hash igual ao que está em passwordHash
}
