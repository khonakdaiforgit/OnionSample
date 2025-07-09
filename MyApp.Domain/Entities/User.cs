namespace MyApp.Domain.Entities;

public class User
{
    public Guid Id { get;  set; }
    public string Username { get;  set; }
    public string Email { get;  set; }
    public string PasswordHash { get;  set; }


    public User(string username, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
    }
}
