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
    public partial class CalificarLibro : Form
    {
        private int currentRating = 0;
        private PictureBox[] stars;

        public int UserRating => currentRating; // Puedes acceder a esto al cerrar

        public CalificarLibro()
        {
            InitializeComponent();
            InicializarEstrellas();
        }

        private void InicializarEstrellas()
        {
            stars = new PictureBox[] { star1, star2, star3, star4, star5 };

            for (int i = 0; i < stars.Length; i++)
            {
                int index = i; // Captura local
                stars[i].Image = Properties.Resources.estrella_vacia;
                stars[i].Cursor = Cursors.Hand;

                stars[i].MouseEnter += (s, e) => PintarEstrellas(index + 1);
                stars[i].MouseLeave += (s, e) => PintarEstrellas(currentRating);
                stars[i].Click += (s, e) =>
                {
                    currentRating = index + 1;
                    PintarEstrellas(currentRating);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };
            }
        }

        private void PintarEstrellas(int cantidad)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].Image = i < cantidad ? Properties.Resources.estrella_llena : Properties.Resources.estrella_vacia;
            }
        }
    }
}
