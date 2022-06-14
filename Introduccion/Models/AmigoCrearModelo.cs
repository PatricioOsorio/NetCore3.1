using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Introduccion.Models
{
  public class AmigoCrearModelo
  {
    [Required(ErrorMessage = "Obligatorio")]
    [MaxLength(100, ErrorMessage = "Maximo 100 caracteres")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Obligatorio")]
    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Formato incorrecto")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "Seleccione ciudad")]
    public Provincia? Ciudad { get; set; }

    public IFormFile Foto { get; set; }
  }
}
