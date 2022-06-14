using System.ComponentModel.DataAnnotations;

namespace Introduccion.Utilities
{
  public class ValidarNombreUsuario : ValidationAttribute
  {
    private readonly string _usuario;

    public ValidarNombreUsuario(string usuario)
    {
      _usuario = usuario;
    }

    public override bool IsValid(object value)
    {
      bool isValid;

      isValid = value.ToString().ToLower().Contains("joder") ? false : true;

      return isValid;
    }
  }
}
