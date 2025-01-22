using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Model.Models.Roles
{
    public class RoleModification //Une el role con el user
    {
        public string RoleNombre { get; set; }
        public string RoleId { get; set; }
        public string[]? AddUserIds { get; set; }
        public string[]? BorrarUserIds { get; set; }
    }
}
