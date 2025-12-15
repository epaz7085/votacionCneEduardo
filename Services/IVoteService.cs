using cneProyectoVotacion.Models;

namespace cneProyectoVotacion.Services
{
    public interface IVoteService
    {
        Task<Vote> CastVote(string userId, string candidateId);
        Task<bool> HasUserVoted(string userId);
    }
}
