using RideSharingApp.Models;

namespace RideSharingApp.Services.Auth
{
    public interface IAuthService
    {
        Task<int> SignupAsync(SignupRequest request);
    }
}
