using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebAPI.Domain.Entities;

namespace WebAPI.Data.Contexts
{
    [ExcludeFromCodeCoverage]
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
