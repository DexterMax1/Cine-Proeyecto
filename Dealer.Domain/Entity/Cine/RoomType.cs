namespace Dealer.Domain.Entity.Cine
{
    // Clasifica la tecnología de la sala (2D, 3D, 4D, IMAX, VIP)
    public class RoomType : BaseClass
    {
        public decimal ExtraPrice { get; set; } = 0; // Recargo base por formato de sala
        public ICollection<Room> Rooms { get; set; } = new List<Room>(); // Salas de este tipo
    }
}