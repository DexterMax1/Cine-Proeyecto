namespace Dealer.Domain.Entity.Cine
{
    // Clase base con identificador y campos de auditoría comunes
    public class BaseClass
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Identificador único
        public string Name { get; set; } = string.Empty; // Nombre descriptivo

        public bool IsDeleted { get; set; } = false; // Eliminación lógica
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de creación
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow; // Última actualización
        public string? CreatedBy { get; set; } = string.Empty; // Creador
        public string? UpdatedBy { get; set; } = string.Empty; // Modificador
    }
}