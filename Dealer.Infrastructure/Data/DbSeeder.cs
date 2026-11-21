using System;
using System.Linq;
using System.Threading.Tasks;
using Dealer.Domain.Entity.Cine;

namespace Dealer.Infrastructure.Data
{
    // Inicializa la base de datos con catálogos, entidades y datos de prueba
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            // Evita duplicar datos si ya existen cines cargados
            if (context.Cinemas.Any()) return;

            // --- 1. CATÁLOGO: TIPOS DE SALA (2D, 3D, 4D, IMAX, VIP) 
            var tipo2D = new RoomType { Name = "2D", ExtraPrice = 0 }; // Formato estándar
            var tipo3D = new RoomType { Name = "3D", ExtraPrice = 150 }; // Recargo por tecnología 3D
            var tipo4D = new RoomType { Name = "4D", ExtraPrice = 250 }; // Recargo por efectos 4D
            var tipoImax = new RoomType { Name = "IMAX", ExtraPrice = 300 }; // Formato gran pantalla IMAX
            var tipoVip = new RoomType { Name = "VIP", ExtraPrice = 200 }; // Sala con mayor confort
            context.RoomTypes.AddRange(tipo2D, tipo3D, tipo4D, tipoImax, tipoVip);

            // --- 2. CATÁLOGO: TANDAS (MATINAL, TARDE, NOCHE) 
            var tandaMatinal = new ShowtimeSession
            {
                Name = "Matinal",
                StartTime = new TimeSpan(09, 00, 00),
                EndTime = new TimeSpan(13, 59, 00)
            }; // Sesiones matinales

            var tandaTarde = new ShowtimeSession
            {
                Name = "Tarde",
                StartTime = new TimeSpan(14, 00, 00),
                EndTime = new TimeSpan(19, 59, 00)
            }; // Sesiones de tarde

            var tandaNoche = new ShowtimeSession
            {
                Name = "Noche",
                StartTime = new TimeSpan(20, 00, 00),
                EndTime = new TimeSpan(23, 59, 00)
            }; // Sesiones nocturnas
            context.ShowtimeSessions.AddRange(tandaMatinal, tandaTarde, tandaNoche);

            // --- 3. CATÁLOGO: TIPOS DE DÍA 
            var diaNormal = new DayTypeEntity { Name = "Normal" }; // Día laborable o estándar
            var diaFestivo = new DayTypeEntity { Name = "Festivo" }; // Día festivo
            var diaVispera = new DayTypeEntity { Name = "Víspera" }; // Víspera de festivo
            var diaEspectador = new DayTypeEntity { Name = "Espectador" }; // Día del espectador
            var diaJubilado = new DayTypeEntity { Name = "Jubilado" }; // Día o tarifa de jubilado
            context.DayTypes.AddRange(diaNormal, diaFestivo, diaVispera, diaEspectador, diaJubilado);

            // --- 4. CINES 
            var saler = new Cinema
            {
                Name = "ABC EL SALER",
                Street = "Centro Comercial El Saler",
                Number = "S/N",
                Phone = "3950592"
            }; // Cine ABC El Saler

            var nuevoCentro = new Cinema
            {
                Name = "CINES NUEVO CENTRO",
                Street = "Avd. Pío XII",
                Number = "2",
                Phone = "3485477"
            }; // Cines Nuevo Centro
            context.Cinemas.AddRange(saler, nuevoCentro);

            // --- 5. SALAS POR CINE ---
            var salerSala1 = new Room { Cinema = saler, RoomType = tipo2D, Name = "Sala 1", RoomNumber = 1, Capacity = 150 }; // Sala 1 en 2D
            var salerSala2 = new Room { Cinema = saler, RoomType = tipo3D, Name = "Sala 2", RoomNumber = 2, Capacity = 120 }; // Sala 2 en 3D
            var ncSala1 = new Room { Cinema = nuevoCentro, RoomType = tipo2D, Name = "Sala 1", RoomNumber = 1, Capacity = 180 }; // Sala 1 Nuevo Centro
            var ncSala2 = new Room { Cinema = nuevoCentro, RoomType = tipoVip, Name = "Sala 2", RoomNumber = 2, Capacity = 90 }; // Sala 2 VIP Nuevo Centro
            context.Rooms.AddRange(salerSala1, salerSala2, ncSala1, ncSala2);

            // --- 6. ACTORES --
            var clint = new Actor { Name = "Clint Eastwood", Nationality = "Estadounidense" }; // Actor de Los puentes de Madison
            var meryl = new Actor { Name = "Meryl Streep", Nationality = "Estadounidense" }; // Actriz de Los puentes de Madison
            var timAllen = new Actor { Name = "Tim Allen", Nationality = "Estadounidense" }; // Actor de ¡Vaya Santa Claus!
            var judge = new Actor { Name = "Judge Reinhold", Nationality = "Estadounidense" }; // Actor de ¡Vaya Santa Claus!
            var animacion = new Actor { Name = "Elenco Animación", Nationality = "Varios" }; // Elenco de Pocahonta
            context.Actors.AddRange(clint, meryl, timAllen, judge, animacion);

            // --- 7. PELÍCULAS ---
            var pocahontas = new Movie
            {
                Name = "Pocahontas",
                Director = "Mike Gabriel",
                Genre = "Dibujos",
                Rating = "T. menores",
                ReleaseDate = new DateTime(1995, 6, 23),
                IsPremiere = false,
                IsPopular = true
            }; // Película Pocahontas

            var puentes = new Movie
            {
                Name = "Los puentes de Madison",
                Director = "Clint Eastwood",
                Genre = "Drama",
                Rating = "No rec. menores 13 años",
                ReleaseDate = DateTime.UtcNow.Date.AddDays(-5),
                IsPremiere = true,
                IsPopular = false
            }; // Película Los puentes de Madison

            var santaClaus = new Movie
            {
                Name = "¡Vaya Santa Claus!",
                Director = "John Pasquin",
                Genre = "Comedia",
                Rating = "T. menores",
                ReleaseDate = DateTime.UtcNow.Date.AddDays(-10),
                IsPremiere = true,
                IsPopular = true
            }; // Película ¡Vaya Santa Claus!
            context.Movies.AddRange(pocahontas, puentes, santaClaus);

            // --- 8. RELACIÓN N:M PELÍCULA <-> ACTOR ---
            context.MovieActors.AddRange(
                new MovieActor { Movie = pocahontas, Actor = animacion, Name = "Pocahontas - Elenco", CharacterName = "Varios", IsLead = true },
                new MovieActor { Movie = puentes, Actor = clint, Name = "Madison - Clint", CharacterName = "Robert Kincaid", IsLead = true },
                new MovieActor { Movie = puentes, Actor = meryl, Name = "Madison - Meryl", CharacterName = "Francesca Johnson", IsLead = true },
                new MovieActor { Movie = santaClaus, Actor = timAllen, Name = "Santa Claus - Tim", CharacterName = "Scott Calvin", IsLead = true },
                new MovieActor { Movie = santaClaus, Actor = judge, Name = "Santa Claus - Judge", CharacterName = "Neal Miller", IsLead = true }
            );

            // --- 9. TARIFAS ---
            context.CinemaTariffs.AddRange(
                new CinemaTariff { Cinema = saler, DayType = diaEspectador, Name = "Miércoles día del espectador", Price = 350 },
                new CinemaTariff { Cinema = saler, DayType = diaFestivo, Name = "Vísperas y festivos", Price = 650 },
                new CinemaTariff { Cinema = saler, DayType = diaNormal, Movie = pocahontas, IsPopularMovie = true, Price = 600, Name = "Tarifa Pocahontas" },
                new CinemaTariff { Cinema = saler, DayType = diaNormal, RoomType = tipo3D, IsPremiere = true, Price = 700, Name = "Tarifa Estreno 3D" },
                new CinemaTariff { Cinema = nuevoCentro, DayType = diaEspectador, Name = "Lunes día del espectador", Price = 350 }
            );

            // --- 10. FUNCIONES / PASES --
            var hoy = DateTime.UtcNow.Date;
            context.MovieShowtimes.AddRange(
                new MovieShowtime
                {
                    Cinema = saler,
                    Room = salerSala1,
                    Movie = pocahontas,
                    ShowtimeSession = tandaTarde,
                    DayType = diaNormal,
                    Name = "Pocahontas Sala 1 Tarde",
                    ShowDate = hoy,
                    Time = "16:30, 18:20"
                },
                new MovieShowtime
                {
                    Cinema = saler,
                    Room = salerSala2,
                    Movie = puentes,
                    ShowtimeSession = tandaNoche,
                    DayType = diaNormal,
                    Name = "Madison Sala 2 Noche",
                    ShowDate = hoy,
                    Time = "22:35"
                },
                new MovieShowtime
                {
                    Cinema = nuevoCentro,
                    Room = ncSala1,
                    Movie = pocahontas,
                    ShowtimeSession = tandaMatinal,
                    DayType = diaNormal,
                    Name = "Pocahontas NC Matinal",
                    ShowDate = hoy,
                    Time = "12:00"
                },
                new MovieShowtime
                {
                    Cinema = nuevoCentro,
                    Room = ncSala2,
                    Movie = santaClaus,
                    ShowtimeSession = tandaNoche,
                    DayType = diaFestivo,
                    Name = "Santa Claus NC VIP Noche",
                    ShowDate = hoy,
                    Time = "20:30, 22:40"
                }
            );

            await context.SaveChangesAsync();
        }
    }
}