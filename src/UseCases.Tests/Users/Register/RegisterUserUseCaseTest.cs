using CashFlow.Application.UseCases.Users;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestsUtilities.Criptography;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using Shouldly;

namespace UseCases.Tests.Users.Register;
public class RegisterUserUseCaseTest
{
    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOffWork = UnitOfWorkBuilder.Build();
        var writeRepository = WriteOnlyUserRepositoryBuilder.Build();
        var passwordEncripter = new PasswordEncripterBuilder().Build();
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();
        var readyOnlyUsersRepository = new ReadOnlyUserRepositoryBuilder();

        if(string.IsNullOrWhiteSpace(email) == false)
        {
            readyOnlyUsersRepository.ExistActiveUserWithEmail(email);
        }

        return new RegisterUserUseCase(mapper, passwordEncripter, readyOnlyUsersRepository.Build(), writeRepository, unitOffWork, tokenGenerator);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var useCase = CreateUseCase();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrosOnValidationException>();
        var errorMessage = result.GetErrors().FirstOrDefault();

        result.GetErrors().Count.ShouldBe(1);
        errorMessage.ShouldBe(ResourceErrorMessages.NAME_USER_REQUIRED);   
    }

    [Fact]
    public async Task Error_Email_Exists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrosOnValidationException>();
        result.GetErrors().Count.ShouldBe(1);

        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
    }
}
