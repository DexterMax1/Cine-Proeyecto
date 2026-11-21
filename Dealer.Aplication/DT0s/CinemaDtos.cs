using System;
using System.Collections.Generic;

namespace Dealer.Aplication.DTOs
{
    // Representa el precio de una función según el día de la semana
    public record TariffDto(string Day, decimal Price);

    // Detalles completos de una función 
    public record ShowtimeDetailDto(
        string CinemaName,
        string Street,
        string Number,
        string Phone,
        string MovieTitle,
        string Director,
        string Genre,
        string Classification,
        string Protagonists,
        string Room,
        string Hour
    );

    // Datos necesarios 
    public record CreateCinemaDto(string Name, string Street, string Number, string Phone);

    public record CreateTariffDto(Guid CinemaId, string Day, decimal Price);

    public record CreateMovieDto(string Title, string Director, string Genre, string Classification, string? Protagonist1, string? Protagonist2, string? Protagonist3);

    public record CreateShowtimeDto(Guid CinemaId, Guid MovieId, string Room, string Hour);
}