using Google.Cloud.Firestore;

namespace cneProyectoVotacion.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Email { get; set; } = string.Empty;

        [FirestoreProperty]
        public string FullName { get; set; } = string.Empty;

        // admin | votante
        [FirestoreProperty]
        public string Role { get; set; } = "votante";

        [FirestoreProperty]
        public bool hasVoted { get; set; } = false;

        [FirestoreProperty]
        public string? VotedFor { get; set; }

        [FirestoreProperty]
        public string? VotedForName { get; set; }

        [FirestoreProperty]
        public DateTime? VoteTimestamp { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public bool IsActive { get; set; } = true;
    }
}
