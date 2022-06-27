using Introduccion.Models;
using Introduccion.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Authorize(Policy = "BorrarRolPolicy")]
    public async Task<IActionResult> BorrarRol(string id)
    {
      var rol = await _roleManager.FindByIdAsync(id);

      if (rol == null)
      {
        ViewData["ErrorMessage"] = $"Rol con Id ({id}) no fue encontrado";
        return View("Error");
      }

      var result = await _roleManager.DeleteAsync(rol);

      if (result.Succeeded) return RedirectToAction("ListaRoles");

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

    /* ======================================== GESTIONAR ROLES USUARIO ======================================== */
    [HttpGet]
    [Route("Administracion/GestionarRolesUsuario")]
    public async Task<IActionResult> GestionarRolesUsuario(string idUsuario)
    {
      ViewBag.IdUsuario = idUsuario;

      var user = await _userManager.FindByIdAsync(idUsuario);

      if (user == null)
      {
        ViewData["ErrorMessage"] = $"El usuario con id: (${idUsuario}) no fue encontrado";
        return View("Error");
      }

      var model = new List<RolUsuarioView>();

      foreach (var rol in _roleManager.Roles)
      {
        var rolUsuarioView = new RolUsuarioView
        {
          RolId = rol.Id,
          RolNombre = rol.Name
        };

        rolUsuarioView.EstaSeleccionado = await _userManager.IsInRoleAsync(user, rol.Name);

        model.Add(rolUsuarioView);
      }

      return View(model);
    }

    [HttpPost]
    [Route("Administracion/GestionarRolesUsuario")]
    public async Task<IActionResult> GestionarRolesUsuario(List<RolUsuarioView> rolUsuarioViews, string idUsuario)
    {
      var user = await _userManager.FindByIdAsync(idUsuario);

      if (user is null)
      {
        ViewData["ErrorMessage"] = $"El usuario con id: (${idUsuario}) no existe";
        return View("Error");
      }

      var roles = await _userManager.GetRolesAsync(user);
      var result = await _userManager.RemoveFromRoleAsync(user, roles.ToString());

      if (!result.Succeeded)
      {
        ModelState.AddModelError(string.Empty, "No podemos borrar usuarios con roles");
        return View(rolUsuarioViews);
      }

      result = await _userManager.AddToRolesAsync(
        user,
        rolUsuarioViews
          .Where(x => x.EstaSeleccionado)
          .Select(y => y.RolNombre)
       );

      if (!result.Succeeded)
      {
        ModelState.AddModelError(string.Empty, "No podemos añadir roles al usuario seleccionados");
        return View(rolUsuarioViews);
      }

      return RedirectToAction("EditarUsuario", new { Id = idUsuario });
    }

    [HttpGet]
    [Route("Administracion/GestionarUsuarioClaims")]
    public async Task<IActionResult> GestionarUsuarioClaims(string idUsuario)
    {
      var user = await _userManager.FindByIdAsync(idUsuario);

      if (user == null)
      {
        ViewData["ErrorMessage"] = $"El usuario con id: (${idUsuario}) no existe";
        return View("Error");
      }

      var userClaims = await _userManager.GetClaimsAsync(user);

      var model = new UserClaimsViewModel()
      {
        IdUser = idUsuario
      };

      // Recorremos los claims
      foreach (Claim claim in RepositoryClaims.AllClaims)
      {
        UserClaims userClaim = new UserClaims
        {
          TypeClaim = claim.Type
        };

        // Si el usuario tiene el claim que estamos recorriendo en este momento lo seleccionamos
        if (userClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
        {
          userClaim.IsSelected = true;
        }

        model.Claims.Add(userClaim);
      }

      return View(model);
    }

    [HttpPost]
    [Route("Administracion/GestionarUsuarioClaims")]
    public async Task<IActionResult> GestionarUsuarioClaims(UserClaimsViewModel model)
    {
      var user = await _userManager.FindByIdAsync(model.IdUser);

      if (user == null)
      {
        ViewData["ErrorMessage"] = $"El usuario con id: ({model.IdUser}) no existe";
        return View("Error");
      }

      // Obtenemos los claims del usuario y los borramos
      var claims = await _userManager.GetClaimsAsync(user);

      var userClaims = await _userManager.GetClaimsAsync(user);

      var result = await _userManager.RemoveClaimsAsync(user, claims);

      if (!result.Succeeded)
      {
        ModelState.AddModelError(string.Empty, "No se pueden borrar los claims de este usuario");
        return View(model);
      }

      // Volvemos a asociar lo seleccionado a la interfaz grafica
      result = await _userManager.AddClaimsAsync(
        user,
        model.Claims
          .Where(c => c.IsSelected)
          .Select(c => new Claim(c.TypeClaim, c.IsSelected ? "true" : "false"))
        );

      if (!result.Succeeded)
      {
        ModelState.AddModelError(string.Empty, "No se agregar claims a este usuario");
        return View(model);
      }

      return RedirectToAction("EditarUsuario", new { Id = model.IdUser });
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
        Notificaciones = usuarioClaims.Select(c => $"{c.Type}: {c.Value}").ToList(),
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
