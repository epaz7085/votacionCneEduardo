using Google.Cloud.Firestore;
using cneProyectoVotacion.DTOs;
using cneProyectoVotacion.Models;

namespace cneProyectoVotacion.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly CollectionReference _candidates;

        public CandidateService(FirebaseServices firebase)
        {
            _candidates = firebase.GetCollection("candidates");
        }

        public async Task<CandidateResponseDto> CreateCandidate(CreateCandidateDto dto, string createdByUserId)
        {
            var id = Guid.NewGuid().ToString();

            var candidate = new Candidate
            {
                Id = id,
                Name = dto.Name,
                Party = dto.Party,
                PhotoUrl = dto.PhotoUrl,
                LogoUrl = dto.LogoUrl,
                Proposals = dto.Proposals,
                VotesCount = 0,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdByUserId,
                IsActive = true
            };

            await _candidates.Document(id).SetAsync(candidate);
            return Map(candidate);
        }

        // GET ALL
        public async Task<List<CandidateResponseDto>> GetAllCandidates()
        {
            var snapshot = await _candidates.GetSnapshotAsync();
            return snapshot.Documents
                .Select(d => Map(d.ConvertTo<Candidate>()))
                .ToList();
        }

        // GET BY ID
        public async Task<CandidateResponseDto?> GetCandidateById(string candidateId)
        {
            var snap = await _candidates.Document(candidateId).GetSnapshotAsync();
            if (!snap.Exists) return null;
            return Map(snap.ConvertTo<Candidate>());
        }

        // UPDATE 
        public async Task<CandidateResponseDto?> UpdateCandidate(string candidateId, UpdateCandidateDto dto)
        {
            var doc = _candidates.Document(candidateId);
            var snap = await doc.GetSnapshotAsync();
            if (!snap.Exists) return null;

            var updates = new Dictionary<string, object>();

            if (dto.Name != null) updates["Name"] = dto.Name;
            if (dto.Party != null) updates["Party"] = dto.Party;
            if (dto.PhotoUrl != null) updates["PhotoUrl"] = dto.PhotoUrl;
            if (dto.LogoUrl != null) updates["LogoUrl"] = dto.LogoUrl;
            if (dto.Proposals != null) updates["Proposals"] = dto.Proposals;

            if (updates.Any())
                await doc.UpdateAsync(updates);

            var updated = (await doc.GetSnapshotAsync()).ConvertTo<Candidate>();
            return Map(updated);
        }

        // DELETE (solo si no tiene votos)
        public async Task<bool> DeleteCandidate(string candidateId)
        {
            var doc = _candidates.Document(candidateId);
            var snap = await doc.GetSnapshotAsync();
            if (!snap.Exists) return false;

            var candidate = snap.ConvertTo<Candidate>();
            if (candidate.VotesCount > 0) return false;

            await doc.DeleteAsync();
            return true;
        }

        // MAPPER
        private static CandidateResponseDto Map(Candidate c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Party = c.Party,
            PhotoUrl = c.PhotoUrl,
            LogoUrl = c.LogoUrl,
            Proposals = c.Proposals,
            VotesCount = c.VotesCount
        };
    }
}
