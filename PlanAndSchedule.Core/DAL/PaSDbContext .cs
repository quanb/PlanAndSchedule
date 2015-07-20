using PlanAndSchedule.Core.Mapping;
using PlanAndSchedule.Core.Object;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace PlanAndSchedule.Core.DAL
{
    public class PaSDbContext : DbContext, IDbContext
    {
        public PaSDbContext()
            : base("name=DbConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            //.Where(type => !String.IsNullOrEmpty(type.Namespace))
            //.Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
            //type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            //foreach (var type in typesToRegister)
            //{
            //    dynamic configurationInstance = Activator.CreateInstance(type);
            //    modelBuilder.Configurations.Add(configurationInstance);
            //}
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new TaskMap());
            modelBuilder.Configurations.Add(new LinkMap());
            modelBuilder.Configurations.Add(new EventMap());
        }


        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
    }
}
