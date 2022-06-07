using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Marmara.Data.Entity
{
    public class MQ2Data : BaseEntity
    {
        public double Co { get; set; }
        public double Lpg { get; set; }
        public double Smoke { get; set; }
    }
}
