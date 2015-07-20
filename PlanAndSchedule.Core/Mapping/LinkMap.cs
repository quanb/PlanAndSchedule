using PlanAndSchedule.Core.Object;
using System.Data.Entity.ModelConfiguration;

namespace PlanAndSchedule.Core.Mapping
{
    public class LinkMap : EntityTypeConfiguration<Link>
    {
        public LinkMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Type).IsRequired().HasMaxLength(1);
            Property(t => t.SourceTaskId).IsRequired();
            Property(t => t.TargetTaskId).IsRequired();
            ToTable("Links");
        }
    }
}
