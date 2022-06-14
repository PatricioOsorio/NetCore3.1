using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Introduccion.ViewModels
{
  public class EditarRolView
  {
    public string Id { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "Nombre rol")]
    public string Name { get; set; }
    public List<string> Users { get; set; }

    public EditarRolView()
    {
      Users = new List<string>();
    }
  }
}
