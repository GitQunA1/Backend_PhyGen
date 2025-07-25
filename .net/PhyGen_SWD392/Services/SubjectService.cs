using AutoMapper;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Models.Entities;

namespace PhyGen_SWD392.Services
{
    public class SubjectService
    {
        private readonly PhyGenDbContext _context;
        private readonly ISubjectRepo _subjectRepo;
        private readonly IMapper _mapper;
        public SubjectService(PhyGenDbContext phyGenDbContext, ISubjectRepo subjectRepo, IMapper mapper)
        {
            _context = phyGenDbContext;
            _subjectRepo = subjectRepo;
            _mapper = mapper;
        }

        public async Task<SubjectDTO> CreateSubjectAsync(SubjectRequest subjectRequest)
        {
            var subject = _mapper.Map<Subject>(subjectRequest);

            if (subjectRequest.GradeEnum != null)
            {
                subject.Grade = subjectRequest.GradeEnum.ToString();
            }

            var createdSubject = await _subjectRepo.CreateAsync(subject);
            var subjectDto = _mapper.Map<SubjectDTO>(createdSubject);
            return subjectDto;
        }

        public async Task<bool> UpdateSubjectAsync(int id, SubjectRequest subjectRequest)
        {
            var existingSubject = await _subjectRepo.GetByIdAsync(id);
            if (existingSubject == null || existingSubject.IsDeleted == true)
                return false;

            _mapper.Map(subjectRequest, existingSubject);
            existingSubject.Id = id;

            if (subjectRequest.GradeEnum != null)
            {
                existingSubject.Grade = subjectRequest.GradeEnum.ToString();
            }

            return await _subjectRepo.UpdateAsync(id, existingSubject);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            return await _subjectRepo.SoftDelete(id);
        }

        public async Task<SubjectDTO?> GetSubjectByIdAsync(int id)
        {
            var subject = await _subjectRepo.GetByIdAsync(id);

            if (subject == null || subject.IsDeleted == true) return null;

            return _mapper.Map<SubjectDTO>(subject);
        }

        public async Task<PagedResult<SubjectDTO>> GetAllWithQueryAsync(SubjectQueryParameters queryParams)
        {
            var pagedResult = await _subjectRepo.GetAllWithQueryAsync(queryParams);
            var mappedItems = _mapper.Map<List<SubjectDTO>>(pagedResult.Items);
            return new PagedResult<SubjectDTO>
            {
                Total = pagedResult.Total,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize,
                Items = mappedItems
            };
        }

        public async Task<List<SubjectDTO>> GetActiveSubjectsAsync()
        {
            var subjects = await _subjectRepo.GetActiveSubjectsAsync();
            return _mapper.Map<List<SubjectDTO>>(subjects);
        }
    }
}
