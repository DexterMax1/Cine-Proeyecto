using System;
using System.Collections.Generic;

namespace Dealer.Aplication.DTOs
{
	// Tarifa con sus condiciones de sala, día, tanda, película y estreno/fama
	public record TariffDto(
		string Name,
		string DayTypeName,
		string? SessionName,
		string? RoomTypeName,
		string? MovieTitle,
		bool? IsPremiere,
		bool? IsPopularMovie,
		decimal Price
	);

	// Detalle completo de una función en cartelera
	public record ShowtimeDetailDto(
		string CinemaName,
		string Street,
		string Number,
		string Phone,
		int RoomNumber,
		string RoomTypeName,
		string SessionName,
		string DayTypeName,
		DateTime ShowDate,
		bool IsPremiere,
		string MovieTitle,
		string Director,
		string Genre,
		string Classification,
		bool IsPopularMovie,
		List<string> Actors,
		string Hour,
		decimal EstimatedPrice
	);

	// DTOs de creación para catálogos y entidades principales
	public record CreateCinemaDto(string Name, string Street, string Number, string Phone);

	public record CreateRoomTypeDto(string Name, decimal ExtraPrice);

	public record CreateRoomDto(Guid CinemaId, Guid RoomTypeId, string Name, int RoomNumber, int Capacity);

	public record CreateActorDto(string Name, string? Nationality);

	public record CreateMovieDto(
		string Title,
		string Director,
		string Genre,
		string Classification,
		DateTime? ReleaseDate,
		bool IsPremiere,
		bool IsPopular,
		List<Guid> ActorIds
	);

	public record CreateTariffDto(
		Guid CinemaId,
		string Name,
		Guid? DayTypeId,
		Guid? ShowtimeSessionId,
		Guid? RoomTypeId,
		Guid? MovieId,
		bool? IsPremiere,
		bool? IsPopularMovie,
		decimal Price
	);

	public record CreateShowtimeDto(
		Guid CinemaId,
		Guid RoomId,
		Guid MovieId,
		Guid? ShowtimeSessionId,
		Guid? DayTypeId,
		DateTime ShowDate,
		string Hour
	);
}