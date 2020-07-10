using System.ComponentModel.DataAnnotations;

namespace GameClientApi.Domain.Enum
{
    public enum PayloadType
    {
        [Display(Name = "game")]
        Game,

        [Display(Name = "user")]
        User
    }
}
