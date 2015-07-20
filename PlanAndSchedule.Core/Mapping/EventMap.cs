using PlanAndSchedule.Core.Object;
using System.Data.Entity.ModelConfiguration;

namespace PlanAndSchedule.Core.Mapping
{
    public class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Text).IsRequired().HasMaxLength(255);
            Property(t => t.StartDate).IsRequired();
            Property(t => t.EndDate).IsRequired();
            ToTable("Events");
        }

    }
}
