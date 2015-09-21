using PlanAndSchedule.Core.DAL;
using PlanAndSchedule.Core.Object;
using System;
using System.Collections.Generic;
using System.Data.Entity;


namespace PlanAndSchedule.Core
{
    class PvSInitializer : DropCreateDatabaseIfModelChanges<PaSDbContext>
    {
        protected override void Seed(PaSDbContext context)
        {
            List<Task> tasks = new List<Task>()
            {
                new Task() { Id = 1, Text = "Project #1", StartDate = DateTime.Now.AddHours(-3), 
                    Duration = 18, SortOrder = 10, Progress = 0.4m, ParentId = null },
                new Task() { Id = 2, Text = "Task #1", StartDate = DateTime.Now.AddHours(-2), 
                    Duration = 8, SortOrder = 10, Progress = 0.6m, ParentId = 1, ProjectId = 1 },
                new Task() { Id = 3, Text = "Task #2", StartDate = DateTime.Now.AddHours(-1), 
                    Duration = 8, SortOrder = 20, Progress = 0.6m, ParentId = 1, ProjectId = 1 }
            };

            tasks.ForEach(s => context.Set<Task>().Add(s));
            context.SaveChanges();

            List<Link> links = new List<Link>()
            {
                new Link() { Id = 1, SourceTaskId = 1, TargetTaskId = 2, Type = "1", ProjectId = 1 },
                new Link() { Id = 2, SourceTaskId = 2, TargetTaskId = 3, Type = "0", ProjectId = 1 }
            };

            links.ForEach(s => context.Set<Link>().Add(s));
            context.SaveChanges();
        }
    }
}
