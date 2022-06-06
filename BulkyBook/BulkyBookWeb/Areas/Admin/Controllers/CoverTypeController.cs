using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.Interfaces;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent unauthorized sources from using the site to create data
        public IActionResult Create(CoverType obj)
        {
            if(!ModelState.IsValid) return View(); // Checks if data object has all the required values

            _unitOfWork.CoverType.Add(obj); // Add
            _unitOfWork.Save(); // Pushes changes to the database
            TempData["success"] = "CoverType created successfully";
            return RedirectToAction("Index");
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (coverTypeFromDb == null) return NotFound();

            return View(coverTypeFromDb);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent unauthorized sources from using the site to create data
        public IActionResult Edit(CoverType obj)
        {
            if (!ModelState.IsValid) return View(); // Checks if data object has all the required values

            _unitOfWork.CoverType.Update(obj); // Update
            _unitOfWork.Save(); // Pushes changes to the database
            TempData["success"] = "CoverType updated successfully";
            return RedirectToAction("Index");
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (coverTypeFromDb == null) return NotFound();

            return View(coverTypeFromDb);
        }

        // POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken] // Prevent unauthorized sources from using the site to create data
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (obj == null) return NotFound();

            _unitOfWork.CoverType.Remove(obj); // Delete
            _unitOfWork.Save(); // Pushes changes to the database
            TempData["success"] = "CoverType deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
