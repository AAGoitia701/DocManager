using DocManager.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Medico> Medico { get; set; }
        public DbSet<Paciente> Paciente { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Importante para configurar las tablas de Identity

            modelBuilder.Entity<Paciente>()
                .HasOne(e => e.Medico) // Propiedad de navegación
                .WithMany(m => m.Pacientes)
                .HasForeignKey(e => e.medico_id) // Clave foránea
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}
