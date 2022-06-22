using Introduccion.Models;
using Introduccion.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Introduccion.Controllers
{
  [Authorize(Roles = "SYSADMIN, ADMINISTRADOR")]
  public class AdministracionController : Controller
  {
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    public AdministracionController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
      _roleManager = roleManager;
      _userManager = userManager;
    }

    /* ======================================== ROLES ======================================== */

    [HttpGet]
    [Route("Administracion/ListaRoles")]
    public IActionResult ListaRoles()
    {
      var roles = _roleManager.Roles;
      return View(roles);
    }

    [HttpGet]
    [Route("Administracion/CrearRol")]
    public IActionResult CrearRol()
    {
      return View();
    }

    [HttpPost]
    [Route("Administracion/CrearRol")]
    public async Task<IActionResult> CrearRol(CrearRolView crearRolView)
    {
      if (ModelState.IsValid)
      {
        IdentityRole identityRole = new IdentityRole
        {
          Name = crearRolView.NombreRol
        };

        IdentityResult identityResult = await _roleManager.CreateAsync(identityRole);
        if (identityResult.Succeeded)
        {
          return RedirectToAction("ListaRoles", "Administracion");
        }

        foreach (var error in identityResult.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
      }
      return View();
    }

    [HttpGet]
    [Route("Administracion/EditarRol")]
    public async Task<IActionResult> EditarRol(string id)
    {
      var rol = await _roleManager.FindByIdAsync(id);

      if (rol == null)
      {
        ViewData["ErrorMessage"] = $"Rol con Id {id} no encontrado";
        return View("Error");
      }

      var model = new EditarRolView
      {
        Id = rol.Id,
        Name = rol.Name
      };

      foreach (var user in _userManager.Users)
      {
        if (await _userManager.IsInRoleAsync(user, rol.Name))
        {
          model.Users.Add(user.UserName);
        }
      }

      return View(model);
    }

    [HttpPost]
    [Route("Administracion/EditarRol")]
    public async Task<IActionResult> EditarRol(EditarRolView editarRolView)
    {
      var rol = await _roleManager.FindByIdAsync(editarRolView.Id);

      if (rol == null)
      {
        ViewData["ErrorMessage"] = $"Rol con Id {editarRolView.Id} no encontrado";
        return View("Error");
      }
      else
      {
        rol.Name = editarRolView.Name;

        var result = await _roleManager.UpdateAsync(rol);

        if (result.Succeeded) return RedirectToAction("ListaRoles");

        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(editarRolView);
      }
    }

    [HttpPost]
    [Route("Administracion/BorrarRol")]
    public async Task<IActionResult> BorrarRol(string id)
    {
      var rol = await _roleManager.FindByIdAsync(id);

      if (rol == null)
      {
        ViewData["ErrorMessage"] = $"Rol con Id ({id}) no fue encontrado";
        return View("Error");
      }

      var result = await _roleManager.DeleteAsync(rol);

      if (result.Succeeded)
      {
        return RedirectToAction("ListaRoles");
      }

      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      return View("ListaRoles");
    }
    /* ======================================== USUARIO ROL ======================================== */

    [HttpGet]
    [Route("Administracion/EditarUsuarioRol")]
    public async Task<IActionResult> EditarUsuarioRol(string rolId)
    {
      ViewData["roleId"] = rolId;

      var role = await _roleManager.FindByIdAsync(rolId);

      if (role == null)
      {
        ViewData["ErrorMessage"] = $"El rol con Id ({rolId}) no se ha encontrado";
        return View("Error");
      }

      var model = new List<UsuarioRol>();

      foreach (var user in _userManager.Users)
      {
        var usuarioRol = new UsuarioRol()
        {
          UsuarioId = user.Id,
          UsuarioNombre = user.UserName,
          //EstaSeleccionado = await _userManager.IsInRoleAsync(user, role.Name)
        };

        usuarioRol.EstaSeleccionado = await _userManager.IsInRoleAsync(user, role.Name) ? true : false;

        model.Add(usuarioRol);
      }

      return View(model);
    }

    [HttpPost]
    [Route("Administracion/EditarUsuarioRol")]
    public async Task<IActionResult> EditarUsuarioRol(List<UsuarioRol> model, string rolId)
    {
      var rol = await _roleManager.FindByIdAsync(rolId);

      if (rol == null)
      {
        ViewData["ErrorMessage"] = $"Rol con Id ({rolId}) no existe";
        return View("Error");
      }

      var lastUser = model.LastOrDefault();

      foreach (var usuarioRol in model)
      {
        var user = await _userManager.FindByIdAsync(usuarioRol.UsuarioId);

        IdentityResult result;

        if (usuarioRol.EstaSeleccionado && !(await _userManager.IsInRoleAsync(user, rol.Name)))
        {
          result = await _userManager.AddToRoleAsync(user, rol.Name);
        }
        else if (!usuarioRol.EstaSeleccionado && await _userManager.IsInRoleAsync(user, rol.Name))
        {
          result = await _userManager.RemoveFromRoleAsync(user, rol.Name);
        }
        else
        {
          continue;
        }

        if (result.Succeeded)
        {
          if (usuarioRol.Equals(lastUser)) return RedirectToAction("EditarRol", new { Id = rolId });
        }
      }

      return RedirectToAction("EditarRol", new { Id = rolId });
    }

    /* ======================================== USUARIOS ======================================== */
    [HttpGet]
    [Route("Administracion/ListaUsuarios")]
    public IActionResult ListaUsuarios()
    {
      var users = _userManager.Users;
      return View(users);
    }

    [HttpGet]
    [Route("Administracion/EditarUsuario")]
    public async Task<IActionResult> EditarUsuario(string id)
    {
      var usuario = await _userManager.FindByIdAsync(id);

      if (usuario == null)
      {
        ViewData["ErrorMessage"] = $"El Usuario con Id ({id}) no se ha encontrado";
        return View("Error");
      }

      // Lista de notificaciones
      var usuarioClaims = await _userManager.GetClaimsAsync(usuario);

      // Regresa la lista de roles de usuario
      var usuarioRoles = await _userManager.GetRolesAsync(usuario);

      var model = new EditarUsuarioView
      {
        Id = usuario.Id,
        NombreUsuario = usuario.UserName,
        Email = usuario.Email,
        Notificaciones = usuarioClaims.Select(c => c.Value).ToList(),
        Roles = usuarioRoles
      };

      return View(model);
    }

    [HttpPost]
    [Route("Administracion/EditarUsuario")]
    public async Task<IActionResult> EditarUsuario(EditarUsuarioView editarUsuarioView)
    {
      var usuario = await _userManager.FindByIdAsync(editarUsuarioView.Id);

      if (usuario == null)
      {
        ViewData["ErrorMessage"] = $"El Usuario con Id ({editarUsuarioView.Id}) no se ha encontrado";
        return View("Error");
      }
      else
      {
        usuario.Email = editarUsuarioView.Email;
        usuario.UserName = editarUsuarioView.NombreUsuario;

        var result = await _userManager.UpdateAsync(usuario);

        if (result.Succeeded)
        {
          return RedirectToAction("ListaUsuarios");
        }

        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(editarUsuarioView);
      }

    }

    [HttpPost]
    [Route("Administracion/BorrarUsuario")]
    public async Task<IActionResult> BorrarUsuario(string id)
    {
      var user = await _userManager.FindByIdAsync(id);

      if (user == null)
      {
        ViewData["ErrorMessage"] = $"Usuario con Id ({id}) no fue encontrado";
        return View("Error");
      }

      var result = await _userManager.DeleteAsync(user);

      if (result.Succeeded)
      {
        return RedirectToAction("ListaUsuarios");
      }

      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      return View("ListaUsuarios");
    }
  }
}
