using Newtonsoft.Json;

namespace RideSharingApp.Models
{
    public class SignupRequest
    {

        [JsonProperty("fName")]
        public string FName { get; set; }

        [JsonProperty("lName")]
        public string LName { get; set; }
        [JsonProperty("phoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
