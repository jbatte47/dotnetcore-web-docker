using System;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

using Autofac;
using Autofac.Extensions.DependencyInjection;

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

    public IContainer Container { get; private set; }
    public IConfigurationRoot Configuration { get; private set; }
    public IHostingEnvironment HostEnvironment { get; private set; }

    public Program(IHostingEnvironment environment)
    {
      HostEnvironment = environment;
      Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("config.json")
        .AddEnvironmentVariables()
        .Build();
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services.AddRouting(options => options.LowercaseUrls = true)
        .AddOptions()
        .AddMvc();

      var builder = new ContainerBuilder();
      builder.Populate(services);
      Container = builder.Build();
      return new AutofacServiceProvider(Container);
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggers, IApplicationLifetime lifetime)
    {
      app.UseStaticFiles(new StaticFileOptions
      {
        FileProvider = new PhysicalFileProvider(HostEnvironment.ContentRootPath),
        RequestPath = new PathString(string.Empty)
      }).UseMvc();

      loggers.AddConsole(Configuration.GetSection("Logging"));
      lifetime.ApplicationStopped.Register(Container.Dispose);
    }
  }
}
