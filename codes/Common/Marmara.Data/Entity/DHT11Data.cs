using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Marmara.Data.Entity
{
    public class DHT11Data : BaseEntity
    {
        public double Huminity { get; set; }
        public double Temperature { get; set; }
    }
}
