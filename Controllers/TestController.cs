    using Microsoft.AspNetCore.Mvc;
    using votacionCneEduardo.Services;

    namespace votacionCneEduardo.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly FirebaseServices _firebase;

        public TestController(FirebaseServices firebase)
        {
            _firebase = firebase;
        }

        [HttpGet("firebase")]
        public async Task<IActionResult> TestFirebase()
        {
            try
            {
                var db = _firebase.GetDb();

                // Test: leer colección "users"
                var usersCollection = _firebase.Collection("users");
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
                    trace = e.StackTrace
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
