using HelpDesk.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HelpDesk.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService; //Instead of creating
                                                                //DepartmentService service = new DepartmentService();
                                                               //ASP.NET Core automatically creates the object because we registered it in Program.cs.


        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // Display all departments
        public async Task<IActionResult> Index(string searchText)
        {
            IEnumerable<HelpDesk.DAL.Models.Department> departments;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                departments = await _departmentService.SearchDepartmentsAsync(searchText);
            }
            else
            {
                departments = await _departmentService.GetAllDepartmentsAsync();
            }

            ViewBag.SearchText = searchText;

            return View(departments);
        }

        // Display Create Page
        public IActionResult Create()
        {
            return View();
        }

        // Save Department
        [HttpPost] 
        [ValidateAntiForgeryToken]    //When the user clicks:Save(The browser sends the form data to this POST action.)
        public async Task<IActionResult> Create(HelpDesk.DAL.Models.Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.AddDepartmentAsync(department); //If everything is valid:The department is inserted into the database.

                return RedirectToAction(nameof(Index)); //takes the user back to the department list.
            }

            return View(department);
        }

        // Display Edit Page
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // Update Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HelpDesk.DAL.Models.Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.UpdateDepartmentAsync(department);

                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // Display Delete Confirmation
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // Delete Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int DepartmentId)
        {
            await _departmentService.DeleteDepartmentAsync(DepartmentId);

            return RedirectToAction(nameof(Index));
        }
    }
}