using System;
using System.ComponentModel.DataAnnotations;

namespace PlanAndSchedule.Core.Object
{
    public class Task : BaseEntity
    {
        public virtual string Text 
        { get; set; }

        public virtual DateTime StartDate 
        { get; set; }

        public virtual int Duration 
        { get; set; }

        public virtual decimal Progress 
        { get; set; }

        public virtual int SortOrder 
        { get; set; }

        public virtual int? ParentId 
        { get; set; }
    }
}