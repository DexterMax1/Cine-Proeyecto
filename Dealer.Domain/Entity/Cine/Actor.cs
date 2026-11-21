namespace Dealer.Domain.Entity.Cine
{
    // Representa una película
    public class Movie : BaseClass
    {
        public string Director { get; set; } = string.Empty; // Director de la película
        public string Genre { get; set; } = string.Empty; // Género (comedia, drama, dibujos, etc.)
        public string Rating { get; set; } = string.Empty; // Clasificación por edad
        public bool IsPopular { get; set; } = false; // Indica si es película famosa/alta demanda (afecta tarifa)

        // Relación con múltiples actores
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();

        // Funciones donde se proyecta la película
        public ICollection<MovieShowtime> Showtimes { get; set; } = new List<MovieShowtime>();

        // Tarifas asociadas directamente a esta película (si aplica precio por película)
        public ICollection<CinemaTariff> Tariffs { get; set; } = new List<CinemaTariff>();
    }
}