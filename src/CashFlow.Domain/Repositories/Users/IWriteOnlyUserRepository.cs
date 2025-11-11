using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;
public interface IWriteOnlyUserRepository
{
    Task Add(User user);
}
