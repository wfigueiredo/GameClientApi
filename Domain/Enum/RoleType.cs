using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Domain.Enum
{
    public enum RoleType
    {
        [Display(Name = "root")]
        Root,

        [Display(Name = "admin")]
        Admin,

        [Display(Name = "user")]
        User
    }
}
