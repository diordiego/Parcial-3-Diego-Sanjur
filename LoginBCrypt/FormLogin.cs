namespace LoginBCrypt;

public class FormLogin : Form
{
    private readonly TextBox txtUsuario = new();
    private readonly TextBox txtPassword = new();
    private readonly Button btnIngresar = new();
    private readonly Button btnRegistrar = new();

    public FormLogin()
    {
        Text = "Login - BCrypt";
        ClientSize = new Size(340, 230);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Font = new Font("Segoe UI", 10);

        var lblTitulo = new Label
        {
            Text = "Iniciar sesión",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(20, 15),
            AutoSize = true
        };

        var lblUsuario = new Label { Text = "Usuario:", Location = new Point(20, 65), AutoSize = true };
        txtUsuario.Location = new Point(120, 62);
        txtUsuario.Width = 190;

        var lblPassword = new Label { Text = "Contraseña:", Location = new Point(20, 105), AutoSize = true };
        txtPassword.Location = new Point(120, 102);
        txtPassword.Width = 190;
        txtPassword.PasswordChar = '●'; // solo oculta en pantalla, no aplica hash

        btnIngresar.Text = "Ingresar";
        btnIngresar.Location = new Point(75, 145);
        btnIngresar.Size = new Size(190, 32);
        btnIngresar.Click += BtnIngresar_Click;

        btnRegistrar.Text = "Registrar usuario";
        btnRegistrar.Location = new Point(75, 185);
        btnRegistrar.Size = new Size(190, 30);
        btnRegistrar.Click += (s, e) => new FormRegistro().ShowDialog(this);

        AcceptButton = btnIngresar;
        Controls.AddRange(new Control[]
        {
            lblTitulo, lblUsuario, txtUsuario, lblPassword, txtPassword, btnIngresar, btnRegistrar
        });
    }

    private void BtnIngresar_Click(object? sender, EventArgs e)
    {
        string usuario = txtUsuario.Text.Trim();
        string password = txtPassword.Text;

        if (usuario == "" || password == "")
        {
            MessageBox.Show("Ingrese usuario y contraseña.", "Aviso",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Usuario? encontrado = UsuarioRepositorio.IniciarSesion(usuario, password);

            if (encontrado != null)
            {
                // Acceso exitoso: se muestra el nombre del usuario
                Hide();
                new FormPrincipal(encontrado).ShowDialog();
                txtPassword.Clear();
                Show();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al conectar con la base de datos:\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}