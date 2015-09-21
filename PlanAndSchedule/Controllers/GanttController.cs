using Newtonsoft.Json;
using PlanAndSchedule.Core.Service;
using PlanAndSchedule.Models;
using PlanAndSchedule.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace PlanAndSchedule.Controllers
{
    public class GanttController : Controller
    {
        private IGanttService _ganttService;

        public GanttController(IGanttService ganttService)
        {
            this._ganttService = ganttService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ContentResult Data()
        {
            var tasks = (from t in _ganttService.GetTasks()
                         select new
                         {
                              id = t.Id,
                              text = t.Text,
                              start_date = t.StartDate,
                              duration = t.Duration,
                              order = t.SortOrder,
                              progress = t.Progress,
                              open = true,
                              parent = t.ParentId,
                              type = (t.Type != null) ? t.Type : String.Empty
                         }).ToArray();
            var links = (from l in _ganttService.GetLinks()
                         select new
                         {
                             id = l.Id,
                             source = l.SourceTaskId,
                             target = l.TargetTaskId,
                             type = l.Type
                         }).ToArray();
            return Content(JsonConvert.SerializeObject(new
            {
                data = tasks,
                links = links
            }, new CustomDateTimeConverter()), "application/json");
        }

        public ContentResult LoadProjectGantt(long id)
        {
            var tasks = (from t in _ganttService.GetProjectTasks(id)
                         select new
                         {
                             id = t.Id,
                             text = t.Text,
                             start_date = t.StartDate,
                             duration = t.Duration,
                             order = t.SortOrder,
                             progress = t.Progress,
                             open = true,
                             parent = t.ParentId,
                             type = (t.Type != null) ? t.Type : String.Empty
                         }).ToArray();
            var links = (from l in _ganttService.GetProjectLinks(id)
                         select new
                         {
                             id = l.Id,
                             source = l.SourceTaskId,
                             target = l.TargetTaskId,
                             type = l.Type
                         }).ToArray();
            return Content(JsonConvert.SerializeObject(new
            {
                data = tasks,
                links = links
            }, new CustomDateTimeConverter()), "application/json");
        }

        /// <summary>
        /// Update Gantt tasks/links: insert/update/delete 
        /// </summary>
        /// <param name="form">Gantt data</param>
        /// <returns>XML response</returns>
        [HttpPost]
        public ContentResult Save(FormCollection form)
        {
            var dataActions = DhtmlxRequest.Parse(form, Request.QueryString["gantt_mode"]);
            try
            {
                foreach (var ganttData in dataActions)
                {
                    switch (ganttData.Mode)
                    {
                        case GanttMode.Tasks:
                            UpdateTasks(ganttData);
                            break;
                        case GanttMode.Links:
                            UpdateLinks(ganttData);
                            break;
                    }
                }
            }
            catch
            {
                // return error to client if something went wrong
                dataActions.ForEach(g => { g.Action = DhtmlxAction.Error; });
            }
            return GanttRespose(dataActions);
        }

        /// <summary>
        /// Update gantt tasks
        /// </summary>
        /// <param name="ganttData">GanttData object</param>
        private void UpdateTasks(DhtmlxRequest ganttData)
        {
            switch (ganttData.Action)
            {
                case DhtmlxAction.Inserted:
                    // add new gantt task entity
                    long? parent = ganttData.UpdatedTask.ParentId;
                    int projectID = -1;
                    while (parent != null)
                    {
                        projectID = (int)parent;
                        parent = _ganttService.GetTask((long)parent).ParentId;
                    }
                    ganttData.UpdatedTask.ProjectId = projectID ;
                    _ganttService.InsertTask(ganttData.UpdatedTask);
                    break;
                case DhtmlxAction.Deleted:
                    // remove gantt task
                    _ganttService.DeleteTaskLinks(ganttData.SourceId);
                    _ganttService.DeleteTask(_ganttService.GetTask(ganttData.SourceId));
                    break;
                case DhtmlxAction.Updated:
                    // update gantt task
                    _ganttService.UpdateTask(ganttData.UpdatedTask);
                    //db.Entry(db.Tasks.Find(ganttData.UpdatedTask.Id)).CurrentValues.SetValues(ganttData.UpdatedTask);
                    break;
                default:
                    ganttData.Action = DhtmlxAction.Error;
                    break;
            }
        }

        /// <summary>
        /// Update gantt links
        /// </summary>
        /// <param name="ganttData">GanttData object</param>
        private void UpdateLinks(DhtmlxRequest ganttData)
        {
            switch (ganttData.Action)
            {
                case DhtmlxAction.Inserted:
                    // add new gantt link entity
                    ganttData.UpdatedLink.ProjectId = _ganttService.GetTask(ganttData.UpdatedLink.SourceTaskId).ProjectId;
                    _ganttService.InsertLink(ganttData.UpdatedLink);
                    break;
                case DhtmlxAction.Deleted:
                    // remove gantt link
                    _ganttService.DeleteLink(_ganttService.GetLink(ganttData.SourceId));
                    break;
                case DhtmlxAction.Updated:
                    // update gantt link
                    _ganttService.UpdateLink(ganttData.UpdatedLink);
                    //db.Entry(db.Tasks.Find(ganttData.UpdatedTask.Id)).CurrentValues.SetValues(ganttData.UpdatedTask);
                    break;
                default:
                    ganttData.Action = DhtmlxAction.Error;
                    break;
            }
        }

        /// <summary>
        /// Create XML response for gantt
        /// </summary>
        /// <param name="ganttData">Gantt data</param>
        /// <returns>XML response</returns>
        private ContentResult GanttRespose(List<DhtmlxRequest> ganttDataCollection)
        {
            var actions = new List<XElement>();
            foreach (var ganttData in ganttDataCollection)
            {
                var action = new XElement("action");
                action.SetAttributeValue("type", ganttData.Action.ToString().ToLower());
                action.SetAttributeValue("sid", ganttData.SourceId);
                action.SetAttributeValue("tid", (ganttData.Action != DhtmlxAction.Inserted) ? ganttData.SourceId :
                    (ganttData.Mode == GanttMode.Tasks) ? ganttData.UpdatedTask.Id : ganttData.UpdatedLink.Id); 
                actions.Add(action);
            }

            var data = new XDocument(new XElement("data", actions));
            data.Declaration = new XDeclaration("1.0", "utf-8", "true");
            return Content(data.ToString(), "text/xml");
        }
    }
}
