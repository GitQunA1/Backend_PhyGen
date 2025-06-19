using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.Entities;
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
            return Ok(ApiResponse<List<Topic>>.SuccessResponse(topics.ToList(), "Get All successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopicByIdAsync(int id)
        {
            var topic = await _topicService.GetTopicByIdAsync(id);
            if (topic == null)
                return NotFound(ApiResponse<Topic>.ErrorResponse(new List<string> {"Topic not found."}, "No data found"));

            return Ok(ApiResponse<Topic>.SuccessResponse(topic, "Get By Id successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopicAsync(int id, [FromBody] Topic topic)
        {
            if (id != topic.Id)
                return BadRequest(ApiResponse<Topic>.ErrorResponse(new List<string> {"Not found Topic."}, "No data found"));

            var updated = await _topicService.UpdateTopicAsync(id, topic);

            if (!updated)
                return NotFound(ApiResponse<Topic>.ErrorResponse(new List<string> { "Not found Topic." }, "No data found"));
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTopicAsync(int id)
        {
            var deleted = await _topicService.DeleteTopicAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<Topic>.ErrorResponse(new List<string> { "Not found Topic." }, "No data found"));

            return NoContent();
        }
    }
}
