using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoCatedraPED.Modelo;

namespace ProyectoCatedraPED
{
    public partial class RegistroForm : Form
    {
        bddLibros bd = new bddLibros();
        public RegistroForm()
        {
            InitializeComponent();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNombre.Text == "" || txtmail.Text == "" || txtpass.Text == "" || txtpassagain.Text == "")
                {
                    MessageBox.Show("Todos los campos deben ir llenos !", "Vuelva a intentarlo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (txtpass.Text != txtpassagain.Text)
                {
                    MessageBox.Show("Las contraseñas deben ser iguales !", "Vuelva a intentarlo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtpass.Clear();
                    txtpassagain.Clear();
                }
                else
                {
                  User user = new User
                {
                    fullname = txtNombre.Text, 
                    username = txtmail.Text, 
                    password = HashPassword(txtpass.Text),
                   
                };
                bd.Users.Add(user);
                    bd.SaveChanges();   
                MessageBox.Show("Usuario creado exitosamente !  Que disfrutes tu experiencia", "Ingreso correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Thread.Sleep(1000);
                    Principal login = new Principal();
                    login.Show();
                    this.Hide();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar los datos: " + ex.Message, "Error de conexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Hide();
        }

        private void RegistroForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
