using HelpDesk.DAL.Models;
using HelpDesk.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;  //UserManager is used to create users,Find users,Change passwords,Assign roles
            _signInManager = signInManager; //SignInManager is used to Login,Logout,Check Passwords
        }
        // GET: Register
        public IActionResult Register()
        {
            return View();
        }
        

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = new ApplicationUser //Creates a new user object.
            {
                FullName = model.FullName, //Copies the Full Name from the form.
                UserName = model.Email, //Uses the email as the username.
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password); //Creates the user and securely stores the password (hashed) in the Identity tables.

                if (result.Succeeded) //If registration is successful
                {
                    // Assign the selected role
                    //  await _userManager.AddToRoleAsync(user, model.Role);
                    await _userManager.AddToRoleAsync(user, "Employee");

                    return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }
        // GET: Login
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, 
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false); //Checks whether:email exists,password is correct ,Creates the authentication cookie if successful.

                if (result.Succeeded) //If successful:The user is redirected to the Dashboard.
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        user.LastLogin = DateTime.Now;

                        await _userManager.UpdateAsync(user);
                    }

                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError("", "Invalid Email or Password."); //If the credentials are incorrect:The login page displays an error message.
            }

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

       
        // GET: AccessDenied
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}