using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestsUtilities.Requests;
public class RequestRegisterExpenseJsonBuilder
{
    //essa classe foi feita para que, sempre que for chamada, seja devolvida uma
    //nova instância de RequestRegisterExpenseJson com dados fakes
    //Isso foi feito para evitar ter que ficar instanciando diversas vezes essa classe nos projetos de Tests que precisarem dela
    public static RequestExpenseJson Build()
    {
        //var faker = new Faker();

        /*return new RequestRegisterExpenseJson()
        {
            Title = faker.Commerce.Product(),
            Date = faker.Date.Past(),

        };*/

        return new Faker<RequestExpenseJson>()
            .RuleFor(request => request.Title, faker => faker.Commerce.ProductName())
            .RuleFor(request => request.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(request => request.Date, faker => faker.Date.Past())
            .RuleFor(request => request.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(request => request.Amount, faker => faker.Random.Decimal(min: 1, max: 100));
    }
}
