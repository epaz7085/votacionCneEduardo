namespace cneProyectoVotacion.DTOs
{
    public class UpdateCandidateDto
    {
        public string? Name { get; set; }
        public string? Party { get; set; }
        public string? PhotoUrl { get; set; }
        public string? LogoUrl { get; set; }
        public List<string>? Proposals { get; set; }
    }
}
