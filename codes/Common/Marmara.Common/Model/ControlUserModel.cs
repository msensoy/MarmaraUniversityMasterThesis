using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.Common.Model
{
    public class ControlUserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
