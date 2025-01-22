using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Model.Models.Roles
{
    public class RoleEdit //Obtenemos la lista de los users que tienen roles, y los que no tienen, junto con los roles. 
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NotMembers { get; set; }
    }
}
