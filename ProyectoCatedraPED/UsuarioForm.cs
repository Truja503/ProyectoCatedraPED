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
    public partial class UsuarioForm : Form
    {
        private GrafoLibro grafo;
        List<Lectura> listaDeLecturas;
        private Dictionary<Libro, Point> posicionesNodos = new Dictionary<Libro, Point>();

        //
        bool listoDibujo;

        //
        Random rand = new Random();
        List<Libro> libros;
        int radio = 40;
        int margenMinimo = 50;
        int margenArista = 20;

        public UsuarioForm()
        {
            InitializeComponent();

            grafo = new GrafoLibro();
            listaDeLecturas = new List<Lectura>();
            listoDibujo = false;

            grafo.ObtenerUsuarios();
            grafo.ObtenerLibros();

            libros = grafo.ListLibro;

            this.DoubleBuffered = true; // Evita parpadeo al dibujar
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Principal principal = new Principal();
            principal.Show();

            this.Close();

        }

        private void button4_Leave(object sender, EventArgs e)
        {

        }

        private void UsuarioForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Principal pForm = new Principal();
            pForm.Show();
            this.Hide();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            listoDibujo = true;

            // Obtenemos las lecturas por usuario desde nuestra DB
            listaDeLecturas = grafo.ObtenerBookRanking();

            // Construiremos el grafo a partir de las lecturas y libros
            grafo.ConstruirDesdeLecturas(listaDeLecturas); // Puede hacerse por detras no es necesario colocarlo aqui

            //
            GenerarPosicionesAleatorias();

            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.FromArgb(30, 30, 30), 2)) // puedes cambiar el grosor aquí
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel1.Width - 1, panel1.Height - 1);
            }

            if (listoDibujo)
            {
                Console.WriteLine("LISTO");
                Dictionary<Libro, List<(Libro destino, float peso)>> conexiones = ObtenerConexiones();

                // Dibujar aristas (flechas)
                foreach (KeyValuePair<Libro, List<(Libro destino, float peso)>> par in conexiones)
                {
                    Point origen = posicionesNodos[par.Key];
                    foreach (var conexion in par.Value)
                    {
                        Libro destinoLibro = conexion.destino;
                        Point destino;
                        if (posicionesNodos.TryGetValue(destinoLibro, out destino))
                        {
                            DibujarFlecha(e.Graphics, origen, destino);
                        }
                    }
                }

                // Dibujar nodos (libros)
                foreach (KeyValuePair<Libro, Point> libro in posicionesNodos)
                {
                    DibujarNodo(e.Graphics, libro.Key, libro.Value);
                }
            }
        }

        private void GenerarPosicionesAleatorias()
        {
            posicionesNodos.Clear();

            foreach (Libro libro in libros)
            {
                Point nuevaPos;
                bool solapa;

                do
                {
                    int x = rand.Next(margenMinimo, panel1.Width - margenMinimo - radio);
                    int y = rand.Next(margenMinimo, panel1.Height - margenMinimo - radio);
                    nuevaPos = new Point(x, y);

                    solapa = false;

                    // Verificar solapamiento con otros nodos
                    foreach (Point posExistente in posicionesNodos.Values)
                    {
                        double distancia = Math.Sqrt(Math.Pow(nuevaPos.X - posExistente.X, 2) + Math.Pow(nuevaPos.Y - posExistente.Y, 2));
                        if (distancia < radio + margenMinimo)
                        {
                            solapa = true;
                            break;
                        }
                    }

                    // Verificar si está muy cerca de una arista
                    if (!solapa)
                    {
                        foreach (var conexion in grafo.Conexiones)
                        {
                            Libro origen = conexion.Key;
                            if (!posicionesNodos.ContainsKey(origen))
                                continue;

                            Point p1 = posicionesNodos[origen];

                            foreach ((Libro destino, float peso) destino in conexion.Value)
                            {
                                if (!posicionesNodos.ContainsKey(destino.destino))
                                    continue;

                                Point p2 = posicionesNodos[destino.destino];

                                double distArista = DistanciaPuntoASegmento(nuevaPos, p1, p2);
                                if (distArista < margenArista)
                                {
                                    solapa = true;
                                    break;
                                }
                            }

                            if (solapa)
                                break;
                        }
                    }

                } while (solapa);

                posicionesNodos[libro] = nuevaPos;
            }
        }

        public static double DistanciaPuntoASegmento(Point p, Point v, Point w)
        {
            double l2 = Math.Pow(w.X - v.X, 2) + Math.Pow(w.Y - v.Y, 2);
            if (l2 == 0) return Math.Sqrt(Math.Pow(p.X - v.X, 2) + Math.Pow(p.Y - v.Y, 2)); // v == w

            double t = ((p.X - v.X) * (w.X - v.X) + (p.Y - v.Y) * (w.Y - v.Y)) / l2;
            t = Math.Max(0, Math.Min(1, t));
            double projX = v.X + t * (w.X - v.X);
            double projY = v.Y + t * (w.Y - v.Y);

            return Math.Sqrt(Math.Pow(p.X - projX, 2) + Math.Pow(p.Y - projY, 2));
        }

        private Dictionary<Libro, List<(Libro destino, float peso)>> ObtenerConexiones()
        {
            System.Reflection.FieldInfo conexionesField = typeof(GrafoLibro).GetField("conexiones", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return conexionesField?.GetValue(grafo) as Dictionary<Libro, List<(Libro destino, float peso)>>;
        }

        private void DibujarNodo(Graphics g, Libro libro, Point punto)
        {
            int radio = 40;
            Rectangle rect = new Rectangle(punto.X - radio / 2, punto.Y - radio / 2, radio, radio);

            g.FillEllipse(Brushes.LightBlue, rect);
            g.DrawEllipse(Pens.Black, rect);

            string titulo = libro.Title.Length > 8 ? libro.Title.Substring(0, 8) + "..." : libro.Title;
            SizeF textoSize = g.MeasureString(titulo, Font);
            g.DrawString(titulo, Font, Brushes.Black, punto.X - textoSize.Width / 2, punto.Y - textoSize.Height / 2);
        }

        private void DibujarFlecha(Graphics g, Point desde, Point hacia)
        {
            Pen flecha = new Pen(Color.Gray, 2);
            flecha.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(6, 6);

            // Calcular dirección
            double dx = hacia.X - desde.X;
            double dy = hacia.Y - desde.Y;
            double distancia = Math.Sqrt(dx * dx + dy * dy);

            // Acortar la línea para no tocar los bordes del nodo
            int radioNodo = 20; // Mitad del ancho/alto del nodo
            float offsetX = (float)(dx * radioNodo / distancia);
            float offsetY = (float)(dy * radioNodo / distancia);

            PointF nuevoDesde = new PointF(desde.X + offsetX, desde.Y + offsetY);
            PointF nuevoHacia = new PointF(hacia.X - offsetX, hacia.Y - offsetY);

            g.DrawLine(flecha, nuevoDesde, nuevoHacia);
        }
    }
}
