using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchoolSystem.Models;

namespace MySchoolSystem.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly MyAppDbContext _context;

        public EnrollmentController(MyAppDbContext context)
        {
            _context = context;
        }

        // GET: Enrollment
        public async Task<IActionResult> Index()
        {
            var myAppDbContext = _context.Enrollments
                .Include(e => e.Course).
                Include(e => e.Grade).Include(e => e.Student);
            return View(await myAppDbContext.ToListAsync());
        }

        // GET: Enrollment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Grade)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollment/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Subject");
            ViewData["LetterGradeId"] = new SelectList(_context.LetterGrades, "Id", "Grade");
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FirstName");
            //done
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName");
            ViewData["Courses"] = new SelectList(_context.Courses, "Id", "Subject");
            return View();
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,InstructorId,CourseId,LetterGradeId,Dropped,Notes")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Subject", enrollment.CourseId);
            //ViewData["LetterGradeId"] = new SelectList(_context.LetterGrades, "Id", "Grade", enrollment.LetterGradeId);
            //ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FirstName", enrollment.InstructorId);
            //ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Subject", enrollment.CourseId);
            //ViewData["LetterGradeId"] = new SelectList(_context.LetterGrades, "Id", "Grade", enrollment.LetterGradeId);
            //ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FirstName", enrollment.InstructorId);
            //ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,InstructorId,CourseId,LetterGradeId,Dropped,Notes")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Subject", enrollment.CourseId);
            //ViewData["LetterGradeId"] = new SelectList(_context.LetterGrades, "Id", "Grade", enrollment.LetterGradeId);
            //ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FirstName", enrollment.InstructorId);
            //ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Grade)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.Id == id);
        }
    }
}
