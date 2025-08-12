using RideSharingApp.Models;
using RideSharingApp.Repository.Auth;

namespace RideSharingApp.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<int> SignupAsync(SignupRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            // Hash the password here (if needed)
            // request.Password = HashPassword(request.Password);

            return await _authRepository.SignupAsync(request);
        }
        public async Task<UserModel?> LoginAsync(LoginRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            // Hash the password here (if needed)
            // request.Password = HashPassword(request.Password);

            return await _authRepository.LoginAsync(request);
        }
    }
}
