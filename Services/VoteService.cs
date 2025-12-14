using Google.Cloud.Firestore;
using cneProyectoVotacion.Models;

namespace cneProyectoVotacion.Services
{
    public class VoteService : IVoteService
    {
        private readonly FirebaseServices _firebaseServices;

        public VoteService(FirebaseServices firebaseServices)
        {
            _firebaseServices = firebaseServices;
        }

        // Verificar si el usuario ya votó
        public async Task<bool> HasUserVoted(string userId)
        {
            var userDoc = await _firebaseServices
                .GetCollection("users")
                .Document(userId)
                .GetSnapshotAsync();

            if (!userDoc.Exists)
                throw new Exception("Usuario no encontrado");

            var user = userDoc.ConvertTo<User>();
            return user.HasVoted;
        }

        // Emitir voto con TRANSACCIÓN
        public async Task<Vote> CastVote(string userId, string candidateId)
        {
            var db = _firebaseServices.GetFirestoreDb();

            return await db.RunTransactionAsync(async transaction =>
            {
                var userRef = _firebaseServices.GetCollection("users").Document(userId);
                var candidateRef = _firebaseServices.GetCollection("candidates").Document(candidateId);
                var voteRef = _firebaseServices.GetCollection("votes").Document();

                var userSnap = await transaction.GetSnapshotAsync(userRef);
                var candidateSnap = await transaction.GetSnapshotAsync(candidateRef);

                if (!userSnap.Exists)
                    throw new Exception("Usuario no encontrado");

                if (!candidateSnap.Exists)
                    throw new Exception("Candidato no encontrado");

                var user = userSnap.ConvertTo<User>();
                var candidate = candidateSnap.ConvertTo<Candidate>();

                //  Validación voto único
                if (user.HasVoted)
                    throw new Exception("El usuario ya votó");

                // Crear voto (auditoría)
                var vote = new Vote
                {
                    Id = voteRef.Id,
                    UserId = user.Id,
                    UserName = user.FullName,
                    CandidateId = candidate.Id,
                    CandidateName = candidate.Name,
                    Timestamp = DateTime.UtcNow
                };

                // Guardar voto
                transaction.Set(voteRef, vote);

                // Actualizar usuario
                transaction.Update(userRef, new Dictionary<string, object>
                {
                    { "hasVoted", true },
                    { "votedFor", candidate.Id },
                    { "votedForName", candidate.Name },
                    { "voteTimestamp", Timestamp.FromDateTime(DateTime.UtcNow) }
                });

                // Incrementar contador del candidato
                transaction.Update(candidateRef, new Dictionary<string, object>
                {
                    { "VotesCount", candidate.VotesCount + 1 }
                });

                return vote;
            });
        }
    }
}
