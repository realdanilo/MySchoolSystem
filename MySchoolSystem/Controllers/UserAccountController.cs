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
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserAccountController(MyAppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        //Get: /Regiser
        [HttpGet]
        public IActionResult Register()
        {
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            RegisterViewModel registerViewModel = new RegisterViewModel(roles);
            return View(registerViewModel);
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
                    //UserName = registerViewModel.FirstName+"_"+ registerViewModel.LastName,
                    //********** IMPORTANT ==> add derived class from IdentityUser to take FirstName, LastName
                    // **** SignInManagerAsync takes Username only, not email ******
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email
                };
                //creating the user from build
                //passing the password here to hash it.
                IdentityResult userRegistration = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                //by default, everyone who registers has student level access
                //can change "Student to an env variable
                IdentityRole selectedRole = _roleManager.Roles.ToList().Where(r => r.Id == registerViewModel.RoleId).SingleOrDefault();
                IdentityResult roleRegistration = await _userManager.AddToRoleAsync(newUser, selectedRole.Name);

                if (userRegistration.Succeeded && roleRegistration.Succeeded)
                {
                    //sign the new user
                    //Identity manages the sessions
                    await _signInManager.SignInAsync(newUser, false);
                    return Redirect("/");
                }
                //if errors for both: userRegistration ,  roleRegistration
                foreach (var error in userRegistration.Errors)
                {
                    ModelState.AddModelError(error.Code.ToString(),error.Description);
                }
                foreach (var error in roleRegistration.Errors)
                {
                    ModelState.AddModelError(error.Code.ToString(), error.Description);
                }
            }

            // ***** fix this *****
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            RegisterViewModel registerViewModelWithSelectors = new RegisterViewModel(roles);
            registerViewModel.Roles = registerViewModelWithSelectors.Roles;
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
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false) ;

                if (result.Succeeded)
                {
                    if (ReturnUrl == null) return Redirect("/");
                    return LocalRedirect(ReturnUrl);
                }
                ModelState.AddModelError("Error", "Log in error");

            }
            return View(loginViewModel);
        }

        //Get: /AccessDenied?ReturnUrl
        public IActionResult AccessDenied(string ReturnUrl)
        {
            //log return url events
            Console.WriteLine(ReturnUrl);
            //To be fixed...
            TempData["Alert"]= true;
            TempData["AlertMessage"] = $"Access Denied from {ReturnUrl}";
            return Redirect("/");
        }
    }
}