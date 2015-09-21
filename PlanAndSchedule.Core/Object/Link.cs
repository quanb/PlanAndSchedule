using System.ComponentModel.DataAnnotations;

namespace PlanAndSchedule.Core.Object
{
    public class Link : BaseEntity
    {     
        public virtual long SourceTaskId 
        { get; set; }

        public virtual long TargetTaskId 
        { get; set; }

        public virtual long ProjectId
        { get; set; }
    }
}