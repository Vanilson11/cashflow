using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpenseUpdateOnlyRepository
{
    Task<Expense?> GetById(User user, long id);
    void Update(Expense entity);
}
