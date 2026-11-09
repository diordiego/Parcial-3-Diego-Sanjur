namespace LoginBCrypt;

public class FormRegistro : Form
{
    private readonly TextBox txtNombre = new();
    private readonly TextBox txtUsuario = new();
    private readonly TextBox txtPassword = new();
    private readonly TextBox txtConfirmar = new();

    public FormRegistro()
    {
        Text = "Registrar usuario";
        ClientSize = new Size(410, 260);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("Segoe UI", 10);

        AgregarCampo("Nombre completo:", txtNombre, 20);
        AgregarCampo("Usuario:", txtUsuario, 60);
        AgregarCampo("Contraseña:", txtPassword, 100);
        AgregarCampo("Confirmar:", txtConfirmar, 140);
        txtPassword.PasswordChar = '●';
        txtConfirmar.PasswordChar = '●';

        var btnGuardar = new Button
        {
            Text = "Guardar",
            Location = new Point(130, 190),
            Size = new Size(150, 34)
        };
        btnGuardar.Click += BtnGuardar_Click;

        AcceptButton = btnGuardar;
        Controls.Add(btnGuardar);
    }

    private void AgregarCampo(string etiqueta, TextBox caja, int y)
    {
        Controls.Add(new Label { Text = etiqueta, Location = new Point(15, y + 3), AutoSize = true });
        caja.Location = new Point(185, y);
        caja.Width = 200;
        Controls.Add(caja);
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        string nombre = txtNombre.Text.Trim();
        string usuario = txtUsuario.Text.Trim();
        string password = txtPassword.Text;

        if (nombre == "" || usuario == "" || password == "")
        {
            MessageBox.Show("Complete todos los campos.", "Aviso",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (password != txtConfirmar.Text)
        {
            MessageBox.Show("Las contraseñas no coinciden.", "Aviso",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            if (UsuarioRepositorio.RegistrarUsuario(usuario, nombre, password))
            {
                MessageBox.Show("Usuario registrado. La contraseña se guardó correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Ese nombre de usuario ya existe.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al guardar:\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}