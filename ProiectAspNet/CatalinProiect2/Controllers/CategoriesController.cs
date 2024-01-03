using Microsoft.AspNetCore.Mvc;

namespace CatalinProiect2.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
