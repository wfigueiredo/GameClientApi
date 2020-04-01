using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GameProducer.Domain.Enum
{
    public enum ConsoleType
    {
        [EnumMember]
        [Display(Name = null)]
        UNKNOWN,

        [EnumMember]
        [Display(Name = "ps4")]
        PS4,

        [EnumMember]
        [Display(Name = "xone")]
        XOne,

        [EnumMember]
        [Display(Name = "switch")]
        Switch
    }
}
