namespace TravelBuddy.Trips.DTOs;

public record DestinationDto(
    int DestinationId,
    string Name,
    string? State,
    string Country,
    decimal? Longitude,
    decimal? Latitude
);