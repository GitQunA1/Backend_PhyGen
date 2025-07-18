using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/matrix_sections")]
    public class MatrixSectionController : ControllerBase
    {
        private readonly MatrixSectionService _service;
        public MatrixSectionController(MatrixSectionService service)
        {
            _service = service;
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] MatrixSectionQueryParameters queryParams)
        {
            var result = await _service.GetAllAsync(queryParams);
            return Ok(ApiResponse<List<MatrixSectionDTO>>.SuccessResponse(result, "Get All successfully"));
        }

        //[Authorize(Roles = "admin,user")]
        //[HttpGet("active")]
        //public async Task<IActionResult> GetActiveMatrixSectionsAsync()
        //{
        //    var sections = await _service.GetActiveMatrixSectionsAsync();

        //    return Ok(ApiResponse<List<MatrixSectionDTO>>.SuccessResponse(sections, "Get All successfully"));
        //}

        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMatrixSectionAsync([FromBody] MatrixSectionRequest request)
        {
            try
            {
                var createdSection = await _service.CreateMatrixSectionAsync(request);

                if (createdSection == null)
                {
                    return NotFound(ApiResponse<MatrixSectionDTO>.ErrorResponse(new List<string> { "Invalid Matrix ID." }, "Creation failed"));
                }

                return Ok(ApiResponse<MatrixSectionDTO>.SuccessResponse(createdSection, "Matrix Section created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<MatrixSectionDTO>.ErrorResponse(new List<string> { ex.Message }, "Failed to create Matrix Section"));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatrixSectionAsync(int id, [FromBody] MatrixSectionRequest request)
        {
            var updated = await _service.UpdateMatrixSectionAsync(id, request);
            if (!updated)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Matrix Section not found." }, "Update failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Matrix Section updated successfully"));
        }
        [Authorize(Roles = "admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var deleted = await _service.SoftDeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Matrix Section not found." }, "Delete failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Matrix Section deleted successfully"));
        }
    }
}
