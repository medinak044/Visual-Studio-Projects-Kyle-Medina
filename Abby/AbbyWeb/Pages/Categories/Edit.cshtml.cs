using AbbyWeb.Model;
using AbbyWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Category Category { get; set; }

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int id)
        {
            Category = _context.Category.Find(id);
        }

        public async Task<IActionResult> OnPost()
        {
            if (Category.Name == Category.DisplayOrder.ToString())
            {
                ModelState.AddModelError(Category.Name, "The DisplayOrder cannot exactly match the Name.");
            }

            #region Duplicate Display Order validation
            //HashSet<int> displaytOrderIdSet = new HashSet<int>();
            //var categoryFromDb = _context.Category.Find(Category.Id);
            //var queryAll = _context.Category.ToList();
            //foreach (var obj in queryAll) {
            //    if (Category.DisplayOrder == categoryFromDb.DisplayOrder) continue;
            //    displaytOrderIdSet.Add(obj.DisplayOrder); 
            //}

            //if (displaytOrderIdSet.Contains(Category.DisplayOrder))
            //{
            //    ModelState.AddModelError(Category.DisplayOrder.ToString(), "The DisplayOrder cannot match an existing value.");
            //}

            #endregion

            if (!ModelState.IsValid) return Page();

            _context.Category.Update(Category); // Prepares changes
            await _context.SaveChangesAsync(); // Saves changes to db
            TempData["success"] = "Category created successfully";
            return RedirectToPage("Index");
        }
    }
}
