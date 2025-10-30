namespace TravelBuddy.Trips
{
    // DTO (Data Transfer Object)
    public record TripDestinationDto(
        int TripDestinationId,
        int DestinationId,
        int TripId,
        DateOnly StartDate,
        DateOnly EndDate,
        int SequenceNumber,
        string? Description,
        bool? IsArchived
    );

    public record TripDestinationSearchDto(
        int TripDestinationId,
        int TripId,
        int DestinationId,
        string DestinationName,
        string Country,
        string State,
        DateOnly DestinationStart,
        DateOnly DestinationEnd,
        int MaxBuddies,
        int AcceptedPersons,
        int RemainingCapacity
    );

    // INTERFACE
    public interface ITripDestinationService
    {
        Task<IEnumerable<TripDestinationSearchDto>> SearchTripsAsync(
            DateOnly? reqStart,
            DateOnly? reqEnd,
            string? country,
            string? state,
            string? name,
            int? partySize,
            string? q
        );
    }

    // CLASS
    public class TripDestinationService : ITripDestinationService
    {
        private readonly ITripDestinationRepository _repository;

        public TripDestinationService(ITripDestinationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TripDestinationSearchDto>> SearchTripsAsync(
            DateOnly? reqStart,
            DateOnly? reqEnd,
            string? country,
            string? state,
            string? name,
            int? partySize,
            string? q)
        {
            var results = await _repository.SearchTripsAsync(reqStart, reqEnd, country, state, name, partySize, q);

              return results.Select(r => new TripDestinationSearchDto(
                r.TripDestinationId,
                r.TripId,
                r.DestinationId,
                r.DestinationName,
                r.Country,
                r.State,
                r.DestinationStart,
                r.DestinationEnd,
                r.MaxBuddies,
                r.AcceptedPersons,
                r.RemainingCapacity
            )).ToList();

        }
    }
}