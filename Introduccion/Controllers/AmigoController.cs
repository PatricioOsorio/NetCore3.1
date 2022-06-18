using Introduccion.Models;
using Introduccion.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Introduccion.Controllers
{
  public class AmigoController : Controller
  {
    private readonly IAmigos amigos;
    private IHostingEnvironment hosting;

    public AmigoController(IAmigos amigos, IHostingEnvironment hosting)
    {
      this.amigos = amigos;
      this.hosting = hosting;
    }

    public IActionResult Index()
    {
      var amigosList = amigos.GetAmigos();
      return View(amigosList);
    }

    public IActionResult DetallesAmigo(int? id)
    {
      Amigo amigo = amigos.GetAmigo(id ?? 1);

      DetailsView detailsView = new DetailsView()
      {
        Titulo = "Lista amigos view models",
        Subtitulo = "Subtitulo",
        Amigo = amigo,
      };

      if (detailsView.Amigo == null)
      {
        //Response.StatusCode = 404;S
        //return View("Error", new { statusCode = 404 });
        return RedirectToAction("Error", new { statusCode = 404 });
      }

      return View(detailsView);
    }

    public IActionResult CrearAmigo()
    {
      return View();
    }

    [HttpPost]
    public IActionResult CrearAmigo(AmigoCrearModelo amigo)
    {
      if (ModelState.IsValid)
      {
        string imagenGuid = null;

        if (amigo.Foto != null)
        {
          string rutaCarpeta = Path.Combine(hosting.WebRootPath, "images");
          imagenGuid = Guid.NewGuid().ToString() + amigo.Foto.FileName;
          string rutaDefinitiva = Path.Combine(rutaCarpeta, imagenGuid);
          amigo.Foto.CopyTo(new FileStream(rutaDefinitiva, FileMode.Create));
        }


        Amigo newAmigo = new Amigo()
        {
          Nombre = amigo.Nombre,
          Correo = amigo.Correo,
          Ciudad = amigo.Ciudad,
          FotoRuta = imagenGuid
        };

        amigos.NewAmigo(newAmigo);

        return RedirectToAction("DetallesAmigo", new { id = newAmigo.Id });
      }

      return View();
    }

    [HttpGet]
    public IActionResult EditarAmigo(int id)
    {
      Amigo amigo = amigos.GetAmigo(id);
      AmigoEditarModelo amigoEditarModelo = new AmigoEditarModelo()
      {
        Id = amigo.Id,
        Nombre = amigo.Nombre,
        Correo = amigo.Correo,
        Ciudad = amigo.Ciudad,
        FotoRutaExistente = amigo.FotoRuta
      };

      return View(amigoEditarModelo);
    }

    [HttpPost]
    public IActionResult EditarAmigo(AmigoEditarModelo amigoEditarModelo)
    {
      if (ModelState.IsValid)
      {
        Amigo amigo = amigos.GetAmigo(amigoEditarModelo.Id);
        amigo.Nombre = amigoEditarModelo.Nombre;
        amigo.Correo = amigoEditarModelo.Correo;
        amigo.Ciudad = amigoEditarModelo.Ciudad;

        if (amigoEditarModelo.Foto != null)
        {
          if (amigoEditarModelo.FotoRutaExistente != null)
          {
            string ruta = Path.Combine(hosting.WebRootPath, "images", amigoEditarModelo.FotoRutaExistente);
            System.IO.File.Delete(ruta);
          }
          amigo.FotoRuta = SubirImagen(amigoEditarModelo);

        }
        Amigo amigoModificado = amigos.UpdateAmigo(amigo);
        return RedirectToAction("Index");
      }
      return View(amigoEditarModelo);
    }

    private string SubirImagen(AmigoEditarModelo amigoEditarModelo)
    {
      string nombreFichero = null;

      if (amigoEditarModelo.Foto != null)
      {
        string carpeta = Path.Combine(hosting.WebRootPath, "images");
        nombreFichero = Guid.NewGuid().ToString() + "_" + amigoEditarModelo.Foto.FileName;
        string ruta = Path.Combine(carpeta, nombreFichero);
        using (var fileStream = new FileStream(ruta, FileMode.Create))
        {
          amigoEditarModelo.Foto.CopyTo(fileStream);
        }
      }
      return nombreFichero;
    }
  }
}
