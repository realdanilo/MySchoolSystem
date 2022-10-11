using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public CourseViewModel courseVM { get; set; }
        private readonly MyAppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnv;

        public CourseController(MyAppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnv = hostingEnvironment;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses
                .Include(p => p.Instructor)
                .Include(p => p.Subject)
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
                .Include(p => p.Subject)
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
            List<Subject> subjects = await _context.Subjects.ToListAsync();

            CourseViewModel courseViewModel = new CourseViewModel(instructors, subjects);
            
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
                Subject subject = await _context.Subjects.FindAsync(courseVM.SubjectId);

                newCourse.CreatedAt = DateTime.Now;
                //newCourse.Subject = await _ context.Subjects.FindAsync(courseVM.SubjectId);
                newCourse.Credits = courseVM.Credits;
                newCourse.LastUpdated = DateTime.Now;
                newCourse.Instructor = instructor;
                newCourse.Subject = subject;
                _context.Add(newCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Instructor> instructors = await _context.Instructors.ToListAsync();
            List<Subject> subjects = await _context.Subjects.ToListAsync();
            courseVM = new CourseViewModel(instructors, subjects);
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
            List<Subject> subjects = await _context.Subjects.ToListAsync();
            CourseViewModel courseVM = new CourseViewModel(instructors, subjects);
            courseVM.Credits = course.Credits;
            courseVM.InstructorId = course.Instructor.Id;
            courseVM.SubjectId = course.Subject.Id;
            //courseVM.Subject = await _ context.Subjects.FindAsync(courseVM.SubjectId);
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
                    Subject subject = await _context.Subjects.FindAsync(courseVM.SubjectId);

                    updateCourse.CreatedAt = DateTime.Now;
                    //updateCourse.Subject = await _ context.Subjects.FindAsync(courseVM.SubjectId);
                    updateCourse.Credits = courseVM.Credits;
                    updateCourse.LastUpdated = DateTime.Now;
                    updateCourse.Instructor = instructor;
                    updateCourse.Subject = subject;
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
            List<Subject> subjects = await _context.Subjects.ToListAsync();
            courseVM = new CourseViewModel(instructors, subjects);
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
                .Include(p => p.Subject)
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

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
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
