using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySchoolSystem.Models;
using MySchoolSystem.Models.ViewModels;

namespace MySchoolSystem.Controllers
{
    [Bind]
    public class UserAccountController : Controller
    {
        public RegisterViewModel registerViewModel1 { get; set; }
        private readonly MyAppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserAccountController(MyAppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Get: /Regiser
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //Post: /Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                //if requirements are valid, building new user
                IdentityUser newUser = new IdentityUser
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email
                };
                //creating the user from build
                //passing the password here to hash it.
                IdentityResult registered = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                if (registered.Succeeded)
                {
                    //sign the new user
                    //Identity manages the sessions
                    await _signInManager.SignInAsync(newUser, false);
                    return Redirect("/");
                }
                //if errors
                foreach (var error in registered.Errors)
                {
                    ModelState.AddModelError(error.Code.ToString(),error.Description);
                }
            }
            return View(registerViewModel);
        }

        //Post: /logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        //Get: /log in
        public IActionResult Login()
        {
            return View();
        }

        //Post: /login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false) ;

                if (result.Succeeded)
                {
                    return Redirect("/");
                }
                ModelState.AddModelError("Error", "Log in error");

            }
            return View(loginViewModel);
        }
    }
}