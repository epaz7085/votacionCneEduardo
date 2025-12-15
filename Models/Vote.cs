using Google.Cloud.Firestore;

namespace cneProyectoVotacion.Models
{
    [FirestoreData]
    public class Vote
    {
        [FirestoreProperty]
        public string Id { get; set; } = string.Empty;

        // Usuario
        [FirestoreProperty]
        public string UserId { get; set; } = string.Empty;

        [FirestoreProperty]
        public string UserName { get; set; } = string.Empty;

        // Candidato
        [FirestoreProperty]
        public string CandidateId { get; set; } = string.Empty;

        [FirestoreProperty]
        public string CandidateName { get; set; } = string.Empty;

        // Auditoría
        [FirestoreProperty]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
