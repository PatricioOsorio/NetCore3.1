using System.Collections.Generic;
using System.Linq;

namespace Introduccion.Models
{
  public class AmigosSql : IAmigos
  {
    private AppDbContext _context;
    private List<Amigo> amigos;

    public AmigosSql(AppDbContext context)
    {
      _context = context;
    }

    public Amigo GetAmigo(int id)
    {
      return _context.Amigos.Find(id);
    }

    public List<Amigo> GetAmigos()
    {
      amigos = _context.Amigos.ToList<Amigo>();
      return amigos;
    }

    public Amigo NewAmigo(Amigo amigo)
    {
      _context.Amigos.Add(amigo);
      _context.SaveChanges();
      return amigo;
    }

    public Amigo UpdateAmigo(Amigo amigo)
    {
      var employee = _context.Amigos.Attach(amigo);
      employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
      _context.SaveChanges();
      return amigo;
    }

    public Amigo DeleteAmigo(int id)
    {
      Amigo amigo = _context.Amigos.Find(id);

      if (amigo != null)
      {
        _context.Amigos.Remove(amigo);
        _context.SaveChanges();
      }

      return amigo;
    }
  }
}
