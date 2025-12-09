using Google.Cloud.Firestore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using votacionCneEduardo.Models;

namespace votacionCneEduardo.Services;

public class AuthService
{
    private readonly FirebaseServices _firebase;
    private readonly IConfiguration _config;

    public AuthService(FirebaseServices firebase, IConfiguration config)
    {
        _firebase = firebase;
        _config = config;
    }


    public async Task<string> Register(User user)
    {
        var users = _firebase.Collection("users");

        var existing = await users.WhereEqualTo("email", user.Email).GetSnapshotAsync();
        if (existing.Count > 0)
            throw new Exception("El correo ya está registrado.");

        user.Id = Guid.NewGuid().ToString();
        user.HasVoted = false;
        user.Rol = "votante";
        user.VoteTimestamp = null;

        await users.Document(user.Id).SetAsync(user);

        return GenerateJwt(user);
    }

  
    public async Task<string> Login(string email)
    {
        var users = _firebase.Collection("users");

        var snapshot = await users.WhereEqualTo("email", email).Limit(1).GetSnapshotAsync();

        if (snapshot.Count == 0)    
            throw new Exception("Usuario no encontrado.");

        var user = snapshot.Documents[0].ConvertTo<User>();
        user.Id = snapshot.Documents[0].Id;

        return GenerateJwt(user);
    }

    private string GenerateJwt(User user)
    {
        var key = _config["Jwt:Key"];
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var expireMinutes = int.Parse(_config["Jwt:ExpireInMinutes"]);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("id", user.Id),
            new Claim("email", user.Email),
            new Claim("role", user.Rol)
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
