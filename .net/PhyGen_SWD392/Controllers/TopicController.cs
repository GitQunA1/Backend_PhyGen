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
    public class TopicController : ControllerBase
    {
        private readonly TopicService _topicService;
        public TopicController(TopicService topicService)
        {
            _topicService = topicService;
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TopicQueryParameters queryParams)
        {
            var topics = await _topicService.GetAllWithQueryAsync(queryParams);
            return Ok(ApiResponse<PagedResult<TopicDTO>>.SuccessResponse(topics, "Get All successfully"));
        }

        //[Authorize(Roles = "admin,user")]
        //[HttpGet("active")]
        //public async Task<IActionResult> GetActiveTopicsAsync()
        //{
        //    var topics = await _topicService.GetActiveTopicsAsync();
        //    return Ok(ApiResponse<List<TopicDTO>>.SuccessResponse(topics.ToList(), "Get All successfully"));
        //}

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopicByIdAsync(int id)
        {
            var topicDto = await _topicService.GetTopicByIdAsync(id);
            if (topicDto == null)
                return NotFound(ApiResponse<TopicDTO>.ErrorResponse(new List<string> {"Topic not found."}, "Failed to get by id"));

            return Ok(ApiResponse<TopicDTO>.SuccessResponse(topicDto, "Get By Id successfully"));
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopicAsync(int id, [FromBody] TopicRequest topicRequest)
        {
            var updated = await _topicService.UpdateTopicAsync(id, topicRequest);
            if (!updated)
                return NotFound(ApiResponse<bool>.ErrorResponse(new List<string> { "Topic not found." }, "Update failed"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Topic updated successfully"));
        }

        [Authorize(Roles = "admin")]
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
