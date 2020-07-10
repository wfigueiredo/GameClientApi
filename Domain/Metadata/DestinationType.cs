using System;
using System.ComponentModel.DataAnnotations;

namespace GameProducer.Domain.Infrastructure
{
    public enum DestinationType
    {
        [Display(Name = "sqs")]
        Sqs,

        [Display(Name = "sns")]
        Sns,

        [Display(Name = "rabbit")]
        Rabbit
    }
}
