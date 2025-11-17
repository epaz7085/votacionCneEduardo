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
                     ?? throw new InvalidOperationException("Firebase:ProjectId no está configurado");

        if (FirebaseApp.DefaultInstance == null)
        {
            var credential = GoogleCredential.GetApplicationDefault();

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