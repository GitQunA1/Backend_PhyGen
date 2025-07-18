using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/matrix_details")]

    public class MatrixDetailController : ControllerBase
    {
        private readonly MatrixDetailService _service;
        public MatrixDetailController(MatrixDetailService service)
        {
            _service = service;
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] MatrixDetailQueryParameters queryParams)
        {
            var details = await _service.GetAllWithQueryAsync(queryParams);
            return Ok(ApiResponse<List<MatrixDetailDTO>>.SuccessResponse(details, "Get All successfully"));
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatrixDetailByIdAsync(int id)
        {
            var detail = await _service.GetMatrixDetailByIdAsync(id);
            if (detail == null)
            {
                return NotFound(ApiResponse<MatrixDetailDTO>.ErrorResponse(new List<string> { "Matrix Detail not found." }, "No data found"));
            }

            return Ok(ApiResponse<MatrixDetailDTO>.SuccessResponse(detail, "Get By Id successfully"));
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMatrixDetailAsync([FromBody] MatrixDetailRequest request)
        {
            try
            {
                var createdDetail = await _service.CreateMatrixDetailAsync(request);

                return Ok(ApiResponse<MatrixDetailDTO>.SuccessResponse(createdDetail, "Matrix Detail created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<MatrixDetailDTO>.ErrorResponse(new List<string> { ex.Message }, "Failed to create Matrix Detail"));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatrixDetailAsync(int id, [FromBody] MatrixDetailRequest request)
        {
            var updated = await _service.UpdateMatrixDetailAsync(id, request);
            if (!updated)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Matrix Detail not found." }, "Update failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Matrix Detail updated successfully"));
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatrixDetailAsync(int id)
        {
            var deleted = await _service.DeleteMatrixDetailAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Matrix Detail not found." }, "Delete failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Matrix Detail deleted successfully"));
        }
    }
}
