using PlanAndSchedule.Core.Object;
using System.Linq;


namespace PlanAndSchedule.Core.Service
{
    public interface IGanttService
    {
        IQueryable<Task> GetTasks();
        Task GetTask(long id);
        void InsertTask(Task task);
        void UpdateTask(Task task);
        void DeleteTask(Task task);

        IQueryable<Link> GetLinks();
        Link GetLink(long id);
        void InsertLink(Link link);
        void UpdateLink(Link link);
        void DeleteLink(Link link);

    }
}
