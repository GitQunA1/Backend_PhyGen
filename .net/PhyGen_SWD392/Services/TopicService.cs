using AutoMapper;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class TopicService
    {
        private readonly PhyGenDbContext _context;
        private readonly ITopicRepo _topicRepo;
        private readonly IMapper _mapper;

        public TopicService(PhyGenDbContext phyGenDbContext, ITopicRepo topicRepo, IMapper mapper)
        {
            _context = phyGenDbContext;
            _topicRepo = topicRepo;
            _mapper = mapper;
        }

        public async Task<TopicDTO> CreateTopicAsync(TopicRequest topicRequest)
        {
            var topic = _mapper.Map<Topic>(topicRequest);

            if (topicRequest.LevelEnum != null)
            {
                topic.Level = topicRequest.LevelEnum.ToString();
            }

            var createdTopic = await _topicRepo.CreateAsync(topic);
            var topicDto = _mapper.Map<TopicDTO>(createdTopic);

            return topicDto;
        }

        public async Task<bool> UpdateTopicAsync(int id, TopicRequest topicRequest)
        {
            var existingTopic = await _topicRepo.GetByIdAsync(id);
            if (existingTopic == null)
                return false;

            if (existingTopic.IsDeleted == true)
                return false;

            _mapper.Map(topicRequest, existingTopic);

            existingTopic.Id = id;

            if (topicRequest.LevelEnum != null)
            {
                existingTopic.Level = topicRequest.LevelEnum.ToString();
            }

            return await _topicRepo.UpdateAsync(id, existingTopic);
        }

        public async Task<TopicDTO?> GetTopicByIdAsync(int id)
        {
            var topic = await _topicRepo.GetByIdAsync(id);
            if (topic == null || topic.IsDeleted == true)
                return null;

            var topicDto = _mapper.Map<TopicDTO>(topic);
            return topicDto;
        }

        public async Task<List<TopicDTO>> GetAllIncludeDeletedAsync()
        {
            var topics = await _topicRepo.GetAllAsync();
            var topicDto = _mapper.Map<List<TopicDTO>>(topics);
            return topicDto;
        }

        public async Task<List<TopicDTO>> GetActiveTopicsAsync()
        {
            var topics = await _topicRepo.GetActiveTopicsAsync();
            var topicDto = _mapper.Map<List<TopicDTO>>(topics);
            return topicDto;
        }

        public async Task<bool> SoftDeleteTopicAsync(int id)
        {
            return await _topicRepo.SoftDelete(id);
        }
    }
}
