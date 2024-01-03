using Microsoft.AspNetCore.Mvc;

namespace CatalinProiect2.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
