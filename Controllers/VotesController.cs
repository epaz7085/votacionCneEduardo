using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using cneProyectoVotacion.Services;

namespace cneProyectoVotacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VotesController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        /// Emitir voto (un solo voto por usuario)
        [HttpPost("{candidateId}")]
        public async Task<IActionResult> Vote(string candidateId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuario no autenticado");

            try
            {
                var vote = await _voteService.CastVote(userId, candidateId);

                return Ok(new
                {
                    message = "Voto registrado exitosamente",
                    voteId = vote.Id,
                    candidate = vote.CandidateName,
                    timestamp = vote.Timestamp
                });
            }
            catch (Exception ex)
            {
                //  Todas las validaciones vienen del service
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }
    }
}
