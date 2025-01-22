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
        public int paciente_id { get; set; }
        [Required]
        public string paciente_nombreCompleto { get; set; }

        [Required]
        public DateOnly paciente_fechaNac {  get; set; }
        public string paciente_estadoCivil { get; set; }
        [Required]
        [MaxLength(80)]
        public string paciente_direccion { get; set; }

        [Required]
        public int paciente_telefono { get; set; }
        [Required]
        public string paciente_correoElectronico { get; set; }

        [Required]
        public string paciente_DNI { get; set; }

        public int medico_id { get; set; }
        public Medico Medico {  get; set; } 
    }
}
