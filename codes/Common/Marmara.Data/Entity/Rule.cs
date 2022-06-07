using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.Data.Entity
{
    public class Rule : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsGreaterThen { get; set; }
        public int Value { get; set; }
    }
}
