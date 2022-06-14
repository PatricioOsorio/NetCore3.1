using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Introduccion.Controllers
{
  public class ErrorController : Controller
  {
    private readonly ILogger<ErrorController> logs;

    public ErrorController(ILogger<ErrorController> logs)
    {
      this.logs = logs;
    }

    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
      switch (statusCode)
      {
        case 404:
          ViewData["Message"] = "Recurso solicitado no encontrado";
          break;
        default:
          ViewData["Message"] = "Ocurrio un error";
          break;
      }
      return View("Error");
    }

    [AllowAnonymous]
    [Route("Error")]
    public IActionResult Error()
    {
      var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

      logs.LogError(
        $"Ruta del error: {exceptionHandlerPathFeature.Path}; " +
        $"Excepcion: ${exceptionHandlerPathFeature.Error}; " +
        $"Traza del error: {exceptionHandlerPathFeature.Error.StackTrace}"
      );

      return View("GenericError");
    }
  }
}
