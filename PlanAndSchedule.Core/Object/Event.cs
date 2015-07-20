using System;
using System.ComponentModel.DataAnnotations;

namespace PlanAndSchedule.Core.Object
{
    public class Event : BaseEntity
    {
        [MaxLength(255)]
        public virtual string Text
        { get; set; }

        public virtual DateTime StartDate
        { get; set; }

        public virtual DateTime EndDate
        { get; set; }
    }
}