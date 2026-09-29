using HelpDesk.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HelpDesk.Presentation.Controllers
{

    [Authorize(Roles = "Admin")]
    public class PriorityController : Controller
    {
        private readonly IPriorityService _priorityService;

        public PriorityController(IPriorityService priorityService)
        {
            _priorityService = priorityService;
        }

        // Display all Priorities
        public async Task<IActionResult> Index(string searchText)
        {
            IEnumerable<HelpDesk.DAL.Models.Priority> priorities;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                priorities = await _priorityService.SearchPrioritiesAsync(searchText);
            }
            else
            {
                priorities = await _priorityService.GetAllPrioritiesAsync();
            }

            ViewBag.SearchText = searchText;

            return View(priorities);
        }
        // Display Create Page
        public IActionResult Create()
        {
            return View();
        }
        // Save Priority
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HelpDesk.DAL.Models.Priority priority)
        {
            if (ModelState.IsValid)
            {
                await _priorityService.AddPriorityAsync(priority);

                return RedirectToAction(nameof(Index));
            }

            return View(priority);
        }
        // Display Edit Page
        public async Task<IActionResult> Edit(int id)
        {
            var priority = await _priorityService.GetPriorityByIdAsync(id);

            if (priority == null)
            {
                return NotFound();
            }

            return View(priority);
        }
        // Update Priority
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HelpDesk.DAL.Models.Priority priority)
        {
            if (ModelState.IsValid)
            {
                await _priorityService.UpdatePriorityAsync(priority);

                return RedirectToAction(nameof(Index));
            }

            return View(priority);
        }
        // Display Delete Confirmation
        public async Task<IActionResult> Delete(int id)
        {
            var priority = await _priorityService.GetPriorityByIdAsync(id);

            if (priority == null)
            {
                return NotFound();
            }

            return View(priority);
        }
        // Delete Priority
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int PriorityId)
        {
            await _priorityService.DeletePriorityAsync(PriorityId);

            return RedirectToAction(nameof(Index));
        }
    }
}