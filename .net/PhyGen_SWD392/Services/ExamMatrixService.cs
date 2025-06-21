using AutoMapper;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Repositories.Repo;

namespace PhyGen_SWD392.Services
{
    public class ExamMatrixService 
    {
        private readonly IExamMatrixRepo _repo;
        private readonly IMapper _mapper;
        private readonly PhyGenDbContext _context;

        public ExamMatrixService(IExamMatrixRepo repo, IMapper mapper, PhyGenDbContext context)
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ExamMatrixDTO> CreateExamMatrixAsync(ExamMatrixRequest examMatrixRequest)
        {
            var examMatrix = _mapper.Map<ExamMatrix>(examMatrixRequest);

            if (examMatrixRequest.ExamTypeEnum != null)
            {
                examMatrix.Examtype = examMatrixRequest.ExamTypeEnum.ToString();
            }
            if (examMatrixRequest.StatusEnum != null)
            {
                examMatrix.Status = examMatrixRequest.StatusEnum.ToString();
            }

            // Hard code CreatedBy = 1 (tạm thời, sau này sẽ lấy từ user đăng nhập)
            examMatrix.CreatedBy = 1;

            var createdExamMatrix = await _repo.CreateAsync(examMatrix);
            var examMatrixDto = _mapper.Map<ExamMatrixDTO>(createdExamMatrix);
            return examMatrixDto;
        }

        public async Task<bool> UpdateExamMatrixAsync(int id, ExamMatrixRequest examMatrixRequest)
        {
            var existingExamMatrix = await _repo.GetByIdAsync(id);
            if (existingExamMatrix == null)
                return false;

            _mapper.Map(examMatrixRequest, existingExamMatrix);
            existingExamMatrix.Id = id;

            existingExamMatrix.CreatedBy = 1; // Hard code CreatedBy = 1 (tạm thời, sau này sẽ lấy từ user đăng nhập)

            if (examMatrixRequest.ExamTypeEnum != null)
            {
                existingExamMatrix.Examtype = examMatrixRequest.ExamTypeEnum.ToString();
            }
            if (examMatrixRequest.StatusEnum != null)
            {
                existingExamMatrix.Status = examMatrixRequest.StatusEnum.ToString();
            }

            return await _repo.UpdateAsync(id, existingExamMatrix);
        }

        public async Task<bool> DeleteExamMatrixAsync(int id)
        {
            return await _repo.Delete(id);
        }

        public async Task<ExamMatrixDTO?> GetExamMatrixByIdAsync(int id)
        {
            var examMatrix = await _repo.GetByIdAsync(id);

            if (examMatrix == null) return null;

            return _mapper.Map<ExamMatrixDTO>(examMatrix);
        }

        public async Task<List<ExamMatrixDTO>> GetAllExamMatrixsAsync()
        {
            var examMatrixs = await _repo.GetAllAsync();

            return _mapper.Map<List<ExamMatrixDTO>>(examMatrixs);
        }
    }
}
