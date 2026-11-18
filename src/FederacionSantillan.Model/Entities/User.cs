using FederacionSantillan.Model.Interfaces;

namespace FederacionSantillan.Model.Entities;

public class User(int id, string username, string passwordHash, string fullName, string role)
    : ICopiable<User>, IClonable<User>, IEquatableDomain<User>, IIdentifiable<int>
{
    private int userId = id;
    private string username = username, passwordHash = passwordHash, fullName = fullName, role = role;
    private bool active = true;

    public User(string username, string passwordHash, string fullName, string role)
        : this(0, username, passwordHash, fullName, role) { }

    public int Id => userId;
    public int UserId { get => userId; set => userId = value; }
    public string Username { get => username; set => username = value; }
    public string PasswordHash { get => passwordHash; set => passwordHash = value; }
    public string FullName { get => fullName; set => fullName = value; }
    public string Role { get => role; set => role = value; }
    public bool Active { get => active; set => active = value; }

    public int getUserId() => userId;
    public void setUserId(int id) => userId = id;
    public string getUsername() => username;
    public void setUsername(string u) => username = u;
    public string getPasswordHash() => passwordHash;
    public void setPasswordHash(string p) => passwordHash = p;
    public string getFullName() => fullName;
    public void setFullName(string n) => fullName = n;
    public string getRole() => role;
    public void setRole(string r) => role = r;
    public bool getActive() => active;
    public void setActive(bool a) => active = a;

    public void copy(User other)
    {
        if (other == null) return;
        (userId, username, passwordHash, fullName, role, active) =
            (other.userId, other.username, other.passwordHash, other.fullName, other.role, other.active);
    }

    public User clone() => new(userId, username, passwordHash, fullName, role) { active = active };

    public bool equals(User other) =>
        other != null && (userId, username, role) == (other.userId, other.username, other.role);

    public override string ToString() => $"{fullName} (@{username} - {role})";
}
