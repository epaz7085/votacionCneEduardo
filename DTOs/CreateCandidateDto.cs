namespace cneProyectoVotacion.DTOs
{
    public class CreateCandidateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? LogoUrl { get; set; }
        public List<string> Proposals { get; set; } = new();
    }
}
