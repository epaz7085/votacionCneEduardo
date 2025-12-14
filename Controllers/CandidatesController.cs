using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using cneProyectoVotacion.DTOs;
using cneProyectoVotacion.Services;
using System.Security.Claims;

namespace cneProyectoVotacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] //  requiere JWT
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidatesController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCandidateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _candidateService.CreateCandidate(dto, userId!);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _candidateService.GetAllCandidates());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var candidate = await _candidateService.GetCandidateById(id);
            if (candidate == null) return NotFound();
            return Ok(candidate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateCandidateDto dto)
        {
            var result = await _candidateService.UpdateCandidate(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _candidateService.DeleteCandidate(id);
            if (!deleted) return BadRequest("No se puede eliminar, tiene votos");
            return Ok();
        }
    }
}
