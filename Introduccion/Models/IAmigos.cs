using System.Collections.Generic;

namespace Introduccion.Models
{
  public interface IAmigos
  {
    Amigo GetAmigo(int id);
    List<Amigo> GetAmigos();
    Amigo NewAmigo(Amigo amigo);
    Amigo UpdateAmigo(Amigo amigo);
    Amigo DeleteAmigo(int id);
  }
}
