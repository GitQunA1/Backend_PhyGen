using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/subject")]
    public class SubjectController : ControllerBase
    {
        private readonly SubjectService _service;
        public SubjectController(SubjectService service)
        {
           _service = service; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubjectAsync()
        {
            var subjectDTOs = await _service.GetAllSubjectsAsync();
            return Ok(ApiResponse<List<SubjectDTO>>.SuccessResponse(subjectDTOs, "Get All successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectByIdAsync(int id)
        {
            var subjectDTO = await _service.GetSubjectByIdAsync(id);
            if (subjectDTO == null)
            {
                return NotFound(ApiResponse<SubjectDTO>.ErrorResponse(new List<string> { "Subject not found." }, "No data found"));
            }

            return Ok(ApiResponse<SubjectDTO>.SuccessResponse(subjectDTO, "Get By Id successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubjectAsync([FromBody] SubjectRequest subjectRequest)
        {
            try
            {
                var createdSubject = await _service.CreateSubjectAsync(subjectRequest);
                return Ok(ApiResponse<SubjectDTO>.SuccessResponse(createdSubject, "Subject created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<SubjectDTO>.ErrorResponse(new List<string> { ex.Message }, "Failed to create subject"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubjectAsync(int id, [FromBody] SubjectRequest subjectRequest)
        {
            var updated = await _service.UpdateSubjectAsync(id, subjectRequest);
            if (!updated)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Subject not found." }, "Update failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Subject updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubjectAsync(int id)
        {
            var deleted = await _service.DeleteSubjectAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<SubjectDTO>.ErrorResponse(new List<string> { "Subject not found." }, "Delete failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Subject deleted successfully"));
        }
    }
}
