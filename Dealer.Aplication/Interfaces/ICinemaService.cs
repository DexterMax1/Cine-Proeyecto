using System.Collections.Generic;
using System.Threading.Tasks;
using Dealer.Aplication.DTOs;
using Dealer.Domain.Entity.Cine;

namespace Dealer.Aplication.Interfaces
{
    // Contrato del servicio de gestión de cines, salas, películas, actores, tarifas y pases
    public interface ICinemaService
    {
        // --- CONSULTAS ---
        Task<List<ShowtimeDetailDto>> GetAllShowtimesAsync();
        Task<List<ShowtimeDetailDto>> GetByMovieTitleAsync(string movieTitle);
        Task<List<ShowtimeDetailDto>> GetByGenreAsync(string genre);
        Task<List<ShowtimeDetailDto>> GetByCinemaNameAsync(string cinemaName);
        Task<List<TariffDto>> GetTariffsByCinemaAsync(string cinemaName);

        // --- CREACIONES ---
        Task<Cinema> CreateCinemaAsync(CreateCinemaDto dto);
        Task<RoomType> CreateRoomTypeAsync(CreateRoomTypeDto dto);
        Task<Room> CreateRoomAsync(CreateRoomDto dto);
        Task<Actor> CreateActorAsync(CreateActorDto dto);
        Task<Movie> CreateMovieAsync(CreateMovieDto dto);
        Task<CinemaTariff> CreateTariffAsync(CreateTariffDto dto);
        Task<MovieShowtime> CreateShowtimeAsync(CreateShowtimeDto dto);
    }
}