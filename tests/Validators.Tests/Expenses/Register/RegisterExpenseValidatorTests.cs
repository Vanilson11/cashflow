using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Expenses.Register;
public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        //ARRANGE
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        //ACT
        var result = validator.Validate(request);
        //ASSERT
        result.IsValid.ShouldBe(true);
    }

    [Theory]
    [InlineData("")]
    [InlineData("       ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.TITLE_IS_REQUIRED));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-7)]
    public void Error_Amount_Invalid(decimal amount)
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Amount = amount;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }

    [Fact]
    public void Error_Date_Future()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.EXPENSE_CANNOT_BE_FOR_THE_FUTURE));
    }

    [Fact]
    public void Error_PaymentType_Invalid()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)70;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.PAYMENT_TYPE_NOT_VALID));
    }
}
