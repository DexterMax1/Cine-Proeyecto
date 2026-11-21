namespace Dealer.Domain.Entity.Cine
{
    // Normaliza condiciones de calendario y perfiles (Normal, Festivo, Víspera, Espectador, Jubilado)
    public class DayTypeEntity : BaseClass
    {
        public ICollection<MovieShowtime> Showtimes { get; set; } = new List<MovieShowtime>(); // Funciones en este tipo de día
        public ICollection<CinemaTariff> Tariffs { get; set; } = new List<CinemaTariff>();     // Tarifas asociadas
    }
}