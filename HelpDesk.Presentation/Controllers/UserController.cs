using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Presentation.Models;

namespace HelpDesk.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Display all users
        public async Task<IActionResult> Index(string searchText)
        {
            var users = _userManager.Users.ToList();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                users = users.Where(x =>
                            (x.FullName != null &&
                             x.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                         ||
                            (x.Email != null &&
                             x.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                         .ToList();
            }

            var model = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            ViewBag.SearchText = searchText;

            return View(model);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "No Role"
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            ViewBag.UserName = user.FullName;
            ViewBag.UserId = user.Id;
            ViewBag.CurrentRole = currentRole;

            ViewBag.Roles = new List<string>
    {
        "Admin",
        "Support Engineer",
        "Employee"
    };

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var addResult = await _userManager.AddToRoleAsync(user, role);

            if (!removeResult.Succeeded)
            {
                return Content("Remove Role Failed");
            }

            if (!addResult.Succeeded)
            {
                return Content(string.Join(", ", addResult.Errors.Select(x => x.Description)));
            }

            return RedirectToAction(nameof(Index));
        }

    }
}