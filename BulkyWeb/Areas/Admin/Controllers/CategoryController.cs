using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Services.IServices;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IInternal _internal;
        public CategoryController(IInternal db)
        {
            _internal = db;
        }
        public IActionResult Index()
        {
            List<Category> categoryList = _internal.CategoryRepository.GetAll().ToList();
            return View(categoryList);
        }

        public IActionResult Create() { 
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid) {

                _internal.CategoryRepository.Add(obj);
                _internal.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0 ) {
                return NotFound();
            }
            Category? categoryFromDb = _internal.CategoryRepository.Get((u) => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFromDb == null) { 
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
           
            if (ModelState.IsValid)
            {

                _internal.CategoryRepository.Update(obj);
                _internal.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _internal.CategoryRepository.Get((u) => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

            Category? categoryFromDb = _internal.CategoryRepository.Get((u) => u.Id == id);
            if (categoryFromDb == null) { 
                return NotFound();
            }
            _internal.CategoryRepository.Remove(categoryFromDb);
            _internal.Save();

              TempData["success"] = "Category deleted successfully";

            return RedirectToAction("Index", "Category");
        }
    }
}
