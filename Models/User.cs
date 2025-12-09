using Google.Cloud.Firestore;

namespace votacionCneEduardo.Models;

[FirestoreData]
public class User
{
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Nombre { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Apellido { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Email { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Rol { get; set; } = "votante"; // admin o votante

    [FirestoreProperty]
    public bool HasVoted { get; set; } = false;

    [FirestoreProperty]
    public string? VotedFor { get; set; }

    [FirestoreProperty]
    public string? VotedForName { get; set; }

    [FirestoreProperty]
    public DateTime? VoteTimestamp { get; set; }
}
