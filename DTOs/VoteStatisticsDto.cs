namespace cneProyectoVotacion.DTOs
{
    public class VoteStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int TotalVotes { get; set; }
        public double ParticipationPercentage { get; set; }

        public List<CandidateVoteResultDto> Results { get; set; } = new();
        public List<VoteTrendDto> VoteTrend { get; set; } = new();
    }

    public class CandidateVoteResultDto
    {
        public string CandidateId { get; set; } = string.Empty;
        public string CandidateName { get; set; } = string.Empty;
        public int VotesCount { get; set; }
        public double Percentage { get; set; }
    }

    public class VoteTrendDto
    {
        public DateTime Date { get; set; }
        public int VotesCount { get; set; }
    }
}
