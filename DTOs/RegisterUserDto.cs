namespace votacionCneEduardo.DTOs;

public class RegisterUserDto
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Correo { get; set; }
    public string Password { get; set; }
    public string Rol { get; set; } = "votante";
}
