using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dealer.Aplication.DTOs;
using Dealer.Aplication.Interfaces;

namespace Dealer.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Expone los endpoints de consulta y registro para cines, salas, películas, actores y tarifas
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;

        public CinemaController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        // --- CONSULTAS ---
        [HttpGet("showtimes")]
        public async Task<IActionResult> GetAllShowtimes() =>
            Ok(await _cinemaService.GetAllShowtimesAsync());

        [HttpGet("showtimes/movie/{title}")]
        public async Task<IActionResult> GetByMovieTitle(string title) =>
            Ok(await _cinemaService.GetByMovieTitleAsync(title));

        [HttpGet("showtimes/genre/{genre}")]
        public async Task<IActionResult> GetByGenre(string genre) =>
            Ok(await _cinemaService.GetByGenreAsync(genre));

        [HttpGet("showtimes/cinema/{cinemaName}")]
        public async Task<IActionResult> GetByCinemaName(string cinemaName) =>
            Ok(await _cinemaService.GetByCinemaNameAsync(cinemaName));

        [HttpGet("tariffs/{cinemaName}")]
        public async Task<IActionResult> GetTariffsByCinema(string cinemaName) =>
            Ok(await _cinemaService.GetTariffsByCinemaAsync(cinemaName));

        // --- CREACIONES ---
        [HttpPost("cinemas")]
        public async Task<IActionResult> CreateCinema([FromBody] CreateCinemaDto dto) =>
            Ok(await _cinemaService.CreateCinemaAsync(dto));

        [HttpPost("room-types")]
        public async Task<IActionResult> CreateRoomType([FromBody] CreateRoomTypeDto dto) =>
            Ok(await _cinemaService.CreateRoomTypeAsync(dto));

        [HttpPost("rooms")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto dto) =>
            Ok(await _cinemaService.CreateRoomAsync(dto));

        [HttpPost("actors")]
        public async Task<IActionResult> CreateActor([FromBody] CreateActorDto dto) =>
            Ok(await _cinemaService.CreateActorAsync(dto));

        [HttpPost("movies")]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto dto) =>
            Ok(await _cinemaService.CreateMovieAsync(dto));

        [HttpPost("tariffs")]
        public async Task<IActionResult> CreateTariff([FromBody] CreateTariffDto dto) =>
            Ok(await _cinemaService.CreateTariffAsync(dto));

        [HttpPost("showtimes")]
        public async Task<IActionResult> CreateShowtime([FromBody] CreateShowtimeDto dto) =>
            Ok(await _cinemaService.CreateShowtimeAsync(dto));
    }
}