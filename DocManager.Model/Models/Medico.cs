using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Model.Models
{
    public class Medico
    {
        [Key]
        public string medico_id {  get; set; } //de identity
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string medico_nombreCompleto { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(10)]
        public int medico_telefono { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(50)]
        public string medico_correo { get; set; }
        
        //public string ApplicationUser_MedicoId { get; set; }

        // Relación: Un médico tiene muchos pacientes
        public ICollection<Paciente> Pacientes { get; set; }
    }
}
