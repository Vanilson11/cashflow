using CashFlow.Domain.Security.Criptography;
using Moq;

namespace CommonTestsUtilities.Criptography;
public class PasswordEncripterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;

    public PasswordEncripterBuilder()
    {
        _mock = new Mock<IPasswordEncripter>();

        _mock.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("criptography_string");
    }

    public PasswordEncripterBuilder Verify(string? requestPassword)
    {
        if(string.IsNullOrWhiteSpace(requestPassword) == false)
        {
            _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(requestPassword, It.IsAny<string>())).Returns(true);
        }

        return this;
    }
    public IPasswordEncripter Build() => _mock.Object;
}
