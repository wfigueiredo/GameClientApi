using System;
using System.ComponentModel.DataAnnotations;

namespace GameProducer.Domain.Infrastructure
{
    public enum DestinationType
    {
        [Display(Name = "queue")]
        Queue, 
        [Display(Name = "topic")]
        Topic
    }
}
