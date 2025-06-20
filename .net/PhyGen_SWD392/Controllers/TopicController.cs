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
    [Route("api/topic")]
    public class TopicController : ControllerBase
    {
        private readonly TopicService _topicService;
        public TopicController(TopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopicsAsync()
        {
            var topics = await _topicService.GetAllTopicsAsync();
            return Ok(ApiResponse<List<TopicDTO>>.SuccessResponse(topics.ToList(), "Get All successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopicByIdAsync(int id)
        {
            var topicDto = await _topicService.GetTopicByIdAsync(id);
            if (topicDto == null)
                return NotFound(ApiResponse<TopicDTO>.ErrorResponse(new List<string> {"Topic not found."}, "No data found"));

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

            var updatedTopic = await _topicService.GetTopicByIdAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Topic updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopicAsync(int id)
        {
            var deleted = await _topicService.DeleteTopicAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<TopicDTO>.ErrorResponse(
                    new List<string> { "Topic not found." },
                    "Delete failed"
                ));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Topic deleted successfully"));
        }
    }
}
