namespace Dealer.Domain.Entity.Cine
{
    // Define el precio según cine, película, sala/tipo de sala, tipo de día, tanda y estreno/fama
    public class CinemaTariff : BaseClass
    {
        public Guid CinemaId { get; set; } // Cine donde aplica la tarifa
        public Cinema? Cinema { get; set; }

        public Guid? RoomTypeId { get; set; } // Tipo de sala opcional (2D, 3D, 4D, IMAX, VIP)
        public RoomType? RoomType { get; set; }

        public Guid? MovieId { get; set; } // Película específica opcional
        public Movie? Movie { get; set; }

        public Guid? DayTypeId { get; set; } // Tipo de día opcional (Normal, Festivo, Víspera, Espectador, Jubilado)
        public DayTypeEntity? DayType { get; set; }

        public Guid? ShowtimeSessionId { get; set; } // Tanda opcional (Matinal, Tarde, Noche)
        public ShowtimeSession? ShowtimeSession { get; set; }

        public bool? IsPremiere { get; set; } // Aplica si la función es de estreno
        public bool? IsPopularMovie { get; set; } // Aplica si la película es de alta demanda/fama

        public decimal Price { get; set; } // Precio de la tarifa
    }
}