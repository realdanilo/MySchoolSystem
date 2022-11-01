using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySchoolSystem.Models;
using MySchoolSystem.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MySchoolSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly MyAppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public RoleController(MyAppDbContext context,RoleManager<IdentityRole> roleManager, UserManager<CustomIdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        //Get: /
        public IActionResult Index()
        {
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            List<CustomIdentityUser> users = _userManager.Users.ToList();
            ViewBag.users = users;
            return View(roles);
        }

        // GET: /CreateRole
        public IActionResult CreateRole()
        {
            return View();
        }

        // Post: /CreateRole
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                //build role
                IdentityRole newRole = new IdentityRole { Name = roleViewModel.RoleName };
                //create role
                IdentityResult result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return Redirect("/role");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(roleViewModel);
        }

        //Get: /edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            RoleViewModel roleViewModel = new RoleViewModel() { Id = role.Id, RoleName = role.Name };
            return View(roleViewModel); 
        }

        //Post: /edit
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole roleUpdate = await _roleManager.FindByIdAsync(roleViewModel.Id);
                if (roleUpdate == null)
                {
                    return NotFound();
                }
               
                roleUpdate.Name = roleViewModel.RoleName;
                IdentityResult result = await _roleManager.UpdateAsync(roleUpdate);
                if (result.Succeeded)
                {
                    return Redirect("/role");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(roleViewModel);
        }

        //Get: /EditAccounts
        public async Task<IActionResult> EditAccounts(string roleId)
        {
            if (roleId == null) return NotFound();

            IdentityRole role = await _roleManager.FindByIdAsync(roleId);

            if (role == null) return NotFound();

            //for view
            ICollection<UserRoleViewModel> userRoleViewModel = new List<UserRoleViewModel>();

            //users
            ICollection<CustomIdentityUser> users = _userManager.Users.ToList();

            //build view_users
            foreach (var user in users)
            {
                var userRole = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                //from asembly: IsInRoleAsync requires role.Name, not roleId
                bool userIsInRole = await _userManager.IsInRoleAsync(user,role.Name);
                if (userIsInRole)
                {
                    userRole.IsSelected = true;
                    //default is false
                }
                userRoleViewModel.Add(userRole);
            }

            ViewBag.RoleName = role.Name;
            ViewBag.RoleId = role.Id;
            return View(userRoleViewModel);
        }

        //Post: /EditAccounts/roleId
        [HttpPost]
        public async Task<IActionResult> EditAccounts(string roleId, List<UserRoleViewModel> listUserRole)
        {
            if (roleId == null) return NotFound();

            IdentityRole role = await _roleManager.FindByIdAsync(roleId);

            if (role == null) return NotFound();

            foreach (var account in listUserRole)
            {
                CustomIdentityUser user = await _userManager.FindByIdAsync(account.UserId);

                //if accounts is selected and is not in role table, add to table
                if (account.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                //account is not selected, but is in role table, then update
                else if ((!account.IsSelected) && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

            }

            return RedirectToAction("Index");
        }

    }

}
