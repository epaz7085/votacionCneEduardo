using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace cneProyectoVotacion.Services;

public class FirebaseServices
{
    private readonly FirestoreDb _firestoreDb;
    private readonly string _projectId;

    public FirebaseServices(IConfiguration configuration)
    {
        _projectId = configuration["Firebase:ProjectId"]
            ?? throw new Exception("Firebase:ProjectId no está configurado");

        var credentialsPath =
            Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

        if (string.IsNullOrEmpty(credentialsPath))
            throw new Exception("GOOGLE_APPLICATION_CREDENTIALS no está configurado");

        if (!File.Exists(credentialsPath))
            throw new Exception($"Archivo de credenciales no existe: {credentialsPath}");

        if (FirebaseApp.DefaultInstance == null)
        {
            var credential = GoogleCredential.FromFile(credentialsPath);

            FirebaseApp.Create(new AppOptions
            {
                Credential = credential,
                ProjectId = _projectId
            });
        }

        _firestoreDb = FirestoreDb.Create(_projectId);
    }

    public FirestoreDb GetFirestoreDb() => _firestoreDb;

    public CollectionReference GetCollection(string collectionName)
    {
        return _firestoreDb.Collection(collectionName);
    }
}
