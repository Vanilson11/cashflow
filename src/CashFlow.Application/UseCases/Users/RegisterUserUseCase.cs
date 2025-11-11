using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Criptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IReadyOnlyUsersRepository _userReadOnlyRepository;
    private readonly IWriteOnlyUserRepository _userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOffWork;
    private readonly IAccessTokenGenerator _tokenGenerator;
    public RegisterUserUseCase(IMapper mapper, IPasswordEncripter passwordEncripter,
        IReadyOnlyUsersRepository userReadOnlyRepository,
        IWriteOnlyUserRepository userWriteOnlyRepository,
        IUnitOfWork unitOffWork,
        IAccessTokenGenerator tokenGenerator)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unitOffWork = unitOffWork;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    { 
        await Validate(request);

        var user = _mapper.Map<User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _userWriteOnlyRepository.Add(user);

        await _unitOffWork.Commit();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var result = new UsersValidator().Validate(request);

        var emailExists = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(errorMessages => errorMessages.ErrorMessage).ToList();
            var errorResponse = new ResponseErrorsJson(errorMessages);

            throw new ErrosOnValidationException(errorMessages);
        }
    }
}
