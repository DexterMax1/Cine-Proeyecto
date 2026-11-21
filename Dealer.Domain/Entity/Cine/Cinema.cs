namespace Dealer.Domain.Entity.Cine
{
    // Representa un cine con sus salas, tarifas y funciones
    public class Cinema : BaseClass
    {
        public string Street { get; set; } = string.Empty; // Calle
        public string Number { get; set; } = string.Empty; // Número o S/N
        public string Phone { get; set; } = string.Empty; // Teléfono de contacto

        public ICollection<Room> Rooms { get; set; } = new List<Room>(); // Salas del cine
        public ICollection<CinemaTariff> Tariffs { get; set; } = new List<CinemaTariff>(); // Tarifas del cine
        public ICollection<MovieShowtime> Showtimes { get; set; } = new List<MovieShowtime>(); // Funciones programadas
    }
}