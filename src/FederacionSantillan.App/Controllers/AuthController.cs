using System.Security.Cryptography;
using System.Text;
using FederacionSantillan.Core;
using FederacionSantillan.Data.DAOs;

namespace FederacionSantillan.App.Controllers;

/// <summary>
/// Controlador MVC de Autenticación: Gestiona el inicio de sesión y alta de usuarios contra MariaDB.
/// </summary>
public class AuthController
{
    private readonly UsuarioDAO _usuarioDao = new UsuarioDAO();

    public static Usuario UsuarioActual { get; private set; }

    public (bool Exito, string Mensaje, Usuario Usuario) IniciarSesion(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return (false, "Debe ingresar usuario y contraseña.", null);

        string hash = CalcularSha256(password);
        var usuario = _usuarioDao.Autenticar(username.Trim(), hash);
        if (usuario == null)
            return (false, "Usuario o contraseña incorrectos, o cuenta inactiva.", null);

        UsuarioActual = usuario;
        return (true, $"Bienvenido, {usuario.obtenerNombreCompleto()}.", usuario);
    }

    public (bool Exito, string Mensaje) RegistrarUsuario(string nombreCompleto, string username, string password, string confirmarPassword, string rol)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return (false, "Todos los campos son obligatorios.");

        if (username.Trim().Length < 3)
            return (false, "El nombre de usuario debe tener al menos 3 caracteres.");

        if (password.Length < 4)
            return (false, "La contraseña debe tener al menos 4 caracteres.");

        if (password != confirmarPassword)
            return (false, "Las contraseñas ingresadas no coinciden.");

        if (_usuarioDao.ExisteUsername(username.Trim()))
            return (false, $"El usuario '{username.Trim()}' ya existe en el sistema.");

        var nuevo = new Usuario(username.Trim(), CalcularSha256(password), nombreCompleto.Trim(), rol);
        bool creado = _usuarioDao.Registrar(nuevo);
        return creado
            ? (true, "Usuario registrado exitosamente. Ya puede iniciar sesión.")
            : (false, "No se pudo registrar el usuario en la base de datos.");
    }

    public void CerrarSesion() => UsuarioActual = null;

    public static string CalcularSha256(string texto)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
