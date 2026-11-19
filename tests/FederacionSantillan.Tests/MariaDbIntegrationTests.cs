using FederacionSantillan.Data.DAOs;
using FederacionSantillan.Data.Factories;
using FederacionSantillan.Model.DTOs.Requests;
using FederacionSantillan.Model.Exceptions;

namespace FederacionSantillan.Tests;

[Trait("Category", "Integration")]
public class MariaDbIntegrationTests
{
    [Fact]
    public void Given_RunningMariaDbContainer_When_QueryingBranches_Then_ReturnsSeededCatalog()
    {
        if (!ConnectionFactory.TestConnection(out _))
            return;

        var branches = new BranchDao().GetAll();

        Assert.NotEmpty(branches);
    }

    [Fact]
    public void Given_NonExistentUser_When_Authenticating_Then_ThrowsInvalidCredentialsException()
    {
        if (!ConnectionFactory.TestConnection(out _))
            return;

        var dao = new UserDao();
        var dto = new SignInRequestDto { Username = "non_existent_user_999", Password = "somePassword" };

        var ex = Assert.Throws<InvalidCredentialsException>(() => dao.Authenticate(dto));
        Assert.Equal(SystemMessages.InvalidCredentials, ex.Message);
        Assert.Equal("Autenticación", ex.Title);
    }

    [Fact]
    public void Given_ExistingUserWithWrongPassword_When_Authenticating_Then_ThrowsSameInvalidCredentialsExceptionWithoutLeakingUserExistence()
    {
        if (!ConnectionFactory.TestConnection(out _))
            return;

        var dao = new UserDao();
        var dto = new SignInRequestDto { Username = "santillan", Password = "wrong_password_123" };

        var ex = Assert.Throws<InvalidCredentialsException>(() => dao.Authenticate(dto));
        Assert.Equal(SystemMessages.InvalidCredentials, ex.Message);
        Assert.Equal("Autenticación", ex.Title);
    }
}
