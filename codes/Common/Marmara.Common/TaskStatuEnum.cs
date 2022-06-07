using System.ComponentModel.DataAnnotations;

namespace Marmara.Common
{
    public enum TaskStatusEnum
    {
        [Display(Name = "Will Work")]
        WillWork = 1,
        [Display(Name = "Working")]
        IsWorking = 2,
        Worked = 3,
        Removed =4
    }

    public enum ObjectNameEnum
    {
        [Display(Name = "CFL")]
        CFL = 1,
        [Display(Name = "FAN")]
        FAN = 2,
    }

    public enum ObjectStatusEnum
    {
        ON = 1,
        OFF = 2,
    }
}
