using System.ComponentModel.DataAnnotations;
using FederacionSantillan.Tests.Factories;

namespace FederacionSantillan.Tests;

public class DtoValidationTests
{
    private static bool ValidateDto(object dto, out List<ValidationResult> results)
    {
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(dto, new ValidationContext(dto), results, validateAllProperties: true);
    }

    [Fact]
    public void Given_ValidInsuredDto_When_Validated_Then_ReturnsTrue()
    {
        var dto = TestDataFactory.CreateValidInsuredDto();

        bool valid = ValidateDto(dto, out var errors);

        Assert.True(valid);
        Assert.Empty(errors);
    }

    [Fact]
    public void Given_InsuredDtoWithInvalidEmail_When_Validated_Then_ReturnsFalse()
    {
        var dto = TestDataFactory.CreateValidInsuredDto();
        dto.Email = "not-an-email";

        bool valid = ValidateDto(dto, out var errors);

        Assert.False(valid);
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void Given_InsuredDtoWithLettersInPhoneOrTaxId_When_Validated_Then_ReturnsFalse()
    {
        var dto = TestDataFactory.CreateValidInsuredDto();
        dto.Phone = "asdsadsa";
        dto.TaxId = "abc-12345";

        bool valid = ValidateDto(dto, out var errors);

        Assert.False(valid);
        Assert.Equal(2, errors.Count);
    }

    [Fact]
    public void Given_RegisterUserDtoWithMismatchedPasswords_When_Validated_Then_ReturnsFalse()
    {
        var dto = TestDataFactory.CreateValidRegisterUserDto();
        dto.ConfirmPassword = "differentPassword";

        bool valid = ValidateDto(dto, out var errors);

        Assert.False(valid);
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void Given_PolicyDtoWithEndDateBeforeStartDate_When_Validated_Then_FailsIValidatableObjectRule()
    {
        var dto = TestDataFactory.CreateValidPolicyDto();
        dto.EndDate = dto.StartDate.AddDays(-1);

        bool valid = ValidateDto(dto, out var errors);

        Assert.False(valid);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(dto.EndDate)));
    }

    [Fact]
    public void Given_SignInDtoWithEmptyOrWhitespace_When_Validated_Then_ReturnsFalse()
    {
        var dto = new FederacionSantillan.Model.DTOs.Requests.SignInRequestDto
        {
            Username = "   ",
            Password = ""
        };

        bool valid = ValidateDto(dto, out var errors);

        Assert.False(valid);
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void Given_RegisterUserDtoWithInvalidCharactersInUsername_When_Validated_Then_ReturnsFalse()
    {
        var dto = TestDataFactory.CreateValidRegisterUserDto();
        dto.Username = "user with spaces!";

        bool valid = ValidateDto(dto, out var errors);

        Assert.False(valid);
        Assert.Contains(errors, e => e.ErrorMessage.Contains("solo puede contener letras, números"));
    }

    [Fact]
    public void Given_PolicyDtoWithFirstDueBeforeStartDate_When_Validated_Then_FailsValidation()
    {
        var dto = TestDataFactory.CreateValidPolicyDto();
        dto.FirstDueDate = dto.StartDate.AddDays(-5);

        bool valid = ValidateDto(dto, out var errors);

        Assert.False(valid);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(dto.FirstDueDate)));
    }
}
