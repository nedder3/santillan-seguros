using FederacionSantillan.Data.DAOs;
using FederacionSantillan.Model.DTOs.Requests;
using FederacionSantillan.Model.Entities;

namespace FederacionSantillan.App.Controllers;

public class AuthController(UserDao userDao = null)
{
    private readonly UserDao _userDao = userDao ?? new();

    public static User CurrentUser { get; private set; }

    public User SignIn(SignInRequestDto dto) => CurrentUser = _userDao.Authenticate(dto);

    public void RegisterUser(RegisterUserRequestDto dto) => _userDao.Register(dto);

    public void SignOut() => CurrentUser = null;

    public bool VerifyCurrentPassword(string password) =>
        CurrentUser != null &&
        string.Equals(CurrentUser.getPasswordHash(), UserDao.ComputeSha256(password), StringComparison.OrdinalIgnoreCase);
}
