namespace Dealer.Domain.Entity.Cine
{
    // Representa una sala física dentro de un cine
    public class Room : BaseClass
    {
        public Guid CinemaId { get; set; } // Cine al que pertenece la sala
        public Cinema? Cinema { get; set; }

        public Guid RoomTypeId { get; set; } // Tipo o tecnología de la sala (2D, 3D, 4D, IMAX, VIP)
        public RoomType? RoomType { get; set; }

        public int RoomNumber { get; set; } // Número de sala
        public int Capacity { get; set; } // Capacidad de butacas

        // Funciones (pases) programadas en esta sala
        public ICollection<MovieShowtime> Showtimes { get; set; } = new List<MovieShowtime>();
    }
}