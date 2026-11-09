using MySql.Data.MySqlClient;

namespace LoginBCrypt;

public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
}

// Acceso a datos
public static class UsuarioRepositorio
{
    private const string CadenaConexion = "Server=localhost;Database=parcial3_login;Uid=root;Pwd=1234;";

    // Genera el hash con BCrypt y lo guarda en la base de datos
    public static bool RegistrarUsuario(string nombreUsuario, string nombreCompleto, string password)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);

        using var conexion = new MySqlConnection(CadenaConexion);
        conexion.Open();

        using (var existe = new MySqlCommand(
            "SELECT COUNT(*) FROM usuarios WHERE nombre_usuario = @usuario", conexion))
        {
            existe.Parameters.AddWithValue("@usuario", nombreUsuario);
            if (Convert.ToInt32(existe.ExecuteScalar()) > 0)
                return false;
        }

        using var cmd = new MySqlCommand(
            "INSERT INTO usuarios (nombre_usuario, nombre_completo, password_hash) " +
            "VALUES (@usuario, @nombre, @hash)", conexion);
        cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
        cmd.Parameters.AddWithValue("@nombre", nombreCompleto);
        cmd.Parameters.AddWithValue("@hash", hash);
        cmd.ExecuteNonQuery();
        return true;
    }

    // Verifica la contraseña con BCrypt.Verify() y registra el intento
    public static Usuario? IniciarSesion(string nombreUsuario, string password)
    {
        using var conexion = new MySqlConnection(CadenaConexion);
        conexion.Open();

        Usuario? usuario = null;
        string? hashGuardado = null;

        using (var cmd = new MySqlCommand(
            "SELECT id, nombre_usuario, nombre_completo, password_hash " +
            "FROM usuarios WHERE nombre_usuario = @usuario", conexion))
        {
            cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
            using var lector = cmd.ExecuteReader();
            if (lector.Read())
            {
                usuario = new Usuario
                {
                    Id = lector.GetInt32(0),
                    NombreUsuario = lector.GetString(1),
                    NombreCompleto = lector.GetString(2)
                };
                hashGuardado = lector.GetString(3);
            }
        }

        if (usuario == null || hashGuardado == null)
        {
            RegistrarIntento(conexion, nombreUsuario, false, "Usuario no existe");
            return null;
        }

        bool valida = BCrypt.Net.BCrypt.Verify(password, hashGuardado);
        RegistrarIntento(conexion, nombreUsuario, valida,
            valida ? "Acceso exitoso" : "Contraseña incorrecta");

        return valida ? usuario : null;
    }

    // Guarda cada intento (exitoso o fallido) con el usuario y la fecha
    private static void RegistrarIntento(MySqlConnection conexion, string nombreUsuario,
                                         bool exitoso, string detalle)
    {
        using var cmd = new MySqlCommand(
            "INSERT INTO intentos_login (nombre_usuario, exitoso, detalle, fecha) " +
            "VALUES (@usuario, @exitoso, @detalle, NOW())", conexion);
        cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
        cmd.Parameters.AddWithValue("@exitoso", exitoso);
        cmd.Parameters.AddWithValue("@detalle", detalle);
        cmd.ExecuteNonQuery();
    }
}