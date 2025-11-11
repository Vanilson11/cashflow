using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestsUtilities.Criptography;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using Shouldly;

namespace UseCases.Tests.Login.DoLogin;
public class DoLoginUseCaseTests
{
    private DoLoginUseCase CreateUseCase(User user, string? requestPassword = null)
    {
        var readOnlyRepository = new ReadOnlyUserRepositoryBuilder().GetUserByEmail(user).Build();
        var passwordEncripter = new PasswordEncripterBuilder().Verify(requestPassword).Build();
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();

        return new DoLoginUseCase(readOnlyRepository, passwordEncripter, tokenGenerator);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user, request.Password);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<InvalidLoginExeception>();
        result.GetErrors().Count.ShouldBe(1);

        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.INVALID_LOGIN);
    }

    [Fact]
    public async Task Error_Password_Invalid()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<InvalidLoginExeception>();
        result.GetErrors().Count.ShouldBe(1);

        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.INVALID_LOGIN);
    }
}
