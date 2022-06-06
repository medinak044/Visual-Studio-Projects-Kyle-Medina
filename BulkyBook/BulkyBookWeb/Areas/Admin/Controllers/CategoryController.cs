using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.Interfaces;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        // GET
        public IActionResult Create()
        {
            //// For convenience, prefill input with the next available display order value
            //IEnumerable<Category> objCategoryList = _context.Categories.ToList();
            //int nextNumber = 0; // Represents the highest display order value
            //foreach (var obj in objCategoryList)
            //{
            //    if (obj.DisplayOrder > nextNumber)
            //    { nextNumber = obj.DisplayOrder; }
            //}


            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent unauthorized sources from using the site to create data
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            if(!ModelState.IsValid) return View(); // Checks if data object has all the required values

            _unitOfWork.Category.Add(obj); // Add
            _unitOfWork.Save(); // Pushes changes to the database
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            //var categoryFromDb = _context.Categories.Find(id);
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDb == null) return NotFound();

            return View(categoryFromDb);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent unauthorized sources from using the site to create data
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            if (!ModelState.IsValid) return View(); // Checks if data object has all the required values

            _unitOfWork.Category.Update(obj); // Update
            _unitOfWork.Save(); // Pushes changes to the database
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            //var categoryFromDb = _context.Categories.Find(id);
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);

            if (categoryFromDb == null) return NotFound();

            return View(categoryFromDb);
        }

        // POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken] // Prevent unauthorized sources from using the site to create data
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);

            if (obj == null) return NotFound();

            _unitOfWork.Category.Remove(obj); // Delete
            _unitOfWork.Save(); // Pushes changes to the database
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
