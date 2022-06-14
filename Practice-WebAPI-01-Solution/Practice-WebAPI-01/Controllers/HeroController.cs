using Microsoft.AspNetCore.Mvc;

namespace Practice_WebAPI_01.Controllers
{
    public class HeroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
