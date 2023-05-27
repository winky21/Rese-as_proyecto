using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Reseñas.Pages
{
    public class AñadirPeliculaModel : PageModel
    {
        private readonly string connectionString = "Server=DESKTOP-H8K7ANG\\SQLEXPRESS01;Database=Reseñas;Trusted_Connection=True;MultipleActiveResultSets=true";

        public bool EsPelicula { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(string Titulo, string Tematica, string Año, string Descripcion, string Rating)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string tabla = EsPelicula ? "Peliculas" : "Series";
                string query = $"INSERT INTO {tabla} (Titulo, Tematica, Año, Descripcion, Rating) VALUES (@Titulo, @Tematica, @Año, @Descripcion, @Rating)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Titulo", Titulo);
                    command.Parameters.AddWithValue("@Tematica", Tematica);
                    command.Parameters.AddWithValue("@Año", Año);
                    command.Parameters.AddWithValue("@Descripcion", Descripcion);
                    command.Parameters.AddWithValue("@Rating", Rating);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToPage("/Index");
        }
    }
}
