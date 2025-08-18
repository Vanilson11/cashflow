using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extentions;
public static class PaymetTypeExtentions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportsExpensesMessages.CASH,
            PaymentType.CreditCard => ResourceReportsExpensesMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportsExpensesMessages.DEBIT_CARD,
            PaymentType.EletronicTrasnfer => ResourceReportsExpensesMessages.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}
