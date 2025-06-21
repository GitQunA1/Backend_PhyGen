using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/exam_matrix")]
    public class ExamMatrixController : ControllerBase
    {
        private readonly ExamMatrixService _service;
        public ExamMatrixController(ExamMatrixService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExamMatricesAsync()
        {
            var examMatrices = await _service.GetAllExamMatrixsAsync();

            return Ok(ApiResponse<List<ExamMatrixDTO>>.SuccessResponse(examMatrices, "Get All successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamMatrixByIdAsync(int id)
        {
            var examMatrix = await _service.GetExamMatrixByIdAsync(id);
            if (examMatrix == null)
            {
                return NotFound(ApiResponse<ExamMatrixDTO>.ErrorResponse(new List<string> { "Exam Matrix not found." }, "No data found"));
            }

            return Ok(ApiResponse<ExamMatrixDTO>.SuccessResponse(examMatrix, "Get By Id successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateExamMatrixAsync([FromBody] ExamMatrixRequest examMatrixRequest)
        {
            try
            {
                var createdExamMatrix = await _service.CreateExamMatrixAsync(examMatrixRequest);

                return Ok(ApiResponse<ExamMatrixDTO>.SuccessResponse(createdExamMatrix, "Exam Matrix created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ExamMatrixDTO>.ErrorResponse(new List<string> { ex.Message }, "Failed to create Exam Matrix"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExamMatrixAsync(int id, [FromBody] ExamMatrixRequest examMatrixRequest)
        {
            var updated = await _service.UpdateExamMatrixAsync(id, examMatrixRequest);
            if (!updated)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Exam Matrix not found." }, "Update failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Exam Matrix updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamMatrixAsync(int id)
        {
            var deleted = await _service.DeleteExamMatrixAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Exam Matrix not found." }, "Delete failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Exam Matrix deleted successfully"));
        }
    }
}
