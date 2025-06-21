using PhyGen_SWD392.Models.Enums;

namespace PhyGen_SWD392.Models.Requests
{
    public class MatrixDetailRequest
    {
        public int? MatrixSectionId { get; set; }

        public int? TopicId { get; set; }
        public MatrixDetailLevelEnum LevelEnum { get; set; }
        public int? Quantity { get; set; }

        public MatrixDetailTypeEnum TypeEnum { get; set; }
    }
}
