using Microsoft.AspNetCore.Mvc;
using votacionCneEduardo.Services;
using votacionCneEduardo.Models;
using votacionCneEduardo.DTOs;

namespace votacionCneEduardo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
        _auth = auth;
    }

    // ------------------------------
    //      REGISTRO DE USUARIO
    // ------------------------------
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        var user = new User
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Correo,
            Rol = "votante",
            HasVoted = false,
            VoteTimestamp = null
        };

        var token = await _auth.Register(user);

        return Ok(new
        {
            success = true,
            message = "Usuario registrado correctamente",
            token
        });
    }

    // ------------------------------
    //            LOGIN
    // ------------------------------
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _auth.Login(dto.Correo);

        return Ok(new
        {
            success = true,
            message = "Login exitoso",
            token
        });
    }
}
