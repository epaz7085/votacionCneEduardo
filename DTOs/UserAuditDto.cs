namespace cneProyectoVotacion.DTOs
{
    public class UserAuditDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool HasVoted { get; set; }
        public string? VotedForName { get; set; }
        public DateTime? VoteTimestamp { get; set; }
    }
}
