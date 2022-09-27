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
    public class EnrollmentController : Controller
    {
        private readonly MyAppDbContext _context;
        public EnrollmentViewModel enrollmentVM { get; set; }

        public EnrollmentController(MyAppDbContext context)
        {
            _context = context;
        }

        // GET: Enrollment
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enrollments
                .Include(p => p.Student)
                .Include(p => p.Course.Subject)
                .ToListAsync());
        }

        // GET: Enrollment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(p => p.Student)
                .Include(p => p.Course.Subject)
                .Include(p => p.Course.Instructor)
                .Include(p => p.Period)
                .Include(p => p.Grade)
                .FirstOrDefaultAsync(m => m.Id == id);
            //can include/add later on tasks submitted
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollment/Create
        public async Task<IActionResult> Create()
        {
            List<Course> courses = await _context.Courses.Include(p => p.Subject).Include(p => p.Instructor).ToListAsync();
            List<Student> students = await _context.Students.ToListAsync();
            List<LetterGrade> grades = await _context.LetterGrades.ToListAsync();
            List<Period> periods = await _context.Periods.ToListAsync();


            EnrollmentViewModel enrollmentViewModel = new EnrollmentViewModel(courses, students, periods, grades);
            enrollmentViewModel.Year = DateTime.Now.Year;
            //when creating an enrollment, grade should be zero // subjective
            return View(enrollmentViewModel);
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentViewModel enrollmentVM)
        {
            if (ModelState.IsValid)
            {
                Enrollment newEnrollment = new Enrollment();
                newEnrollment.Course = await _context.Courses.FirstAsync(i => i.Id == enrollmentVM.CourseId);
                newEnrollment.Student = await _context.Students.FirstAsync(i => i.Id == enrollmentVM.StudentId);
                newEnrollment.Period = await _context.Periods.FirstAsync(i => i.Id == enrollmentVM.PeriodId);
                newEnrollment.Year = enrollmentVM.Year;
                _context.Add(newEnrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enrollmentVM);
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

            List<Course> courses = await _context.Courses.Include(p => p.Subject).Include(p => p.Instructor).ToListAsync();
            List<Student> students = await _context.Students.ToListAsync();
            List<LetterGrade> grades = await _context.LetterGrades.ToListAsync();
            List<Period> periods = await _context.Periods.ToListAsync();


            EnrollmentViewModel enrollmentViewModel = new EnrollmentViewModel(courses, students, periods, grades);
            enrollmentViewModel.Id = enrollment.Id;
            enrollmentViewModel.CourseId = enrollment.Course.Id;
            enrollmentViewModel.StudentId = enrollment.Student.Id;
            enrollmentViewModel.PeriodId = enrollment.Period.Id;
            enrollmentViewModel.Year = enrollment.Year;
            enrollmentViewModel.GradeId = enrollment?.Grade?.Id;
            enrollmentViewModel.Dropped = enrollment.Dropped;
            enrollmentViewModel.Notes = enrollment.Notes;

            return View(enrollmentViewModel);

        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EnrollmentViewModel enrollmentVM)
        {
            if (id != enrollmentVM.Id)
            {
                return NotFound();
            }
            Enrollment updateEnrollment = await _context.Enrollments.FindAsync(id);
            if (updateEnrollment == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Enrollment newEnrollment = new Enrollment();
                    updateEnrollment.Course = await _context.Courses.FirstAsync(i => i.Id == enrollmentVM.CourseId);
                    updateEnrollment.Student = await _context.Students.FirstAsync(i => i.Id == enrollmentVM.StudentId);
                    updateEnrollment.Period = await _context.Periods.FirstAsync(i => i.Id == enrollmentVM.PeriodId);
                    updateEnrollment.Grade = enrollmentVM.GradeId == null ? null :  await _context.LetterGrades.FirstAsync(i => i.Id == enrollmentVM.GradeId);
                    updateEnrollment.Year = enrollmentVM.Year;
                    updateEnrollment.Dropped = enrollmentVM.Dropped;
                    //updateEnrollment.OpenForEnrollment = enrollmentVM.OpenForEnrollment; >> i  think this should not be available
                    updateEnrollment.Notes = enrollmentVM.Notes;
                    _context.Update(updateEnrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return NotFound(e.Message);

                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Enrollment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
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
