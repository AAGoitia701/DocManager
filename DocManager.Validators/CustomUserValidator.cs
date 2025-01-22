using DocManager.Model.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DocManager.Validators
{
    public class CustomUserValidator<TUser> : UserValidator<TUser> where TUser : ApplicationUser
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var result = await base.ValidateAsync(manager, user);
            var errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            if (!IsValidUserName(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "InvalidUserName",
                    Description = "El nombre de usuario solo puede contener letras, números, puntos (.), guiones bajos (_) y arrobas (@)."
                });
            }

            return errors.Any() ? IdentityResult.Failed(errors.ToArray()): IdentityResult.Success;

        }
        private bool IsValidUserName(string userName) 
        {
            var regex = new Regex(@"^[a-zA-Z0-9._@]+$");
            return regex.IsMatch(userName);
        }

    }
}
