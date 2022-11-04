using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchoolSystem.Models;
using MySchoolSystem.Models.ViewModels;

namespace MySchoolSystem.Controllers
{
    [Bind]
    [Authorize(Roles = "Instructor, Admin, Student")]
    public class EnrollmentController : Controller
    {
        private readonly MyAppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public EnrollmentViewModel enrollmentVM { get; set; }

        public EnrollmentController(MyAppDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<CustomIdentityUser> userManager)
        {
            _context = context;
            _hostingEnv = hostingEnvironment;
            _userManager = userManager;
        }

        // GET: Enrollment
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enrollments
                .Where(e => e.Dropped == false)
                .Include(p => p.Student)
                .Include(p => p.Course.Subject)
                .ToListAsync());
        }

        // GET: Enrollment/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound(); 

            var enrollment = await _context.Enrollments
                .Where(e => e.Id == id)
                .Include(p => p.Student)
                .Include(p => p.Course.Subject)
                .Include(p => p.Course.Instructor)
                .Include(p => p.Grade)
                .Include(p => p.Submitted_Assignments)
                .FirstOrDefaultAsync();

            if (enrollment == null) return NotFound(); 

            //generating the todos for that enrollment.
            //Can I make a static method to generate it? check..
            await _context.Courses.Where(c => c.Id == enrollment.Course.Id)
            .Include(p => p.Todos)
            .FirstOrDefaultAsync();

            //cleaning Submittes_Assignments File Path for front end
            if(enrollment.Submitted_Assignments.Count > 0)
            {
                string rootKeyword = "wwwroot";
                int index = 0;

                foreach (var assignment in enrollment.Submitted_Assignments)
                {
                    index = assignment.FileLocation.IndexOf(rootKeyword) + 7;
                    assignment.FileLocation = assignment.FileLocation.Substring(index);
                }
            }

            return View(enrollment);
        }

        // GET: Enrollment/Create
        public async Task<IActionResult> Create()
        {
            IEnumerable<CustomIdentityUser> students = await _userManager.GetUsersInRoleAsync("Student");
            List<Course> courses = await _context.Courses.Include(p => p.Subject).Include(p => p.Instructor).ToListAsync();
            List<LetterGrade> grades = await _context.LetterGrades.ToListAsync();

            EnrollmentViewModel enrollmentViewModel = new EnrollmentViewModel(courses, students, grades);
            //when creating an enrollment, grade should be zero 
            return View(enrollmentViewModel);
        }

        // POST: Enrollment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentViewModel enrollmentVM)
        {
            if (ModelState.IsValid)
            {
                //check if student  is already enrolled
                bool isEnrolled = CheckDuplicateEnrollment(enrollmentVM);
                if (isEnrolled)
                {
                    TempData["Alert"] = true;
                    TempData["AlertMessage"] = $"Already Enrolled";
                    return RedirectToAction(nameof(Create));
                }
                //check if there is opening
                if(!IsEnrollmentAvailable(enrollmentVM))
                {
                    TempData["Alert"] = true;
                    TempData["AlertMessage"] = $"Enrollment is Maxed Out";
                    return RedirectToAction(nameof(Create));
                }

                Enrollment newEnrollment = new Enrollment();
                newEnrollment.Course = await _context.Courses.FirstAsync(i => i.Id == enrollmentVM.CourseId);
                newEnrollment.Student = await _userManager.FindByIdAsync(enrollmentVM.StudentId);
                _context.Add(newEnrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enrollmentVM);
        }

        // GET: Enrollment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            Enrollment enrollment = await _context.Enrollments
                .Where(e => e.Id == id)
                .Include(p => p.Student)
                .Include(p => p.Course)
                .Include(p => p.Grade)
                .FirstOrDefaultAsync();

            if (enrollment == null) return NotFound(); 

            //List<Student> students = await _context.Students.ToListAsync();
            IEnumerable<CustomIdentityUser> students = await _userManager.GetUsersInRoleAsync("Student");
            // Select only the student. check to improve code ***
            students = students.Where(x => x.Id == enrollment.Student.Id);

            List<Course> courses = await _context.Courses.Include(p => p.Subject).Include(p => p.Instructor).ToListAsync();
            List<LetterGrade> grades = await _context.LetterGrades.ToListAsync();

            EnrollmentViewModel enrollmentViewModel = new EnrollmentViewModel(courses, students, grades);
            enrollmentViewModel.Id = enrollment.Id;
            enrollmentViewModel.CourseId = enrollment.Course.Id;
            enrollmentViewModel.StudentId = enrollment.Student.Id;
            enrollmentViewModel.GradeId = enrollment?.Grade?.Id;
            enrollmentViewModel.Dropped = enrollment.Dropped;
            enrollmentViewModel.Notes = enrollment.Notes;

            return View(enrollmentViewModel);
        }

        // POST: Enrollment/Edit/5
        //Sumarry: Should only allow changes on grade, notes, and enrollment status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EnrollmentViewModel enrollmentVM)
        {
            if (id != enrollmentVM.Id) return NotFound();

            Enrollment updateEnrollment = await _context.Enrollments.FindAsync(id);
            if (updateEnrollment == null) return NotFound(); 

            if (ModelState.IsValid)
            {
                try
                {
                    updateEnrollment.Grade = enrollmentVM.GradeId == null ? null : await _context.LetterGrades.FirstAsync(i => i.Id == enrollmentVM.GradeId);
                    updateEnrollment.Dropped = enrollmentVM.Dropped;
                    updateEnrollment.Notes = enrollmentVM.Notes;
                    _context.Update(updateEnrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e) { return NotFound(e.Message); }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            }
            return RedirectToAction(nameof(Details),new { id });
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

        [HttpPost]
        public async Task<IActionResult> SubmitFile([FromForm]IFormFile FileUpload, [FromForm]int CourseId, [FromForm]int EnrollmentId, [FromForm]int TodoId)
        {
            if(FileUpload != null)
            {
                var checkTxt = FileUpload.FileName.Substring(FileUpload.FileName.Length - 3);
                if (checkTxt != "txt") return NotFound();

                string uploadFolderPath = Path.Combine(_hostingEnv.WebRootPath, "public","assignments");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileUpload.FileName;

                string filePath = Path.Combine(uploadFolderPath, uniqueFileName);
                //saving to server
                using (var fStream = new FileStream(filePath, FileMode.Create))
                {
                    fStream.Position = 0;
                    await FileUpload.CopyToAsync(fStream);
                    await fStream.FlushAsync();
                }
                //DOESNT WORK >>> await FileUpload.CopyToAsync(new FileStream(filePath, FileMode.Create));
                //reading file
                using(var reader = new StreamReader(FileUpload.OpenReadStream()))
                {
                    Console.WriteLine(await reader.ReadToEndAsync());
                }

                //creating new submitted_assignment
                //check if theres already a submitted file
                Enrollment currentEnrollement = await _context.Enrollments.FirstOrDefaultAsync(e => e.Id == EnrollmentId);

                var submittedTodo = await _context.Submitted_Assignments
                    .Include(p => p.Enrollment)
                    .Include(p => p.Task)
                    .Where(assignment => assignment.Enrollment.Id == currentEnrollement.Id && assignment.Task.Id == TodoId)
                    .FirstOrDefaultAsync();


                //var ii = from assignment in _context.Submitted_Assignments
                //         where assignment.Task.Id == TodoId && assignment.Enrollment.Id == EnrollmentId
                //         select assignment.Task;

                if (submittedTodo != null)
                {
                    //delete old file
                    System.IO.File.Delete(submittedTodo.FileLocation);

                    //update new file
                    submittedTodo.FileLocation = filePath;
                }
                else
                {
                    Submitted_Assignments newSubmittedAssignment = new Submitted_Assignments();
                    newSubmittedAssignment.Task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == TodoId);
                    newSubmittedAssignment.Enrollment = currentEnrollement;
                    newSubmittedAssignment.FileLocation = filePath;
                    _context.Add(newSubmittedAssignment);

                    //saving to the specific enrollment-relation
                    currentEnrollement.Submitted_Assignments.Add(newSubmittedAssignment);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = EnrollmentId });
        }

        // POST: Enrollment/UpdatePoints
        [HttpPost]
        public async Task<IActionResult> UpdatePoints(int SubmittedAssignmentId, int EnrollmentId, int Points)
        {
            try
            {
                var submittedAssingment = await  _context.Submitted_Assignments
                    .Where(assignment => assignment.Enrollment.Id == EnrollmentId && assignment.Task.Id == SubmittedAssignmentId)
                    .FirstOrDefaultAsync();
                //can check the max points allowed
                if (Points >=0 && Points <=100)
                {
                    submittedAssingment.GradedPoints = Points;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return RedirectToAction(nameof(Details), new { id = EnrollmentId});
        }

        private bool CheckDuplicateEnrollment(EnrollmentViewModel enrollmentVM)
        {
            return _context.Enrollments
                    .Any(e => (e.Course.Id == enrollmentVM.CourseId)
                    && (e.Student.Id == enrollmentVM.StudentId));
        }

        private bool IsEnrollmentAvailable(EnrollmentViewModel enrollmentVM)
        {
            int courseMax = _context.Courses.FirstOrDefault(c => c.Id == enrollmentVM.CourseId).MaxNumberStudents;

            int enrollments =  _context.Enrollments
                .Count(e => e.Course.Id == enrollmentVM.CourseId
                && e.Dropped == false);

            return enrollments <= courseMax;
        }
    }
}
