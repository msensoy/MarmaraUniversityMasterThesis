using Marmara.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marmara.W1.Models
{
    public class RuleTaskModel
    {
        public bool checkTemp { get; set; }
        public bool radioTempGreaterThen { get; set; }
        public int valueTemp { get; set; }

        public bool checkLux { get; set; }
        public bool radioLuxGreaterThen { get; set; }
        public int valueLux { get; set; }
    }
}
