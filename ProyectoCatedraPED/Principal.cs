using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoCatedraPED
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
          

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Principal_Load(object sender, EventArgs e)
        {
            Libro libro1 = new Libro();
            Libro libro2 = new Libro();

            libro1.Autor = "Hola";
            libro2.Autor = "Hola2";
            libro2.Titulo = "JAJAJAJJA";
            libro1.Titulo = "Barcelona";

            TarjetaLibro tl = new TarjetaLibro();
            TarjetaLibro tl1 = new TarjetaLibro();
            tl.CargarDatos(libro1);
            tl1.CargarDatos(libro2);

            flpLib.Controls.Add(tl);
            flpLib.Controls.Add(tl1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            UsuarioForm userForm = new UsuarioForm();
            userForm.Show();
            this.Hide();
        }
    }
}
