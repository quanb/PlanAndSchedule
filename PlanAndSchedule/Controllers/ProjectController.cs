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

namespace PlanAndSchedule.Controllers
{
    public class ProjectController : Controller
    {
         private IGanttService _ganttService;

         public ProjectController(IGanttService ganttService)
        {
            this._ganttService = ganttService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ContentResult Projects(JqInViewModel jqParams)
        {
            var projects = _ganttService.GetProjects(jqParams.page - 1, jqParams.rows,
                                                    jqParams.sidx, jqParams.sord == "asc");
            int totalProjects = _ganttService.TotalProjects();
            return Content(JsonConvert.SerializeObject(new
            {
                page = jqParams.page,
                records = totalProjects,
                rows = projects,
                total = Math.Ceiling(Convert.ToDouble(totalProjects) / jqParams.rows)
            }, new CustomDateTimeConverter()), "application/json");
        }

        [HttpPost]
        public ContentResult AddProject(Task task)
        {
            string json;
            _ganttService.InsertTask(task);

             json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = true,
                    message = "Post added successfully."
                });
            
            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult DeleteProject(int id)
        {
            _ganttService.DeleteProjectLinks(id);
            _ganttService.DeleteProjectTasks(id);
            _ganttService.DeleteTask(_ganttService.GetTask(id));

            var json = JsonConvert.SerializeObject(new
            {
                success = true,
                message = "Post deleted successfully."
            });

            return Content(json, "application/json");
        }
    }
}
