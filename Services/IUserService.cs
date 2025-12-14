using cneProyectoVotacion.DTOs;

namespace cneProyectoVotacion.Services
{
    public interface IUserService
    {
        Task<List<UserAuditDto>> GetAllUsers();
    }
}
