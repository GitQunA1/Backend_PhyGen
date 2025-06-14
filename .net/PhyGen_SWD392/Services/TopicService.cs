using PhyGen_SWD392.Models;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class TopicService
    {
        private readonly PhyGenDbContext _context;
        private readonly ITopicRepo _topicRepo;

        public TopicService(PhyGenDbContext phyGenDbContext, ITopicRepo topicRepo)
        {
            _context = phyGenDbContext;
            _topicRepo = topicRepo;
        }

        public async Task<Topic> CreateTopicAsync(Topic topic)
        {
            return await _topicRepo.CreateAsync(topic);
        }

        public async Task<bool> UpdateTopicAsync(int id, Topic topic)
        {
            return await _topicRepo.UpdateAsync(id, topic);
        }

        public async Task<bool> DeleteTopicAsync(int id)
        {
            return await _topicRepo.Delete(id);
        }
        public async Task<Topic?> GetTopicByIdAsync(int id)
        {
            return await _topicRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
        {
            return await _topicRepo.GetAllAsync();
        }
    }
}
