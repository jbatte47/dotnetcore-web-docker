using System;
using System.Net;

using Microsoft.AspNetCore.Hosting;

namespace Batte.Examples
{
  public static class Extensions
  {
    private static readonly Func<string,string> _env = Environment.GetEnvironmentVariable;

    public static IWebHostBuilder UseKestrelHttps(this IWebHostBuilder builder) => builder
      .UseKestrel(options => options.UseHttps(_env("CERT_PATH"), _env("CERT_PASSWORD")))
      .UseUrls($"https://{IPAddress.Any}:{_env("HTTPS_PORT")}");
  }
}
