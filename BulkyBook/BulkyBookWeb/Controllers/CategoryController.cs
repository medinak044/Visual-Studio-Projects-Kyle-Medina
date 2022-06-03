using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _context.Categories.ToList();
            return View(objCategoryList);
        }

        // GET
        public IActionResult Create()
        {
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

            _context.Categories.Add(obj); // Add
            _context.SaveChanges(); // Pushes changes to the database
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var categoryFromDb = _context.Categories.Find(id);
            //var categoryFromDb = _context.Categories.Where(c => c.Id == id).FirstOrDefault();

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

            _context.Categories.Update(obj); // Update
            _context.SaveChanges(); // Pushes changes to the database
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var categoryFromDb = _context.Categories.Find(id);
            //var categoryFromDb = _context.Categories.Where(c => c.Id == id).FirstOrDefault();

            if (categoryFromDb == null) return NotFound();

            return View(categoryFromDb);
        }

        // DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken] // Prevent unauthorized sources from using the site to create data
        public IActionResult DeletePOST(int? id)
        {
            var obj = _context.Categories.Find(id);

            if (obj == null) return NotFound();

            _context.Categories.Remove(obj); // Delete
            _context.SaveChanges(); // Pushes changes to the database
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
