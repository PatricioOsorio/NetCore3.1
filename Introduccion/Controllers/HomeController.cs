using Introduccion.Models;
using Introduccion.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Introduccion.Controllers
{
  [AllowAnonymous]
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
