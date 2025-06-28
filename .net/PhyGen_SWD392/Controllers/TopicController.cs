using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [ApiController]
    [Route("api/topics")]
    [Authorize(Roles = "Admin")]

    public class TopicController : ControllerBase
    {
        private readonly TopicService _topicService;
        public TopicController(TopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopicsIncludeDeletedAsync()
        {
            var topics = await _topicService.GetAllIncludeDeletedAsync();
            return Ok(ApiResponse<List<TopicDTO>>.SuccessResponse(topics.ToList(), "Get All successfully"));
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveTopicsAsync()
        {
            var topics = await _topicService.GetActiveTopicsAsync();
            return Ok(ApiResponse<List<TopicDTO>>.SuccessResponse(topics.ToList(), "Get All successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopicByIdAsync(int id)
        {
            var topicDto = await _topicService.GetTopicByIdAsync(id);
            if (topicDto == null)
                return NotFound(ApiResponse<TopicDTO>.ErrorResponse(new List<string> {"Topic not found."}, "Failed to get by id"));

            return Ok(ApiResponse<TopicDTO>.SuccessResponse(topicDto, "Get By Id successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopicAsync([FromBody] TopicRequest topicRequest)
        {
            try
            {
                var createdTopic = await _topicService.CreateTopicAsync(topicRequest);
                return Ok(ApiResponse<TopicDTO>.SuccessResponse(createdTopic, "Topic created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TopicDTO>.ErrorResponse(new List<string> { ex.Message }, "Failed to create topic"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopicAsync(int id, [FromBody] TopicRequest topicRequest)
        {
            var updated = await _topicService.UpdateTopicAsync(id, topicRequest);
            if (!updated)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Topic not found." }, "Update failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Topic updated successfully"));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteTopicAsync(int id)
        {
            var deleted = await _topicService.SoftDeleteTopicAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<bool>.ErrorResponse(
                    new List<string> { "Topic not found." },
                    "Soft delete failed"
                ));
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Topic deleted successfully"));
        }

    }
}
