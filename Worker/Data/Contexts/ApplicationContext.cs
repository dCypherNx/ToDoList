using Microsoft.EntityFrameworkCore;
using Worker.Domain.Entities;

namespace Worker.Data.Contexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>().HasKey(t => t.Id);
        }
    }
}
