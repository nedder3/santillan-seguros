using FederacionSantillan.Model.Exceptions;
using FederacionSantillan.Model.Validation;
using FederacionSantillan.Tests.Factories;

namespace FederacionSantillan.Tests;

public class DtoValidationTests
{
    [Fact]
    public void Given_ValidInsuredDto_When_Validated_Then_DoesNotThrow()
    {
        var dto = TestDataFactory.CreateValidInsuredDto();

        var ex = Record.Exception(() => DtoValidator.Validate(dto));

        Assert.Null(ex);
    }

    [Fact]
    public void Given_InsuredDtoWithInvalidEmail_When_Validated_Then_ThrowsBusinessRuleException()
    {
        var dto = TestDataFactory.CreateValidInsuredDto();
        dto.Email = "not-an-email";

        Assert.Throws<BusinessRuleException>(() => DtoValidator.Validate(dto));
    }

    [Fact]
    public void Given_RegisterUserDtoWithMismatchedPasswords_When_Validated_Then_ThrowsBusinessRuleException()
    {
        var dto = TestDataFactory.CreateValidRegisterUserDto();
        dto.ConfirmPassword = "differentPassword";

        Assert.Throws<BusinessRuleException>(() => DtoValidator.Validate(dto));
    }

    [Fact]
    public void Given_PolicyDtoWithInstallmentsOutOfRange_When_Validated_Then_ThrowsBusinessRuleException()
    {
        var dto = TestDataFactory.CreateValidPolicyDto();
        dto.InstallmentsCount = 36;

        Assert.Throws<BusinessRuleException>(() => DtoValidator.Validate(dto));
    }
}
