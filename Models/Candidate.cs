using Google.Cloud.Firestore;

namespace cneProyectoVotacion.Models
{
    [FirestoreData]
    public class Candidate
    {
        [FirestoreProperty]
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Name { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Party { get; set; } = string.Empty;

        [FirestoreProperty]
        public string? PhotoUrl { get; set; }

        [FirestoreProperty]
        public string? LogoUrl { get; set; }

        [FirestoreProperty]
        public List<string> Proposals { get; set; } = new();

        //  UN SOLO CONTADOR
        [FirestoreProperty]
        public int VotesCount { get; set; } = 0;

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public string CreatedBy { get; set; } = string.Empty;

        [FirestoreProperty]
        public bool IsActive { get; set; } = true;
    }
}
