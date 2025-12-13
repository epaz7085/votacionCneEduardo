using cneProyectoVotacion.DTOs;
using cneProyectoVotacion.Models;

namespace cneProyectoVotacion.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);

        Task<User?> GetUserById(string userId);
        Task<User?> GetUserByEmail(string email);

        string GenerateJwtToken(User user);
    }
}
