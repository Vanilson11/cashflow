using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestsUtilities.Repositories;
public class ReadOnlyUserRepositoryBuilder
{
    private readonly Mock<IReadyOnlyUsersRepository> _repository;

    public ReadOnlyUserRepositoryBuilder()
    {
        _repository = new Mock<IReadyOnlyUsersRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(userReadOnly => userReadOnly.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public ReadOnlyUserRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(userRepository => userRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IReadyOnlyUsersRepository Build() => _repository.Object;
}
