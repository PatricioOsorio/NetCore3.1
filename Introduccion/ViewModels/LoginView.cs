using System.ComponentModel.DataAnnotations;

namespace Introduccion.ViewModels
{
  public class LoginView
  {
    [Required(ErrorMessage = "Campo obligatorio")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [DataType(DataType.Password)]
    [Display(Name = "Constraseña")]
    public string Password { get; set; }

    [Display(Name = "Recuerdame")]
    public bool RememberMe { get; set; }
  }
}
