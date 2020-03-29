using System.ComponentModel.DataAnnotations;

namespace GameProducer.Domain.Enum
{
    public enum PayloadType
    {
        [Display(Name = "game")]
        Game,

        [Display(Name = "user")]
        User
    }
}
