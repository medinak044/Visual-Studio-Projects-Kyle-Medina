using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.Interfaces;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }

        // GET
        public IActionResult Upsert(int? id)
        {
            // Query partial data and stuff them all into one custom object
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                    // Hand pick properties to display to page
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
            };

            if (id == null || id == 0)
            {
                // Create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                // Update product
            }

            return View(productVM);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent unauthorized sources from using the site to create data
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (!ModelState.IsValid) return View(obj);

            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images/products");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }

            _unitOfWork.Product.Add(obj.Product); // Update
            _unitOfWork.Save(); // Pushes changes to the database
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index");
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);

            if (productFromDb == null) return NotFound();

            return View(productFromDb);
        }

        // POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken] // Prevent unauthorized sources from using the site to create data
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);

            if (obj == null) return NotFound();

            _unitOfWork.Product.Remove(obj); // Delete
            _unitOfWork.Save(); // Pushes changes to the database
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
