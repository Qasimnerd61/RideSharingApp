using Newtonsoft.Json;

namespace RideSharingApp.Models
{
    public class MatchingRidesRequest
    {
        [JsonProperty("startLat")]
        public double StartLat { get; set; }
        [JsonProperty("startLng")]
        public double StartLng { get; set; }
        [JsonProperty("endLat")]
        public double EndLat { get; set; }
        [JsonProperty("endLng")]
        public double EndLng { get; set; }
        [JsonProperty("bufferDistance")]
        public double BufferDistance { get; set; } = 1000; // Default buffer in meters
        [JsonProperty("departureTime")]
        public string DepartureTime { get; set; }
        [JsonProperty("passengerRideId")]
        public int PassengerRideId { get; set; }

    }
}
