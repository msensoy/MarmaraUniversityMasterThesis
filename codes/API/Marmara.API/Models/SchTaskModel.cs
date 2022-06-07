using Marmara.Common;
using Marmara.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marmara.API.Models
{
    public class SchTaskModel
    {
        public ObjectNameEnum ObjectName { get; set; }
        public ObjectStatusEnum ObjectStatus { get; set; }
        public DateTime ScheduleTime { get; set; }
    }
}
