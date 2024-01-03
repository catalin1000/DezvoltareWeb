using Microsoft.AspNetCore.Mvc;

namespace CatalinProiect2.Controllers
{
    public class DrinksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
