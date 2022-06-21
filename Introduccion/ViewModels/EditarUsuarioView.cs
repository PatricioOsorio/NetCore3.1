using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Introduccion.ViewModels
{
  public class EditarUsuarioView
  {
    public string Id { get; set; }

    [Required]
    [Display(Name = "Nombre de usuario")]
    public string NombreUsuario { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public List<string> Notificaciones { get; set; }

    public IList<string> Roles { get; set; }


    public EditarUsuarioView()
    {
      Notificaciones = new List<string>();
      Roles = new List<string>();
    }
  }
}
