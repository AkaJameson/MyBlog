using Microsoft.AspNetCore.Mvc;

namespace MyBlog.WebApi
{
    [Route("/api/[controller]/[action]")]
    [ResponseCache(Duration = 30, VaryByHeader = "User-Agent")]
    public class DefaultController : ControllerBase
    {

    }
}
