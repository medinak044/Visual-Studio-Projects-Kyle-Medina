using AbbyWeb.Model;
using AbbyWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int id)
        {
            Category = _context.Category.Find(id);
        }

        public async Task<IActionResult> OnPost()
        {
            var categoryFromDb = _context.Category.Find(Category.Id);
            if (categoryFromDb == null) return Page();

            _context.Category.Remove(categoryFromDb); // Prepares changes
            await _context.SaveChangesAsync(); // Saves changes to db
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }
    }
}
