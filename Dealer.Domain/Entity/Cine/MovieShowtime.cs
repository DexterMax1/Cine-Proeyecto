namespace Dealer.Domain.Entity.Cine
{
    // Representa una función o pase programado en una sala y fecha/hora específicas
    public class MovieShowtime : BaseClass
    {
        public Guid CinemaId { get; set; } // Cine donde se realiza la función
        public Cinema? Cinema { get; set; }

        public Guid RoomId { get; set; } // Sala donde se proyecta
        public Room? Room { get; set; }

        public Guid MovieId { get; set; } // Película que se proyecta
        public Movie? Movie { get; set; }

        public Guid? ShowtimeSessionId { get; set; } // Tanda o franja horaria (Matinal, Tarde, Noche)
        public ShowtimeSession? ShowtimeSession { get; set; }

        public Guid? DayTypeId { get; set; } // Tipo de día (Normal, Festivo, Víspera, Espectador, Jubilado)
        public DayTypeEntity? DayType { get; set; }

        public DateTime ShowDate { get; set; } // Fecha de la función
        public string Time { get; set; } = string.Empty; // Hora del pase
    }
}