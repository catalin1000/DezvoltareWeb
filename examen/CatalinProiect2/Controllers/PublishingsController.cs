using CatalinProiect2.Data;
using CatalinProiect2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CatalinProiect2.Controllers
{
    public class PublishingsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public PublishingsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }


        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }




            if (User.IsInRole("User") || User.IsInRole("Editor"))
            {
                var pubs = from pub in db.Publishings.Include("User")
                               .Where(b => b.UserId == _userManager.GetUserId(User))
                             select pub;

                ViewBag.Orders = pubs;

                return View();
            }
            else
            {
                var pubs = from pub in db.Publishings.Include("User")
                             select pub;

                ViewBag.Orders = pubs;

                return View();
            }
        }





        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {

            if (User.IsInRole("User") || User.IsInRole("Editor"))
            {
                var pubs = db.Publishings
                                  .Include("Writtings.Book.ApplicationUser")

                                  .Where(b => b.Id == id)
                                  .Where(b => b.UserId == _userManager.GetUserId(User))
                                  .FirstOrDefault();

                if (pubs == null)
                {
                    TempData["message"] = "Nu aveti acces";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Books");
                }
                ViewBag.PoateModifica = false; //poate modifica ddca este admin View-ul asociat este doar pentru comenzi "comandate"
                return View(pubs);
            }

            else
            {
                var pubs = db.Publishings
                                  .Include("Writtings.Book.ApplicationUser")
                                  .Include("User")
                                  .Where(b => b.Id == id)
                                  .FirstOrDefault();


                if (pubs == null)
                {
                    TempData["message"] = "Resursa cautata nu poate fi gasita";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Drinks");
                }

                ViewBag.PoateModifica = true;
                return View(pubs);
            }

        }



    }
}
