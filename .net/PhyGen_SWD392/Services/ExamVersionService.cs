using AutoMapper;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class ExamVersionService
    {
        private readonly IExamVersionRepo _examVersionRepo;
        private readonly IMapper _mapper;

        public ExamVersionService(IExamVersionRepo examVersionRepo, IMapper mapper)
        {
            _examVersionRepo = examVersionRepo;
            _mapper = mapper;
        }

        public async Task<List<ExamVersionDto>> GetExamVersionsWithFilters(ExamVersionQueryParameters parameters)
        {
            var examVersions = await _examVersionRepo.GetAllWithQueryAsync(parameters);
            return _mapper.Map<List<ExamVersionDto>>(examVersions);
        }
    }
}
