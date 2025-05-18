using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProyectoDeCatedra
{
    class GrafoLibro
    {
        // Diccionario que representa el grafo dirigido:
        // Cada libro (nodo origen) apunta a una lista de libros destino
        private Dictionary<Libro, List<(Libro destino, float peso)>> conexiones = new Dictionary<Libro, List<(Libro destino, float peso)>>();

        // lista de libros/lecturas
        private List<Libro> listaLibros = new List<Libro>();
        private List<Lectura> listaLectura = new List<Lectura>(); // (Poco optimo)

        // listado de todos los usuarios en nuestra aplicacion (Poco optimo)
        private List<Usuario> listaUsuarios = new List<Usuario>();

        // Get [Lista de libros]
        public List<Libro> ListLibro
        {
            get { return this.listaLibros; }
        }

        public List<Lectura> ListLectura
        {
            get { return this.listaLectura; }
        }

        public Dictionary<Libro, List<(Libro destino, float peso)>> Conexiones
        {
            get { return this.conexiones; }
        }

        // Obtencion de datos individuales (Libros, Usuarios, Lecturas)
        public Libro ObtenerLibro(int id_libro)
        {
            foreach (Libro libro in listaLibros)
            {
                if (libro.ID == id_libro)
                {
                    return libro;
                }
            }

            return null;
        }

        public Usuario ObtenerUsuario(int id_user)
        {
            foreach (Usuario usuario in listaUsuarios)
            {
                if (usuario.ID == id_user)
                {
                    return usuario;
                }
            }

            return null;
        }

        public List<Libro> ObtenerLibros()
        {
            listaLibros.Clear();

            string connectionString = "Server=localhost;Database=bibliotech;User Id=sa;Password=Pass123$_;";
            string query = "SELECT A.id, A.isbn, A.title, B.genre_name as genre, description, A.autor, A.year, A.image_name, C.score " +
                "FROM Books A " +
                "LEFT JOIN Genres B ON A.genre_id = B.id " +
                "LEFT JOIN Score C ON C.book_id = A.id"; // query personalizado

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Creamos los diferentes libros
                        Libro libroEnBase = new Libro(Int32.Parse(reader["id"].ToString()), Int32.Parse(reader["isbn"].ToString()), reader["title"].ToString(), 0, reader["description"].ToString(), reader["autor"].ToString(), Int32.Parse(reader["year"].ToString()), reader["image_name"].ToString(), float.Parse(reader["score"].ToString()));
                        libroEnBase.GenreName = reader["genre"].ToString();

                        listaLibros.Add(libroEnBase);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return listaLibros;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            listaUsuarios.Clear();

            string connectionString = "Server=localhost;Database=bibliotech;User Id=sa;Password=Pass123$_;";
            string query = "SELECT id, fullname, username FROM Users"; // query personalizado

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Creamos los diferentes libros
                        Usuario usuarioEnBase = new Usuario(Int32.Parse(reader["id"].ToString()), reader["fullname"].ToString(), reader["username"].ToString());

                        listaUsuarios.Add(usuarioEnBase);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return listaUsuarios;
        }

        public List<Lectura> ObtenerBookRanking()
        {
            listaLectura.Clear();

            string connectionString = "Server=localhost;Database=bibliotech;User Id=sa;Password=Pass123$_;";
            string query = "SELECT * FROM BookRatings"; // query personalizado

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Obtenemos el libro de la lectura extraida
                        Libro libroDeLectura = ObtenerLibro(Int32.Parse(reader["book_id"].ToString()));

                        // Obtenemos el usuario de la lectura extraida
                        Usuario usuarioDeLectura = ObtenerUsuario(Int32.Parse(reader["user_id"].ToString()));

                        // Creamos los diferentes instancias de lectura
                        Lectura libroEnBase = new Lectura(usuarioDeLectura, libroDeLectura, float.Parse(reader["rating"].ToString()), bool.Parse(reader["completed"].ToString()));

                        listaLectura.Add(libroEnBase); // Las agregamos a la lista de lecturas
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return listaLectura;
        }

        public void AgregarLibros(Libro libro)
        {

        }

        // Método para agregar una conexión dirigida de 'origen' hacia 'destino'
        public void AgregarConexion(Libro origen, Libro destino, float peso)
        {
            if (!conexiones.ContainsKey(origen))
            {
                conexiones[origen] = new List<(Libro destino, float peso)>();
            }

            // Verifica si ya existe una conexión con ese destino
            (Libro destino, float peso) existente = conexiones[origen].FirstOrDefault(x => x.destino.Equals(destino));

            if (!existente.Equals(default((Libro, float))))
            {
                // Actualiza el peso acumulando
                conexiones[origen].Remove(existente);
                conexiones[origen].Add((destino, existente.peso + peso));
            }
            else
            {
                conexiones[origen].Add((destino, peso));
            }
        }

        // Construimos el grafo a partir de las lecturas de los usuarios
        public void ConstruirDesdeLecturas(List<Lectura> lecturas)
        {
            IEnumerable<IGrouping<Usuario, Lectura>> lecturasPorUsuario = lecturas.GroupBy(l => l.Usuario);

            foreach (IGrouping<Usuario, Lectura> grupo in lecturasPorUsuario)
            {
                List<Lectura> libros = grupo.ToList();

                for (int i = 0; i < libros.Count; i++)
                {
                    for (int j = 0; j < libros.Count; j++)
                    {
                        if (i != j)
                        {
                            Libro origen = libros[i].Libro;
                            Libro destino = libros[j].Libro;

                            if (origen.GenreName == destino.GenreName)
                            {
                                float peso = CalcularPeso(libros[i], libros[j]);
                                if (peso > 0) // Solo agregamos conexiones con peso útil
                                {
                                    AgregarConexion(origen, destino, peso);
                                }
                            }
                        }
                    }
                }
            }
        }

        private float CalcularPeso(Lectura a, Lectura b)
        {
            float peso = 0.0f;

            if (a.Completado && b.Completado)
            {
                peso = 5.0f + ((a.Calificacion + b.Calificacion) / 2.0f); // base para completados
            }
            else if (a.Completado || b.Completado)
            {
                peso = 2.0f + ((a.Calificacion + b.Calificacion) / 4.0f);
            }
            else
            {
                peso = 0.1f;
            }

            // más peso si tienen el mismo autor
            if (a.Libro.Autor == b.Libro.Autor)
            {
                peso *= 1.5f; // o puedes usar +2.0f, según quieras hacer más fuerte la relación
            }

            return peso;
        }

        // Calculamos el PageRank de cada libro usando el método iterativo
        /* CAMBIAR ESTA PARTE: 
         * Si se crea un diccionario con llave 1 a 1 y hay libros que tengan el mismo pagerank el programa colapsa
         * A menos que la llave sea el libro
         */
        public Dictionary<Libro, float> CalcularPageRank(float damping = 0.85f, int iteraciones = 20)
        {
            List<Libro> libros = conexiones.Keys
                .Union(conexiones.Values.SelectMany(x => x.Select(p => p.destino)))
                .Distinct()
                .ToList();

            int N = libros.Count;
            Dictionary<Libro, float> rank = libros.ToDictionary(libro => libro, libro => 1.0f / N);

            for (int iter = 0; iter < iteraciones; iter++)
            {
                Dictionary<Libro, float> nuevoRank = libros.ToDictionary(libro => libro, libro => (1 - damping) / N);

                foreach (Libro libro in libros)
                {
                    if (!conexiones.ContainsKey(libro) || conexiones[libro].Count == 0)
                        continue;

                    // Suma total de pesos de las conexiones salientes
                    float sumaPesos = conexiones[libro].Sum(x => x.peso);

                    if (sumaPesos == 0)
                        continue;

                    foreach (var (destino, peso) in conexiones[libro])
                    {
                        float proporcion = peso / sumaPesos;
                        nuevoRank[destino] += damping * rank[libro] * proporcion;
                    }
                }

                rank = nuevoRank;
            }

            return rank;
        }
    }
}
