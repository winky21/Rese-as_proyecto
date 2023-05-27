using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Reseñas.Pages
{

    public class PeliculasModel : PageModel
    {
        private readonly string connectionString = "Server=DESKTOP-H8K7ANG\\SQLEXPRESS01;Database=Reseñas;Trusted_Connection=True;MultipleActiveResultSets=true";

        public List<Pelicula> Peliculas { get; set; }

        public async Task<IActionResult> OnGet(string tematica)
        {
            if (string.IsNullOrEmpty(tematica))
            {
                // Si no se proporciona una temática, mostrar todas las películas
                await LoadAllPeliculas();
            }
            else
            {
                // Filtrar las películas por la temática proporcionada
                await LoadPeliculasByTematica(tematica);
            }

            return Page();
        }

        private async Task LoadAllPeliculas()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Peliculas";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Construir la lista de películas
                    Peliculas = new List<Pelicula>();
                    while (await reader.ReadAsync())
                    {
                        // Leer los datos de la fila y crear instancias de Pelicula
                        Pelicula pelicula = new Pelicula
                        {
                            ID = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Tematica = reader.GetString(2),
                            Año = reader.GetInt32(3),
                            Descripcion = reader.GetString(4),
                            Rating = reader.GetDecimal(5),

                        };

                        Peliculas.Add(pelicula);
                    }
                }
            }
        }
         private async Task LoadPeliculasByTematica(string tematica)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Filtrar las películas por temática en la consulta SQL
                string query = "SELECT * FROM Peliculas WHERE Tematica = " + "'"+tematica+"'";
                using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Construir la lista de películas filtradas por temática
                        Peliculas = new List<Pelicula>();
                        while (await reader.ReadAsync())
                        {
                            // Leer los datos de la fila y crear instancias de Pelicula
                            Pelicula pelicula = new Pelicula
                            {
                                ID = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                                Tematica = reader.GetString(2),
                                Año = reader.GetInt32(3),
                                Descripcion = reader.GetString(4),
                                Rating = reader.GetDecimal(5),
                               
                            };

                            Peliculas.Add(pelicula);
                        }
                    }
                }
            }
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
   
}
