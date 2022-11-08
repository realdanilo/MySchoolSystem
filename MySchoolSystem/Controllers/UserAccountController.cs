using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchoolSystem.Models;
using MySchoolSystem.Models.ViewModels;

namespace MySchoolSystem.Controllers
{
    [Bind]
    public class UserAccountController : Controller
    {
        public RegisterViewModel registerViewModel { get; set; }
        private readonly MyAppDbContext _context;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserAccountController(MyAppDbContext context, UserManager<CustomIdentityUser> userManager, SignInManager<CustomIdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
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
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, bool? KeepOriginal)
        {
            if (ModelState.IsValid)
            {
                //if requirements are valid, building new user
                CustomIdentityUser newUser = new CustomIdentityUser
                {
                    //********** IMPORTANT
                    // **** PasswordSignInAsync takes username, however it will be hashed with email******
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Country = registerViewModel.Country
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
                    //KeepOriginal, admin makes a new account
                    if (KeepOriginal == true) return Redirect("/Role");

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
            //TempData["Alert"]= true;
            //TempData["AlertMessage"] = $"Access Denied from {ReturnUrl}";
            TempData["Alert"] = true;
            TempData["AlertMessage"] = $"Access Denied from {ReturnUrl}";
            return Redirect("/");
        }

        //Get: /Profile
        //Summary: Allows to edit Profile
        //Needs to check if is admin or is the User
        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            CustomIdentityUser user = await _userManager.FindByIdAsync(id);
            if (String.IsNullOrEmpty(id) || user == null) return NotFound();

            return View(user);
        }

        //Post: /Profile
        //Needs to check if is admin or is the User
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile([Bind("Id,FirstName,LastName,PhoneNumber,Email,Country")] CustomIdentityUser customIdentityUser)
        {

            CustomIdentityUser user = await _userManager.FindByIdAsync(customIdentityUser.Id);
            CustomIdentityUser checkUser = await _userManager.GetUserAsync(User);

            if (user == null || (checkUser.Id != user.Id)) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(instructor);
                    //CustomIdentityUser user = await _userManager.FindByIdAsync(instructor.Id);
                    user.FirstName = customIdentityUser.FirstName;
                    user.LastName = customIdentityUser.LastName;
                    user.Country = customIdentityUser.Country;
                    //user.Email = customIdentityUser.Email;
                    user.PhoneNumber = customIdentityUser.PhoneNumber;
                    await _userManager.UpdateAsync(user);

                    //**** check how to update password ****
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new Exception(e.Message);
                }
            }
            //return RedirectToAction(nameof(Profile));
            return Redirect("/");
         }

        //Get: ResetPassword
        [Authorize]
        public IActionResult ResetPassword(string userId)
        {
            //var admin = 
            ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel()
            {
                UserId = userId,
                IsAdmin = User.IsInRole("Admin"),
            };
            return View(resetPasswordViewModel);
        }

        //Post:/ResetPassword
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                IdentityResult res;
                //check who is making the call
                //only admin can Reset password
                CustomIdentityUser user = await _userManager.GetUserAsync(User);
                if (await _userManager.IsInRoleAsync(user, "Admin") && resetPasswordViewModel.IsAdmin)
                {
                    //can change password without current password
                    CustomIdentityUser userToChangePassword = await _userManager.FindByIdAsync(resetPasswordViewModel.UserId);
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userToChangePassword);
                    res = await _userManager.ResetPasswordAsync(userToChangePassword,resetToken, resetPasswordViewModel.NewPassword);
                }
                else
                {
                    //has to change password using current password
                     res = await _userManager.ChangePasswordAsync(user, resetPasswordViewModel.CurrentPassword, resetPasswordViewModel.NewPassword);
                }

                //setting responses
                if (res.Succeeded)
                {
                    TempData["Alert"] = true;
                    TempData["AlertMessage"] = "Successful, password has been updated";
                    return Redirect("/");
                }
                else
                {
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    //return View();
                }
            }
            return View();
        }


        //Get: /Details
        public async Task<IActionResult> Details(string id)
        {
            CustomIdentityUser user = await _userManager.FindByIdAsync(id);
            if (String.IsNullOrEmpty(id) || user == null) return NotFound();
            ViewBag.Roles = await _userManager.GetRolesAsync(user);
            return View(user);
        }
    }
}