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

        public IQueryable<Task> GetTasks()
        {
            return _taskRepository.Table;
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
            _taskRepository.Update(task);
        }

        public void DeleteTask(Task task)
        {
            //userProfileRepository.Delete(user.UserProfile);
            _taskRepository.Delete(task);
        }

        public IQueryable<Link> GetLinks()
        {
            return _linkRepository.Table;
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
            _linkRepository.Update(link);
        }

        public void DeleteLink(Link link)
        {
            //userProfileRepository.Delete(user.UserProfile);
            _linkRepository.Delete(link);
        }
    }
}
