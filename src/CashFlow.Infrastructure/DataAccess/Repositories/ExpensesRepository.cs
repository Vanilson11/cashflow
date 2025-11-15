using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;
internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpenseUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll(User user)
    {
        return await _dbContext.Expenses.AsNoTracking().Where(expense => expense.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id){
       return await _dbContext.Expenses.AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(User user,long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }
    public void Update(Expense entity)
    {
        _dbContext.Expenses.Update(entity);
    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Expenses.FirstAsync(expense => expense.Id == id);

        _dbContext.Expenses.Remove(result!);
    }

    public async Task<List<Expense>> FilterByMonth(User user, DateOnly month)
    {
        var startedMonth = new DateTime(year: month.Year, month: month.Month, day: 1).Date;
        var daysOfMonth = DateTime.DaysInMonth(year: month.Year, month: month.Month);
        var endMonth = new DateTime(year: month.Year, month: month.Month, day: daysOfMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext.Expenses.AsNoTracking()
            .Where(expense => expense.UserId == user.Id && expense.Date >= startedMonth && expense.Date <= endMonth)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }
}
