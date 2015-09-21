using Ioc.Data;
using PlanAndSchedule.Core.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PlanAndSchedule.Core.Service
{
    public class GanttService : IGanttService
    {
        private IRepository<Task> _taskRepository;
        private IRepository<Link> _linkRepository;

        public GanttService(IRepository<Task> taskRepository, IRepository<Link> linkRepository)
        {
            this._taskRepository = taskRepository;
            this._linkRepository = linkRepository;
        }

        public IQueryable<Task> GetProjects(int pageNo, int pageSize, string sortColumn,
                                            bool sortByAscending)
        {
            IQueryable<Task> projects;
            if (sortByAscending)
            {
                projects = (from t in _taskRepository.Table
                               where t.ParentId == null
                               orderby t.Text ascending 
                               select t
                           ).Skip(pageNo * pageSize)
                            .Take(pageSize);
            }
            else
            {
                projects = (from t in _taskRepository.Table
                            where t.ParentId == null
                            orderby t.Text descending
                            select t
                           ).Skip(pageNo * pageSize)
                            .Take(pageSize);
            }
            return projects;
        }

        public int TotalProjects()
        {
            int totalProjects = (from t in _taskRepository.Table
                                 where t.ParentId == null
                                 select t
                                ).Count();
            return totalProjects;
        }

        public IQueryable<Task> GetTasks()
        {
            return _taskRepository.Table;
        }

        public IQueryable<Task> GetProjectTasks(long project_id)
        {
            return (from t in _taskRepository.Table
                    where t.ProjectId == project_id || t.Id == project_id
                    select t
                    );
        }

        public Task GetTask(long id)
        {
            return _taskRepository.GetById(id);
        }

        public void InsertTask(Task task)
        {
            _taskRepository.Insert(task);
        }

        public void UpdateTask(Task task)
        {
            Task t = _taskRepository.GetById(task.Id);
            t.Text = task.Text;
            t.Type = task.Type;
            t.StartDate = task.StartDate;
            t.SortOrder = task.SortOrder;
            t.Progress = task.Progress;
            t.ParentId = task.ParentId;
            t.Duration = task.Duration;
            _taskRepository.Update(t);
        }

        public void DeleteTask(Task task)
        {
            _taskRepository.Delete(task);
        }

        public void DeleteProjectTasks(long projectID)
        {
            List<Task> tasks = (from t in _taskRepository.Table
                                where t.ProjectId == projectID
                                select t
                               ).ToList();
            tasks.ForEach(t => _taskRepository.Delete(t));
        }

        public IQueryable<Link> GetLinks()
        {
            return _linkRepository.Table;
        }

        public IQueryable<Link> GetProjectLinks(long project_id)
        {
            return (from t in _linkRepository.Table
                    where t.ProjectId == project_id
                    select t
                    );
        }

        public Link GetLink(long id)
        {
            return _linkRepository.GetById(id);
        }

        public void InsertLink(Link link)
        {
            _linkRepository.Insert(link);
        }

        public void UpdateLink(Link link)
        {
            Link l = _linkRepository.GetById(link.Id);
            l.Type = link.Type;
            l.SourceTaskId = link.SourceTaskId;
            l.TargetTaskId = link.TargetTaskId;
            _linkRepository.Update(l);
        }

        public void DeleteLink(Link link)
        {
            _linkRepository.Delete(link);
        }

        public void DeleteTaskLinks(long sourceID)
        {
            List<Link> links = (from t in _linkRepository.Table
                                where t.SourceTaskId == sourceID
                                select t
                               ).ToList();
            links.ForEach(l => _linkRepository.Delete(l));
        }

        public void DeleteProjectLinks(long projectID)
        {
            List<Link> links = (from t in _linkRepository.Table
                                where t.ProjectId == projectID
                                select t
                               ).ToList();
            links.ForEach(l => _linkRepository.Delete(l));
        }
    }
}
