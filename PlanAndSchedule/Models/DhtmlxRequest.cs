using PlanAndSchedule.Core.Object;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace PlanAndSchedule.Models
{
    public class DhtmlxRequest
    {
        public GanttMode Mode { get; set; }
        public DhtmlxAction Action { get; set; }

        public Task UpdatedTask { get; set; }
        public Link UpdatedLink { get; set; }
        public Event UpdatedEvent { get; set; }
        public long SourceId { get; set; }

        public static DhtmlxRequest ParseEventRequest(FormCollection form)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var request = new DhtmlxRequest();
            request.SourceId = Int64.Parse(form["id"]);
            request.Action = (DhtmlxAction)Enum.Parse(typeof(DhtmlxAction), form["!nativeeditor_status"], true);

            if (request.Action != DhtmlxAction.Deleted)
            {
                request.UpdatedEvent = new Event()
                {
                    Id = (request.Action == DhtmlxAction.Updated) ? (int)request.SourceId : 0,
                    Text = form["text"],
                    StartDate = DateTime.Parse(form["start_date"]),
                    EndDate = DateTime.Parse(form["end_date"]),
                    Type = (form["type"] != null) ? form["type"] : "",
                };
            }

            Thread.CurrentThread.CurrentCulture = currentCulture;
            return request;
        }
        public static List<DhtmlxRequest> Parse(FormCollection form, string mode)
        {
            // save current culture and change it to InvariantCulture for data parsing
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var dataActions = new List<DhtmlxRequest>();
            var prefixes = form["ids"].Split(',');

            foreach (var prefix in prefixes)
            {
                var request = new DhtmlxRequest();

                // lambda expression for form data parsing
                Func<string, string> parse = x => form[String.Format("{0}_{1}", prefix, x)];

                request.Mode = (GanttMode)Enum.Parse(typeof(GanttMode), mode, true);
                request.Action = (DhtmlxAction)Enum.Parse(typeof(DhtmlxAction), parse("!nativeeditor_status"), true);
                request.SourceId = Int64.Parse(parse("id"));

                // parse gantt task
                if (request.Action != DhtmlxAction.Deleted && request.Mode == GanttMode.Tasks)
                {
                    request.UpdatedTask = new Task()
                    {
                        Id = (request.Action == DhtmlxAction.Updated) ? (int)request.SourceId : 0,
                        Text = parse("text"),
                        StartDate = DateTime.Parse(parse("start_date")),
                        Duration = Int32.Parse(parse("duration")),
                        Progress = (parse("progress") != null)? Decimal.Parse(parse("progress")) : 0,
                        ParentId = (parse("parent") != "0") ? Int32.Parse(parse("parent")) : (int?)null,
                        SortOrder = (parse("order") != null) ? Int32.Parse(parse("order")) : 0,
                        Type = parse("type")
                    };
                }
                // parse gantt link
                else if (request.Action != DhtmlxAction.Deleted && request.Mode == GanttMode.Links)
                {
                    request.UpdatedLink = new Link()
                    {
                        Id = (request.Action == DhtmlxAction.Updated) ? (int)request.SourceId : 0,
                        SourceTaskId = Int32.Parse(parse("source")),
                        TargetTaskId = Int32.Parse(parse("target")),
                        Type = parse("type")
                    };
                }

                dataActions.Add(request);
            }

            // return current culture back
            Thread.CurrentThread.CurrentCulture = currentCulture;

            return dataActions;
        }
    }

    /// <summary>
    /// Gantt modes
    /// </summary>
    public enum GanttMode
    {
        Tasks,
        Links,
        Events
    }

    /// <summary>
    /// Gantt actions
    /// </summary>
    public enum DhtmlxAction
    {
        Inserted,
        Updated,
        Deleted,
        Error
    }
}