using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Models;
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
        public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
        {
            return await _topicService.GetAllTopicsAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopicByIdAsync(int id)
        {
            var topic = await _topicService.GetTopicByIdAsync(id);
            if (topic == null)
                return NotFound();

            return Ok(topic);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopicAsync(int id, [FromBody] Topic topic)
        {
            if (id != topic.Id)
                return BadRequest("Topic ID mismatch.");

            var updated = await _topicService.UpdateTopicAsync(id, topic);

            if (!updated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTopicAsync(int id)
        {
            var deleted = await _topicService.DeleteTopicAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
