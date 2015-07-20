using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanAndSchedule.Core.Object
{
    public class BaseEntity
    {
        public virtual Int64 Id
        { get; set; }

        public virtual string Type 
        { get; set; }
    }
}
