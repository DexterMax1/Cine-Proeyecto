using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dealer.Aplication.DTOs;
using Dealer.Aplication.Interfaces;
using Dealer.Domain.Entity.Cine;
using Dealer.Infrastructure.Data;

namespace Dealer.Infrastructure.Services
{
    // Implementación de la lógica de negocio y cálculo dinámico de tarifas[cite: 3, 4, 5, 6]
    public class CinemaService : ICinemaService
    {
        private readonly AppDbContext _context;

        public CinemaService(AppDbContext context)
        {
            _context = context;
        }

        // Calcula el precio considerando tarifa coincidente, tipo de sala, tanda, día y estreno/fama[cite: 3, 4, 5]
        private decimal CalculatePrice(MovieShowtime ms, List<CinemaTariff> tariffs)
        {
            bool isPremiere = ms.Movie?.IsPremiere ?? false;
            bool isPopular = ms.Movie?.IsPopular ?? false;

            var tariff = tariffs.FirstOrDefault(t =>
                t.CinemaId == ms.CinemaId &&
                (t.RoomTypeId == null || t.RoomTypeId == ms.Room?.RoomTypeId) &&
                (t.MovieId == null || t.MovieId == ms.MovieId) &&
                (t.DayTypeId == null || t.DayTypeId == ms.DayTypeId) &&
                (t.ShowtimeSessionId == null || t.ShowtimeSessionId == ms.ShowtimeSessionId) &&
                (t.IsPremiere == null || t.IsPremiere == isPremiere) &&
                (t.IsPopularMovie == null || t.IsPopularMovie == isPopular)
            );

            decimal basePrice = tariff?.Price ?? 500m; // Tarifa base si no hay regla específica
            decimal roomTypeExtra = ms.Room?.RoomType?.ExtraPrice ?? 0m; // Recargo por formato de sala (2D, 3D, 4D, IMAX, VIP)[cite: 3]
            decimal premiereExtra = isPremiere ? 100m : 0m; // Recargo adicional por película de estreno[cite: 4, 5]

            return basePrice + roomTypeExtra + premiereExtra;
        }

        // Consulta base con relaciones cargadas y proyección a DTO[cite: 3, 4, 5, 6]
        private async Task<List<ShowtimeDetailDto>> QueryShowtimesAsync(Func<MovieShowtime, bool>? predicate = null)
        {
            var showtimes = await _context.MovieShowtimes
                .Include(ms => ms.Cinema)
                .Include(ms => ms.Room)
                    .ThenInclude(r => r!.RoomType)
                .Include(ms => ms.ShowtimeSession)
                .Include(ms => ms.DayType)
                .Include(ms => ms.Movie)
                    .ThenInclude(m => m!.MovieActors)
                        .ThenInclude(ma => ma.Actor)
                .Where(ms => !ms.IsDeleted)
                .ToListAsync();

            var tariffs = await _context.CinemaTariffs
                .Where(t => !t.IsDeleted)
                .ToListAsync();

            if (predicate != null)
            {
                showtimes = showtimes.Where(predicate).ToList();
            }

            return showtimes.Select(ms => new ShowtimeDetailDto(
                ms.Cinema?.Name ?? "",
                ms.Cinema?.Street ?? "",
                ms.Cinema?.Number ?? "",
                ms.Cinema?.Phone ?? "",
                ms.Room?.RoomNumber ?? 0,
                ms.Room?.RoomType?.Name ?? "",
                ms.ShowtimeSession?.Name ?? "",
                ms.DayType?.Name ?? "",
                ms.ShowDate,
                ms.Movie?.IsPremiere ?? false,
                ms.Movie?.Name ?? "",
                ms.Movie?.Director ?? "",
                ms.Movie?.Genre ?? "",
                ms.Movie?.Rating ?? "",
                ms.Movie?.IsPopular ?? false,
                ms.Movie?.MovieActors
                    .Where(ma => !ma.IsDeleted && ma.Actor != null)
                    .Select(ma => ma.Actor!.Name)
                    .ToList() ?? new List<string>(),
                ms.Time,
                CalculatePrice(ms, tariffs)
            )).ToList();
        }

        // --- CONSULTAS ---
        public Task<List<ShowtimeDetailDto>> GetAllShowtimesAsync() =>
            QueryShowtimesAsync();

        public Task<List<ShowtimeDetailDto>> GetByMovieTitleAsync(string movieTitle) =>
            QueryShowtimesAsync(ms => ms.Movie != null && ms.Movie.Name.Contains(movieTitle, StringComparison.OrdinalIgnoreCase));

        public Task<List<ShowtimeDetailDto>> GetByGenreAsync(string genre) =>
            QueryShowtimesAsync(ms => ms.Movie != null && ms.Movie.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));

        public Task<List<ShowtimeDetailDto>> GetByCinemaNameAsync(string cinemaName) =>
            QueryShowtimesAsync(ms => ms.Cinema != null && ms.Cinema.Name.Contains(cinemaName, StringComparison.OrdinalIgnoreCase));

        public async Task<List<TariffDto>> GetTariffsByCinemaAsync(string cinemaName)
        {
            return await _context.CinemaTariffs
                .Include(t => t.Cinema)
                .Include(t => t.DayType)
                .Include(t => t.ShowtimeSession)
                .Include(t => t.RoomType)
                .Include(t => t.Movie)
                .Where(t => t.Cinema != null && t.Cinema.Name.Contains(cinemaName) && !t.IsDeleted)
                .Select(t => new TariffDto(
                    t.Name,
                    t.DayType != null ? t.DayType.Name : "",
                    t.ShowtimeSession != null ? t.ShowtimeSession.Name : null,
                    t.RoomType != null ? t.RoomType.Name : null,
                    t.Movie != null ? t.Movie.Name : null,
                    t.IsPremiere,
                    t.IsPopularMovie,
                    t.Price
                ))
                .ToListAsync();
        }

        // --- CREACIONES ---
        public async Task<Cinema> CreateCinemaAsync(CreateCinemaDto dto)
        {
            var cinema = new Cinema { Name = dto.Name, Street = dto.Street, Number = dto.Number, Phone = dto.Phone };
            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();
            return cinema;
        }

        public async Task<RoomType> CreateRoomTypeAsync(CreateRoomTypeDto dto)
        {
            var roomType = new RoomType { Name = dto.Name, ExtraPrice = dto.ExtraPrice };
            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();
            return roomType;
        }

        public async Task<Room> CreateRoomAsync(CreateRoomDto dto)
        {
            var room = new Room
            {
                CinemaId = dto.CinemaId,
                RoomTypeId = dto.RoomTypeId,
                Name = dto.Name,
                RoomNumber = dto.RoomNumber,
                Capacity = dto.Capacity
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Actor> CreateActorAsync(CreateActorDto dto)
        {
            var actor = new Actor { Name = dto.Name, Nationality = dto.Nationality };
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return actor;
        }

        public async Task<Movie> CreateMovieAsync(CreateMovieDto dto)
        {
            var movie = new Movie
            {
                Name = dto.Title,
                Director = dto.Director,
                Genre = dto.Genre,
                Rating = dto.Classification,
                ReleaseDate = dto.ReleaseDate,
                IsPremiere = dto.IsPremiere,
                IsPopular = dto.IsPopular
            };

            // Vincula múltiples actores mediante la entidad intermedia MovieActor[cite: 6]
            foreach (var actorId in dto.ActorIds.Distinct())
            {
                movie.MovieActors.Add(new MovieActor
                {
                    ActorId = actorId,
                    Name = $"Relación {actorId}"
                });
            }

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<CinemaTariff> CreateTariffAsync(CreateTariffDto dto)
        {
            var tariff = new CinemaTariff
            {
                CinemaId = dto.CinemaId,
                Name = dto.Name,
                DayTypeId = dto.DayTypeId,
                ShowtimeSessionId = dto.ShowtimeSessionId,
                RoomTypeId = dto.RoomTypeId,
                MovieId = dto.MovieId,
                IsPremiere = dto.IsPremiere,
                IsPopularMovie = dto.IsPopularMovie,
                Price = dto.Price
            };
            _context.CinemaTariffs.Add(tariff);
            await _context.SaveChangesAsync();
            return tariff;
        }

        public async Task<MovieShowtime> CreateShowtimeAsync(CreateShowtimeDto dto)
        {
            var showtime = new MovieShowtime
            {
                CinemaId = dto.CinemaId,
                RoomId = dto.RoomId,
                MovieId = dto.MovieId,
                ShowtimeSessionId = dto.ShowtimeSessionId,
                DayTypeId = dto.DayTypeId,
                Name = $"Pase {dto.ShowDate:yyyy-MM-dd} {dto.Hour}",
                ShowDate = dto.ShowDate,
                Time = dto.Hour
            };
            _context.MovieShowtimes.Add(showtime);
            await _context.SaveChangesAsync();
            return showtime;
        }
    }
}