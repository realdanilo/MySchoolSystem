using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchoolSystem.Models;

namespace MySchoolSystem.Controllers
{
    [Authorize(Roles = "Instructor, Admin")]
    public class InstructorController : Controller
    {
        private readonly MyAppDbContext _context;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public InstructorController(MyAppDbContext context, UserManager<CustomIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Instructor
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.GetUsersInRoleAsync("Instructor"));
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomIdentityUser instructor = await _userManager.FindByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructor/Create
        // Redirects to /UserAccount/Register
        public IActionResult Create()
        {
            return Redirect("/UserAccount/Register?KeepOriginal=True");
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,FirstName,LastName")] Instructor instructor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(instructor);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(instructor);
        //}


        // **** MOVE TO /PROFILE controller ***** =============
        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomIdentityUser instructor = await _userManager.FindByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,PhoneNumber,Email,Country")] CustomIdentityUser instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(instructor);
                    CustomIdentityUser user =  await _userManager.FindByIdAsync(instructor.Id);
                    user.FirstName = instructor.FirstName;
                    user.LastName = instructor.LastName;
                    user.Country = instructor.Country;
                    user.Email = instructor.Email;
                    user.PhoneNumber = instructor.PhoneNumber;
                    await _userManager.UpdateAsync(user);

                    //**** check how to update password ****
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    //var exists = await _userManager.FindByIdAsync(instructor.Id);
                    if (await _userManager.FindByIdAsync(instructor.Id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw new Exception(e.Message);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var instructor = await _context.Instructors
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (instructor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(instructor);
        //}

        // POST: Instructor/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var instructor = await _context.Instructors.FindAsync(id);
        //    _context.Instructors.Remove(instructor);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool InstructorExists(int id)
        //{
        //    return _context.Instructors.Any(e => e.Id == id);
        //}
    }
}
