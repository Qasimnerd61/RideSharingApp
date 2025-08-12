using Newtonsoft.Json;

namespace RideSharingApp.Models
{
    public class RideMatchRequest
    {
        [JsonProperty("driverRideId")]
        public int DriverRideId { get; set; }
        [JsonProperty("passengerRideId")]
        public int PassengerRideId { get; set; }
        [JsonProperty("fare")]
        public decimal? Fare { get; set; }
    }
}
