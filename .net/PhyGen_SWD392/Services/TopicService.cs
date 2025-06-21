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

            if(topicRequest.LevelEnum != null)
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

            _mapper.Map(topicRequest, existingTopic);

            existingTopic.Id = id;

            if (topicRequest.LevelEnum != null)
            {
                existingTopic.Level = topicRequest.LevelEnum.ToString();
            }

            return await _topicRepo.UpdateAsync(id, existingTopic);
        }

        public async Task<bool> DeleteTopicAsync(int id)
        {
            return await _topicRepo.Delete(id);
        }
        public async Task<TopicDTO?> GetTopicByIdAsync(int id)
        {
            var topic = await _topicRepo.GetByIdAsync(id);
            var topicDto = _mapper.Map<TopicDTO>(topic);
            return topicDto;
        }

        public async Task<List<TopicDTO>> GetAllTopicsAsync()
        {
            var topics = await _topicRepo.GetAllAsync();
            var topicDto = _mapper.Map<List<TopicDTO>>(topics);
            return topicDto;
        }
    }
}
