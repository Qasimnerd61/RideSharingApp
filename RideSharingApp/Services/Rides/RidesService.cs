using Microsoft.SqlServer.Types;
using RideSharingApp.Models;
using RideSharingApp.Repository.Rides;
using System.Data.SqlTypes;

namespace RideSharingApp.Services.Rides
{
    public class RidesService : IRidesService
    {
        private readonly IRidesRepository _ridesRepository;

        public RidesService(IRidesRepository ridesRepository)
        {
            _ridesRepository = ridesRepository;
        }

        public async Task<int> CreateRideAsync(RidesRequest rideDto)
        {
            // Decode polyline to geography LINESTRING (if driver and polyline provided)
            SqlGeography? routeGeography = null;
            if (rideDto.UserType == "Driver" && !string.IsNullOrEmpty(rideDto.EncodedPolyline))
            {
                var points = PolylineDecoder.DecodePolyline(rideDto.EncodedPolyline); // Use your helper

                if (points != null && points.Count > 1)
                {
                    var lineStringWkt = PolylineDecoder.ToLineStringWkt(points);
                    routeGeography = SqlGeography.STLineFromText(new SqlChars(lineStringWkt), 4326);
                }
            }

            // Pass DTO & decoded route to repository
            return await _ridesRepository.CreateRideAsync(rideDto, routeGeography);
        }
        public async Task<IEnumerable<RideDto>> GetAvailableRidesForPassengerAsync(MatchingRidesRequest request)
        {
            return await _ridesRepository.GetAvailableRidesForPassengerAsync(request);
        }
        public async Task<IEnumerable<RideMatchDto>> GetPendingRequestsForDriver(int driverId)
        {
            return await _ridesRepository.GetPendingRequestsForDriver(driverId);
        }

        public async Task<int> AddRideMatch(RideMatchRequest request)
        {
            return await _ridesRepository.AddRideMatch(request);
        }
        public async Task UpdateRideMatchStatusAsync(int matchId, string newStatus)
        {
            await _ridesRepository.UpdateRideMatchStatusAsync(matchId, newStatus);
        }
    }
}
