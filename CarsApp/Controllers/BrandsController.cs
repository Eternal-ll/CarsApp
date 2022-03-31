using CarsApp.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarsApp.Controllers
{
    public class BrandsController : Controller
    {

        private readonly CarsAppContext Context;

        public BrandsController(CarsAppContext context)
        {
            Context = context;
        }

        public IActionResult Index()
        {
            var brands = Context.Brands
                .AsEnumerable();

            return View(brands);
        }

        public IActionResult Details(int id)
        {
            var brand = Context.Brands
                .Include(b=>b.Models)
                .FirstOrDefault(b => b.Id == id);
            if (brand == null)
            {
                return RedirectToAction("index");
            }

            return View(brand);
        }
        public IActionResult Create() => View();
        [HttpPost]
        public IActionResult Create(Brand model)
        {
            if (ModelState.IsValid)
            {
                if (Context.Brands.Any(b => b.Name.ToLower() == model.Name.ToLower()))
                {
                    ModelState.AddModelError("", "Марка с таким названием уже существует");
                    return View(model);
                }
                var ent = Context.Brands.Add(model);
                Context.SaveChanges();
                return RedirectToAction("details", ent.Entity.Id);
            }
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var brand = Context.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
            {
                return RedirectToAction("index");
            }
            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand model)
        {
            if (ModelState.IsValid)
            {
                if (Context.Brands.Any(b => b.Name.ToLower() == model.Name.ToLower() && b.Id != model.Id))
                {
                    ModelState.AddModelError("", "Марка с таким названием уже существует");
                    return View(model);
                }
                Context.Brands.Update(model);
                Context.SaveChanges();
                return RedirectToAction("details", model.Id);
            }
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            var brand = Context.Brands
                .Include(b=>b.Models)
                .FirstOrDefault(b => b.Id == id);
            if (brand == null)
            {
                return RedirectToAction("index");
            }
            return View(brand);
        }
            
        [HttpPost]
        public IActionResult DeleteBrand(int id)
        {
            var brand = Context.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
            {
                return RedirectToAction("index");
            }
            Context.Brands.Remove(brand);
            Context.SaveChanges();
            return RedirectToAction("index");
        }

        [HttpPost]
        public IActionResult Filter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return View("List", Context.Brands.AsEnumerable());
            }
            var brands = Context.Brands
                .Where(m => m.Name.ToLower().Contains(filter.ToLower()));
            return View("List", brands);
        }
    }
}
