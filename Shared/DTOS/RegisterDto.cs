namespace Shared.DTOS;

public record RegisterDto(string Username, string Email, string Password,ICollection<string>? Roles);
