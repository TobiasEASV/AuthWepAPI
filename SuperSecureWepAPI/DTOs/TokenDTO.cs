namespace SuperSecureWepAPI.DTOs;

public class TokenDTO
{
    public bool success { get; set; }
    public string message { get; set; }
    public string token { get; set; }

    public TokenDTO(bool success, string message, string token)
    {
        this.success = success;
        this.message = message;
        this.token = token;
    }
}