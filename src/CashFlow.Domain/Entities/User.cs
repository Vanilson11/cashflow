using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;
public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; }//identificador único que vai dentro do TOKEN para identificar o usuário
    public string Role { get; set; } = Roles.TEAM_MEMBER;
}
