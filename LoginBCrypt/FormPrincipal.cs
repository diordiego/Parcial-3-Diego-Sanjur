namespace LoginBCrypt;

// Se muestra tras un acceso exitoso e indica el nombre del usuario
public class FormPrincipal : Form
{
    public FormPrincipal(Usuario usuario)
    {
        Text = "Inicio";
        ClientSize = new Size(560, 170);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Font = new Font("Segoe UI", 10);

        var lblBienvenida = new Label
        {
            Text = $"¡Bienvenido, {usuario.NombreCompleto}!",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(20, 25),
            AutoSize = true
        };

        var lblUsuario = new Label
        {
            Text = $"Usuario: {usuario.NombreUsuario}",
            Location = new Point(22, 70),
            AutoSize = true
        };

        var btnCerrar = new Button
        {
            Text = "Cerrar sesión",
            Location = new Point(22, 110),
            Size = new Size(140, 32)
        };
        btnCerrar.Click += (s, e) => Close();

        Controls.AddRange(new Control[] { lblBienvenida, lblUsuario, btnCerrar });
    }
}