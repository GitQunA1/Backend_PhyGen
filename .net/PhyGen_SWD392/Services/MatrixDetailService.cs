using AutoMapper;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class MatrixDetailService
    {
        private readonly IMatrixDetailRepo _repo;
        private readonly PhyGenDbContext _context;
        private readonly IMapper _mapper;
        public MatrixDetailService(IMatrixDetailRepo repo, PhyGenDbContext context, IMapper mapper)
        {
            _repo = repo;
            _context = context;
            _mapper = mapper;
        }

        public async Task<MatrixDetailDTO> CreateMatrixDetailAsync(MatrixDetailRequest request)
        {
            var detail = _mapper.Map<MatrixDetail>(request);

            var createdDetail = await _repo.CreateAsync(detail);

            var detailWithSectionAndTopic = await _repo.GetByIdAsync(createdDetail.Id);

            var detailDto = _mapper.Map<MatrixDetailDTO>(detailWithSectionAndTopic);
            return detailDto;
        }

        public async Task<bool> UpdateMatrixDetailAsync(int id, MatrixDetailRequest request)
        {
            var existingDetail = await _repo.GetByIdAsync(id);
            if (existingDetail == null)
                return false;

            _mapper.Map(request, existingDetail);
            existingDetail.Id = id;

            return await _repo.UpdateAsync(id, existingDetail);
        }

        public async Task<bool> DeleteMatrixDetailAsync(int id)
        {
            return await _repo.Delete(id);
        }

        public async Task<MatrixDetailDTO?> GetMatrixDetailByIdAsync(int id)
        {
            var detail = await _repo.GetByIdAsync(id);

            if (detail == null) return null;

            return _mapper.Map<MatrixDetailDTO>(detail);
        }

        public async Task<PagedResult<MatrixDetailDTO>> GetAllWithQueryAsync(MatrixDetailQueryParameters queryParams)
        {
            var pagedResult = await _repo.GetAllWithQueryAsync(queryParams);
            var mappedItems = _mapper.Map<List<MatrixDetailDTO>>(pagedResult.Items);
            return new PagedResult<MatrixDetailDTO>
            {
                Total = pagedResult.Total,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize,
                Items = mappedItems
            };
        }
    }
}
