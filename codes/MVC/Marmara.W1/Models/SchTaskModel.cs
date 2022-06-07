using Marmara.Common;
using Marmara.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marmara.W1.Models
{
    public class SchTaskModel
    {
        public int Id { get; set; }
        public string ObjectName { get; set; }
        public string ObjectStatus { get; set; }
        public DateTime ScheduleTime { get; set; }
        public TaskStatusEnum TaskStatus { get; set; }
    }
}
