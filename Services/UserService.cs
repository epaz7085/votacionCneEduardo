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
                var data = doc.ToDictionary();

                DateTime? voteDate = null;
                if (data.ContainsKey("voteTimestamp") && data["voteTimestamp"] is Timestamp ts)
                {
                    voteDate = ts.ToDateTime();
                }

                return new UserAuditDto
                {
                    Id = data.ContainsKey("Id") ? data["Id"]?.ToString() ?? "" : "",
                    Email = data.ContainsKey("Email") ? data["Email"]?.ToString() ?? "" : "",
                    FullName = data.ContainsKey("FullName") ? data["FullName"]?.ToString() ?? "" : "",
                    HasVoted = data.ContainsKey("hasVoted") && Convert.ToBoolean(data["hasVoted"]),
                    VotedForName = data.ContainsKey("votedForName")
                        ? data["votedForName"]?.ToString()
                        : null,
                    VoteTimestamp = voteDate
                };
            }).ToList();
        }

    }
}
