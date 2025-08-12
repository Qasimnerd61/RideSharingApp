using Microsoft.SqlServer.Types;
using RideSharingApp.Models;

namespace RideSharingApp.Repository.Rides
{
    public interface IRidesRepository
    {
        Task<int> CreateRideAsync(RidesRequest rideDto, SqlGeography? routeGeography);
        Task<IEnumerable<RideDto>> GetAvailableRidesForPassengerAsync(MatchingRidesRequest request);
        Task<IEnumerable<RideMatchDto>> GetPendingRequestsForDriver(int driverId);
        Task<int> AddRideMatch(RideMatchRequest request);
        Task UpdateRideMatchStatusAsync(int matchId, string newStatus);
    }
}
