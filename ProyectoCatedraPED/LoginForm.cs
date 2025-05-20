using ProyectoCatedraPED.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoCatedraPED
{
    public partial class LoginForm : Form
    {
        bddLibros bd = new bddLibros();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if(textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Todos los campos deben ir llenos !", "Vuelva a intentarlo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    string name = textBox1.Text;
                    string passw = HashPassword(textBox2.Text);

                    Console.WriteLine("PASO 1");

                    var usuario = bd.Users.FirstOrDefault(u => u.username == name);
                    
                    Console.WriteLine("PASO 2");

                    if (usuario != null && usuario.password == passw)
                    {
                        MessageBox.Show($"¡Bienvenido {usuario.fullname}!");
                        Principal principal = new Principal(usuario.id);
                        principal.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Correo o contraseña incorrectos.");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar los datos: " + ex.Message);

            }

        }
        string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistroForm r= new RegistroForm();
            r.Show();
            this.Hide();
        }
    }
}
