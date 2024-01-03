using Microsoft.AspNetCore.Mvc;

namespace CatalinProiect2.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
