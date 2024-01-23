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
                                       .Include("ApplicationUser")
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
                            .Include("ApplicationUser")
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
            p.ApplicationUserId = _userManager.GetUserId(User);
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


                TempData["Message"] = "Bautura adaugata cu succes";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");

            }
            p.Categ = GetAllCategories();
            return View(p);

        }
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            var drink = db.Drinks
                         .Include("ApplicationUser")
                         .Include("Category")
                         .Include("Reviews")
                          .Include("Reviews.ApplicationUser")
                         .Where(c => c.Id == id).First();

            if (drink.Reviews != null && drink.Reviews.Any())
            {
                double averageRating = drink.Reviews.Average(r => r.Rating ?? 0); // Utilizarea operatorului coalescent pentru a trata valorile nullable
                drink.Rating = (int?)(double)averageRating;
            }


            else
            {
                drink.Rating = null; // Dacă nu există comentarii, setați Rating la null sau la o valoare implicită, în funcție de necesități
            }


            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }
            if (drink == null)
            {
                TempData["Message"] = "Bautura nu exista in Baza de Date";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");

            }
            GetButtonRights();
            if (User.IsInRole("Admin"))
            {
                ViewBag.CanToggleStatus = true;
                return View(drink);
            }



            return View(drink);
        }


        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show([FromForm] Review review)
        {
            review.Date = DateTime.Now;
            review.ApplicationUserId = _userManager.GetUserId(User);

            var existingReview = db.Reviews
      .FirstOrDefault(r => r.DrinkId == review.DrinkId &&
                           r.ApplicationUserId == _userManager.GetUserId(User));

            if (existingReview == null)
            {
                // Utilizatorul nu a mai scris o recenzie, permite adaugarea
                if (ModelState.IsValid)
                {
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    return Redirect("/Drinks/Show/" + review.DrinkId);
                }
            }

            else
            {
                // Utilizatorul a scris deja o recenzie, poate fi redirectionat la editare sau afisat un mesaj de eroare
                TempData["message"] = "Ati scris deja o recenzie pentru aceasta bautura.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Drinks");
            }

            Drink drink = db.Drinks.Include("Category")
                     .Include("ApplicationUser")
                     .Include("Reviews")
                       .Include("Reviews.ApplicationUser")
                     .Where(drink => drink.Id == review.DrinkId)
                     .FirstOrDefault();


            ViewBag.UserOrders = db.Orders
                                      .Where(b => b.UserId == _userManager.GetUserId(User))
                                      .ToList();

            GetButtonRights();

            return View(drink);

        }

        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var drink = db.Drinks.Include("Category").Where(db => db.Id == id).First();
            drink.Categ = GetAllCategories();
            if (User.IsInRole("Admin") || _userManager.GetUserId(User) == drink.ApplicationUserId)
            {
                return View(drink);
            }
            TempData["Message"] = "Nu ai dreptul la aceasta resursa";
            TempData["messageType"] = "alert-danger";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Edit(int id, Drink p, IFormFile? DrinkPhoto)
        {
            Drink query = db.Drinks.Find(id);

            if (User.IsInRole("Admin") || _userManager.GetUserId(User) == query.ApplicationUserId)
            {
                if (ModelState.IsValid)
                {

                    if (DrinkPhoto != null && DrinkPhoto.Length > 0)
                    {
                        string cacheBuster = DateTime.Now.Ticks.ToString();//probleme de caching cand folosesc firefox: poza nu isi da update in front-end cand ii dau updte in back-end trebui deci un cache buster care schimba numele fisierului
                        var entityPath = Path.Combine(
                            _env.WebRootPath,
                            "images",
                            query.Id.ToString()
                        );

                        var storagePath = Path.Combine(entityPath, cacheBuster + DrinkPhoto.FileName);

                        if (Directory.Exists(entityPath))
                        {
                            Directory.Delete(entityPath, true);
                        }
                        Directory.CreateDirectory(entityPath);


                        query.Photo = "/images/" + query.Id.ToString() + "/" + cacheBuster + DrinkPhoto.FileName;


                        using (var fileStream = new FileStream(storagePath, FileMode.Create))
                        {
                            await DrinkPhoto.CopyToAsync(fileStream);
                        }

                    }
                    query.CategoryId = p.CategoryId;
                    query.Name = p.Name;
                    query.Content = p.Content;
                    query.Price = p.Price;


                    db.SaveChanges();
                    TempData["Message"] = "Prous editat cu succes.";
                    TempData["messageType"] = "alert-success";
                    return RedirectToAction("Index");

                }
                p.Categ = GetAllCategories();
                return View(p);

            }

            TempData["Message"] = "Nu ai dreptul la aceasta resursa";
            TempData["messageType"] = "alert-danger";
            return RedirectToAction("Index");


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
        [NonAction]
        private void GetButtonRights()
        {
            ViewBag.IsAdmin = User.IsInRole("Admin");

            ViewBag.UserId = _userManager.GetUserId(User);
            ViewBag.IsEditor = User.IsInRole("Editor");

        }
        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Delete(int id)
        {
            try
            {

                var query = db.Drinks.Find(id);
                if (User.IsInRole("Admin") || query.ApplicationUserId == _userManager.GetUserId(User))
                {
                    db.Drinks.Remove(query);
                    db.SaveChanges();
                    TempData["Message"] = "Bautura stearsa cu succes";
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
        [HttpPost]
        public IActionResult AddDrink([FromForm] DrinkOrder drinkOrder)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Register");
            }

            var cart = db.Orders.Where(or => or.IsCart == true && or.UserId == _userManager.GetUserId(User));

            // creează coșul dacă nu există
            if (!cart.Any())
            {
                db.Orders.Add(new Order
                {
                    UserId = _userManager.GetUserId(User),
                    IsCart = true,

                });
                db.SaveChanges();

                cart = db.Orders.Where(or => or.IsCart == true && or.UserId == _userManager.GetUserId(User));
            }

            var orderId = cart.First().OrderId;

            Drink p = db.Drinks.Find(drinkOrder.DrinkId);

            if (ModelState.IsValid && drinkOrder.DrinkId > 0)
            {

                drinkOrder.OrderId = orderId;


                db.DrinkOrders.Add(drinkOrder);
                db.SaveChanges();

                TempData["message"] = "Bautura a fost adaugat in cosul dumneavostra";
                TempData["messageType"] = "alert-success";
            }
            else
            {
                TempData["message"] = "Date invalide pentru adaugarea bauturii in cos";
                TempData["messageType"] = "alert-danger";
            }

            return RedirectToAction("Show", new { id = drinkOrder.DrinkId });
        }
    }
}
