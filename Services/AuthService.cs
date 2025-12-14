using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Google.Cloud.Firestore;
using Microsoft.IdentityModel.Tokens;
using cneProyectoVotacion.DTOs;
using cneProyectoVotacion.Models;

namespace cneProyectoVotacion.Services
{
    public class AuthService : IAuthService
    {
        private readonly FirebaseServices _firebaseService;
        private readonly IConfiguration _configuration;

        public AuthService(FirebaseServices firebaseService, IConfiguration configuration)
        {
            _firebaseService = firebaseService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            // Verificar si el usuario ya existe
            var existingUser = await GetUserByEmail(registerDto.Email);
            if (existingUser != null)
                throw new Exception("El usuario ya existe");

            var userId = Guid.NewGuid().ToString();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                Id = userId,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Role = string.IsNullOrWhiteSpace(registerDto.Role) ? "votante": registerDto.Role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                HasVoted = false
            };

            var userData = new Dictionary<string, object>
            {
                { "Id", user.Id },
                { "Email", user.Email },
                { "FullName", user.FullName },
                { "Role", user.Role },
                { "HasVoted", user.HasVoted },
                { "CreatedAt", user.CreatedAt },
                { "IsActive", user.IsActive },
                { "PasswordHash", passwordHash }
            };

            await _firebaseService
                .GetCollection("users")
                .Document(user.Id)
                .SetAsync(userData);

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        //  LOGIN
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var usersCollection = _firebaseService.GetCollection("users");
            var query = usersCollection.WhereEqualTo("Email", loginDto.Email).Limit(1);
            var snapshot = await query.GetSnapshotAsync();

            if (snapshot.Count == 0)
                throw new Exception("Credenciales inválidas");

            var userDoc = snapshot.Documents[0];
            var data = userDoc.ToDictionary();

            if (!data.ContainsKey("PasswordHash"))
                throw new Exception("Usuario sin contraseña");

            var passwordHash = data["PasswordHash"].ToString();

            if (string.IsNullOrEmpty(passwordHash) ||
                !BCrypt.Net.BCrypt.Verify(loginDto.Password, passwordHash))
                throw new Exception("Credenciales inválidas");

            var user = new User
            {
                Id = data["Id"].ToString()!,
                Email = data["Email"].ToString()!,
                FullName = data["FullName"].ToString()!,
                Role = data["Role"].ToString()!,
                HasVoted = data.ContainsKey("HasVoted") && Convert.ToBoolean(data["HasVoted"]),
                IsActive = data.ContainsKey("IsActive") && Convert.ToBoolean(data["IsActive"]),
                CreatedAt = data.ContainsKey("CreatedAt")
                    ? ((Timestamp)data["CreatedAt"]).ToDateTime()
                    : DateTime.UtcNow
            };

            if (!user.IsActive)
                throw new Exception("Usuario inactivo");

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        //  OBTENER USUARIO POR ID
        public async Task<User?> GetUserById(string userId)
        {
            var snapshot = await _firebaseService
                .GetCollection("users")
                .Document(userId)
                .GetSnapshotAsync();

            if (!snapshot.Exists)
                return null;

            var data = snapshot.ToDictionary();

            return new User
            {
                Id = data["Id"].ToString()!,
                Email = data["Email"].ToString()!,
                FullName = data["FullName"].ToString()!,
                Role = data["Role"].ToString()!,
                HasVoted = data.ContainsKey("HasVoted") && Convert.ToBoolean(data["HasVoted"]),
                IsActive = data.ContainsKey("IsActive") && Convert.ToBoolean(data["IsActive"]),
                CreatedAt = data.ContainsKey("CreatedAt")
                    ? ((Timestamp)data["CreatedAt"]).ToDateTime()
                    : DateTime.UtcNow
            };
        }

        // OBTENER USUARIO POR EMAIL
        public async Task<User?> GetUserByEmail(string email)
        {
            var usersCollection = _firebaseService.GetCollection("users");
            var query = usersCollection.WhereEqualTo("Email", email).Limit(1);
            var snapshot = await query.GetSnapshotAsync();

            if (snapshot.Count == 0)
                return null;

            var data = snapshot.Documents[0].ToDictionary();

            return new User
            {
                Id = data["Id"].ToString()!,
                Email = data["Email"].ToString()!,
                FullName = data["FullName"].ToString()!,
                Role = data["Role"].ToString()!
            };
        }

        //  GENERAR JWT
        public string GenerateJwtToken(User user)
        {
            var key = _configuration["Jwt:Key"]
                      ?? throw new Exception("JWT Key no configurada");

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expires = int.Parse(_configuration["Jwt:ExpireInMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expires),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
