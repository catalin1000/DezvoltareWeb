using CatalinProiect2.Data;
using CatalinProiect2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CatalinProiect2.Controllers
{
    public class BooksController : Controller
    {
        private IWebHostEnvironment _env;
        private string? databaseFileName;



        // PASUL 10 - useri si roluri


        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public BooksController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment env
            )
        {
            db = context;

            _env = env;

            _userManager = userManager;

            _roleManager = roleManager;
        }

      



        [Authorize(Roles = "Editor, Admin")]
        public IActionResult New()
        {
            Book p = new Book();
            return View(p);
        }

        [Authorize(Roles = "Editor, Admin")]
        [HttpPost]
        public async Task<IActionResult> New(Book p)
        {
            p.ApplicationUserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                db.Books.Add(p);
                db.SaveChanges();
              


                TempData["Message"] = "Carte adaugata cu succes";
                TempData["messageType"] = "alert-success";

            }
            return View(p);



        }
        public IActionResult Index(string sortOrder, string sortDirection)
        {

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }

            var books = db.Books.Include("Category")
                                       .Include("ApplicationUser")
                                       .OrderBy(a => a.Price);



            var search = "";

            ViewBag.SearchString = search;

            // AFISARE PAGINATA


            int _perPage = 3;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.Alert = TempData["messageType"];
            }

            int totalItems = books.Count();


            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);


            var offset = 0;


            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }




            var paginatedBooks = books.Skip(offset).Take(_perPage);

            ViewBag.Books = paginatedBooks;


            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);


            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/Books/Index/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/Books/Index/?page";
            }

            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            var book = db.Books
                         .Include("ApplicationUser")
                         .Include("Author")
                         .Where(c => c.Id == id).First();



            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }
            if (book == null)
            {
                TempData["Message"] = "Cartea nu exista in Baza de Date";
                TempData["messageType"] = "alert-danger";

            }



            return View(book);
        }





        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Delete(int id)
        {
            try
            {

                var query = db.Books.Find(id);
                if (User.IsInRole("Admin") || query.ApplicationUserId == _userManager.GetUserId(User))
                {
                    db.Books.Remove(query);
                    db.SaveChanges();
                    TempData["Message"] = "Carte stearsa cu succes";
                    TempData["messageType"] = "alert alert-success";
                    return RedirectToAction("Index");
                }
                TempData["Message"] = "Nu ai dreptul la resursa asta";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "a aparut o eroare; poate resursa este deja stearsa";
                TempData["messageType"] = " alert-danger";
                return RedirectToAction("Index");
            }

        }


    }
}
