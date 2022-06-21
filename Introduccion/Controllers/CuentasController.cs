using Introduccion.Models;
using Introduccion.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Introduccion.Controllers
{
  [Authorize]
  public class CuentasController : Controller
  {
    private readonly UserManager<IdentityUser> _userManager; // Permite administrar y gestionar usuarios
    private readonly SignInManager<IdentityUser> _signInManager; // Contiene metodos para iniciar sesion

    public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
      this._userManager = userManager;
      this._signInManager = signInManager;
    }

    [HttpGet]
    [Route("Cuentas/Registro")]
    [AllowAnonymous]
    public IActionResult Registro()
    {
      return View();
    }

    [HttpPost]
    [Route("Cuentas/Registro")]
    [AllowAnonymous]
    public async Task<IActionResult> Registro(Registro registroModel)
    {
      if (ModelState.IsValid)
      {
        // se guardan los datos en una instancia de IdentityUser
        var user = new IdentityUser
        {
          UserName = registroModel.Email,
          Email = registroModel.Email
        };

        // Guardamos los datos en ua tabla de AspNetUser
        var result = await _userManager.CreateAsync(user, registroModel.Password);

        // Si el usuario se creo correctamente y se logeo, se redigira a la pagina de inicio
        if (result.Succeeded)
        {
          if (_signInManager.IsSignedIn(User) && User.IsInRole("Administrador"))
          {
            return RedirectToAction("ListaUsuarios", "Administracion");
          }
          await _signInManager.SignInAsync(user, isPersistent: false);
          return RedirectToAction("Index", "Home");
        }

        // Se controlan los errores en caso que se produzcan
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
      }
      return View(registroModel);
    }

    [HttpGet]
    [Route("Cuentas/IniciarSesion")]
    [AllowAnonymous]
    public IActionResult IniciarSesion()
    {
      return View();
    }

    [HttpPost]
    [Route("Cuentas/IniciarSesion")]
    [AllowAnonymous]
    public async Task<IActionResult> IniciarSesion(LoginView loginView)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByEmailAsync(loginView.Email);

        var result = await _signInManager.PasswordSignInAsync(
          user,
          loginView.Password,
          loginView.RememberMe,
          false
        );

        if (result.Succeeded)
        {
          return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Inicio de sesion no valido");
      }
      return View(loginView);
    }

    [HttpPost]
    [Route("Cuentas/CerrarSesion")]
    public async Task<IActionResult> CerrarSesion()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }

    [AcceptVerbs("Get", "Post")]
    [AllowAnonymous]
    [Route("Cuentas/ComprobarEmail")]
    public async Task<IActionResult> ComprobarEmail(string email)
    {
      var user = await _userManager.FindByEmailAsync(email);
      return (user == null)
        ? Json(true)
        : Json($"El email: {email} no está disponible");
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("Cuentas/AccesoDenegado")]
    public IActionResult AccesoDenegado()
    {
      return View();
    }
  }
}
