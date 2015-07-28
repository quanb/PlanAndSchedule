using Ioc.Data;
using PlanAndSchedule.Core.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanAndSchedule.Core.Service
{
    public class EventService : IEventService
    {
        private IRepository<Event> _eventRepository;

        public EventService(IRepository<Event> eventRepository)
        {
            this._eventRepository = eventRepository;
        }

        public IQueryable<Event> GetEvents()
        {
            return _eventRepository.Table;
        }

        public Event GetEvent(long id)
        {
            return _eventRepository.GetById(id);
        }

        public void InsertEvent(Event ev)
        {
            _eventRepository.Insert(ev);
        }

        public void UpdateEvent(Event ev)
        {
            Event e = _eventRepository.GetById(ev.Id);
            e.Text = ev.Text;
            e.StartDate = ev.StartDate;
            e.Type = ev.Type;
            e.EndDate = ev.EndDate;
            _eventRepository.Update(e);
        }

        public void DeleteEvent(Event ev)
        {
            //userProfileRepository.Delete(user.UserProfile);
            _eventRepository.Delete(ev);
        }
    }
}
