using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDeCatedra
{
    class Lectura
    {
        private Usuario usuario; // El usuario que leyó el libro
        private Libro libro; // El libro que fue leído
        private float calificacion; // Calificación dada por el usuario (de 1 a 5 esterllas)
        private bool completado; // usuario completó la lectura del libro (Verdadero / Falso)

        public float Calificacion
        {
            get { return this.calificacion; }
            set { this.calificacion = value; }
        }

        public bool Completado
        {
            get { return this.completado; }
            set { this.completado = value; }
        }

        public Usuario Usuario
        {
            get { return this.usuario; }
            set { this.usuario = value; }
        }

        public Libro Libro
        {
            get { return this.libro; }
            set { this.libro = value; }
        }

        public Lectura(Usuario usuario, Libro libro, float calificacion, bool completado)
        {
            this.usuario = usuario;
            this.libro = libro;
            this.calificacion = calificacion;
            this.completado = completado;
        }
    }
}
