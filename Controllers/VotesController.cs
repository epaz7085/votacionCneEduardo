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

        
        [HttpPost("{candidateId}")]
        public async Task<IActionResult> Vote(string candidateId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                var vote = await _voteService.CastVote(userId, candidateId);
                return Ok(vote);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("status")]
        public async Task<IActionResult> Status()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var hasVoted = await _voteService.HasUserVoted(userId);
            return Ok(new { hasVoted });
        }
    }
}
