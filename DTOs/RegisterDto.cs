using System.ComponentModel.DataAnnotations;

namespace cneProyectoVotacion.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es requerido")]
        public string FullName { get; set; } = string.Empty;

        // Opcional: si no viene, será "votante"
        public string? Role { get; set; }
    }
}
