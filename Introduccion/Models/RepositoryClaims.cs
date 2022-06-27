using System.Collections.Generic;
using System.Security.Claims;

namespace Introduccion.Models
{
  public static class RepositoryClaims
  {
    public static List<Claim> AllClaims = new List<Claim>()
    {
      new Claim("Crear rol", "Crear rol"),
      new Claim("Editar rol", "Editar rol"),
      new Claim("Borrar rol", "Borrar rol")
    };
  }
}
