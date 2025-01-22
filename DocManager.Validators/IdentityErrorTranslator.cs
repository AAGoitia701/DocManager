using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Validators
{
    public class IdentityErrorTranslator
    {
        public static IdentityError Translate(IdentityError error)
        {
            switch (error.Code)
            {
                case "DuplicateEmail":
                    return new IdentityError
                    {
                        Code = error.Code,
                        Description = "Este correo electrónico ya está registrado."
                    };
                case "DuplicateUserName":
                    return new IdentityError
                    {
                        Code = error.Code,
                        Description = "Este nombre de usuario ya está registrado."
                    };
                // Agregar más traducciones aquí según sea necesario
                default:
                    return error; // Para otros errores, no traducir
            }
        }
    }

}
