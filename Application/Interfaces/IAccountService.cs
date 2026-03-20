using Application.DTOs;
using Application.Wrappers;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<AuthenticationResponse>> Authenticate(AuthenticationRequest request);
        Task<ApiResponse<Guid>> RegisterUser(RegisterRequest registerRequest);
        Task<ApiResponse<bool>> ConfirmEmail(string userId, string token);
    }
}
