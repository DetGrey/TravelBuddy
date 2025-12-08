using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TravelBuddy.Trips;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.DTOs; // for Trip*Dto types

namespace TravelBuddy.Tests
{
    public class TripServiceTests
    {
        private const int DefaultUserId = 42;
        private const int DefaultTripId = 100;
        private const int DefaultTripDestinationId = 200;
        private const int DefaultChangedBy = 99;

        private readonly Mock<ITripRepository> _tripRepoMock;
        private readonly TripService _tripService;

        public TripServiceTests()
        {
            _tripRepoMock = new Mock<ITripRepository>(MockBehavior.Strict);

            // Same pattern as UserServiceTests: use factory, but keep it local
            var factoryMock = new Mock<ITripRepositoryFactory>(MockBehavior.Strict);
            factoryMock
                .Setup(f => f.GetTripRepository())
                .Returns(_tripRepoMock.Object);

            _tripService = new TripService(factoryMock.Object);
        }

        // --------------------------------------------------------------------
        // IsTripOwnerAsync
        // --------------------------------------------------------------------

        [Fact]
        public async Task IsTripOwnerAsync_ReturnsTrue_WhenUserIsOwner()
        {
            // Arrange
            _tripRepoMock
                .Setup(r => r.GetTripOwnerAsync(DefaultTripDestinationId))
                .ReturnsAsync(DefaultUserId);

            // Act
            var result = await _tripService.IsTripOwnerAsync(DefaultUserId, DefaultTripDestinationId);

            // Assert
            Assert.True(result);
            _tripRepoMock.Verify(r => r.GetTripOwnerAsync(DefaultTripDestinationId), Times.Once);
        }

        [Fact]
        public async Task IsTripOwnerAsync_ReturnsFalse_WhenUserIsNotOwner()
        {
            // Arrange
            const int actualOwnerId = 123;

            _tripRepoMock
                .Setup(r => r.GetTripOwnerAsync(DefaultTripDestinationId))
                .ReturnsAsync(actualOwnerId);

            // Act
            var result = await _tripService.IsTripOwnerAsync(DefaultUserId, DefaultTripDestinationId);

            // Assert
            Assert.False(result);
            _tripRepoMock.Verify(r => r.GetTripOwnerAsync(DefaultTripDestinationId), Times.Once);
        }

        // --------------------------------------------------------------------
        // LeaveTripDestinationAsync
        // --------------------------------------------------------------------

        [Fact]
        public async Task LeaveTripDestinationAsync_UsesDefaultReason_WhenReasonIsNull_AndSelfOrAdmin()
        {
            // Arrange
            (bool Success, string? ErrorMessage) repoResult = (true, null);

            _tripRepoMock
                .Setup(r => r.LeaveTripDestinationAsync(
                    DefaultUserId,
                    DefaultTripDestinationId,
                    DefaultChangedBy,
                    It.Is<string>(reason =>
                        reason.Contains("voluntarily", StringComparison.OrdinalIgnoreCase) ||
                        reason.Contains("admin", StringComparison.OrdinalIgnoreCase)
                    )))
                .ReturnsAsync(repoResult);

            // Act
            var (success, error) = await _tripService.LeaveTripDestinationAsync(
                DefaultUserId,
                DefaultTripDestinationId,
                DefaultChangedBy,
                departureReason: null,
                isSelfOrAdmin: true);

            // Assert
            Assert.True(success);
            Assert.Null(error);

            _tripRepoMock.Verify(r => r.LeaveTripDestinationAsync(
                    DefaultUserId,
                    DefaultTripDestinationId,
                    DefaultChangedBy,
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task LeaveTripDestinationAsync_UsesDefaultReason_WhenReasonIsNull_AndNotSelfOrAdmin()
        {
            // Arrange
            (bool Success, string? ErrorMessage) repoResult = (true, null);

            _tripRepoMock
                .Setup(r => r.LeaveTripDestinationAsync(
                    DefaultUserId,
                    DefaultTripDestinationId,
                    DefaultChangedBy,
                    It.Is<string>(reason =>
                        reason.Contains("Removed by owner", StringComparison.OrdinalIgnoreCase)
                    )))
                .ReturnsAsync(repoResult);

            // Act
            var (success, error) = await _tripService.LeaveTripDestinationAsync(
                DefaultUserId,
                DefaultTripDestinationId,
                DefaultChangedBy,
                departureReason: null,
                isSelfOrAdmin: false);

            // Assert
            Assert.True(success);
            Assert.Null(error);

            _tripRepoMock.Verify(r => r.LeaveTripDestinationAsync(
                    DefaultUserId,
                    DefaultTripDestinationId,
                    DefaultChangedBy,
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task LeaveTripDestinationAsync_PassesThroughCustomReason_WhenProvided()
        {
            // Arrange
            const string customReason = "Left because of schedule change";

            (bool Success, string? ErrorMessage) repoResult = (true, null);

            _tripRepoMock
                .Setup(r => r.LeaveTripDestinationAsync(
                    DefaultUserId,
                    DefaultTripDestinationId,
                    DefaultChangedBy,
                    customReason))
                .ReturnsAsync(repoResult);

            // Act
            var (success, error) = await _tripService.LeaveTripDestinationAsync(
                DefaultUserId,
                DefaultTripDestinationId,
                DefaultChangedBy,
                departureReason: customReason,
                isSelfOrAdmin: true);

            // Assert
            Assert.True(success);
            Assert.Null(error);

            _tripRepoMock.Verify(r => r.LeaveTripDestinationAsync(
                    DefaultUserId,
                    DefaultTripDestinationId,
                    DefaultChangedBy,
                    customReason),
                Times.Once);
        }

        // --------------------------------------------------------------------
        // UpdateTripInfoAsync (owner updates)
        // --------------------------------------------------------------------

        [Fact]
        public async Task UpdateTripInfoAsync_ReturnsError_WhenBothNameAndDescriptionAreEmpty()
        {
            // Arrange
            // No repository setup: we expect service to fail before calling repo

            // Act
            var (success, error) = await _tripService.UpdateTripInfoAsync(
                tripId: DefaultTripId,
                ownerId: DefaultUserId,
                tripName: "   ",           // whitespace only
                description: null);

            // Assert
            Assert.False(success);
            Assert.Equal("At least one field (trip name or description) must be provided for update.", error);

            _tripRepoMock.Verify(r => r.UpdateTripInfoAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string?>(),
                    It.IsAny<string?>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateTripInfoAsync_CallsRepository_WhenNameProvided()
        {
            // Arrange
            const string newName = "Updated Trip Name";

            (bool Success, string? ErrorMessage) repoResult = (true, null);

            _tripRepoMock
                .Setup(r => r.UpdateTripInfoAsync(
                    DefaultTripId,
                    DefaultUserId,
                    newName,
                    null))
                .ReturnsAsync(repoResult);

            // Act
            var (success, error) = await _tripService.UpdateTripInfoAsync(
                tripId: DefaultTripId,
                ownerId: DefaultUserId,
                tripName: newName,
                description: null);

            // Assert
            Assert.True(success);
            Assert.Null(error);

            _tripRepoMock.Verify(r => r.UpdateTripInfoAsync(
                    DefaultTripId,
                    DefaultUserId,
                    newName,
                    null),
                Times.Once);
        }

        [Fact]
        public async Task UpdateTripInfoAsync_CallsRepository_WhenDescriptionProvided()
        {
            // Arrange
            const string newDescription = "New description only";

            (bool Success, string? ErrorMessage) repoResult = (true, null);

            _tripRepoMock
                .Setup(r => r.UpdateTripInfoAsync(
                    DefaultTripId,
                    DefaultUserId,
                    null,
                    newDescription))
                .ReturnsAsync(repoResult);

            // Act
            var (success, error) = await _tripService.UpdateTripInfoAsync(
                tripId: DefaultTripId,
                ownerId: DefaultUserId,
                tripName: null,
                description: newDescription);

            // Assert
            Assert.True(success);
            Assert.Null(error);

            _tripRepoMock.Verify(r => r.UpdateTripInfoAsync(
                    DefaultTripId,
                    DefaultUserId,
                    null,
                    newDescription),
                Times.Once);
        }

        // --------------------------------------------------------------------
        // DeleteTripAsync (admin delete wrapper)
        // --------------------------------------------------------------------

        [Fact]
        public async Task DeleteTripAsync_DelegatesToRepository_AndReturnsResult()
        {
            // Arrange
            (bool Success, string? ErrorMessage) repoResult = (false, "Trip already deleted");

            _tripRepoMock
                .Setup(r => r.DeleteTripAsync(DefaultTripId, DefaultChangedBy))
                .ReturnsAsync(repoResult);

            // Act
            var (success, error) = await _tripService.DeleteTripAsync(DefaultTripId, DefaultChangedBy);

            // Assert
            Assert.False(success);
            Assert.Equal("Trip already deleted", error);

            _tripRepoMock.Verify(r => r.DeleteTripAsync(DefaultTripId, DefaultChangedBy), Times.Once);
        }

        // --------------------------------------------------------------------
        // Mapping tests – read methods
        // --------------------------------------------------------------------

        [Fact]
        public async Task GetTripDestinationInfoAsync_ReturnsNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            _tripRepoMock
                .Setup(r => r.GetTripDestinationInfoAsync(DefaultTripDestinationId))
                .ReturnsAsync((TripDestinationInfo?)null);

            // Act
            var result = await _tripService.GetTripDestinationInfoAsync(DefaultTripDestinationId);

            // Assert
            Assert.Null(result);
            _tripRepoMock.Verify(r => r.GetTripDestinationInfoAsync(DefaultTripDestinationId), Times.Once);
        }

        [Fact]
        public async Task GetTripDestinationInfoAsync_MapsModelToDto_WhenResultExists()
        {
            // Arrange
            var startDate = new DateOnly(2025, 5, 1);
            var endDate = new DateOnly(2025, 5, 10);

            var model = new TripDestinationInfo
            {
                TripDestinationId = DefaultTripDestinationId,
                DestinationStartDate = startDate,
                DestinationEndDate = endDate,
                DestinationDescription = "Some description",
                DestinationIsArchived = false,
                TripId = DefaultTripId,
                MaxBuddies = 5,
                DestinationId = 7,
                DestinationName = "Copenhagen",
                DestinationState = "Hovedstaden",
                DestinationCountry = "Denmark",
                Longitude = 12.5683M,
                Latitude = 55.6761M,
                OwnerUserId = DefaultUserId,
                OwnerName = "Owner Name",
                GroupConversationId = 555,
                AcceptedBuddies = new List<BuddyInfo>
                {
                    new BuddyInfo
                    {
                        BuddyId = 1,
                        PersonCount = 2,
                        BuddyNote = "Note",
                        BuddyUserId = 10,
                        BuddyName = "Alice"
                    }
                },
                PendingRequests = new List<BuddyRequestInfo>
                {
                    new BuddyRequestInfo
                    {
                        BuddyId = 2,
                        PersonCount = 1,
                        BuddyNote = "Request note",
                        RequesterUserId = 11,
                        RequesterName = "Bob"
                    }
                }
            };

            _tripRepoMock
                .Setup(r => r.GetTripDestinationInfoAsync(DefaultTripDestinationId))
                .ReturnsAsync(model);

            // Act
            var result = await _tripService.GetTripDestinationInfoAsync(DefaultTripDestinationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.TripDestinationId, result!.TripDestinationId);
            Assert.Equal(model.DestinationStartDate, result.DestinationStartDate);
            Assert.Equal(model.DestinationEndDate, result.DestinationEndDate);
            Assert.Equal(model.DestinationDescription, result.DestinationDescription);
            Assert.Equal(model.DestinationIsArchived, result.DestinationIsArchived);
            Assert.Equal(model.TripId, result.TripId);
            Assert.Equal(model.MaxBuddies, result.MaxBuddies);
            Assert.Equal(model.DestinationId, result.DestinationId);
            Assert.Equal(model.DestinationName, result.DestinationName);
            Assert.Equal(model.DestinationState, result.DestinationState);
            Assert.Equal(model.DestinationCountry, result.DestinationCountry);
            Assert.Equal(model.Longitude, result.Longitude);
            Assert.Equal(model.Latitude, result.Latitude);
            Assert.Equal(model.OwnerUserId, result.OwnerUserId);
            Assert.Equal(model.OwnerName, result.OwnerName);
            Assert.Equal(model.GroupConversationId, result.GroupConversationId);

            var accepted = result.AcceptedBuddies.ToList();
            var pending = result.PendingRequests.ToList();

            Assert.Single(accepted);
            Assert.Single(pending);

            Assert.Equal(model.AcceptedBuddies[0].BuddyId, accepted[0].BuddyId);
            Assert.Equal(model.AcceptedBuddies[0].PersonCount, accepted[0].PersonCount);
            Assert.Equal(model.AcceptedBuddies[0].BuddyNote, accepted[0].BuddyNote);
            Assert.Equal(model.AcceptedBuddies[0].BuddyUserId, accepted[0].BuddyUserId);
            Assert.Equal(model.AcceptedBuddies[0].BuddyName, accepted[0].BuddyName);

            Assert.Equal(model.PendingRequests[0].BuddyId, pending[0].BuddyId);
            Assert.Equal(model.PendingRequests[0].PersonCount, pending[0].PersonCount);
            Assert.Equal(model.PendingRequests[0].BuddyNote, pending[0].BuddyNote);
            Assert.Equal(model.PendingRequests[0].RequesterUserId, pending[0].RequesterUserId);
            Assert.Equal(model.PendingRequests[0].RequesterName, pending[0].RequesterName);

            _tripRepoMock.Verify(r => r.GetTripDestinationInfoAsync(DefaultTripDestinationId), Times.Once);
        }

        [Fact]
        public async Task GetFullTripOverviewAsync_ReturnsNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            _tripRepoMock
                .Setup(r => r.GetFullTripOverviewAsync(DefaultTripId))
                .ReturnsAsync((TripOverview?)null);

            // Act
            var result = await _tripService.GetFullTripOverviewAsync(DefaultTripId);

            // Assert
            Assert.Null(result);
            _tripRepoMock.Verify(r => r.GetFullTripOverviewAsync(DefaultTripId), Times.Once);
        }

        [Fact]
        public async Task GetFullTripOverviewAsync_MapsModelToDto_WhenResultExists()
        {
            // Arrange
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 15);

            var overview = new TripOverview
            {
                TripId = DefaultTripId,
                TripName = "Summer Trip",
                TripStartDate = startDate,
                TripEndDate = endDate,
                MaxBuddies = 4,
                TripDescription = "Description",
                OwnerUserId = DefaultUserId,
                OwnerName = "Owner Name",
                Destinations = new List<SimplifiedTripDestination>
                {
                    new SimplifiedTripDestination
                    {
                        TripDestinationId = DefaultTripDestinationId,
                        TripId = DefaultTripId,
                        DestinationStartDate = startDate,
                        DestinationEndDate = endDate,
                        DestinationName = "Copenhagen",
                        DestinationState = "Hovedstaden",
                        DestinationCountry = "Denmark",
                        MaxBuddies = 3,
                        AcceptedBuddiesCount = 2
                    }
                }
            };

            _tripRepoMock
                .Setup(r => r.GetFullTripOverviewAsync(DefaultTripId))
                .ReturnsAsync(overview);

            // Act
            var result = await _tripService.GetFullTripOverviewAsync(DefaultTripId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(overview.TripId, result!.TripId);
            Assert.Equal(overview.TripName, result.TripName);
            Assert.Equal(overview.TripStartDate, result.TripStartDate);
            Assert.Equal(overview.TripEndDate, result.TripEndDate);
            Assert.Equal(overview.MaxBuddies, result.MaxBuddies);
            Assert.Equal(overview.TripDescription, result.TripDescription);
            Assert.Equal(overview.OwnerUserId, result.OwnerUserId);
            Assert.Equal(overview.OwnerName, result.OwnerName);

            var dests = result.Destinations.ToList();
            Assert.Single(dests);

            Assert.Equal(overview.Destinations[0].TripDestinationId, dests[0].TripDestinationId);
            Assert.Equal(overview.Destinations[0].TripId, dests[0].TripId);
            Assert.Equal(overview.Destinations[0].DestinationStartDate, dests[0].DestinationStartDate);
            Assert.Equal(overview.Destinations[0].DestinationEndDate, dests[0].DestinationEndDate);
            Assert.Equal(overview.Destinations[0].DestinationName, dests[0].DestinationName);
            Assert.Equal(overview.Destinations[0].DestinationState, dests[0].DestinationState);
            Assert.Equal(overview.Destinations[0].DestinationCountry, dests[0].DestinationCountry);
            Assert.Equal(overview.Destinations[0].MaxBuddies, dests[0].MaxBuddies);
            Assert.Equal(overview.Destinations[0].AcceptedBuddiesCount, dests[0].AcceptedBuddiesCount);

            _tripRepoMock.Verify(r => r.GetFullTripOverviewAsync(DefaultTripId), Times.Once);
        }
    }
}

