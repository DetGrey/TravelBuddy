using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Api.Services;

public interface IWeatherService
{
    Task<WeatherSummary?> GetWeatherForTripAsync(string location, DateOnly start, DateOnly end);
}

public class VisualCrossingWeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public VisualCrossingWeatherService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["VisualCrossing:ApiKey"] ?? throw new InvalidOperationException("VisualCrossing:ApiKey is not configured");
    }

    public async Task<WeatherSummary?> GetWeatherForTripAsync(string location, DateOnly start, DateOnly end)
    {
        try
        {
            // Format dates for API (yyyy-MM-dd)
            var startStr = start.ToString("yyyy-MM-dd");
            var endStr = end.ToString("yyyy-MM-dd");
            
            // Visual Crossing automatically handles "Future" vs "Forecast" based on the date
            var url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{location}/{startStr}/{endStr}?unitGroup=metric&include=days&key={_apiKey}&contentType=json";

            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode) return null; // Fail gracefully

            var json = await response.Content.ReadAsStringAsync();
            dynamic? data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            
            if (data == null) return null;

            // Calculate averages or grab the first day's overview
            // For a trip, an average of the days is usually best for a summary
            double totalMax = 0;
            double totalMin = 0;
            int count = 0;

            foreach (var day in data.days)
            {
                totalMax += (double)day.tempmax;
                totalMin += (double)day.tempmin;
                count++;
            }

            return new WeatherSummary(
                Math.Round(totalMax / count, 1),
                Math.Round(totalMin / count, 1),
                (string)data.description, // Overall trip description
                (string)data.days![0].icon // Use the first day's icon or a generic one
            );
        }
        catch
        {
            // Log error here
            return null; // Return null so the app doesn't crash if weather fails
        }
    }
}