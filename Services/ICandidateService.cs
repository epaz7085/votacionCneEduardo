using cneProyectoVotacion.DTOs;

namespace cneProyectoVotacion.Services
{
    public interface ICandidateService
    {
        Task<CandidateResponseDto> CreateCandidate(CreateCandidateDto dto, string createdByUserId);
        Task<List<CandidateResponseDto>> GetAllCandidates();
        Task<CandidateResponseDto?> GetCandidateById(string candidateId);
        Task<CandidateResponseDto?> UpdateCandidate(string candidateId, UpdateCandidateDto dto);
        Task<bool> DeleteCandidate(string candidateId);
    }
}
