using Google.Cloud.Firestore;

namespace votacionCneEduardo.Models;

[FirestoreData]
public class Candidate
{
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Nombre { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Partido { get; set; } = string.Empty;

    [FirestoreProperty]
    public string FotoUrl { get; set; } = string.Empty;

    [FirestoreProperty]
    public string LogoUrl { get; set; } = string.Empty;

    [FirestoreProperty]
    public int TotalVotos { get; set; } = 0;
}
