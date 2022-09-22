namespace SuperSecureWepAPI.Services;

public class AuthenticationService : IAuthenticationService
{
    private string[] _userNames = { "Peter", "Alex" };
    private Dictionary<string, byte[]> _userNameToHash;
    private Dictionary<string, byte[]> _userNameToSalt;

    public AuthenticationService()
    {
        _userNameToHash = new Dictionary<string, byte[]>();
        _userNameToSalt = new Dictionary<string, byte[]>();

        CreateUser("Peter", "p@ssword");
        CreateUser("Alex", "secret");
    }

    public void CreateUser(string userName, string password)
    {
        byte[] salt;
        byte[] passwordHash;
        CreatePasswordHash(password, out passwordHash, out salt);
        _userNameToHash.Add(userName, passwordHash);
        _userNameToSalt.Add(userName, salt);
    }

    public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
        }

        return true;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public bool ValidateUser(string userName, string password, out string token)
    {
        byte[] storePasswordHash = _userNameToHash[userName];
        byte[] salt = _userNameToSalt[userName];

        bool isOk = VerifyPasswordHash(password, storePasswordHash, salt);

        token = "TBA.";
        
        return isOk;
    }
}