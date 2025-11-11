using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestsUtilities.Repositories;
public class WriteOnlyUserRepositoryBuilder
{
    public static IWriteOnlyUserRepository Build()
    {
        var mock = new Mock<IWriteOnlyUserRepository>();

        return mock.Object;
    }
}
