using System.ComponentModel;

namespace PhyGen_SWD392.Models.Enums
{
    public enum AccountStatusEnum
    {
        [Description("Active")]
        Active = 1,

        [Description("Inactive")]
        Inactive = 2,

        [Description("Banned")]
        Banned = 3
    }
}
