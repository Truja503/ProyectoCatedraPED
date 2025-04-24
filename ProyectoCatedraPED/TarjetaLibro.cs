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
    public partial class TarjetaLibro : UserControl
    {
        public TarjetaLibro()
        {
            InitializeComponent();
        }
        public void CargarDatos(Libro libro)
        {
            lblTitulo.Text = libro.Titulo;
            lblAutor.Text = libro.Autor;
        }
    }
}
