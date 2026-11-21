namespace Dealer.Domain.Entity.Cine
{
    // Representa una película con control de estreno
    public class Movie : BaseClass
    {
        public string Director { get; set; } = string.Empty; // Director
        public string Genre { get; set; } = string.Empty;    // Género
        public string Rating { get; set; } = string.Empty;   // Clasificación
        public DateTime? ReleaseDate { get; set; }           // Fecha de estreno
        public bool IsPremiere { get; set; } = false;        // Indica si aplica recargo o restricción de estreno
        public bool IsPopular { get; set; } = false;         // Película de alta demanda/fama

        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>(); // Relación N:M con actores
        public ICollection<MovieShowtime> Showtimes { get; set; } = new List<MovieShowtime>(); // Funciones
        public ICollection<CinemaTariff> Tariffs { get; set; } = new List<CinemaTariff>();     // Tarifas vinculadas
    }
}