namespace TravelBuddy.Trips.DTOs;

public record WeatherSummary(
    double MaxTemp,
    double MinTemp,
    string Description, // e.g., "Partly cloudy throughout the day"
    string Icon // Visual Crossing gives icon names like "rain", "clear-day"
);

public record BuddyTripWithWeatherDto(
    BuddyTripSummaryDto TripDetails, 
    WeatherSummary? Weather // Nullable in case the API fails or dates are too far in the future
);