using System.ComponentModel.DataAnnotations;
using FederacionSantillan.Model.Exceptions;

namespace FederacionSantillan.Model.DTOs.Requests;

public class SignInRequestDto
{
    [Required(ErrorMessage = SystemMessages.CredentialsRequired)]
    public string Username { get; set; }

    [Required(ErrorMessage = SystemMessages.CredentialsRequired)]
    public string Password { get; set; }
}

public class PaymentRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = SystemMessages.SelectionRequired)]
    public int InstallmentId { get; set; }

    [Required(ErrorMessage = SystemMessages.SelectionRequired)]
    public string PaymentMethod { get; set; }

    public string Notes { get; set; } = string.Empty;
}
