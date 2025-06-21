using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/matrix_section")]
    [Authorize(Roles = "Admin")]

    public class MatrixSectionController : ControllerBase
    {
        private readonly MatrixSectionService _service;
        public MatrixSectionController(MatrixSectionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMatrixSectionsAsync()
        {
            var sections = await _service.GetAllMatrixSectionsAsync();

            return Ok(ApiResponse<List<MatrixSectionDTO>>.SuccessResponse(sections, "Get All successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatrixSectionByIdAsync(int id)
        {
            var section = await _service.GetMatrixSectionByIdAsync(id);
            if (section == null)
            {
                return NotFound(ApiResponse<MatrixSectionDTO>.ErrorResponse(new List<string> { "Matrix Section not found." }, "No data found"));
            }

            return Ok(ApiResponse<MatrixSectionDTO>.SuccessResponse(section, "Get By Id successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatrixSectionAsync([FromBody] MatrixSectionRequest request)
        {
            try
            {
                var createdSection = await _service.CreateMatrixSectionAsync(request);

                return Ok(ApiResponse<MatrixSectionDTO>.SuccessResponse(createdSection, "Matrix Section created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<MatrixSectionDTO>.ErrorResponse(new List<string> { ex.Message }, "Failed to create Matrix Section"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatrixSectionAsync(int id, [FromBody] MatrixSectionRequest request)
        {
            var updated = await _service.UpdateMatrixSectionAsync(id, request);
            if (!updated)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Matrix Section not found." }, "Update failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Matrix Section updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatrixSectionAsync(int id)
        {
            var deleted = await _service.DeleteMatrixSectionAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Matrix Section not found." }, "Delete failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Matrix Section deleted successfully"));
        }
    }
}
