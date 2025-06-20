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
        private readonly PhyGenDbContext _context;
        public MatrixSectionService(IMapper mapper, IMatrixSectionRepo repo, PhyGenDbContext context)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
        }

        public async Task<MatrixSectionDTO> CreateMatrixSectionAsync(MatrixSectionRequest matrixSectionRequest)
        {
            var section = _mapper.Map<MatrixSection>(matrixSectionRequest);

            var createdSection = await _repo.CreateAsync(section);
            var sectionDto = _mapper.Map<MatrixSectionDTO>(createdSection);
            return sectionDto;
        }

        public async Task<bool> UpdateMatrixSectionAsync(int id, MatrixSectionRequest matrixSectionRequest)
        {
            var existingSection = await _repo.GetByIdAsync(id);
            if (existingSection == null)
                return false;

            _mapper.Map(matrixSectionRequest, existingSection);
            existingSection.Id = id;

            return await _repo.UpdateAsync(id, existingSection);
        }

        public async Task<bool> DeleteMatrixSectionAsync(int id)
        {
            return await _repo.Delete(id);
        }

        public async Task<MatrixSectionDTO?> GetMatrixSectionByIdAsync(int id)
        {
            var section = await _repo.GetByIdAsync(id);

            if (section == null) return null;

            return _mapper.Map<MatrixSectionDTO>(section);
        }

        public async Task<List<MatrixSectionDTO>> GetAllMatrixSectionsAsync()
        {
            var sections = await _repo.GetAllAsync();
            return _mapper.Map<List<MatrixSectionDTO>>(sections);
        }
    }
}
