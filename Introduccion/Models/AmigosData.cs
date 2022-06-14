using System.Collections.Generic;
using System.Linq;

namespace Introduccion.Models
{
  public class AmigosData : IAmigos
  {
    private List<Amigo> amigos;

    public AmigosData()
    {
      this.amigos = new List<Amigo>()
      {
        new Amigo() { Id = 1, Nombre = "Pedro", Ciudad = Provincia.Acapulco, Correo = "pedro@correo.com" },
        new Amigo() { Id = 2, Nombre = "Maria", Ciudad = Provincia.Chihuahua, Correo = "maria@correo.com" },
        new Amigo() { Id = 3, Nombre = "Juan", Ciudad = Provincia.Colima, Correo = "juan@correo.com" }
      };
    }

    public Amigo GetAmigo(int id) => this.amigos.Find(element => element.Id == id);

    public List<Amigo> GetAmigos() => this.amigos;

    public Amigo NewAmigo(Amigo amigo)
    {
      amigo.Id = amigos.Max(a => a.Id) + 1;
      amigos.Add(amigo);
      return amigo;
    }

    public Amigo UpdateAmigo(Amigo amigoModificado)
    {
      Amigo amigoActualizado = amigos.FirstOrDefault(e => e.Id == amigoModificado.Id);

      if (amigoActualizado != null)
      {
        amigoActualizado.Nombre = amigoModificado.Nombre;
        amigoActualizado.Correo = amigoModificado.Correo;
        amigoActualizado.Ciudad = amigoModificado.Ciudad;
      }

      return amigoActualizado;
    }

    public Amigo DeleteAmigo(int id)
    {
      Amigo amigo = amigos.FirstOrDefault(e => e.Id == id);

      if (amigo != null)
      {
        amigos.Remove(amigo);
      }

      return amigo;
    }
  }
}
