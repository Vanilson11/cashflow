using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;
public interface IReadyOnlyUsersRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<User?> GetUserByEmail(string email);
}
