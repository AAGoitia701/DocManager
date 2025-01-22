using DocManager.Model.Models;
using DocManager.Model.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DocManager.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;  
        public RoleController(RoleManager<IdentityRole> roleMngr, UserManager<ApplicationUser> userMngr)
        {
            roleManager = roleMngr;
            userManager = userMngr;
        }

        public ViewResult Index() => View(roleManager.Roles); //Devolvemos todos los roles

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("Index", roleManager.Roles);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            var users = await userManager.Users.ToListAsync();
            List<ApplicationUser> members = new List<ApplicationUser>();
            List<ApplicationUser> nonMembers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    members.Add(user);
                }
                else
                {
                    nonMembers.Add(user);
                }
            }

            return View(
                new RoleEdit
                {
                    Members = members,
                    NotMembers = nonMembers,
                    Role = role
                }
                );
        }

        [HttpPost] //Asigna y Eimina roles de users
        public async Task<IActionResult> Update(RoleModification roleMod)
        {
            IdentityResult result;
            if (ModelState.IsValid) 
            {
                foreach (string userId in roleMod.AddUserIds ?? new string[] { })
                {
                    ApplicationUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, roleMod.RoleNombre);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }

                foreach (string userId in roleMod.BorrarUserIds ?? new string[] { })
                {
                    ApplicationUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, roleMod.RoleNombre);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }
            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Update(roleMod.RoleId);
        }
    }
}
