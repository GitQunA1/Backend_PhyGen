using PhyGen_SWD392.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PhyGen_SWD392.Models.Requests
{
    public class TopicRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(TopicLevelEnum))]
        public TopicLevelEnum LevelEnum { get; set; }

        public int? SubjectId { get; set; } 

        public int? ParentId { get; set; } 
    }
}
