using Newtonsoft.Json;

namespace RideSharingApp.Models
{
    public class RideMatchDto
    {
        [JsonProperty("matchId")]
        public int MatchId { get; set; }
        [JsonProperty("passengerRideId")]
        public int PassengerRideId { get; set; }
        [JsonProperty("driverRideId")]
        public int DriverRideId { get; set; }
        [JsonProperty("fare")]
        public decimal? Fare { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("requestedAt")]
        public DateTime RequestedAt { get; set; }
        [JsonProperty("passengerName")]
        public string PassengerName { get; set; }
        [JsonProperty("passengerPickup")]
        public string PassengerPickup { get; set; }
        [JsonProperty("passengerDropoff")]
        public string PassengerDropoff { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

    }
}
