namespace SuperSecureWepAPI.Services;

public class AuthenticationService : IAuthenticationService
{

    private string _salt = "A_TOTALLY_RANDOM_SALT";

    private string[] _userNames = { "Peter", "Alex" };
    private string[] _passwords = { "password123", "super_secret" };

    
    
    public bool ValidateUser(string userName, string password)
    {
        throw new NotImplementedException();
    }
    
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
    
}