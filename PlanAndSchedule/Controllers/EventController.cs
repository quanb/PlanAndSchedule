using Newtonsoft.Json;
using PlanAndSchedule.Core.Object;
using PlanAndSchedule.Core.Service;
using PlanAndSchedule.Models;
using PlanAndSchedule.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace PlanAndSchedule.Controllers
{
    public class EventController : Controller
    {
        private IEventService _eventService;

        public EventController(IEventService eventService)
        {
            this._eventService = eventService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ContentResult Data()
        {
            var events = (from t in _eventService.GetEvents()
                         select new
                         {
                             id = t.Id,
                             text = t.Text,
                             start_date = t.StartDate,
                             end_date = t.EndDate,
                             type = (t.Type != null) ? t.Type : String.Empty
                         }).ToArray();
            return Content(JsonConvert.SerializeObject(events
            , new CustomDateTimeConverter()), "application/json");
        }

        public ContentResult Save(FormCollection form)
        {
            var dataAction = DhtmlxRequest.ParseEventRequest(form);
            try
            {
                switch (dataAction.Action)
                {
                    case DhtmlxAction.Inserted:
                        // add new gantt task entity
                        _eventService.InsertEvent(dataAction.UpdatedEvent);
                        break;
                    case DhtmlxAction.Deleted:
                        // remove gantt tasks
                        _eventService.DeleteEvent(_eventService.GetEvent(dataAction.SourceId));
                        break;
                    case DhtmlxAction.Updated:
                        // update gantt task
                        _eventService.UpdateEvent(dataAction.UpdatedEvent);
                        //db.Entry(db.Tasks.Find(ganttData.UpdatedTask.Id)).CurrentValues.SetValues(ganttData.UpdatedTask);
                        break;
                    default:
                        dataAction.Action = DhtmlxAction.Error;
                        break;
                }
            }
            catch
            {
                // return error to client if something went wrong
                dataAction.Action = DhtmlxAction.Error; ;
            }

            return EventRespose(dataAction);
        }

        private ContentResult EventRespose(DhtmlxRequest eventData)
        {
            var actions = new List<XElement>();
            var action = new XElement("action");
            action.SetAttributeValue("type", eventData.Action.ToString().ToLower());
            action.SetAttributeValue("sid", eventData.SourceId);
            action.SetAttributeValue("tid", (eventData.Action != DhtmlxAction.Inserted) ? eventData.SourceId : eventData.UpdatedEvent.Id);
            actions.Add(action);

            var data = new XDocument(new XElement("data", actions));
            data.Declaration = new XDeclaration("1.0", "utf-8", "true");
            return Content(data.ToString(), "text/xml");
        }
    }
}
