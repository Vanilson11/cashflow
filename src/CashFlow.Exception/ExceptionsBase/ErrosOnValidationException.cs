using System.Net;

namespace CashFlow.Exception.ExceptionsBase;
public class ErrosOnValidationException : CashFlowException
{
    private readonly List<string> _erros;

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrosOnValidationException(List<string> messages) : base(string.Empty)
    {
        _erros = messages;
    }

    public override List<string> GetErrors()
    {
        return _erros;
    }
}
