using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController : ControllerBase
    {
        private readonly SubjectService _service;
        public SubjectController(SubjectService service)
        {
           _service = service; 
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SubjectQueryParameters queryParams)
        {
            var subjects = await _service.GetAllWithQueryAsync(queryParams);
            return Ok(ApiResponse<PagedResult<SubjectDTO>>.SuccessResponse(subjects, "Get All successfully"));
        }

        //[Authorize(Roles = "admin,user")]
        //[HttpGet("active")]
        //public async Task<IActionResult> GetActiveSubjectsAsync()
        //{
        //    var subjects = await _service.GetActiveSubjectsAsync();
        //    return Ok(ApiResponse<List<SubjectDTO>>.SuccessResponse(subjects.ToList(), "Get All successfully"));
        //}

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectByIdAsync(int id)
        {
            var subjectDTO = await _service.GetSubjectByIdAsync(id);
            if (subjectDTO == null)
            {
                return NotFound(ApiResponse<SubjectDTO>.ErrorResponse(new List<string> { "Subject not found." }, "Failed to get subject by id"));
            }

            return Ok(ApiResponse<SubjectDTO>.SuccessResponse(subjectDTO, "Get By Id successfully"));
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubjectAsync(int id, [FromBody] SubjectRequest subjectRequest)
        {
            var updated = await _service.UpdateSubjectAsync(id, subjectRequest);
            if (!updated)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Subject not found." }, "Update failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Subject updated successfully"));
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var deleted = await _service.SoftDeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<SubjectDTO>.ErrorResponse(new List<string> { "Subject not found." }, "Delete failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Subject deleted successfully"));
        }

    }
}
