using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Text;
using Microsoft.Data.SqlClient;

namespace Reseñas.Pages
{
    public class ComentariosModel : PageModel
    {
        public List<Comentario> comentarios = new List<Comentario>();
        public List<Pelicula> Peliculas = new List<Pelicula>();
        public void OnGet()
        {

            try
            {
                // ...

                // Establecer la cadena de conexión a tu base de datos SQL Server
                string connectionString = "Server=DESKTOP-H8K7ANG\\SQLEXPRESS01;Database=Reseñas;Trusted_Connection=True;MultipleActiveResultSets=true";

                // Establecer la consulta SQL para recuperar los datos de los perfiles
                string query = "SELECT * FROM Peliculas";

                // Crear una instancia de SqlConnection con la cadena de conexión
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión a la base de datos
                    connection.Open();

                    // Crear una instancia de SqlCommand con la consulta SQL y la conexión
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Ejecutar la consulta y obtener los resultados en un SqlDataReader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Crear un StringBuilder para construir el HTML de los perfiles
                            StringBuilder htmlBuilder = new StringBuilder();

                            // Iterar sobre los resultados y generar los perfiles
                            while (reader.Read())
                            {
                                // Obtener los valores de cada columna del resultado
                                Pelicula pelicula = new Pelicula();

                                int id = pelicula.ID = reader.GetInt32(0);
                                pelicula.Titulo = reader.GetString(1);
                                pelicula.Tematica = reader.GetString(2);
                                pelicula.Año = reader.GetInt32(3);
                                pelicula.Descripcion = reader.GetString(4);
                                    pelicula.Rating = reader.GetDecimal(5);

                                 List<Comentario> comentarios = ObtenerComentariosPerfil(pelicula.ID);
                                pelicula.Comentarios = comentarios;


                                Peliculas.Add(pelicula);
                                // Generar el HTML del perfil utilizando interpolación de cadenas
                               

                                // Agregar el perfil generado al StringBuilder
                                
                            }
                            connection.Close();
                            

                            // Asignar el HTML generado al modelo
                         

                            // Devolver la vista con el layout y el modelo

                            //return Content(profilesHtml, "text/html");


                        }
                    }

                    // Cerrar la conexión a la base de datos

                }

                // Obtener el HTML completo de los perfiles


                // Usar el HTML generado en tu página web para mostrar los perfiles dinámicamente
                // (por ejemplo, asignándolo a una etiqueta div en tu página)

            }
            catch (Exception ex)
            {

            }


        }
        private List<Comentario> ObtenerComentariosPerfil(int PeliculaId)
        {


            // Establecer la cadena de conexión a tu base de datos SQL Server
            string connectionString = "Server=DESKTOP-H8K7ANG\\SQLEXPRESS01;Database=Reseñas;Trusted_Connection=True;MultipleActiveResultSets=true";

            // Establecer la consulta SQL para recuperar los comentarios del perfil
            string query = "SELECT * FROM Comentarios WHERE PeliculaId = @PeliculaId";

            // Crear una instancia de SqlConnection con la cadena de conexión
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión a la base de datos
                connection.Open();

                // Crear una instancia de SqlCommand con la consulta SQL y la conexión
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer los parámetros de la consulta
                    command.Parameters.AddWithValue("@PeliculaId", PeliculaId);

                    // Ejecutar la consulta y obtener los resultados en un SqlDataReader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Iterar sobre los resultados y crear los objetos de Comentario
                        while (reader.Read())
                        {
                            Comentario comentario = new Comentario();
                            comentario.Id = reader.GetInt32(0);
                            comentario.PeliculaId = reader.GetInt32(1);
                            comentario.Autor = reader.GetString(2);
                            comentario.Puntuacion = reader.GetDecimal(3);
                            comentario.ComentarioTexto = reader.GetString(4);

                            comentarios.Add(comentario);
                        }
                    }
                }
            }

            return comentarios;
        }
        public IActionResult OnPostCrearComentario(int peliculaId, string autor, decimal puntuacion, string comentario)
        {
            try
            {
                // Establecer la cadena de conexión a tu base de datos SQL Server
                string connectionString = "Server=DESKTOP-H8K7ANG\\SQLEXPRESS01;Database=Reseñas;Trusted_Connection=True;MultipleActiveResultSets=true";

                // Establecer la consulta SQL para insertar el nuevo comentario
                string query = "INSERT INTO Comentarios (PeliculaId, Autor, Puntuacion, ComentarioTexto) " +
                               "VALUES (@PeliculaId, @Autor, @Puntuacion, @Comentario)";

                // Crear una instancia de SqlConnection con la cadena de conexión
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión a la base de datos
                    connection.Open();

                    // Crear una instancia de SqlCommand con la consulta SQL y la conexión
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Establecer los parámetros de la consulta
                        command.Parameters.AddWithValue("@PeliculaId", peliculaId);
                        command.Parameters.AddWithValue("@Autor", autor);
                        command.Parameters.AddWithValue("@Puntuacion", puntuacion);
                        command.Parameters.AddWithValue("@Comentario", comentario);

                        // Ejecutar la consulta
                        command.ExecuteNonQuery();
                    }
                }

                // Redirige al usuario nuevamente a la página de perfiles
                return RedirectToPage("/Comentarios");
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes personalizarlo según tus necesidades)
                return Page();
            }
        }

        public class Pelicula
        {
            public int ID { get; set; }
            public string Titulo { get; set; }
            public string Tematica { get; set; }
            public int Año { get; set; }
            public string Descripcion { get; set; }
            public decimal Rating { get; set; }

        public List<Comentario> Comentarios { get; set; }
        }
        public class Comentario
        {
            public int Id { get; set; }
            public int PeliculaId { get; set; }
            public string Autor { get; set; }
            public decimal Puntuacion { get; set; }
            public string ComentarioTexto { get; set; }
        }
    }
}

