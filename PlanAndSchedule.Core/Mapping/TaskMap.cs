using PlanAndSchedule.Core.Object;
using System.Data.Entity.ModelConfiguration;


namespace PlanAndSchedule.Core.Mapping
{
    public class TaskMap : EntityTypeConfiguration<Task>
    {
        public TaskMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Text).IsRequired();
            Property(t => t.StartDate).IsRequired();
            Property(t => t.Duration).IsRequired();
            Property(t => t.Progress).IsRequired();
            Property(t => t.SortOrder).IsRequired();
            Property(t => t.Type);
            Property(t => t.ParentId);
            ToTable("Tasks");
        }
    }
}
