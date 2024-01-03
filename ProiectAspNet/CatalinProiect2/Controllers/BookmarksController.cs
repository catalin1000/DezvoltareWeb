using Microsoft.AspNetCore.Mvc;

namespace CatalinProiect2.Controllers
{
    public class BookmarksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
