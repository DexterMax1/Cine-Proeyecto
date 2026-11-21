using Microsoft.EntityFrameworkCore;
using Dealer.Domain.Entity.Cine;

namespace Dealer.Infrastructure.Data
{
    // Contexto principal de Entity Framework Core para el dominio de cine
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // --- TABLAS Y CATÁLOGOS (DbSets) ---
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; } // Tipos de sala (2D, 3D, 4D, IMAX, VIP)
        public DbSet<Room> Rooms { get; set; } // Salas de cada cine
        public DbSet<ShowtimeSession> ShowtimeSessions { get; set; } // Tandas o franjas horarias (Matinal, Tarde, Noche)
        public DbSet<DayTypeEntity> DayTypes { get; set; } // Tipos de día (Normal, Festivo, Víspera, Espectador, Jubilado)
        public DbSet<Movie> Movies { get; set; } // Películas
        public DbSet<Actor> Actors { get; set; } // Actores
        public DbSet<MovieActor> MovieActors { get; set; } // Relación N:M entre Película y Actor
        public DbSet<MovieShowtime> MovieShowtimes { get; set; } // Pases o funciones programadas
        public DbSet<CinemaTariff> CinemaTariffs { get; set; } // Tarifas de precios

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- SALAS Y TIPO DE SALA ---
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Cinema)
                .WithMany(c => c.Rooms)
                .HasForeignKey(r => r.CinemaId)
                .OnDelete(DeleteBehavior.Restrict); // Un cine tiene varias salas

            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomType)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Cada sala tiene un tipo o formato

            // --- RELACIÓN N:M PELÍCULA <-> ACTOR ---
            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // Película asociada en la tabla intermedia

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId)
                .OnDelete(DeleteBehavior.Restrict); // Actor asociado en la tabla intermedia

            // --- FUNCIONES / PASES (MOVIESHOWTIME) ---
            modelBuilder.Entity<MovieShowtime>()
                .HasOne(ms => ms.Cinema)
                .WithMany(c => c.Showtimes)
                .HasForeignKey(ms => ms.CinemaId)
                .OnDelete(DeleteBehavior.Restrict); // Cine donde se proyecta el pase

            modelBuilder.Entity<MovieShowtime>()
                .HasOne(ms => ms.Room)
                .WithMany(r => r.Showtimes)
                .HasForeignKey(ms => ms.RoomId)
                .OnDelete(DeleteBehavior.Restrict); // Sala donde se realiza el pase

            modelBuilder.Entity<MovieShowtime>()
                .HasOne(ms => ms.Movie)
                .WithMany(m => m.Showtimes)
                .HasForeignKey(ms => ms.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // Película proyectada en el pase

            modelBuilder.Entity<MovieShowtime>()
                .HasOne(ms => ms.ShowtimeSession)
                .WithMany(ss => ss.Showtimes)
                .HasForeignKey(ms => ms.ShowtimeSessionId)
                .OnDelete(DeleteBehavior.Restrict); // Tanda o franja horaria del pase

            modelBuilder.Entity<MovieShowtime>()
                .HasOne(ms => ms.DayType)
                .WithMany(dt => dt.Showtimes)
                .HasForeignKey(ms => ms.DayTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Tipo de día del pase

            // --- TARIFAS (CINEMATARIFF) ---
            modelBuilder.Entity<CinemaTariff>()
                .HasOne(t => t.Cinema)
                .WithMany(c => c.Tariffs)
                .HasForeignKey(t => t.CinemaId)
                .OnDelete(DeleteBehavior.Restrict); // Cine tiene múltiples tarifas

            modelBuilder.Entity<CinemaTariff>()
                .HasOne(t => t.RoomType)
                .WithMany()
                .HasForeignKey(t => t.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Tarifa según tipo de sala

            modelBuilder.Entity<CinemaTariff>()
                .HasOne(t => t.Movie)
                .WithMany(m => m.Tariffs)
                .HasForeignKey(t => t.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // Tarifa específica por película

            modelBuilder.Entity<CinemaTariff>()
                .HasOne(t => t.DayType)
                .WithMany(dt => dt.Tariffs)
                .HasForeignKey(t => t.DayTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Tarifa según tipo de día

            modelBuilder.Entity<CinemaTariff>()
                .HasOne(t => t.ShowtimeSession)
                .WithMany()
                .HasForeignKey(t => t.ShowtimeSessionId)
                .OnDelete(DeleteBehavior.Restrict); // Tarifa según tanda horaria
        }
    }
}