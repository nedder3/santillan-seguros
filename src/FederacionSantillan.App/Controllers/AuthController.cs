using System.Security.Cryptography;
using System.Text;
using FederacionSantillan.Core;
using FederacionSantillan.Core.DTOs;
using FederacionSantillan.Core.Exceptions;
using FederacionSantillan.Core.Validation;
using FederacionSantillan.Data.DAOs;

namespace FederacionSantillan.App.Controllers;

public class AuthController
{
    private readonly UsuarioDAO _usuarioDao = new UsuarioDAO();

    public static Usuario UsuarioActual { get; private set; }

    public Usuario IniciarSesion(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new NegocioException(MensajesSistema.CredencialesRequeridas);

        var usuario = _usuarioDao.Autenticar(username.Trim(), CalcularSha256(password));
        if (usuario == null)
            throw new NegocioException(MensajesSistema.CredencialesInvalidas);

        UsuarioActual = usuario;
        return usuario;
    }

    public void RegistrarUsuario(RegistroUsuarioDTO dto)
    {
        DtoValidator.Validar(dto);

        string user = dto.Username.Trim();
        if (_usuarioDao.ExisteUsername(user))
            throw new EntidadDuplicadaException(MensajesSistema.Formatear(MensajesSistema.UsuarioDuplicado, user));

        var nuevo = new Usuario(user, CalcularSha256(dto.Password), dto.NombreCompleto.Trim(), dto.Rol);
        _usuarioDao.Registrar(nuevo);
    }

    public void CerrarSesion() => UsuarioActual = null;

    public static string CalcularSha256(string texto)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
