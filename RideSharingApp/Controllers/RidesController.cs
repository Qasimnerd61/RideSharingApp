using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSharingApp.Models;
using RideSharingApp.Services.Rides;

namespace RideSharingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : ControllerBase
    {
        private readonly IRidesService _ridesService;

        public RidesController(IRidesService ridesService)
        {
            _ridesService = ridesService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateRide([FromBody] RidesRequest rideDto)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var createdRide = await _ridesService.CreateRideAsync(rideDto);
            return Ok(createdRide);
        }
        [HttpPost("available")]
        public async Task<ActionResult<RideDto>> GetAvailableRidesForPassenger([FromBody] MatchingRidesRequest request)
        {
            var matchingRides = await _ridesService.GetAvailableRidesForPassengerAsync(request);
            return Ok(matchingRides);
        }
        [HttpPost("ride-match")]
        public async Task<ActionResult<int>> AddRideMatch([FromBody] RideMatchRequest request)
        {
            var matchId = await _ridesService.AddRideMatch(request);
            return Ok(new { MatchId = matchId });
        }
        [HttpGet("pending/driver/{driverId}")]
        public async Task<ActionResult<RideDto>> GetPendingRideRequests(int driverId)
        {
            var matches = await _ridesService.GetPendingRequestsForDriver(driverId);
            return Ok(matches);
        }
        [HttpPost("update-status")]
        public async Task<ActionResult> UpdateRideMatchStatus([FromQuery] int matchId, [FromQuery] string newStatus)
        {
            await _ridesService.UpdateRideMatchStatusAsync(matchId, newStatus);
            return Ok(new { message = $"Ride match updated to {newStatus}" });
        }
    }
}
