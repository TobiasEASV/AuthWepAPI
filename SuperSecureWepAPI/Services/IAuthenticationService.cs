namespace SuperSecureWepAPI.Services;

public interface IAuthenticationService
{

    public bool ValidateUser(string userName, string password, out string token);

}