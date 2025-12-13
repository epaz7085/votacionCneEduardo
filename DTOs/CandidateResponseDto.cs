namespace cneProyectoVotacion.DTOs
{
    public class CandidateResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? LogoUrl { get; set; }
        public List<string> Proposals { get; set; } = new();
        public int VotesCount { get; set; }
    }
}
