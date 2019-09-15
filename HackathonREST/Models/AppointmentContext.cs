using Microsoft.EntityFrameworkCore;

namespace HackathonREST.Models
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<AppointmentResult>().HasKey(a=> new { a.Center, a.Date });
        //}

        public DbSet<Appointment> Appointments { get; set; }

    }

    public class AppointmentResultContext : DbContext
    {
        public AppointmentResultContext(DbContextOptions<AppointmentResultContext> options) : base(options)
        {

        }

        public DbSet<AppointmentResult> AppointmentResults { get; set; }
    }
}
