using Microsoft.AspNetCore.Mvc;

namespace Batte.Examples
{
  [Route("api/[controller]")]
  public class HelloController : Controller
  {
    [HttpGet("{recipient}")]
    public ContentResult Get(string recipient)
    {
      return Content($"Hello, {recipient}!");
    }
  }
}
