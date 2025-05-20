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
        private GrafoLibro grafo;
        List<Lectura> listaDeLecturas;
        Dictionary<Libro, float> DiccionarioLibrosRankeados;

        // Datos Usuario
        int id_user;
        string username;

        public Principal(int id_user = 0)
        {
            InitializeComponent();

            // Harcoding User 
            this.id_user = (id_user != 0) ? id_user : 1;

            grafo = new GrafoLibro();
            listaDeLecturas = new List<Lectura>();

            //
            grafo.ObtenerUsuarios();
            grafo.ObtenerLibros();

            flpLib.WrapContents = true;
            flpLib.AutoScroll = true;
            flpLib.MaximumSize = new Size(1077, 1150);
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            Label titulo = new Label();
            titulo.Text = "Nuestro Catalogo";
            titulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            titulo.ForeColor = Color.White;
            titulo.Margin = new Padding(10, 20, 0, 10); // Izquierda, Arriba, Derecha, Abajo
            titulo.AutoSize = false;
            titulo.Width = flpLib.Width - 10;
            titulo.Height = 30;

            //
            FlowLayoutPanel AleatoriaPanel = GenerarTarjetasGeneral();

            // Agregar al contenedor principal
            flpLib.Controls.Add(titulo);
            flpLib.Controls.Add(AleatoriaPanel);

            //
            Label titulo2 = new Label();
            titulo2.Text = "Recomendados por nuestros usuarios";
            titulo2.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            titulo2.ForeColor = Color.White;
            titulo2.Margin = new Padding(10, 20, 0, 10); // Izquierda, Arriba, Derecha, Abajo
            titulo2.AutoSize = false;
            titulo2.Width = flpLib.Width - 10;
            titulo2.Height = 30;

            //
            FlowLayoutPanel recomendadosPanel = GenerarTarjetasRecomendados(0);

            // Extrae los 8 con mejor PageRank
            flpLib.Controls.Add(titulo2);
            flpLib.Controls.Add(recomendadosPanel);


            //
            Label titulo3 = new Label();
            titulo3.Text = "Lo mejor de Tech";
            titulo3.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            titulo3.ForeColor = Color.White;
            titulo3.Margin = new Padding(10, 20, 0, 10); // Izquierda, Arriba, Derecha, Abajo
            titulo3.AutoSize = false;
            titulo3.Width = flpLib.Width - 10;
            titulo3.Height = 30;

            //
            FlowLayoutPanel recomendadosTechPanel = GenerarTarjetasRecomendados(4);

            // Extrae los 8 con mejor PageRank
            flpLib.Controls.Add(titulo3);
            flpLib.Controls.Add(recomendadosTechPanel);

            //
            Label titulo4 = new Label();
            titulo4.Text = "Descubre algo nuevo";
            titulo4.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            titulo4.ForeColor = Color.White;
            titulo4.Margin = new Padding(10, 20, 0, 10); // Izquierda, Arriba, Derecha, Abajo
            titulo4.AutoSize = false;
            titulo4.Width = flpLib.Width - 10;
            titulo4.Height = 30;

            //
            FlowLayoutPanel DesconocidosPanel = GenerarTarjetasDesconocidos();

            // Extrae los 8 con mejor PageRank
            flpLib.Controls.Add(titulo4);
            flpLib.Controls.Add(DesconocidosPanel);
        }

        private FlowLayoutPanel GenerarTarjetasGeneral()
        {
            int limit = 0;

            //
            List<Libro> librosGeneral = grafo.ObtenerLibros();

            FlowLayoutPanel panelLibros = new FlowLayoutPanel();
            panelLibros.FlowDirection = FlowDirection.LeftToRight;
            panelLibros.AutoSize = true;
            panelLibros.WrapContents = true;
            panelLibros.Height = 200; // alto de las tarjetas
            panelLibros.AutoScroll = false;
            panelLibros.BackColor = Color.Transparent;
            panelLibros.Margin = new Padding(10, 0, 0, 0);

            // Aquí agregas tus tarjetas (pueden ser UserControls personalizados)
            foreach (var libro in librosGeneral)
            {
                limit++;

                if(limit == 9)
                {
                    break;
                }

                var tarjeta = CrearTarjetaLibro(libro); // Devuelve un Panel, UserControl, etc.
                panelLibros.Controls.Add(tarjeta);
            }

            return panelLibros;
        }

        private FlowLayoutPanel GenerarTarjetasRecomendados(int id_genre = 0)
        {
            int limit = 0;

            List<Libro> librosRecomendados = new List<Libro>();

            // Obtenemos las lecturas por usuario desde nuestra DB
            listaDeLecturas = grafo.ObtenerBookRanking();

            // Construiremos el grafo a partir de las lecturas y libros
            grafo.ConstruirDesdeLecturas(listaDeLecturas); // Puede hacerse por detras no es necesario colocarlo aqui

            // Contruimos un diccionario que nos devolvera cada libro con su PageRank Asignado
            DiccionarioLibrosRankeados = grafo.CalcularPageRank();

            // Obtenemos los libros de nuestra DB
            foreach (var libro in DiccionarioLibrosRankeados.OrderByDescending(x => x.Value))
            {
                if (limit == 8)
                {
                    break;
                }

                if (id_genre == 0)
                {
                    limit++;
                    librosRecomendados.Add(libro.Key);
                }
                else
                {
                    if (libro.Key.Genre == id_genre)
                    {
                        limit++;
                        librosRecomendados.Add(libro.Key);
                    }
                }
            }

            FlowLayoutPanel panelLibros = new FlowLayoutPanel();
            panelLibros.FlowDirection = FlowDirection.LeftToRight;
            panelLibros.AutoSize = true;
            panelLibros.WrapContents = false;
            panelLibros.Height = 200; // alto de las tarjetas
            panelLibros.AutoScroll = true;
            panelLibros.BackColor = Color.Transparent;
            panelLibros.Margin = new Padding(10, 0, 0, 0);

            // Aquí agregas tus tarjetas (pueden ser UserControls personalizados)
            foreach (var libro in librosRecomendados)
            {
                var tarjeta = CrearTarjetaLibro(libro); // Devuelve un Panel, UserControl, etc.
                panelLibros.Controls.Add(tarjeta);
            }

            return panelLibros;
        }

        private FlowLayoutPanel GenerarTarjetasDesconocidos()
        {
            int limit = 0;

            //
            List<Libro> librosGeneral = grafo.ObtenerLibrosPocoConocidos();

            FlowLayoutPanel panelLibros = new FlowLayoutPanel();
            panelLibros.FlowDirection = FlowDirection.LeftToRight;
            panelLibros.AutoSize = true;
            panelLibros.WrapContents = true;
            panelLibros.Height = 200; // alto de las tarjetas
            panelLibros.AutoScroll = false;
            panelLibros.BackColor = Color.Transparent;
            panelLibros.Margin = new Padding(10, 0, 0, 0);

            // Aquí agregas tus tarjetas (pueden ser UserControls personalizados)
            foreach (var libro in librosGeneral)
            {
                limit++;

                if (limit == 9)
                {
                    break;
                }

                var tarjeta = CrearTarjetaLibro(libro); // Devuelve un Panel, UserControl, etc.
                panelLibros.Controls.Add(tarjeta);
            }

            return panelLibros;
        }

        private Panel CrearTarjetaLibro(Libro libro)
        {
            Panel tarjeta = new Panel();
            tarjeta.Width = 120;
            tarjeta.Height = 180;
            tarjeta.Margin = new Padding(5);
            tarjeta.BorderStyle = BorderStyle.FixedSingle;
            tarjeta.BackColor = Color.FromArgb(80, 80, 80);

            // Titulo
            Label lblTitulo = new Label();
            lblTitulo.Text = libro.Title;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblTitulo.Height = 40;
            lblTitulo.ForeColor = Color.White;

            // Imagen del libro
            PictureBox imagenLibro = new PictureBox();
            imagenLibro.Width = tarjeta.Width - 20;
            imagenLibro.Height = 100;
            imagenLibro.SizeMode = PictureBoxSizeMode.Zoom;
            imagenLibro.Image = Properties.Resources._default;
            imagenLibro.Location = new Point(10, 40);

            // Boton del libro
            Button btnVerMas = new Button();
            btnVerMas.Text = "Leer libro";
            btnVerMas.Height = 40;
            btnVerMas.Width = tarjeta.Width; // Ajusta al ancho completo
            btnVerMas.BackColor = Color.FromArgb(44, 194, 149);
            btnVerMas.ForeColor = Color.White;
            btnVerMas.FlatAppearance.BorderColor = Color.FromArgb(44, 194, 149);
            btnVerMas.FlatStyle = FlatStyle.Flat;
            btnVerMas.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnVerMas.Location = new Point(-1, 140);

            // Guarda el ID del libro en la propiedad Tag
            btnVerMas.Tag = libro.ID;

            // Agrega el evento Click
            btnVerMas.Click += BtnVerDetalles_Click;

            // Puedes agregar una imagen, autor, etc.
            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(imagenLibro);
            tarjeta.Controls.Add(btnVerMas);
            return tarjeta;
        }

        private void BtnVerDetalles_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                int libroId = (int)btn.Tag;

                // Abre un formulario de detalle y le pasa el ID
                DetallesLibro detallesForm = new DetallesLibro(libroId, id_user);
                detallesForm.Show(); // o .ShowDialog() si quieres modal
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UsuarioForm userForm = new UsuarioForm(id_user);
            userForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            grafo.ActualizarScoreLibros();
        }
    }
}
