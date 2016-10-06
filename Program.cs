using System;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Batte.Examples
{
  public class Program
  {
    public static void Main(string[] args) => new WebHostBuilder()
      .UseContentRoot(Path.Combine(Directory.GetCurrentDirectory(), "static"))
      .UseKestrelHttps()
      .UseStartup<Program>()
      .Build()
      .Run();

    public IConfigurationRoot Configuration { get; private set; }
    public IHostingEnvironment HostEnvironment { get; private set; }

    public Program(IHostingEnvironment environment)
    {
      HostEnvironment = environment;

      Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();
    }

    public void ConfigureServices(IServiceCollection services) => services
      .AddRouting(options => options.LowercaseUrls = true)
      .AddOptions()
      .AddMvc();

    public void Configure(IApplicationBuilder builder) => builder
      .UseStaticFiles(new StaticFileOptions
      {
        FileProvider = new PhysicalFileProvider(HostEnvironment.ContentRootPath),
        RequestPath = new PathString(string.Empty)
      })
      .UseMvc()
      .ApplicationServices.GetService<ILoggerFactory>()
        .AddConsole(Configuration.GetSection("Logging"))
        .AddDebug();
  }
}
