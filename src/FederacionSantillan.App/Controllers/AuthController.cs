using System.Security.Cryptography;
using System.Text;
using FederacionSantillan.Data.DAOs;
using FederacionSantillan.Model;
using FederacionSantillan.Model.DTOs;
using FederacionSantillan.Model.Exceptions;
using FederacionSantillan.Model.Validation;

namespace FederacionSantillan.App.Controllers;

public class AuthController
{
    private readonly UserDao _userDao = new UserDao();

    public static User CurrentUser { get; private set; }

    public User SignIn(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new BusinessRuleException(SystemMessages.CredentialsRequired);

        var user = _userDao.Authenticate(username.Trim(), ComputeSha256(password));
        if (user == null)
            throw new BusinessRuleException(SystemMessages.InvalidCredentials);

        CurrentUser = user;
        return user;
    }

    public void RegisterUser(RegisterUserDto dto)
    {
        DtoValidator.Validate(dto);

        string cleanUser = dto.Username.Trim();
        if (_userDao.ExistsUsername(cleanUser))
            throw new DuplicateEntityException(SystemMessages.Format(SystemMessages.DuplicateUsername, cleanUser));

        var user = new User(cleanUser, ComputeSha256(dto.Password), dto.FullName.Trim(), dto.Role);
        _userDao.Register(user);
    }

    public void SignOut() => CurrentUser = null;

    public bool VerifyCurrentPassword(string password)
    {
        if (CurrentUser == null || string.IsNullOrWhiteSpace(password))
            return false;

        return string.Equals(CurrentUser.getPasswordHash(), ComputeSha256(password), StringComparison.OrdinalIgnoreCase);
    }

    public static string ComputeSha256(string text)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
