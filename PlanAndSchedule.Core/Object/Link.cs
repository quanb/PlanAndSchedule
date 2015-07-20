using System.ComponentModel.DataAnnotations;

namespace PlanAndSchedule.Core.Object
{
    public class Link : BaseEntity
    {     
        public virtual int SourceTaskId 
        { get; set; }

        public virtual int TargetTaskId 
        { get; set; }
    }
}