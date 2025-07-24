using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/AI")]
    public class AIGenerationController : ControllerBase
    {
        private readonly AIGenerationService _aIGenerationService;
        private readonly IMapper _mapper;
        public AIGenerationController(AIGenerationService aIGenerationService, IMapper mapper)
        {
            _aIGenerationService = aIGenerationService;
            _mapper = mapper;
        }

        [HttpPost("{matrixId}")]
        public async Task<IActionResult> GenerateExam(int matrixId)
        {
            var userIdClaim = User.FindFirst("id") ?? User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse(
                    new List<string> { "User ID not found in token." }, "Unauthorized"));
            }

            try
            {
                var examResponse = await _aIGenerationService.GenerateExamFromMatrix(matrixId, userId);
                return Ok(examResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(new List<string> { ex.Message },"Failed to generate exam"
                ));
            }
        }
    }
}
