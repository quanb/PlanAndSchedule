using PlanAndSchedule.Core.Object;
using System.Data.Entity;

namespace PlanAndSchedule.Core.DAL
{
    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        int SaveChanges();
    }
}
