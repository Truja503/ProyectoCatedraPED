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
    public partial class LecturaLibro : Form
    {
        private GrafoLibro grafo;

        public LecturaLibro(int libroId)
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            grafo = new GrafoLibro();
            grafo.ObtenerLibros();

            Libro libroDetallado = grafo.ObtenerLibro(libroId);

            this.Text = libroDetallado.Title;

        }
    }
}
