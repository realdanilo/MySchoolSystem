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

        // GET: Course/{CourseId}/Todos
        public async Task<IActionResult> Todos(int CourseId)
        {
            //can save api calls
            //if (String.IsNullOrEmpty(CourseId.ToString())) return RedirectToAction(nameof(Index))
            bool courseExists =  CourseExists(CourseId);
            //can add error msg
            if (!courseExists) return RedirectToAction(nameof(Index));

            Course course = await _context.Courses.Include(p => p.Todos).FirstOrDefaultAsync(c => c.Id == CourseId);
            List<Todo> courseTodos = course.Todos.ToList();
            ViewBag.CourseId = CourseId;
            return View(courseTodos);
        }

        //POST: Course/{CourseId}/Todo
        [HttpPost]
        public async Task<IActionResult> AddTodo([FromBody] string value,int CourseId)
        {
            Console.WriteLine("hit");
            Console.WriteLine(CourseId);
            Console.WriteLine(value);
            return RedirectToAction(nameof(Todos),CourseId.ToString());
        }


    }
}
