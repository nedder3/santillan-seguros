using FederacionSantillan.Core.Domain.Abstractions;

namespace FederacionSantillan.Core;

/// <summary>
/// Entidad de Dominio: Usuario del sistema (Autenticación y Control de Acceso).
/// Implementa Tríada Sagrada (copy, clone, equals) y expression bodies.
/// </summary>
public class Usuario : ICopiable<Usuario>, IClonable<Usuario>, IIdentificable<int>
{
    private int idUsuario;
    private string username, passwordHash, nombreCompleto, rol;
    private bool activo;

    public int Id => idUsuario;

    public Usuario(int id, string user, string passHash, string nombre, string r)
    {
        idUsuario = id;
        username = user;
        passwordHash = passHash;
        nombreCompleto = nombre;
        rol = r;
        activo = true;
    }

    public Usuario(string user, string passHash, string nombre, string r)
        : this(0, user, passHash, nombre, r) { }

    // Propiedades
    public int IdUsuario { get => idUsuario; set => idUsuario = value; }
    public string Username { get => username; set => username = value; }
    public string PasswordHash { get => passwordHash; set => passwordHash = value; }
    public string NombreCompleto { get => nombreCompleto; set => nombreCompleto = value; }
    public string Rol { get => rol; set => rol = value; }
    public bool Activo { get => activo; set => activo = value; }

    // Getters y Setters explícitos (Convención Cátedra)
    public int obtenerIdUsuario() => idUsuario;
    public void establecerIdUsuario(int id) => idUsuario = id;
    public string obtenerUsername() => username;
    public void establecerUsername(string u) => username = u;
    public string obtenerPasswordHash() => passwordHash;
    public void establecerPasswordHash(string p) => passwordHash = p;
    public string obtenerNombreCompleto() => nombreCompleto;
    public void establecerNombreCompleto(string n) => nombreCompleto = n;
    public string obtenerRol() => rol;
    public void establecerRol(string r) => rol = r;
    public bool obtenerActivo() => activo;
    public void establecerActivo(bool a) => activo = a;

    // Tríada Sagrada
    public void copy(Usuario u)
    {
        if (u == null) return;
        idUsuario = u.idUsuario;
        username = u.username;
        passwordHash = u.passwordHash;
        nombreCompleto = u.nombreCompleto;
        rol = u.rol;
        activo = u.activo;
    }

    public Usuario clone() => new Usuario(idUsuario, username, passwordHash, nombreCompleto, rol) { activo = activo };

    public bool equals(Usuario u) =>
        u != null && idUsuario == u.idUsuario && username == u.username && rol == u.rol;

    public override string ToString() => $"{nombreCompleto} (@{username} - {rol})";
}
