using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Services.IServices;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IInternal _internal;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IInternal db, IWebHostEnvironment webHostEnvironment)
        {
            _internal = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> ProductList = _internal.ProductRepository.GetAll(includeProperties:"Category").ToList();
            return View(ProductList);
        }

        public IActionResult Upsert(int? id) {
       
            // ViewBag.Categories = Categories;
           // ViewData["Categories"] = Categories;

            ProductVM productVM = new ProductVM();

            productVM.Product = new Product();
            
            productVM.CategoryList = _internal.CategoryRepository.GetAll().Select((u) => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            if (id == null || id == 0) {
                //create
                return View(productVM);
            } 
            else
            {
                //update
                productVM.Product = _internal.ProductRepository.Get((u) => u.Id == id );
                return View(productVM);
            }

            
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? formFile)
        {

            if (ModelState.IsValid) {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(formFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if( !string.IsNullOrEmpty(productVM.Product.ImageUrl) )
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if( System.IO.File.Exists(oldImagePath) )
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create )  )
                    {
                        formFile.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;

                }
                if(productVM.Product.Id == 0 )
                {
                    _internal.ProductRepository.Add(productVM.Product);
                } 
                else
                {
                    _internal.ProductRepository.Update(productVM.Product);
                }

                 _internal.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            } else
            {
              

                productVM.CategoryList = _internal.CategoryRepository.GetAll().Select((u) => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });
                return View(productVM);

            }
               
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
