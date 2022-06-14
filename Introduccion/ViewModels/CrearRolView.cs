using System.ComponentModel.DataAnnotations;

namespace Introduccion.ViewModels
{
  public class CrearRolView
  {
    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "Rol")]
    public string NombreRol { get; set; }
  }
}
