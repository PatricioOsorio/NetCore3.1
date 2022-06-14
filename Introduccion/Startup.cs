using Introduccion.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Introduccion
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContextPool<AppDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("ConnectionDB")));
      services.AddMvc(options => options.EnableEndpointRouting = false);
      services.AddTransient<IAmigos, AmigosSql>();

      // Uso de Identity
      services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddErrorDescriber<MessageErrorsSpanish>();

      // Ruta por defecto cuando se solicita una pagina con autirizacion
      services.ConfigureApplicationCookie(options =>
      {
        options.LoginPath = "/Cuentas/IniciarSesion";
        options.AccessDeniedPath = "/Cuentas/AccesoDenegado";
      });

    // Configuraciones de los requerimientos de la contraseña
    services.Configure<IdentityOptions>(opciones =>
      {
        opciones.Password.RequiredLength = 8;
        opciones.Password.RequiredUniqueChars = 3;
        opciones.Password.RequireNonAlphanumeric = false;
      });
    }

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
  if (env.IsDevelopment())
  {
    app.UseDeveloperExceptionPage();
  }
  else
  {
    //app.UseExceptionHandler("/Error/{0}");
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithRedirects("/Error/{0}");
  }

  app.UseStaticFiles();

  app.UseAuthentication();

  app.UseMvc(routes =>
  {
    routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
  });


  //app.UseRouting();

  //app.UseAuthorization();

  //app.UseEndpoints(endpoints =>
  //{
  //  endpoints.MapRazorPages();
  //});


}
  }
}
