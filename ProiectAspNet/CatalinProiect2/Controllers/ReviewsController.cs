using CatalinProiect2.Data;
using CatalinProiect2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace CatalinProiect2.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ReviewsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Delete(int id)
        {
            Review comm = db.Reviews.Find(id);

            if (comm.ApplicationUserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Reviews.Remove(comm);
                db.SaveChanges();
                return Redirect("/Drinks/Show/" + comm.DrinkId);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti reviewul";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Drinks");
            }
        }


        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Edit(int id)
        {
            Review comm = db.Reviews.Find(id);

            if (comm.ApplicationUserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(comm);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati reviewul";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Drinks");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Edit(int id, Review requestReview)
        {
            Review comm = db.Reviews.Find(id);

            if (comm.ApplicationUserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    comm.Content = requestReview.Content;
                    comm.Rating = requestReview.Rating;

                    db.SaveChanges();

                    return Redirect("/Drinks/Show/" + comm.DrinkId);
                }
                else
                {
                    return View(requestReview);
                }
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Drinks");
            }
        }
    }
}
