using System;

using Microsoft.AspNetCore.Hosting;

namespace Batte.Examples
{
  public static class Extensions
  {
    private static readonly Func<string,string> _env = Environment.GetEnvironmentVariable;

    public static S GetService<S>(this IServiceProvider provider) where S : class => provider
      .GetService(typeof(S)) as S;

    public static IWebHostBuilder UseKestrelHttps(this IWebHostBuilder builder) => builder
      .UseKestrel(options => options.UseHttps(_env("CERT_PATH"), _env("CERT_PASSWORD")))
      .UseUrls($"https://0.0.0.0:{_env("HTTPS_PORT")}");
  }
}
