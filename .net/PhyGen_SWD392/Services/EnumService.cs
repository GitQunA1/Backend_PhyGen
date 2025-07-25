using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class EnumService
    {
        private readonly IEnumRepo _enumRepo;
        public EnumService(IEnumRepo enumRepo)
        {
            _enumRepo = enumRepo;
        }

        public List<EnumValue> GetSubjectGrades()
        {
            return _enumRepo.GetSubjectGrades();
        }
    }
}
