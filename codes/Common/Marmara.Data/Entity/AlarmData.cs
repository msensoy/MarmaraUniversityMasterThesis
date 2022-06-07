using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.Data.Entity
{
    public class AlarmData : BaseEntity, IActuator
    {
        public string Status { get; set; }
        public DateTime RunTimeStart { get; set; }
        public DateTime RunTimeEnd { get; set; }
    }
}
