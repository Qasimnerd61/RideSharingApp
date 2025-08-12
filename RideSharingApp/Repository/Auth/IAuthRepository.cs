using RideSharingApp.Models;

namespace RideSharingApp.Repository.Auth
{
    public interface IAuthRepository
    {
        Task<int> SignupAsync(SignupRequest request);
        Task<UserModel?> LoginAsync(LoginRequest request);
    }
}
