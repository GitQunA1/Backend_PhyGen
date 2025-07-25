using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/enums")]
    [Authorize(Roles ="admin, manager")]
    public class EnumController : ControllerBase
    {
        private readonly EnumService _enumService;

        public EnumController(EnumService enumService)
        {
            _enumService = enumService;
        }

        [HttpGet("subject-grades")]
        public IActionResult GetSubjectGrades()
        {
            var grades = _enumService.GetSubjectGrades();
            return Ok(ApiResponse<List<EnumValue>>.SuccessResponse(
                grades,
                "Subject grades retrieved successfully"
            ));
        }
    }
}
