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
        private Dictionary<Libro, List<Libro>> conexiones = new Dictionary<Libro, List<Libro>>();

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
            foreach(Usuario usuario in listaUsuarios)
            {
                if(usuario.ID == id_user)
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
            string query = "SELECT A.isbn, A.title, B.genre_name as genre, description, A.autor, A.year, A.image_name, C.score " +
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
                        Libro libroEnBase = new Libro(Int32.Parse(reader["isbn"].ToString()), reader["title"].ToString(), 0, reader["description"].ToString(), reader["autor"].ToString(), Int32.Parse(reader["year"].ToString()), reader["image_name"].ToString(), float.Parse(reader["score"].ToString()));
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
            listaLibros.Clear();

            string connectionString = "Server=localhost;Database=bibliotech;User Id=sa;Password=Pass123$_;";
            string query = "SELECT id, fullname, username FORM Users"; // query personalizado

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
                        Usuario usuarioEnBase = new Usuario(reader["fullname"].ToString(), reader["username"].ToString());

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
            string connectionString = "Server=localhost;Database=bibliotech;User Id=sa;Password=Pass123$_;";
            string query = "SELECT * FROM BookRatings WHERE completed != 0"; // query personalizado

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
                        Lectura libroEnBase = new Lectura(null, libroDeLectura, float.Parse(reader["clasificacion"].ToString()), bool.Parse(reader["completed"].ToString()));

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
        public void AgregarConexion(Libro origen, Libro destino)
        {
            // Si el libro origen no tiene conexiones, lo inicializamos
            if (!conexiones.ContainsKey(origen))
            {
                conexiones[origen] = new List<Libro>();
            }

            // Evitamos duplicar conexiones
            if (!conexiones[origen].Contains(destino))
            {
                conexiones[origen].Add(destino);
            }
        }

        // Construimos el grafo a partir de las lecturas de los usuarios
        public void ConstruirDesdeLecturas(List<Lectura> lecturas)
        {
            // Agrupamos lecturas completas por cada usuario
            IEnumerable<IGrouping<Usuario, Lectura>> lecturasPorUsuario = lecturas.GroupBy(l => l.Usuario);

            foreach (IGrouping<Usuario, Lectura> grupo in lecturasPorUsuario)
            {
                // Lista de libros que ese usuario completó
                List<Libro> librosLeidos = grupo.Select(l => l.Libro).Distinct().ToList();

                // Conectamos todos los pares de libros que ese usuario leyó
                for (int i = 0; i < librosLeidos.Count; i++)
                {
                    for (int j = 0; j < librosLeidos.Count; j++)
                    {
                        if (i != j)
                        {
                            // Aquí se conecta i → j (grafo dirigido)
                            AgregarConexion(librosLeidos[i], librosLeidos[j]);
                        }
                    }
                }
            }
        }

        // Calculamos el PageRank de cada libro usando el método iterativo
        /* CAMBIAR ESTA PARTE: 
         * Si se crea un diccionario con llave 1 a 1 y hay libros que tengan el mismo pagerank el programa colapsa
         * A menos que la llave sea el libro
         */
        public Dictionary<Libro, float> CalcularPageRank(float damping = 0.85f, int iteraciones = 20)
        {
            // Obtenemos todos los libros únicos en el grafo (nodos)
            List<Libro> libros = conexiones.Keys.Union(conexiones.Values.SelectMany(x => x)).Distinct().ToList();
            int N = libros.Count;

            // Inicializamos los scores de todos los libros en 1/N
            Dictionary<Libro, float> rank = libros.ToDictionary(libro => libro, libro => 1.0f / N);

            // Ejecutamos iterativamente el algoritmo de PageRank
            for (int iter = 0; iter < iteraciones; iter++)
            {
                // Empezamos con una pequeña fracción de puntuación base
                Dictionary<Libro, float> nuevoRank = libros.ToDictionary(libro => libro, libro => (1 - damping) / N);

                foreach (Libro libro in libros)
                {
                    // Si el libro no tiene enlaces salientes, no aporta PageRank
                    if (!conexiones.ContainsKey(libro) || conexiones[libro].Count == 0)
                        continue;

                    // Distribuimos el PageRank actual entre sus enlaces salientes
                    float distribucion = rank[libro] / conexiones[libro].Count;

                    foreach (Libro destino in conexiones[libro])
                    {
                        nuevoRank[destino] += damping * distribucion;
                    }
                }

                // Actualizamos los scores
                rank = nuevoRank;
            }

            return rank;
        }
    }
}
