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

            //Para mostrar roles en cada user
            var usersList = new List<User>();
            foreach (var user in usersDB)
            {
                var roles = await _userManager.GetRolesAsync(user); // Obtener roles del usuario
                var mappedUser = UserMapper.FromAppUserToUser(user);

                mappedUser.RoleName = string.Join(",", roles); // Agregar roles al objeto User -- Join, por si tiene varios roles

                usersList.Add(mappedUser); //añadir 

            }

            //var usersList = usersDB.Select(x => UserMapper.FromAppUserToUser(x)).ToList();

            int pageSize = 5;
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

                        await AddUserRoleDataAsync(user);

                        
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

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var userDb = await _userManager.FindByIdAsync(id);  
            var roles = await _roleManager.Roles.ToListAsync();
            var roleUser = _userManager.GetRolesAsync(userDb).ToString();
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var userDto = UserMapper.FromAppUserToUser(userDb);
                userDto.RoleList = roles;
                userDto.RoleName = roleUser;
                return View(userDto);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm]User user)
        {
            var userDb = await _userManager.FindByIdAsync(user.User_Id);
            if (userDb == null)
            {
                ModelState.AddModelError("", "El usuario no existe");
                return View();
            }

            // Poblar RoleList para la vista
            user.RoleList = await _roleManager.Roles.ToListAsync();

            // Actualizar los campos del usuario solo si no están vacíos
            userDb.Email = !string.IsNullOrEmpty(user.User_Email) ? user.User_Email : userDb.Email;
            userDb.PasswordHash = !string.IsNullOrEmpty(user.User_Password) ? user.User_Password : userDb.PasswordHash;
            userDb.Nombre = !string.IsNullOrEmpty(user.User_Nombre) ? user.User_Nombre : userDb.Nombre;

            // Actualizar roles si RoleName no es nulo o vacío
            if (!string.IsNullOrEmpty(user.RoleName))
            {
                var currentRoles = await _userManager.GetRolesAsync(userDb);
                await _userManager.RemoveFromRolesAsync(userDb, currentRoles); // Eliminar roles actuales
                await _userManager.AddToRoleAsync(userDb, user.RoleName);      // Asignar nuevo rol

                // Agregar registros adicionales en base al rol
                await AddUserRoleDataAsync(user);
            }

            // Guardar los cambios en el usuario
            var result = await _userManager.UpdateAsync(userDb);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            // Si algo falla, devolver la vista con errores
            ModelState.AddModelError("", "Ocurrió un error al actualizar el usuario");
            return View();
        }

        // Método auxiliar para manejar datos específicos de los roles
        private async Task AddUserRoleDataAsync(User user)
        {
            switch (user.RoleName)
            {
                case "Medico":
                    var medico = new Medico
                    {
                        medico_correo = user.User_Email,
                        medico_nombreCompleto = user.User_Nombre
                    };
                    await _context.AddAsync(medico);
                    break;

                case "Paciente":
                    var paciente = new Paciente
                    {
                        paciente_correoElectronico = user.User_Email,
                        paciente_nombreCompleto = user.User_Nombre
                    };
                    await _context.AddAsync(paciente);
                    break;
            }

            await _context.SaveChangesAsync(); // Guardar cambios en la base de datos
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
