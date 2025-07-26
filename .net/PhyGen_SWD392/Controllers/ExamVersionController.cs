using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/exam-versions")]
    [Authorize(Roles ="user, admin, manager")]
    public class ExamVersionController : ControllerBase
    {
        private readonly ExamVersionService _examVersionService;

        public ExamVersionController(ExamVersionService examVersionService)
        {
            _examVersionService = examVersionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExamVersions([FromQuery] ExamVersionQueryParameters parameters)
        {
            try
            {
                var examVersions = await _examVersionService.GetExamVersionsWithFilters(parameters);
                return Ok(ApiResponse<List<ExamVersionDto>>.SuccessResponse(
                    examVersions,
                    "Exam versions retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    new List<string> { ex.Message },
                    "Failed to retrieve exam versions"
                ));
            }
        }
    }
}
