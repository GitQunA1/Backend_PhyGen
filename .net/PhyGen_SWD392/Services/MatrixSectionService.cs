using AutoMapper;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Repositories.Repo;

namespace PhyGen_SWD392.Services
{
    public class MatrixSectionService
    {
        private readonly IMapper _mapper;
        private readonly IMatrixSectionRepo _repo;
        private readonly ExamMatrixService _examMatrixService;
        private readonly PhyGenDbContext _context;
        public MatrixSectionService(IMapper mapper, IMatrixSectionRepo repo, PhyGenDbContext context, ExamMatrixService examMatrixService)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
            _examMatrixService = examMatrixService;
        }

        public async Task<PagedResult<MatrixSectionDTO>> GetAllAsync(MatrixSectionQueryParameters queryParams)
        {
            var pagedResult = await _repo.GetAllWithQueryAsync(queryParams);
            var mappedItems = _mapper.Map<List<MatrixSectionDTO>>(pagedResult.Items);
            return new PagedResult<MatrixSectionDTO>
            {
                Total = pagedResult.Total,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize,
                Items = mappedItems
            };
        }

        public async Task<MatrixSectionDTO> CreateMatrixSectionAsync(MatrixSectionRequest matrixSectionRequest)
        {
            var matrix = await _examMatrixService.GetExamMatrixByIdAsync(matrixSectionRequest.MatrixId);

            if (matrix == null)
                return null;

            var section = _mapper.Map<MatrixSection>(matrixSectionRequest);

            var createdSection = await _repo.CreateAsync(section);
            var sectionDto = _mapper.Map<MatrixSectionDTO>(createdSection);
            return sectionDto;
        }

        public async Task<bool> UpdateMatrixSectionAsync(int id, MatrixSectionRequest matrixSectionRequest)
        {
            var existingSection = await _repo.GetByIdAsync(id);
            if (existingSection == null || existingSection.IsDeleted == true)
                return false;

            _mapper.Map(matrixSectionRequest, existingSection);
            existingSection.Id = id;

            return await _repo.UpdateAsync(id, existingSection);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            return await _repo.SoftDelete(id);
        }

        public async Task<MatrixSectionDTO?> GetMatrixSectionByIdAsync(int id)
        {
            var section = await _repo.GetByIdAsync(id);

            if (section == null || section.IsDeleted == true)
                return null;

            return _mapper.Map<MatrixSectionDTO>(section);
        }

        public async Task<List<MatrixSectionDTO>> GetAllIncludeDeletedAsync()
        {
            var sections = await _repo.GetAllAsync();
            return _mapper.Map<List<MatrixSectionDTO>>(sections);
        }

        public async Task<List<MatrixSectionDTO>> GetActiveMatrixSectionsAsync()
        {
            var sections = await _repo.GetActiveMatrixSectionsAsync();
            return _mapper.Map<List<MatrixSectionDTO>>(sections);
        }
    }
}
