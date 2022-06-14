using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Extensions.Logging;

namespace Introduccion
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging((hostingContext, loggin) =>
            {
              loggin.AddConfiguration(hostingContext.Configuration.GetSection("Loggin"));
              loggin.AddConsole();
              loggin.AddDebug();
              loggin.AddEventSourceLogger();
              loggin.AddNLog();
            });
  }
}
