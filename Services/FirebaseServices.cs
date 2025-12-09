
using Google.Cloud.Firestore;

namespace votacionCneEduardo.Services;

public class FirebaseServices
{
    private readonly FirestoreDb _db;

    public FirebaseServices()
    {
        // Ruta al archivo JSON dentro del proyecto
        string credentialPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Keys",
            "firebaseKey.json"
        );

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

        _db = FirestoreDb.Create("proyectovotacion-7f1b2");
    }
    public CollectionReference Collection(string name)
    {
        return _db.Collection(name);
    }
    public FirestoreDb GetDb()
    {
        return _db;
    }

}
