using DocManager.DataAccess;
using DocManager.Model.Mappers;
using DocManager.Model.Models;
using DocManager.Model.Models.DTOs;
using DocManager.Pagination;
using DocManager.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DocManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public AdminController(UserManager<ApplicationUser> usrMngr, IPasswordHasher<ApplicationUser> passHash, RoleManager<IdentityRole> roleMngr, ApplicationDbContext cont)
        {
            _userManager = usrMngr;
            _passwordHasher = passHash;
            _roleManager = roleMngr;
            _context = cont;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var usersDB = await _userManager.Users.ToListAsync();

            var usersList = usersDB.Select(x => UserMapper.FromAppUserToUser(x)).ToList();

            int pageSize = 3;
            return View(await PaginatedList<User>.CreateAsync(usersList, pageNumber ?? 1, pageSize));

            //return View(usersList);
        }

        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var user = new User
            {
                RoleList = (IEnumerable<IdentityRole>)roles
            };

            return View(user);

        }


        [HttpPost] //crear user
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = UserMapper.FromUserToAppUser(user); 

                IdentityResult result = await _userManager.CreateAsync(appUser, user.User_Password);

                if (result.Succeeded)
                {
                    //Asignamos el Id de appUser a user.
                    user.User_Id = appUser.Id;

                    if (!String.IsNullOrEmpty(user.RoleName)) 
                    {
                        await _userManager.AddToRoleAsync(appUser, user.RoleName);

                        if(user.RoleName == "Medico")
                        {
                            var medico = new Medico
                            {
                                medico_correo = user.User_Email,
                                medico_nombreCompleto = user.User_Nombre,
                            };
                            await _context.AddAsync(medico);

                            if (user.RoleName == "Paciente"){
                                var paciente = new Paciente
                                {
                                    paciente_correoElectronico = user.User_Email,
                                    paciente_nombreCompleto = user.User_Nombre,

                                };
                                await _context.AddAsync(paciente);
                            }

                            _context.SaveChanges();
                        }
                        
                    }


                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        var translatedError = IdentityErrorTranslator.Translate(error);

                        // Verifica si el error ya está en ModelState antes de agregarlo
                        if (!ModelState.Values.Any(v => v.Errors.Any(e => e.ErrorMessage == translatedError.Description)))
                        {
                            ModelState.AddModelError("", translatedError.Description);
                        }
                    }
                }
            }

            user.RoleList = await _roleManager.Roles.ToListAsync();
            return View(user);
        }


        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);  
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(user);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password, string nombre)
        {
            var userDb = await _userManager.FindByIdAsync(id);

            if (userDb != null)
            {
                if (!String.IsNullOrEmpty(email))
                {
                    userDb.Email = email;
                }
                else
                {
                    userDb.Email = userDb.Email;
                }

                if (!String.IsNullOrEmpty(password))
                {
                    userDb.PasswordHash = password;
                }
                else
                {
                    userDb.PasswordHash = userDb.PasswordHash;
                }
                if (!string.IsNullOrEmpty(nombre)) 
                {
                    userDb.Nombre = nombre;
                }
                else
                {
                    userDb.Nombre = userDb.Nombre;
                }

                if(!String.IsNullOrEmpty(email) || !String.IsNullOrEmpty(password) || !String.IsNullOrEmpty(nombre))
                {
                    IdentityResult result = await _userManager.UpdateAsync(userDb);

                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        
                    }
                }

            }
            else
            {
                ModelState.AddModelError("", "El usuario no existe");
            }

            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(result.Errors.ToList());
                }
            }
            else
            {
                ModelState.AddModelError("", "El usuario no existe");
            }
            return View("Index");  
        }

        [HttpGet]
        public async Task<IActionResult> Search(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if(user == null)
            {
                ModelState.AddModelError("", "El usuario no existe");
                return RedirectToAction("Index");
            }
            else
            {
                return View(UserMapper.FromAppUserToUser(user));
            }

            return View("Index");
        }
    }
}
