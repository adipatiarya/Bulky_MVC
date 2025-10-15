using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Services.IServices;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IInternal _internal;
        public ProductController(IInternal db)
        {
            _internal = db;
        }
        public IActionResult Index()
        {
            List<Product> ProductList = _internal.ProductRepository.GetAll().ToList();
            return View(ProductList);
        }

        public IActionResult Create() {
            IEnumerable<SelectListItem> Categories = _internal.CategoryRepository.GetAll().Select((u) => new SelectListItem { 
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            ViewBag.Categories = Categories;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {

            if (ModelState.IsValid) {

                _internal.ProductRepository.Add(obj);
                _internal.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0 ) {
                return NotFound();
            }
            Product? ProductFromDb = _internal.ProductRepository.Get((u) => u.Id == id);
            //Product? ProductFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Product? ProductFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (ProductFromDb == null) { 
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
           
            if (ModelState.IsValid)
            {

                _internal.ProductRepository.Update(obj);
                _internal.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? ProductFromDb = _internal.ProductRepository.Get((u) => u.Id == id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

            Product? ProductFromDb = _internal.ProductRepository.Get((u) => u.Id == id);
            if (ProductFromDb == null) { 
                return NotFound();
            }
            _internal.ProductRepository.Remove(ProductFromDb);
            _internal.Save();

              TempData["success"] = "Product deleted successfully";

            return RedirectToAction("Index", "Product");
        }
    }
}
