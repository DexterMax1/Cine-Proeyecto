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
    // Servicio principal para gestionar cartelera, salas, actores, tarifas y precios
    public class CinemaService : ICinemaService
    {
        private readonly AppDbContext _context;

        public CinemaService(AppDbContext context)
        {
            _context = context;
        }

        // Calcula el precio considerando tarifa base/específica, sala, día, hora, estreno y fama
        private decimal CalculatePrice(MovieShowtime ms, List<CinemaTariff> tariffs)
        {
            var tariff = tariffs.FirstOrDefault(t =>
                t.CinemaId == ms.CinemaId &&
                (t.MovieId == null || t.MovieId == ms.MovieId) &&
                (t.RoomId == null || t.RoomId == ms.RoomId) &&
                (string.IsNullOrEmpty(t.DayType) || t.DayType.Equals(ms.DayType, StringComparison.OrdinalIgnoreCase)) &&
                (t.IsPremiere == null || t.IsPremiere == ms.IsPremiere) &&
                (t.IsPopularMovie == null || t.IsPopularMovie == (ms.Movie?.IsPopular ?? false))
            );

            decimal basePrice = tariff?.Price ?? 500m; // Precio por defecto si no hay coincidencia exacta
            decimal roomExtra = ms.Room?.ExtraPrice ?? 0m; // Recargo por tipo de sala (ej. 3D, VIP)
            decimal premiereExtra = ms.IsPremiere ? 100m : 0m; // Recargo si es función de estreno

            return basePrice + roomExtra + premiereExtra;
        }

        // Consulta base de funciones con actores y precio estimado
        private async Task<List<ShowtimeDetailDto>> QueryShowtimesAsync(Func<MovieShowtime, bool>? predicate = null)
        {
            var showtimes = await _context.MovieShowtimes
                .Include(ms => ms.Cinema)
                .Include(ms => ms.Room)
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
                ms.Room?.FormatFeature ?? "",
                ms.ShowDate,
                ms.DayType,
                ms.IsPremiere,
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
                .Include(t => t.Movie)
                .Include(t => t.Room)
                .Where(t => t.Cinema != null && t.Cinema.Name.Contains(cinemaName) && !t.IsDeleted)
                .Select(t => new TariffDto(
                    t.Name,
                    t.DayType,
                    t.TimeSlot,
                    t.Movie != null ? t.Movie.Name : null,
                    t.Room != null ? t.Room.Name : null,
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

        public async Task<Room> CreateRoomAsync(CreateRoomDto dto)
        {
            var room = new Room
            {
                CinemaId = dto.CinemaId,
                Name = dto.Name,
                RoomNumber = dto.RoomNumber,
                Capacity = dto.Capacity,
                FormatFeature = dto.FormatFeature,
                ExtraPrice = dto.ExtraPrice
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
                IsPopular = dto.IsPopular
            };

            foreach (var actorId in dto.ActorIds.Distinct())
            {
                movie.MovieActors.Add(new MovieActor
                {
                    ActorId = actorId,
                    Name = $"Actor {actorId}"
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
                DayType = dto.DayType,
                TimeSlot = dto.TimeSlot,
                MovieId = dto.MovieId,
                RoomId = dto.RoomId,
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
                Name = $"Sala {dto.RoomId} - {dto.Hour}",
                ShowDate = dto.ShowDate,
                DayType = dto.DayType,
                Time = dto.Hour,
                IsPremiere = dto.IsPremiere
            };
            _context.MovieShowtimes.Add(showtime);
            await _context.SaveChangesAsync();
            return showtime;
        }
    }
}