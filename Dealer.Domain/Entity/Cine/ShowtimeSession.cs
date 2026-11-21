namespace Dealer.Domain.Entity.Cine
{
    // Agrupa horarios en franjas comerciales (Matinal, Tarde, Noche)
    public class ShowtimeSession : BaseClass
    {
        public TimeSpan StartTime { get; set; } // Hora de inicio de la tanda
        public TimeSpan EndTime { get; set; }   // Hora de fin de la tanda

        public ICollection<MovieShowtime> Showtimes { get; set; } = new List<MovieShowtime>(); // Pases en esta tanda
    }
}