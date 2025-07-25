using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Enums;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class EnumRepo : IEnumRepo
    {
        public List<EnumValue> GetSubjectGrades()
        {
            return Enum.GetValues(typeof(SubjectGradeEnum))
                .Cast<SubjectGradeEnum>()
                .Select(e => new EnumValue
                {
                    Id = (int)e,
                    Name = e.ToString()
                })
                .ToList();
        }
    }
}
