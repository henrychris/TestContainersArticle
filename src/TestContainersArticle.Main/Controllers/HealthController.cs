using HenryUtils.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TestContainersArticle.Host.Controllers;

public class HealthController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Hello World!");
    }
}
