using Newtonsoft.Json;

namespace RideSharingApp.Models
{
    public class RidesRequest
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("userType")]
        public string UserType { get; set; } // "Driver" or "Passenger"
        [JsonProperty("startLat")]
        public double StartLat { get; set; }
        [JsonProperty("startLng")]
        public double StartLng { get; set; }
        [JsonProperty("endLat")]
        public double EndLat { get; set; }
        [JsonProperty("endLng")]
        public double EndLng { get; set; }

        [JsonProperty("encodedPolyline")]
        public string? EncodedPolyline { get; set; }

        [JsonProperty("departureTime")]
        public DateTime DepartureTime { get; set; }

        [JsonProperty("availableSeats")]
        public int? AvailableSeats { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("pickupLocationName")]
        public string? PickupLocationName { get; set; }
        [JsonProperty("dropoffLocationName")]
        public string? DropoffLocationName { get; set; }
    }

}
