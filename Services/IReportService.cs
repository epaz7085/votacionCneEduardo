using cneProyectoVotacion.DTOs;

namespace cneProyectoVotacion.Services
{
    public interface IReportService
    {
        Task<VoteStatisticsDto> GetVoteStatistics();
    }
}
