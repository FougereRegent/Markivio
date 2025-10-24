namespace Markivio.Domain.Entities;


public sealed class User : Entity
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AuthId { get; set; } = string.Empty;
}
