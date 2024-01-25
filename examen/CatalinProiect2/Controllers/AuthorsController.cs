using CatalinProiect2.Data;
using CatalinProiect2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalinProiect2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext db;

        public AuthorsController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.Alert = TempData["messageType"];
            }

            var authors = from author in db.Authors
                             orderby author.AuthorName
                             select author;

            ViewBag.Authors = authors;
            return View();
        }

        public IActionResult Show(int id)
        {
            Author author = db.Authors.Find(id);
            return View(author);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Author author)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);
                db.SaveChanges();
                TempData["message"] = "Autorul a fost adaugata";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }

            else
            {
                return View(author);
            }
        }

        public IActionResult Edit(int id)
        {
            Author author = db.Authors.Find(id);
            return View(author);
        }

        [HttpPost]
        public IActionResult Edit(int id, Author requestAuthor)
        {
            Author author = db.Authors.Find(id);

            if (ModelState.IsValid)
            {
                author.AuthorName = requestAuthor.AuthorName;
                db.SaveChanges();
                TempData["message"] = "Autorul a fost modificata!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                return View(requestAuthor);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
            TempData["message"] = "Autorul a fost sters";
            TempData["messageType"] = "alert-success";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
