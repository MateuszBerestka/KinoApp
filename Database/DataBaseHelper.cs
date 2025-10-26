using System;
using Npgsql; // biblioteka do połączenia z bazą PostgreSQL
using KinoApp.Models; // odwołanie do modelu Movie (Id, Title, ImagePath)

namespace KinoWinForms
{
    public static class DataBaseHelper
    {
        
        private static readonly string connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=kino";

      
        public static bool IsSeatTaken(int filmId, int row, int col)
        {
           
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // Tworzymy zapytanie SQL sprawdzające czy w tabeli "rezerwacje"
            // istnieje rekord z danym filmem, rzędem i miejscem
            using var cmd = new NpgsqlCommand(
                "SELECT COUNT(*) FROM rezerwacje WHERE film_id = @filmId AND rzad = @row AND miejsce = @col", conn
            );

            // Przypisujemy wartości do parametrów zapytania
            cmd.Parameters.AddWithValue("filmId", filmId);
            cmd.Parameters.AddWithValue("row", row);
            cmd.Parameters.AddWithValue("col", col);

            
            long count = (long)cmd.ExecuteScalar();

           
            return count > 0;
        }

      
        public static void ReserveSeat(int filmId, int row, int col)
        {
            // Otwieramy połączenie z bazą
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // Tworzymy zapytanie INSERT do dodania nowej rezerwacji
            using var cmd = new NpgsqlCommand(
                "INSERT INTO rezerwacje (film_id, rzad, miejsce, czy_zajete) VALUES (@filmId, @row, @col, TRUE)", conn
            );

            // Przekazujemy dane filmu, rzędu i miejsca
            cmd.Parameters.AddWithValue("filmId", filmId);
            cmd.Parameters.AddWithValue("row", row);
            cmd.Parameters.AddWithValue("col", col);

            // Wykonujemy zapytanie
            cmd.ExecuteNonQuery();
        }

       
        public static List<Movie> GetMovies()
        {
            var movies = new List<Movie>(); // lista do przechowywania pobranych filmów

            // Otwieramy połączenie z bazą danych
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Zapytanie SQL pobierające dane z tabeli "movies"
                using (var cmd = new NpgsqlCommand("SELECT id, title, image_path FROM movies", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    // Czytamy każdy wiersz i tworzymy obiekt Movie
                    
                    while (reader.Read())
                    {
                        movies.Add(new Movie
                        {
                            Id = reader.GetInt32(0),          // kolumna 1: id
                            Title = reader.GetString(1),      // kolumna 2: tytuł filmu
                            ImagePath = reader.GetString(2)   // kolumna 3: ścieżka do obrazka
                        });
                    }
                }
            }

            return movies; 
        }
    }
}
