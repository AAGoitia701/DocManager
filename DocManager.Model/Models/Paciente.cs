using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Model.Models
{
    public class Paciente
    {
        [Key]
        public string paciente_id { get; set; } //de identity
        [Required]
        public string paciente_nombreCompleto { get; set; } = "NA";

        [Required]
        public DateOnly paciente_fechaNac {  get; set; }
        public string paciente_estadoCivil { get; set; } = "NA";
        [Required]
        [MaxLength(80)]
        public string paciente_direccion { get; set; }

        [Required]
        public int paciente_telefono { get; set; }
        [Required]
        public string paciente_correoElectronico { get; set; }

        [Required]
        public string paciente_DNI { get; set; }

        //public string ApplicationUser_PacienteId { get; set; }
        public string medico_id { get; set; } //de identity
        public Medico Medico {  get; set; } 
    }
}
