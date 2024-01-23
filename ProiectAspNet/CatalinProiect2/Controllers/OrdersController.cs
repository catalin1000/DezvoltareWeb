using CatalinProiect2.Data;
using CatalinProiect2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalinProiect2.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public OrdersController(
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

            SetAccessRights();


            if (User.IsInRole("User") || User.IsInRole("Editor"))
            {
                var orders = from order in db.Orders.Include("User")
                               .Where(b => b.UserId == _userManager.GetUserId(User))
                               .Where(b => b.IsCart == false)
                             select order;

                ViewBag.Orders = orders;

                return View();
            }
            else
            {
                var orders = from order in db.Orders.Include("User")
                               .Where(b => b.IsCart == false)
                             select order;

                ViewBag.Orders = orders;

                return View();
            }
        }


        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult MyCart()
        {
            var cart = db.Orders
                      .Where(or => or.IsCart == true && or.UserId == _userManager.GetUserId(User))
                     .FirstOrDefault();


            if (cart == null)
            {
                cart = new Order
                {
                    UserId = _userManager.GetUserId(User),
                    IsCart = true,

                };
                db.Orders.Add(cart);
                db.SaveChanges();
            }

            var query = db.DrinkOrders.Where(b => b.Order.IsCart == true);
            foreach (var order in query)
            {
                db.Remove(order);//stergem produsele dezactivate care sunt in cos
            }

            db.SaveChanges();

            var userCart = db.Orders
                    .Include(o => o.DrinkOrders)
                   .ThenInclude(po => po.Drink)
                   .ThenInclude(b => b.Category)
                   .Include(b => b.DrinkOrders)
                   .ThenInclude(b => b.Drink)
                   .ThenInclude(b => b.ApplicationUser)
                   .Where(or => or.IsCart == true && or.UserId == _userManager.GetUserId(User))
                   .FirstOrDefault();

            return View(userCart);
        }


        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public IActionResult DeleteDrink(DrinkOrder p, [FromForm] string returnUrl)
        {
            DrinkOrder? p2 = db.DrinkOrders.Include(o => o.Order)
                               .Where(o => o.DrinkId == p.DrinkId && o.Id == p.Id && o.OrderId == p.OrderId).First();
            if (p2 == null)
            {
                TempData["message"] = "nu exista bautura aceasta";
                TempData["messageType"] = "alert-danger";
                return Redirect("/Drinks/Index");
            }

            var user = p2.Order.UserId;
            if (user != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
            {//dc nu e al lui si nu e admin nu poate  sterge

                TempData["message"] = "nu aveti dreptul la resursa asta";
                TempData["messageType"] = "alert-danger";
                return Redirect("/Drinks/Index");

            }

            if (!User.IsInRole("Admin") && p2.Order.IsCart == false) // dc este user nu are dreptul sa isi strearge retrospectiv produsele din comanda; poate doar din cos
            {
                TempData["message"] = "nu aveti dreptul la resursa asta";
                TempData["messageType"] = "alert-danger";
                return Redirect("/Drinks/Index");
            }

            db.DrinkOrders.Remove(p2);
            db.SaveChanges();
            return Redirect(returnUrl);
        }

        public IActionResult Order()
        {
            var todelete = db.DrinkOrders.Where(b =>  b.Order.IsCart == true);
            foreach (var order in todelete)
            {
                db.Remove(order);//stergem produsele dezactivate care sunt in cosul utilizatorului
            }
            db.SaveChanges();


            //cartul devine ordin acuma
            var query = db.Orders.Where(o => o.UserId == _userManager.GetUserId(User) && o.IsCart == true)
                .Include(o => o.DrinkOrders)
                .ThenInclude(po => po.Drink)
                .First();

            float sum = 0;
            foreach (DrinkOrder po in query.DrinkOrders)
            {
                sum += po.Drink.Price ?? 0;
            }

            query.Price = sum;
            query.Date = DateTime.Now;
            query.IsCart = false;
            db.SaveChanges();
            TempData["Message"] = "Bauturi cumparate";
            TempData["messageType"] = "alert-success";
            return Redirect("/Drinks/Index");

        }

        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            SetAccessRights();

            if (User.IsInRole("User") || User.IsInRole("Editor"))
            {
                var orders = db.Orders
                                  .Include("DrinkOrders.Drink.Category")
                                  .Include("DrinkOrders.Drink.ApplicationUser")

                                  .Where(b => b.OrderId == id)
                                  .Where(b => b.UserId == _userManager.GetUserId(User))
                                  .FirstOrDefault();

                if (orders == null)
                {
                    TempData["message"] = "Nu aveti acces";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Drinks");
                }
                ViewBag.PoateModifica = false; //poate modifica ddca este admin View-ul asociat este doar pentru comenzi "comandate"
                return View(orders);
            }

            else
            {
                var orders = db.Orders
                                  .Include("DrinkOrders.Drink.Category")
                                  .Include("DrinkOrders.Drink.ApplicationUser")
                                  .Include("User")
                                  .Where(b => b.OrderId == id)
                                  .FirstOrDefault();


                if (orders == null)
                {
                    TempData["message"] = "Resursa cautata nu poate fi gasita";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Products");
                }

                ViewBag.PoateModifica = true;
                return View(orders);
            }

        }


        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Editor") || User.IsInRole("User"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.EsteAdmin = User.IsInRole("Admin");
        }
    }
}
