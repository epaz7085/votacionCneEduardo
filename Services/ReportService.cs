using Google.Cloud.Firestore;
using cneProyectoVotacion.DTOs;
using cneProyectoVotacion.Models;

namespace cneProyectoVotacion.Services
{
    public class ReportService : IReportService
    {
        private readonly FirebaseServices _firebase;

        public ReportService(FirebaseServices firebase)
        {
            _firebase = firebase;
        }

        public async Task<VoteStatisticsDto> GetVoteStatistics()
        {
            var usersSnapshot = await _firebase
                .GetCollection("users")
                .GetSnapshotAsync();

            var votesSnapshot = await _firebase
                .GetCollection("votes")
                .GetSnapshotAsync();

            var candidatesSnapshot = await _firebase
                .GetCollection("candidates")
                .GetSnapshotAsync();

            int totalUsers = usersSnapshot.Count;
            int totalVotes = votesSnapshot.Count;

            double participation = totalUsers == 0
                ? 0
                : Math.Round((double)totalVotes / totalUsers * 100, 2);

            // ============================
            // RESULTADOS POR CANDIDATO
            // ============================
            var results = candidatesSnapshot.Documents.Select(doc =>
            {
                var candidate = doc.ConvertTo<Candidate>();

                double percentage = totalVotes == 0
                    ? 0
                    : Math.Round((double)candidate.VotesCount / totalVotes * 100, 2);

                return new CandidateVoteResultDto
                {
                    CandidateId = candidate.Id,
                    CandidateName = candidate.Name,
                    VotesCount = candidate.VotesCount,
                    Percentage = percentage
                };
            }).ToList();

            // ============================
            // TENDENCIA TEMPORAL
            // ============================
            var trend = votesSnapshot.Documents
                .Select(doc => doc.ConvertTo<Vote>())
                .GroupBy(v => v.Timestamp.Date)
                .Select(g => new VoteTrendDto
                {
                    Date = g.Key,
                    VotesCount = g.Count()
                })
                .OrderBy(t => t.Date)
                .ToList();

            return new VoteStatisticsDto
            {
                TotalUsers = totalUsers,
                TotalVotes = totalVotes,
                ParticipationPercentage = participation,
                Results = results,
                VoteTrend = trend
            };
        }
    }
}
