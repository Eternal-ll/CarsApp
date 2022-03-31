using CarsApp.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CarsApp.Controllers
{
    public class ModelsController : Controller
    {
        private readonly CarsAppContext Context;

        public ModelsController(CarsAppContext context)
        {
            Context = context;
        }

        public IActionResult Index()
        {
            var models = Context.Models
                .Include(m => m.Brand)
                .AsEnumerable();

            return View(models);
        }

        public IActionResult Delete(int id)
        {
            var model = Context.Models
                .Include(m => m.Brand)
                .FirstOrDefault(m => m.Id == id);
            if (model is null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteModel(int id)
        {
            var model = Context.Models.FirstOrDefault(m => m.Id == id);
            if (model is null)
            {
                return NotFound();
            }
            Context.Models.Remove(model);
            Context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var model = Context.Models
                .Include(m => m.Brand)
                .FirstOrDefault(m => m.Id == id);
            if (model is null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Model model)
        {
            if (ModelState.IsValid)
            {
                if (Context.Models.Any(m => m.Name.ToLower() == model.Name.ToLower() && m.Id != model.Id))
                {
                    ModelState.AddModelError("", "Данная модель уже зарегистрирована");
                    return View(model);
                }

                Context.Models.Update(model);
                Context.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = Context.Models
                .Include(m => m.Brand)
                .FirstOrDefault(m => m.Id == id);
            if (model is null)
            {
                return RedirectToAction("index");
            }
            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Model model)
        {
            if (ModelState.IsValid)
            {
                if (Context.Models.Any(m => m.Name.ToLower() == model.Name.ToLower()))
                {
                    ModelState.AddModelError("", "Данная модель уже зарегистрирована");
                    return View(model);
                }
                var ent = Context.Models.Add(model);
                Context.SaveChanges();
                return RedirectToAction("details", ent.Entity.Id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Filter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return View("List", Context.Models.Include(m => m.Brand).AsEnumerable());
            }
            var models = Context.Models
                .Include(m => m.Brand)
                .Where(m => m.Name.ToLower().Contains(filter.ToLower()) || m.Brand.Name.ToLower().Contains(filter.ToLower()));
            return View("List", models);
        }
    }
}
