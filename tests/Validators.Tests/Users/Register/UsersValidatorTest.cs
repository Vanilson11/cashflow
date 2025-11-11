using CashFlow.Application.UseCases.Users;
using CashFlow.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Users.Register;
public class UsersValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UsersValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(true);
    }

    [Theory]
    [InlineData("")]
    [InlineData("       ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var validator = new UsersValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_USER_REQUIRED));
    }

    [Theory]
    [InlineData("")]
    [InlineData("       ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var validator = new UsersValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_REQUIRED));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UsersValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "vanilson.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new UsersValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_EMPTY));
    }
}