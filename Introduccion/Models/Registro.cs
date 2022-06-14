using Introduccion.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Introduccion.Models
{
  public class Registro
  {
    [Required(ErrorMessage = "Campo obligatorio")]
    [EmailAddress]
    [Remote(action: "ComprobarEmail", controller: "Cuentas")]
    [ValidarNombreUsuario(usuario: "joder", ErrorMessage = "Palabra no permitida")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [DataType(DataType.Password)]
    [Display(Name = "Repetir Password")]
    [Compare("Password", ErrorMessage = "El password de confirmacion no coincide")]
    public string PasswordValidator { get; set; }
  }
}
