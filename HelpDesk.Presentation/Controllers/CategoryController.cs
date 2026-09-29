using HelpDesk.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HelpDesk.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Display all Categories
        public async Task<IActionResult> Index(string searchText)
        {
            IEnumerable<HelpDesk.DAL.Models.Category> categories;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                categories = await _categoryService.SearchCategoriesAsync(searchText);
            }
            else
            {
                categories = await _categoryService.GetAllCategoriesAsync();
            }

            ViewBag.SearchText = searchText;

            return View(categories);
        }
        // Display Create Page
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Save Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HelpDesk.DAL.Models.Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddCategoryAsync(category);

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // Display Edit Page
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        // Update Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HelpDesk.DAL.Models.Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(category);

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // Display Delete Confirmation
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        // Delete Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CategoryId)
        {
            await _categoryService.DeleteCategoryAsync(CategoryId);

            return RedirectToAction(nameof(Index));
        }
    }
}