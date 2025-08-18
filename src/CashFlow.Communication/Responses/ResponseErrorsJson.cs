namespace CashFlow.Communication.Responses;
public class ResponseErrorsJson
{
    public List<string> MessagesError { get; set; }

    public ResponseErrorsJson(string errorMessage)
    {
        MessagesError = [errorMessage];
    }

    public ResponseErrorsJson(List<string> messages)
    {
        MessagesError = messages;
    }
}
