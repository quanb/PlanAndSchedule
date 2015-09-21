using PlanAndSchedule.Core.Object;
using System.Linq;


namespace PlanAndSchedule.Core.Service
{
    public interface IGanttService
    {
        IQueryable<Task> GetProjects(int pageNo, int pageSize, string sortColumn,
                                            bool sortByAscending);
        int TotalProjects();
        IQueryable<Task> GetTasks();
        IQueryable<Task> GetProjectTasks(long project_id);
        Task GetTask(long id);
        void InsertTask(Task task);
        void UpdateTask(Task task);
        void DeleteTask(Task task);
        void DeleteProjectTasks(long projectID);

        IQueryable<Link> GetLinks();
        IQueryable<Link> GetProjectLinks(long project_id);
        Link GetLink(long id);
        void InsertLink(Link link);
        void UpdateLink(Link link);
        void DeleteLink(Link link);
        void DeleteTaskLinks(long sourceID);
        void DeleteProjectLinks(long projectID);

    }
}
