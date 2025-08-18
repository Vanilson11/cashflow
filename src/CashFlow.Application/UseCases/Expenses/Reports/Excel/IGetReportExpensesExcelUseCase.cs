namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;
public interface IGetReportExpensesExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
