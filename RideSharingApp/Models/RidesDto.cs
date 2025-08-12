using Newtonsoft.Json;

namespace RideSharingApp.Models
{
    public class RideDto
    {
        [JsonProperty("rideId")]
        public int RideId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("userType")]
        public string UserType { get; set; }

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

        // Add other fields as needed
    }

}
