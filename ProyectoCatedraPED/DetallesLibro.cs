using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoCatedraPED
{
    public partial class DetallesLibro : Form
    {
        private GrafoLibro grafo;
        int libro_id;

        //
        int id_user;

        public DetallesLibro(int libroId, int UserId)
        {
            InitializeComponent();

            //
            id_user = UserId;
        
            libro_id = libroId;
            this.DoubleBuffered = true;

            grafo = new GrafoLibro();
            grafo.ObtenerLibros();

            Libro libroDetallado = grafo.ObtenerLibro(libroId);

            this.Text = libroDetallado.Title;

            // Aquí puedes cargar los datos del libro por ID
            CargarDetallesLibro(libroDetallado);
        }

        private void CargarDetallesLibro(Libro libro)
        {
            //
            lblTitle.Text = libro.Title;
            lblGenero.Text = libro.GenreName;
            lblDescripcion.Text = libro.Description;
            lblNota.Text = "⭐" + libro.Score.ToString();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (LinearGradientBrush brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    Color.Transparent, // Color inicial (será manejado con blend)
                    Color.Black,       // Color final (también con blend)
                    LinearGradientMode.Horizontal))
                {
                    // Define el degradado personalizado
                    ColorBlend blend = new ColorBlend();
                blend.Colors = new Color[] {
                    Color.FromArgb(255, 0, 0, 0),   // 100%
                    Color.FromArgb(255, 11, 11, 11), // 45%
                    Color.FromArgb(255, 255, 255, 255)    // 0% - transparente
                };
    
                blend.Positions = new float[] { 0.0f, 0.50f, 1.0f };

                brush.InterpolationColors = blend;

                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLeer_Click(object sender, EventArgs e)
        {
            LecturaLibro lecturaLibroForm = new LecturaLibro(libro_id, id_user);
            lecturaLibroForm.Show();
            this.Hide();
        }
    }
}
