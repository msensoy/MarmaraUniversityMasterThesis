using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.Data.Entity
{
    public interface IActuator
    {
        string Status { get; set; }
        DateTime RunTimeStart { get; set; }
        DateTime RunTimeEnd { get; set; }
    }
}
