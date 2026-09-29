using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Presentation.Models;

namespace HelpDesk.Presentation.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser model, IFormFile? profilePhoto)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Department = model.Department;
            user.PhoneNumber = model.PhoneNumber;

            // Upload Profile Image
            if (profilePhoto != null && profilePhoto.Length > 0)
            {
                // Validate file extension
                string extension = Path.GetExtension(profilePhoto.FileName).ToLower();

                string[] allowedExtensions =
                {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif"
    };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Only JPG, JPEG, PNG and GIF images are allowed.");

                    return View(model);
                }

                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/profileimages");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid() + extension;

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePhoto.CopyToAsync(stream);
                }

                user.ProfileImage = "/profileimages/" + fileName;
            }

            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(
                                user,
                                model.CurrentPassword,
                                model.NewPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Password changed successfully.";

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}