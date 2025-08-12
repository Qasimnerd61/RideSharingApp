using RideSharingApp.Models;

namespace RideSharingApp.Services.Rides
{
    public interface IRidesService
    {
        Task<int> CreateRideAsync(RidesRequest rideDto);
        Task<IEnumerable<RideDto>> GetAvailableRidesForPassengerAsync(MatchingRidesRequest request);
        Task<IEnumerable<RideMatchDto>> GetPendingRequestsForDriver(int driverId);
        Task<int> AddRideMatch(RideMatchRequest request);
        Task UpdateRideMatchStatusAsync(int matchId, string newStatus);
    }
}
