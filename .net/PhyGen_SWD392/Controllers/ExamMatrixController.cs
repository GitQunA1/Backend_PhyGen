using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/exam_matrices")]

    public class ExamMatrixController : ControllerBase
    {
        private readonly ExamMatrixService _service;
        public ExamMatrixController(ExamMatrixService service)
        {
            _service = service;
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ExamMatrixQueryParameters queryParams)
        {
            var result = await _service.GetAllAsync(queryParams);
            return Ok(ApiResponse<List<ExamMatrixDTO>>.SuccessResponse(result, "Get All successfully"));
        }

        //[Authorize(Roles = "admin,user")]
        //[HttpGet("active")]
        //public async Task<IActionResult> GetActiveExamMatrixAsync()
        //{
        //    var examMatrices = await _service.GetActiveExamMatrixAsync();

        //    return Ok(ApiResponse<List<ExamMatrixDTO>>.SuccessResponse(examMatrices, "Get All successfully"));
        //}
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamMatrixByIdAsync(int id)
        {
            var examMatrix = await _service.GetExamMatrixByIdAsync(id);
            if (examMatrix == null)
            {
                return NotFound(ApiResponse<ExamMatrixDTO>.ErrorResponse(new List<string> { "Exam Matrix not found." }, "Failed to get by id"));
            }

            return Ok(ApiResponse<ExamMatrixDTO>.SuccessResponse(examMatrix, "Get By Id successfully"));
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateExamMatrixAsync([FromBody] ExamMatrixRequest examMatrixRequest)
        {
            try
            {
                var userIdClaim = User.FindFirst("id") ?? User.FindFirst("sub");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse(
                        new List<string> { "User ID not found in token." }, "Unauthorized"));
                }

                var createdExamMatrix = await _service.CreateExamMatrixAsync(examMatrixRequest, userId);

                if (createdExamMatrix == null)
                    return NotFound(ApiResponse<ExamMatrixDTO>.ErrorResponse(new List<string> { "SubjectID not found" }, "Failed to create Exam Matrix"));

                return Ok(ApiResponse<ExamMatrixDTO>.SuccessResponse(createdExamMatrix, "Exam Matrix created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ExamMatrixDTO>.ErrorResponse(new List<string> { ex.Message }, "Failed to create Exam Matrix"));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExamMatrixAsync(int id, [FromBody] ExamMatrixRequest examMatrixRequest)
        {
            var userIdClaim = User.FindFirst("id") ?? User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse(
                    new List<string> { "User ID not found in token." }, "Unauthorized"));
            }

            var updated = await _service.UpdateExamMatrixAsync(id, examMatrixRequest, userId);
            if (!updated)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Exam Matrix not found." }, "Update failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Exam Matrix updated successfully"));
        }
        [Authorize(Roles = "admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var deleted = await _service.SoftDeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Exam Matrix not found." }, "Delete failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Exam Matrix deleted successfully"));
        }
    }
}
