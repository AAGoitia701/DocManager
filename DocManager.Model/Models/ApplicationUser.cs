using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Model.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Nombre { get; set; }


    }
}
