using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDeCatedra
{
    class Libro
    {
        // Atributtes
        int id;
        int isbn;
        string title;
        int genre;
        string genreName;
        string description;
        string autor;
        int year;
        string image;
        float score;

        // Get&Set
        public int ID
        {
            get { return id; }
        }

        public int ISBN
        {
            get { return isbn; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public int Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        public string GenreName
        {
            get { return genreName; }
            set { genreName = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Autor
        {
            get { return autor; }
            set { autor = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public float Score
        {
            get { return score; }
            set { score = value; }
        }

        // Methods
        public Libro(int id, int isbn, string title, int genre, string description, string autor, int year, string image = null, float score = 0)
        {
            this.id = id;
            this.isbn = isbn;
            this.title = title;
            this.genre = genre;
            this.description = description;
            this.autor = autor;
            this.year = year;
            this.image = image;
            this.score = score;
        }

        public Libro(int isbn, string title, int genre, string description, string autor, int year, string image = null, float score = 0)
        {
            this.isbn = isbn;
            this.title = title;
            this.genre = genre;
            this.description = description;
            this.autor = autor;
            this.year = year;
            this.image = image;
            this.score = score;
        }

        // Agregar
        public void AgregarLibro()
        {
            //
        }

        // Modificar Libro
        public void ModificarLibro()
        {
            //
        }
    }
}
