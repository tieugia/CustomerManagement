namespace CustomerManagement.Application.Interfaces.Services;

public interface IAuthService
{
    string? Login(string email, string password);
}
