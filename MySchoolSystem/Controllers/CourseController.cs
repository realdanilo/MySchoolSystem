using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchoolSystem.Models;
using MySchoolSystem.Models.ViewModels;

namespace MySchoolSystem.Controllers
{
    [BindProperties]
    public class CourseController : Controller
    {
        //public Course Course { get; set; }
        public CourseViewModel courseVM { get; set; }
        private readonly MyAppDbContext _context;

        public CourseController(MyAppDbContext context)
        {
            _context = context;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses
                .Include(p => p.Instructor)
                .ToListAsync());
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = await _context.Courses
                .Include(p => p.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        public async Task<IActionResult> Create()
        {
            List<Instructor> instructors = await _context.Instructors.ToListAsync();
            CourseViewModel courseViewModel = new CourseViewModel(instructors);
            
            return View(courseViewModel);
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel courseVM)
        {
            if (ModelState.IsValid)
            {
                Course newCourse = new Course();
                Instructor instructor = await _context.Instructors.FindAsync(courseVM.InstructorId);

                newCourse.CreatedAt = DateTime.Now;
                newCourse.Subject = courseVM.Subject;
                newCourse.Credits = courseVM.Credits;
                newCourse.LastUpdated = DateTime.Now;
                newCourse.Instructor = instructor;
                _context.Add(newCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Instructor> instructors = await _context.Instructors.ToListAsync();
            courseVM = new CourseViewModel(instructors);
            return View(courseVM);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            List<Instructor> instructors = await _context.Instructors.ToListAsync();
            CourseViewModel courseVM = new CourseViewModel(instructors);
            courseVM.Credits = course.Credits;
            courseVM.InstructorId = course.Instructor.Id;
            courseVM.Subject = course.Subject;
            //check
            courseVM.Id = id;
            return View(courseVM);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CourseViewModel courseVM)
        {
            //check
            if (id != courseVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    //Course newCourse = new Course();
                    Course updateCourse = await _context.Courses.FindAsync(courseVM.Id);
                    Instructor instructor = await _context.Instructors.FindAsync(courseVM.InstructorId);

                    updateCourse.CreatedAt = DateTime.Now;
                    updateCourse.Subject = courseVM.Subject;
                    updateCourse.Credits = courseVM.Credits;
                    updateCourse.LastUpdated = DateTime.Now;
                    updateCourse.Instructor = instructor;
                    //_context.Add(newCourse);
                    _context.Update(updateCourse);
                    await _context.SaveChangesAsync();
                  
                }
                catch (DbUpdateConcurrencyException e)
                {

                    throw new Exception(e.Message);
                  
                }
                return RedirectToAction(nameof(Index));
            }
            List<Instructor> instructors = await _context.Instructors.ToListAsync();
            courseVM = new CourseViewModel(instructors);
            return View(courseVM);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool CourseExists(int id)
        //{
        //    return _context.Courses.Any(e => e.Id == id);
        //}
    }
}
