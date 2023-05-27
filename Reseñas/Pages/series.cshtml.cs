using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reseñas.Pages
{
    public class SeriesModel : PageModel
    {
        private readonly string connectionString = "Server=DESKTOP-H8K7ANG\\SQLEXPRESS01;Database=Reseñas;Trusted_Connection=True;MultipleActiveResultSets=true";

        public List<Serie> Series { get; set; }

        public async Task<IActionResult> OnGet(string tematica)
        {
            if (string.IsNullOrEmpty(tematica))
            {
                // Si no se proporciona una temática, mostrar todas las series
                await LoadAllSeries();
            }
            else
            {
                // Filtrar las series por la temática proporcionada
                await LoadSeriesByTematica(tematica);
            }

            return Page();
        }

        private async Task LoadAllSeries()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Series";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Construir la lista de series
                    Series = new List<Serie>();
                    while (await reader.ReadAsync())
                    {
                        // Leer los datos de la fila y crear instancias de Serie
                        Serie serie = new Serie
                        {
                            ID = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Tematica = reader.GetString(2),
                            Año = reader.GetInt32(3),
                            Descripcion = reader.GetString(4),
                            Rating = reader.GetDecimal(5),
                        };

                        Series.Add(serie);
                    }
                }
            }
        }

        private async Task LoadSeriesByTematica(string tematica)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Filtrar las series por temática en la consulta SQL
                string query = "SELECT * FROM Series WHERE Tematica = " + "'" + tematica + "'";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Construir la lista de series filtradas por temática
                        Series = new List<Serie>();
                        while (await reader.ReadAsync())
                        {
                            // Leer los datos de la fila y crear instancias de Serie
                            Serie serie = new Serie
                            {
                                ID = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                                Tematica = reader.GetString(2),
                                Año = reader.GetInt32(3),
                                Descripcion = reader.GetString(4),
                                Rating = reader.GetDecimal(5),
                            };

                            Series.Add(serie);
                        }
                    }
                }
            }
        }
    }

    public class Serie
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Tematica { get; set; }
        public int Año { get; set; }
        public string Descripcion { get; set; }
        public decimal Rating { get; set; }
    }

