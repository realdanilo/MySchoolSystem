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
    [Authorize(Roles = "Instructor, Admin")]
    public class CourseController : Controller
    {
        public CourseViewModel courseVM { get; set; }
        private readonly MyAppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public CourseController(MyAppDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<CustomIdentityUser> userManager)
        {
            _context = context;
            _hostingEnv = hostingEnvironment;
            _userManager = userManager;
        }

        // GET: Course
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses
                .Where(c => c.OpenForEnrollment == true)
                .Include(p => p.Instructor)
                .Include(p => p.Subject)
                .Include(p => p.Period)
                .ToListAsync());
        }

        // GET: Course/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = await _context.Courses
                .Include(p => p.Instructor)
                .Include(p => p.Subject)
                .Include(p => p.Period)
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
            //List<Instructor> instructors = await _context.Instructors.ToListAsync();
            IEnumerable<CustomIdentityUser> instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            List<Subject> subjects = await _context.Subjects.ToListAsync();
            List<Period> periods = await _context.Periods.ToListAsync();

            CourseViewModel courseViewModel = new CourseViewModel(instructors, subjects, periods);
            
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
                //Check if course already exists
                bool courseExists = CheckDuplicateCourse(courseVM);

                if (courseExists)
                {
                    TempData["Alert"] = true;
                    TempData["AlertMessage"] = $"Course already exists";
                    return RedirectToAction(nameof(Create));
                }

                Course newCourse = new Course();
                //Instructor instructor = await _context.Instructors.FindAsync(courseVM.InstructorId);
                CustomIdentityUser instructor = await _userManager.FindByIdAsync(courseVM.InstructorId);

                Subject subject = await _context.Subjects.FindAsync(courseVM.SubjectId);
                Period period = await _context.Periods.FirstAsync(i => i.Id == courseVM.PeriodId);
                newCourse.CreatedAt = DateTime.Now;
                newCourse.Credits = courseVM.Credits;
                newCourse.LastUpdated = DateTime.Now;
                newCourse.Instructor = instructor;
                newCourse.Subject = subject;
                newCourse.Period = period;
                newCourse.OpenForEnrollment = courseVM.OpenForEnrollment;
                newCourse.Year = courseVM.Year;
                newCourse.Notes = courseVM.Notes;
                newCourse.MaxNumberStudents = courseVM.MaxNumberStudents;
                _context.Add(newCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //List<Instructor> instructors = await _context.Instructors.ToListAsync();
            IEnumerable<CustomIdentityUser> instructors = await _userManager.GetUsersInRoleAsync("Instructor");

            List<Subject> subjects = await _context.Subjects.ToListAsync();
            List<Period> periods = await _context.Periods.ToListAsync();

            courseVM = new CourseViewModel(instructors, subjects, periods);
            return View(courseVM);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            Course course = await _context.Courses
                .Where(c => c.Id == id)
                .Include(c => c.Subject)
                .Include(c => c.Period)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync();

            //CustomIdentityUser ii = await _userManager.FindByIdAsync(course.Instructor.Id);
            if (course == null) return NotFound();

            //Authenticate
            if (!(_userManager.GetUserId(User) == course.Instructor.Id || User.IsInRole("Admin")))
            {
                TempData["Alert"] = true;
                TempData["AlertMessage"] = $"You are not the owner or have no permission";
                return RedirectToAction(nameof(Index));
            }

            //IEnumerable<CustomIdentityUser> wont work
            //only allow to make changes on meta data
            List<CustomIdentityUser> instructors = new List<CustomIdentityUser>();
            instructors.Add(course.Instructor);

            List<Subject> subjects = new List<Subject>();
            subjects.Add(course.Subject);

            List<Period> periods = new List<Period>();
            periods.Add(course.Period);

            CourseViewModel courseVM = new CourseViewModel(instructors, subjects,periods);
            courseVM.Credits = course.Credits;
            courseVM.InstructorId = course.Instructor.Id;
            courseVM.SubjectId = course.Subject.Id;
            courseVM.PeriodId = course.Period.Id;
            courseVM.OpenForEnrollment = course.OpenForEnrollment;
            courseVM.Notes = course.Notes;
            courseVM.Year = course.Year;
            courseVM.MaxNumberStudents = course.MaxNumberStudents;
            //FOR EDIT POST CHECK MATCHING, AND MODELSTATE VALIDATION
            courseVM.Id = id;
            return View(courseVM);
        }

        //POST: Course/Edit/5
        //Only allows changes on meta data, cannot update subject, teacher, period once created
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CourseViewModel courseVM)
        {
            if (id != courseVM.Id) return NotFound();

            Course updateCourse = await _context.Courses.Where(c => c.Id == id)
                .Include(c => c.Subject)
                .Include(c => c.Period)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync();

            if (updateCourse == null) return NotFound();

            //Authenticate
            if (!(_userManager.GetUserId(User) == courseVM.InstructorId || User.IsInRole("Admin")))
            {
                TempData["Alert"] = true;
                TempData["AlertMessage"] = $"You are not the owner or have no permission";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    updateCourse.Credits = courseVM.Credits;
                    updateCourse.LastUpdated = DateTime.Now;
                    updateCourse.OpenForEnrollment = courseVM.OpenForEnrollment;
                    updateCourse.Year = courseVM.Year;
                    updateCourse.Notes = courseVM.Notes;
                    updateCourse.MaxNumberStudents = courseVM.MaxNumberStudents;
                    _context.Update(updateCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new Exception(e.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            //Refactor to only show the original field. user is only allowed to change meta data.
            //IEnumerable<CustomIdentityUser> instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            //List<Subject> subjects = await _context.Subjects.ToListAsync();
            //List<Period> periods = await _context.Periods.ToListAsync();

            List<CustomIdentityUser> instructors = new List<CustomIdentityUser>();
            instructors.Add(updateCourse.Instructor);

            List<Subject> subjects = new List<Subject>();
            subjects.Add(updateCourse.Subject);

            List<Period> periods = new List<Period>();
            periods.Add(updateCourse.Period);

            courseVM = new CourseViewModel(instructors, subjects,periods);
            return View(courseVM);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound(); 

            var course = await _context.Courses
                .Include(p => p.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null) return NotFound(); 
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

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
        private bool CheckDuplicateCourse(CourseViewModel courseVM)
        {
            return   _context.Courses
                    .Any(c => (c.Subject.Id == courseVM.SubjectId)
                    && (c.Instructor.Id == courseVM.InstructorId)
                    && (c.Period.Id == courseVM.PeriodId)
                    && (c.Year == courseVM.Year)
                    && (c.OpenForEnrollment == true));
        }
        private bool TodoExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }

        // GET: Course/{CourseId}/Todos
        public async Task<IActionResult> Todos(int CourseId)
        {
            //can save api calls  if (String.IsNullOrEmpty(CourseId.ToString())) return RedirectToAction(nameof(Index))
            bool courseExists =  CourseExists(CourseId);
            //can add error msg
            if (!courseExists) return RedirectToAction(nameof(Index));

            Course course = await _context.Courses
                .Include(p => p.Todos)
                .Include(p => p.Subject)
                .FirstOrDefaultAsync(c => c.Id == CourseId);
            Course_TodoViewModel courseTodoVM = new Course_TodoViewModel();
            courseTodoVM.Todos = course.Todos.ToList();
            courseTodoVM.Id = CourseId;
            courseTodoVM.Subject = course.Subject.SubjectName.ToString();

            if(course.Todos.Count > 0)
            {
                string rootKeyword = "wwwroot";
                int index = 0;
                foreach (var todo in course.Todos)
                {
                    index = todo.Rubric.IndexOf(rootKeyword) + 7;
                    todo.Rubric = todo.Rubric.Substring(index);
                }
            }

            return View(courseTodoVM);
        }

        //rubric
        //POST: Course/{CourseId}/Todo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTodo([FromForm] IFormFile Rubric, int CourseId, [Bind("Id,Type,Points,ExpirationDate")] Course_TodoViewModel course_todoVM)
        {
            course_todoVM.Rubric = Rubric;
            if (ModelState.IsValid && CourseId == course_todoVM.Id && Rubric != null)
            {
                try
                {
                    Course course = await _context.Courses.Include(p => p.Todos)
                        .FirstOrDefaultAsync(c => c.Id == course_todoVM.Id);

                    Todo newTodo = new Todo();
                    newTodo.Type = course_todoVM.Type;
                    newTodo.Points = course_todoVM.Points;
                    newTodo.ExpirationDate = course_todoVM.ExpirationDate;
                    //rubric uploading / path
                    string uploadFolderPath = Path.Combine(_hostingEnv.WebRootPath, "public","rubrics");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + course_todoVM.Rubric.FileName;

                    string filePath = Path.Combine(uploadFolderPath, uniqueFileName);
                    //saving to server
                    using (var fStream = new FileStream(filePath, FileMode.Create))
                    {
                        fStream.Position = 0;
                        await course_todoVM.Rubric.CopyToAsync(fStream);
                        await fStream.FlushAsync();
                    }
                    newTodo.Rubric = filePath;

                    _context.Add(newTodo);
                    course.Todos.Add(newTodo);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException e)
                {
                    //check
                    throw new Exception(e.Message);
                    //return RedirectToAction(nameof(Todos), new { CourseId });
                }

            }
            // find a middleware for msg alerts (success, error, etc..)
            return RedirectToAction(nameof(Todos));
        }

        //rubric
        //POST: Course/{CourseId}/UpdateTodo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTodo([FromForm] IFormFile Rubric, int CourseId, int TodoId, [Bind("Id,Type,Points,ExpirationDate")] Course_TodoViewModel course_todoVM)
        {
            course_todoVM.Rubric = Rubric;
            if (ModelState.IsValid)
            {
                if (!CourseExists(CourseId) && !TodoExists(CourseId)) return NotFound();
                try
                {
                    Course course = await _context.Courses.Include(p => p.Todos)
                        .FirstOrDefaultAsync(c => c.Id == CourseId);

                    Todo t = course.Todos.Where(t => t.Id == TodoId).Select(t => t).Single();
                    t.Type = course_todoVM.Type;
                    t.Points = course_todoVM.Points;
                    t.ExpirationDate = course_todoVM.ExpirationDate;

                    if(Rubric != null)
                    {
                        //rubric uploading / path
                        string uploadFolderPath = Path.Combine(_hostingEnv.WebRootPath, "public", "rubrics");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + course_todoVM.Rubric.FileName;

                        string filePath = Path.Combine(uploadFolderPath, uniqueFileName);
                        //saving to server
                        using (var fStream = new FileStream(filePath, FileMode.Create))
                        {
                            fStream.Position = 0;
                            await course_todoVM.Rubric.CopyToAsync(fStream);
                            await fStream.FlushAsync();
                        }

                        //delete old file4
                        if (System.IO.File.Exists(t.Rubric))
                        {
                            System.IO.File.Delete(t.Rubric);
                        }

                        //update new file
                        t.Rubric = filePath;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new Exception(e.Message);
                }

            }
            return RedirectToAction(nameof(Todos));
        }
    }
}
