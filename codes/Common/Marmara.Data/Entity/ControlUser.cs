using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.Data.Entity
{
    public class ControlUser : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
