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
        private List<string> paginas;
        private int paginaActual = 0;

        private GrafoLibro grafo;

        //
        int id_user;

        //
        int id_book;

        public LecturaLibro(int libroId, int UserId)
        {
            InitializeComponent();
            InicializarPaginas();
            MostrarPagina();

            //
            Console.WriteLine("ID USER: " + UserId + " | ID BOOK: " + libroId);
            id_user = UserId;
            id_book = libroId;

            this.DoubleBuffered = true;

            grafo = new GrafoLibro();
            grafo.ObtenerLibros();

            //
            Lectura lec = grafo.ObtenerBookRankingUser(UserId, libroId);
            if (lec != null)
            {
                btnCompletar.Enabled = false;
                btnAbandonar.Enabled = false;
                lblCalificado.Text = "Ya has calificado este libro - ⭐" + lec.Calificacion;
            }


            Libro libroDetallado = grafo.ObtenerLibro(libroId);

            this.Text = libroDetallado.Title;

            richTextBox1.Font = new Font("Georgia", 12);
        }

        private void InicializarPaginas()
        {
            paginas = new List<string>
            {
                // Página 1 (portada)
                "Título: Explorando GitHub: Control de versiones moderno\n" +
                "Autor: BiblioTech Press\n\n" +
                "Resumen:\n" +
                "Este libro es una introducción al mundo de GitHub, la plataforma líder para el control de versiones y la colaboración de software. A través de estas páginas, aprenderás cómo funciona Git, cómo usar GitHub para trabajar en equipo y las buenas prácticas que te harán un desarrollador más eficiente.",

                // Página 2
                "Capítulo 1: ¿Qué es Git y GitHub?\n\n" +
                "Git es un sistema de control de versiones distribuido creado por Linus Torvalds. Permite registrar los cambios en archivos y colaborar con otros. GitHub es una plataforma basada en la web que aloja repositorios Git y permite compartir código, hacer revisiones, manejar issues, y mucho más.\n\n" +
                "A diferencia de otros sistemas de control de versiones, Git permite trabajar sin conexión, con cambios que luego pueden sincronizarse con otros colaboradores.",

                // Página 3
                "Capítulo 2: Comenzando con GitHub\n\n" +
                "Para empezar a trabajar con GitHub:\n" +
                "1. Crea una cuenta en github.com.\n" +
                "2. Instala Git en tu computadora.\n" +
                "3. Crea un repositorio desde GitHub o localmente con `git init`.\n" +
                "4. Conecta tu repo local con uno remoto usando:\n   `git remote add origin <url-del-repo>`\n" +
                "5. Usa `git add`, `git commit`, y `git push` para subir tus cambios.\n\n" +
                "Aprender estos comandos es esencial para trabajar con equipos distribuidos.",

                // Página 4
                "Capítulo 3: Buenas prácticas en GitHub\n\n" +
                "- Escribe mensajes de commit claros y concisos.\n" +
                "- Usa branches (ramas) para trabajar en nuevas funcionalidades.\n" +
                "- Haz pull requests para que otros revisen tu código.\n" +
                "- Usa el archivo README.md para documentar tu proyecto.\n" +
                "- Configura workflows automáticos con GitHub Actions.\n\n" +
                "Estas prácticas te ayudarán a mantener proyectos bien organizados y fáciles de colaborar."
            };
        }

        private void MostrarPagina()
        {
            richTextBox1.Text = paginas[paginaActual];
            progressBar1.Value = (int)(((paginaActual + 1) / (float)paginas.Count) * 100);
            btnAnterior.Enabled = paginaActual > 0;
            btnSiguiente.Enabled = paginaActual < paginas.Count - 1;
        }

        private void btnSiguiente_Click_1(object sender, EventArgs e)
        {
            if (paginaActual < paginas.Count - 1)
            {
                paginaActual++;
                MostrarPagina();
            }
        }

        private void btnAnterior_Click_1(object sender, EventArgs e)
        {
            if (paginaActual > 0)
            {
                paginaActual--;
                MostrarPagina();
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCompletar_Click(object sender, EventArgs e)
        {
            CalificarLibro calificarForm = new CalificarLibro();

            if (calificarForm.ShowDialog() == DialogResult.OK)
            {
                int calificacion = calificarForm.UserRating;
                bool completed = true;

                grafo.AgregarCalificacion(id_book, id_user, completed, calificacion);

                btnCompletar.Enabled = false;
                btnAbandonar.Enabled = false;
            }
        }

        private void btnAbandonar_Click(object sender, EventArgs e)
        {
            CalificarLibro calificarForm = new CalificarLibro();

            if (calificarForm.ShowDialog() == DialogResult.OK)
            {
                int calificacion = calificarForm.UserRating;
                bool completed = false;

                grafo.AgregarCalificacion(id_book, id_user, completed, calificacion);

                btnCompletar.Enabled = false;
                btnAbandonar.Enabled = false;
            }
        }
    }
}
