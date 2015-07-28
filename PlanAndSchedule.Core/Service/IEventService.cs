using PlanAndSchedule.Core.Object;
using System.Linq;


namespace PlanAndSchedule.Core.Service
{
    public interface IEventService
    {
        IQueryable<Event> GetEvents();
        Event GetEvent(long id);
        void InsertEvent(Event ev);
        void UpdateEvent(Event ev);
        void DeleteEvent(Event ev);
    }
}
