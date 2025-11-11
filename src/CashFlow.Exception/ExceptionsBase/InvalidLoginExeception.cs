
using System.Net;

namespace CashFlow.Exception.ExceptionsBase;
public class InvalidLoginExeception : CashFlowException
{
    public InvalidLoginExeception() : base(ResourceErrorMessages.INVALID_LOGIN)
    {
        
    }
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
