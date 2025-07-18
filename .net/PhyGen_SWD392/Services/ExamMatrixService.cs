using AutoMapper;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Enums;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Repositories.Repo;

namespace PhyGen_SWD392.Services
{
    public class ExamMatrixService 
    {
        private readonly IExamMatrixRepo _repo;
        private readonly SubjectService _subjectService;
        private readonly IMapper _mapper;
        private readonly PhyGenDbContext _context;

        public ExamMatrixService(IExamMatrixRepo repo, IMapper mapper, PhyGenDbContext context, SubjectService subjectService)
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
            _subjectService = subjectService;
        }

        public async Task<List<ExamMatrixDTO>> GetAllAsync(ExamMatrixQueryParameters queryParams)
        {
            var pagedResult = await _repo.GetAllWithQueryAsync(queryParams);
            return _mapper.Map<List<ExamMatrixDTO>>(pagedResult.Items);
        }

        public async Task<ExamMatrixDTO> CreateExamMatrixAsync(ExamMatrixRequest examMatrixRequest, int userID)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(examMatrixRequest.SubjectId);
            if (subject == null)
                return null;

            var examMatrix = _mapper.Map<ExamMatrix>(examMatrixRequest);

            if (examMatrixRequest.ExamTypeEnum != null)
            {
                examMatrix.Examtype = examMatrixRequest.ExamTypeEnum.ToString();
            }
            if (examMatrixRequest.StatusEnum != null)
            {
                examMatrix.Status = examMatrixRequest.StatusEnum.ToString();
            }

            examMatrix.CreatedBy = userID;

            var createdExamMatrix = await _repo.CreateAsync(examMatrix);
            var examMatrixDto = _mapper.Map<ExamMatrixDTO>(createdExamMatrix);
            return examMatrixDto;
        }

        public async Task<bool> UpdateExamMatrixAsync(int id, ExamMatrixRequest examMatrixRequest, int userID)
        {
            var existingExamMatrix = await _repo.GetByIdAsync(id);
            if (existingExamMatrix == null || existingExamMatrix.Status == ExamMatrixStatusEnum.inactive.ToString())
                return false;

            _mapper.Map(examMatrixRequest, existingExamMatrix);
            existingExamMatrix.Id = id;

            existingExamMatrix.CreatedBy = userID;

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

        public async Task<bool> SoftDeleteAsync(int id)
        {
            return await _repo.SoftDelete(id);
        }

        public async Task<ExamMatrixDTO?> GetExamMatrixByIdAsync(int id)
        {
            var examMatrix = await _repo.GetByIdAsync(id);

            if (examMatrix == null || examMatrix.Status == ExamMatrixStatusEnum.inactive.ToString()) 
                return null;

            return _mapper.Map<ExamMatrixDTO>(examMatrix);
        }

        public async Task<List<ExamMatrixDTO>> GetAllIncludeDeletedAsync()
        {
            var examMatrixs = await _repo.GetAllAsync();

            return _mapper.Map<List<ExamMatrixDTO>>(examMatrixs);
        }

        public async Task<List<ExamMatrixDTO>> GetActiveExamMatrixAsync()
        {
            var examMatrixs = await _repo.GetActiveExamMatricesAsync();

            return _mapper.Map<List<ExamMatrixDTO>>(examMatrixs);
        }
    }
}
