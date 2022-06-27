using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq;

namespace Introduccion.Models
{
  public class AppDbContext : IdentityDbContext
  {
    public DbSet<Amigo> Amigos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Quitar la eliminacion en cascada
      foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
      {
        fk.DeleteBehavior = DeleteBehavior.Restrict;
      }
    }
  }
}
