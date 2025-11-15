
using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;
public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IMapper _mapper;
    private readonly IExpenseUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public UpdateExpenseUseCase(
        IMapper mapper, 
        IExpenseUpdateOnlyRepository repository, 
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }
    public async Task Execute(RequestExpenseJson request, long id)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var expense = await _repository.GetById(loggedUser, id);

        if (expense is null) throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        _mapper.Map(request, expense);

        _repository.Update(expense);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestExpenseJson request) {
        var validator = new ExpenseValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorsMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            var errorResponse = new ResponseErrorsJson(errorsMessages);

            throw new ErrosOnValidationException(errorsMessages);
        }
    }
}
