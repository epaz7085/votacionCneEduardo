using Google.Cloud.Firestore;
using cneProyectoVotacion.DTOs;
using cneProyectoVotacion.Models;

namespace cneProyectoVotacion.Services
{
    public class UserService : IUserService
    {
        private readonly FirebaseServices _firebase;

        public UserService(FirebaseServices firebase)
        {
            _firebase = firebase;
        }

        public async Task<List<UserAuditDto>> GetAllUsers()
        {
            var snapshot = await _firebase
                .GetCollection("users")
                .GetSnapshotAsync();

            return snapshot.Documents.Select(doc =>
            {
                var user = doc.ConvertTo<User>();

                return new UserAuditDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    HasVoted = user.HasVoted,
                    VotedForName = user.VotedForName,
                    VoteTimestamp = user.VoteTimestamp
                };
            }).ToList();
        }
    }
}
