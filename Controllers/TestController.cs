using Microsoft.AspNetCore.Mvc;
using cneProyectoVotacion.Services;


namespace cneProyectoVotacion.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly FirebaseServices _firebaseServices;

    public TestController(FirebaseServices firebaseServices)
    {
        _firebaseServices = firebaseServices;
    }

    [HttpGet("firebase")]
    public async Task<IActionResult> TestFirebase()
    {
        try
        {
            var db = _firebaseServices.GetFirestoreDb();

            // Leer la colección "users" (o cualquiera que quieras probar)
            var usersCollection = _firebaseServices.GetCollection("users");
            var snapshot = await usersCollection.Limit(1).GetSnapshotAsync();

            return Ok(new
            {
                success = true,
                message = "Conexión exitosa con Firebase",
                projectId = db.ProjectId,
                documentsFound = snapshot.Count
            });
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                success = false,
                message = "Error al conectar con Firebase",
                error = e.Message,
                stackTrace = e.StackTrace
            });
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "API funcionando correctamente",
            timestamp = DateTime.UtcNow
        });
    }
}