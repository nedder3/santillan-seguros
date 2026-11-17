using FederacionSantillan.Core.DTOs;
using FederacionSantillan.Core.Exceptions;
using FederacionSantillan.Core.Validation;

namespace FederacionSantillan.Tests;

public class DtoValidationTests
{
    [Fact]
    public void Validar_AseguradoValido_NoLanzaExcepcion()
    {
        var dto = new AseguradoInputDTO
        {
            DniCuit = "20-34567890-1",
            ApellidoNombre = "Gómez, Carlos",
            Telefono = "291-4123456",
            Email = "carlos@email.com",
            Domicilio = "Av. Alem 1240"
        };

        var ex = Record.Exception(() => DtoValidator.Validar(dto));
        Assert.Null(ex);
    }

    [Fact]
    public void Validar_AseguradoSinDni_LanzaNegocioException()
    {
        var dto = new AseguradoInputDTO
        {
            DniCuit = "",
            ApellidoNombre = "Gómez, Carlos",
            Telefono = "291-4123456"
        };

        Assert.Throws<NegocioException>(() => DtoValidator.Validar(dto));
    }

    [Fact]
    public void Validar_AseguradoEmailInvalido_LanzaNegocioException()
    {
        var dto = new AseguradoInputDTO
        {
            DniCuit = "20-34567890-1",
            ApellidoNombre = "Gómez, Carlos",
            Telefono = "291-4123456",
            Email = "correo-invalido"
        };

        Assert.Throws<NegocioException>(() => DtoValidator.Validar(dto));
    }

    [Fact]
    public void Validar_RegistroPasswordsDistintas_LanzaNegocioException()
    {
        var dto = new RegistroUsuarioDTO
        {
            NombreCompleto = "Juan Pérez",
            Username = "jperez",
            Password = "password123",
            ConfirmarPassword = "otra",
            Rol = "PRODUCTOR"
        };

        Assert.Throws<NegocioException>(() => DtoValidator.Validar(dto));
    }

    [Fact]
    public void Validar_PolizaCantidadCuotasFueraDeRango_LanzaNegocioException()
    {
        var dto = new PolizaInputDTO
        {
            NumeroPoliza = 900101,
            IdAsegurado = 1,
            IdRamo = 1,
            Compania = "Federación Patronal Seguros",
            BienAsegurado = "Toyota Hilux",
            PrimaTotal = 150000m,
            CantidadCuotas = 36
        };

        Assert.Throws<NegocioException>(() => DtoValidator.Validar(dto));
    }
}
