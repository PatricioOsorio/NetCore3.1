using Introduccion.Models;
using Introduccion.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Introduccion.Controllers
{
  [AllowAnonymous]
  public class HomeController : Controller
  {
    private readonly UserManager<IdentityUser> _userManager; // Permite administrar y gestionar usuarios
    private readonly SignInManager<IdentityUser> _signInManager; // Contiene metodos para iniciar sesion
    private readonly RoleManager<IdentityRole> _roleManager;


    public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
      var user = await _userManager.FindByEmailAsync("sysadmin@hotmail.com");

      if (user == null)
      {
        await _roleManager.CreateAsync(new IdentityRole("SYSADMIN"));
        await _roleManager.CreateAsync(new IdentityRole("ADMINISTRADOR"));
        await _roleManager.CreateAsync(new IdentityRole("USUARIO"));

        var newUserSysadmin = new IdentityUser { UserName="sysadmin", Email = "sysadmin@hotmail.com" };
        var newUserAdministrador = new IdentityUser { UserName="administrador", Email = "administrador@hotmail.com" };
        var newUserUsuario = new IdentityUser { UserName="usuario", Email = "usuario@hotmail.com" };

        await _userManager.CreateAsync(newUserSysadmin, "Pato12345");
        await _userManager.AddToRoleAsync(newUserSysadmin, "SYSADMIN");

        await _userManager.CreateAsync(newUserAdministrador, "Pato12345");
        await _userManager.AddToRoleAsync(newUserAdministrador, "ADMINISTRADOR");

        await _userManager.CreateAsync(newUserUsuario, "Pato12345");
        await _userManager.AddToRoleAsync(newUserUsuario, "USUARIO");
      }

      return View();
    }
  }
}
