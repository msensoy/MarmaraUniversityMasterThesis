using Marmara.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.Data.Entity
{
    public class SchTask : BaseEntity
    {
        public string ObjectName { get; set; }
        public string ObjectStatus { get; set; }
        public DateTime ScheduleTime { get; set; }
        public TaskStatusEnum TaskStatus { get; set; }
    }


}
