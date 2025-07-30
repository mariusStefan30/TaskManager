namespace TaskManager.Models
{
    // a simple Data Transfer Object
    public class LoginDto
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
