using CatalinProiect2.Data;
using CatalinProiect2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CatalinProiect2.Controllers
{
    public class DrinksController : Controller
    {
        private IWebHostEnvironment _env;
        private string? databaseFileName;



        // PASUL 10 - useri si roluri


        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public DrinksController(
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

        public object DrinkPhoto { get; private set; }

        // Se afiseaza lista tuturor articolelor impreuna cu categoria 
        // din care fac parte
        // Pentru fiecare articol se afiseaza si userul care a postat articolul respectiv
        // HttpGet implicit
        [Authorize(Roles = "User,Editor,Admin")]

        public IActionResult Index(string sortOrder, string sortDirection)
        {

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }

            var drinks = db.Drinks.Include("Category")
                                       .Include("User")
                                       .OrderBy(a => a.Date);

            foreach (var p in drinks)
            {
                double? d = db.Reviews.Where(r => r.DrinkId == p.Id).Average(r => r.Rating);
                p.Rating = (int?)d;

            }

            var search = "";
            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();

                // Cautare in articol (Title si Content)
                List<int> drinksIds = db.Drinks
                                    .Where(
                                        at => at.Name.Contains(search)
                                        || at.Content.Contains(search)
                                    ).Select(a => a.Id).ToList();

                // Cautare in comentarii (Content)

                List<int> drinksIdsOfReviewsWithSearchString =
                            db.Reviews.Where
                            (c => c.Content.Contains(search)).Select(c => (int)c.DrinkId).ToList();
                // Se formeaza o singura lista formata din toate id-urile selectate anterior

                List<int> mergedIds = drinksIds.Union(drinksIdsOfReviewsWithSearchString).ToList();
                // Lista articolelor care contin cuvantul cautat
                // fie in articol -> Title si Content
                // fie in comentarii -> Content
                drinks = db.Drinks.Where(drinks =>
                            mergedIds.Contains(drinks.Id))
                            .Include("Category")
                            .Include("User")
                            .OrderBy(a => a.Date);
            }
            ViewBag.SearchString = search;

            // AFISARE PAGINATA

            // Alegem sa afisam 3 articole pe pagina
            int _perPage = 3;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.Alert = TempData["messageType"];
            }

            // Fiind un numar variabil de bauturi, verificam de fiecare data utilizand metoda Count()
            int totalItems = drinks.Count();

            // Se preia pagina curenta din View-ul asociat
            // Numarul paginii este valoarea parametrului page din ruta
            // /Articles/Index?page=valoare
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            // Pentru prima pagina offsetul o sa fie zero
            // Pentru pagina 2 o sa fie 3
            // Asadar offsetul este egal cu numarul de articole care au fost deja afisate pe paginile anterioare
            var offset = 0;

            // Se calculeaza offsetul in functie de numarul paginii la care suntem
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            switch (sortOrder)
            {
                case "Price":
                    drinks = sortDirection == "asc"
                        ? drinks.OrderBy(p => p.Price)
                        : drinks.OrderByDescending(p => p.Price);
                    break;
                case "Rating":

                    drinks = sortDirection == "asc"
                        ? drinks.OrderBy(p => p.Rating)
                        : drinks.OrderByDescending(p => p.Rating);
                    break;
                default:
                    drinks = drinks.OrderBy(p => p.Date);
                    break;
            }

            // Se preiau articolele corespunzatoare pentru fiecare pagina la care ne aflam
            // in functie de offset
            var paginatedDrinks = drinks.Skip(offset).Take(_perPage);

            ViewBag.Drinks = paginatedDrinks;
            // Preluam numarul ultimei pagini

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);
            // Trimitem articolele cu ajutorul unui ViewBag catre View-ul corespunzator

            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/Drinks/Index/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/Drinks/Index/?page";
            }

            return View();
        }


        [Authorize(Roles = "Editor, Admin")]
        public IActionResult New()
        {
            Drink p = new Drink();
            p.Categ = GetAllCategories();
            return View(p);
        }

        [Authorize(Roles = "Editor, Admin")]
        [HttpPost]
        public async Task<IActionResult> New(Drink p, IFormFile? DrinkPhoto)
        {
            p.UserId = _userManager.GetUserId(User);
            p.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Drinks.Add(p);
                db.SaveChanges();
                if (DrinkPhoto != null && DrinkPhoto.Length > 0)
                {

                    var entityPath = Path.Combine(
                                _env.WebRootPath,
                                "images",
                                p.Id.ToString()
                            );

                    var storagePath = Path.Combine(entityPath, DrinkPhoto.FileName);
                    Directory.CreateDirectory(entityPath);
                    p.Photo = "/images/" + p.Id.ToString() + "/" + DrinkPhoto.FileName;


                    using (var fileStream = new FileStream(storagePath, FileMode.Create))
                    {
                        await DrinkPhoto.CopyToAsync(fileStream);
                    }

                    db.SaveChanges();
                }


                TempData["Message"] = "Prous adaugat cu succes in coada de asteptare. Va rugam asteptati ca un administrator sa aprobe cererea de adaugare";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");

            }
            p.Categ = GetAllCategories();
            return View(p);

        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllCategories()
        {
            var c = new List<SelectListItem>();

            var query = db.Categories;
            foreach (Category cat in query)
            {
                c.Add(new SelectListItem { Value = cat.Id.ToString(), Text = cat.CategoryName });
            }

            return c;
        }
    }
}
