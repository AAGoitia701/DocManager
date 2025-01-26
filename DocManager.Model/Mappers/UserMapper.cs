using DocManager.Model.Models;
using DocManager.Model.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Model.Mappers
{
    public static class UserMapper
    {
        public static User FromAppUserToUser(ApplicationUser appUserDb)
        {
            return new User
            {
                User_Nombre = appUserDb.Nombre,
                User_Email = appUserDb.Email,
                User_Password = appUserDb.PasswordHash,
                User_Id = appUserDb.Id,
            };
        }

        public static ApplicationUser FromUserToAppUser(User userDto)
        {
            return new ApplicationUser
            {
                Nombre = userDto.User_Nombre,
                Email = userDto.User_Email,
                UserName = userDto.User_Email, // Si UserName es el email
                
            };
        }
    }
}
