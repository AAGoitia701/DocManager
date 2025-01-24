using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Model.Models.DTOs
{
    public class User  //DTO
    {
        [Display(Name="ID")]
        public string?  User_Id { get; set; }
        [Display(Name="Nombre")]
        [Required(ErrorMessage = "Nombre es un campo requerido")]
        public string User_Nombre { get; set; }
        [Display(Name="Email")]
        [Required(ErrorMessage = "Email es un campo requerido") ]
        public string User_Email { get; set; }
        [Display(Name="Contraseña")]
        [Required(ErrorMessage = "La contraseña es un campo requerido")]
        public string User_Password { get; set; }

        public string RoleName { get; set; }
        [ValidateNever]
        public IEnumerable<IdentityRole> RoleList {  get; set; }
    }
}
