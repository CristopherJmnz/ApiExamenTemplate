using ApiExamenTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiExamenTemplate.Data
{
    public class DoctoresContext : DbContext
    {
        public DoctoresContext(DbContextOptions<DoctoresContext>options):base(options)
        {
            
        }

        public DbSet<Doctor> Doctores{ get; set; }
    }
}
