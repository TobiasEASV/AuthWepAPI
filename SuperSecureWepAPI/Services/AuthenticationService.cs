using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace SuperSecureWepAPI.Services;

public class AuthenticationService : IAuthenticationService
{
    private string[] _userNames = { "Peter", "Alex" };
    private Dictionary<string, byte[]> _userNameToHash;
    private Dictionary<string, byte[]> _userNameToSalt;

    private byte[] _secretBytes;

    public AuthenticationService(byte[] secretBytes)
    {
        _secretBytes = secretBytes;
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

    private string CreateToken(string username)
    {
        List<Claim> claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
        var token = new JwtSecurityToken(
            new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(_secretBytes),
                SecurityAlgorithms.HmacSha512)),
            new JwtPayload(null, null, claims, DateTime.Now, DateTime.Now.AddMinutes(10))
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateUser(string userName, string password, out string token)
    {
        byte[] storePasswordHash = _userNameToHash[userName];
        byte[] salt = _userNameToSalt[userName];

        bool isOk = VerifyPasswordHash(password, storePasswordHash, salt);

        token = isOk ? CreateToken(userName) : "";

        return isOk;
    }
}