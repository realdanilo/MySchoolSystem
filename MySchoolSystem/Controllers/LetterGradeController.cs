using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchoolSystem.Models;

namespace MySchoolSystem.Controllers
{
    [Authorize(Roles = "Instructor, Admin")]
    public class LetterGradeController : Controller
    {
        private readonly MyAppDbContext _context;

        public LetterGradeController(MyAppDbContext context)
        {
            _context = context;
        }

        // GET: LetterGrade
        public async Task<IActionResult> Index()
        {
            return View(await _context.LetterGrades.ToListAsync());
        }

        // GET: LetterGrade/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var letterGrade = await _context.LetterGrades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (letterGrade == null)
            {
                return NotFound();
            }

            return View(letterGrade);
        }

        // GET: LetterGrade/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LetterGrade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Grade,Weight")] LetterGrade letterGrade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(letterGrade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(letterGrade);
        }

        // GET: LetterGrade/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var letterGrade = await _context.LetterGrades.FindAsync(id);
            if (letterGrade == null)
            {
                return NotFound();
            }
            return View(letterGrade);
        }

        // POST: LetterGrade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Grade,Weight")] LetterGrade letterGrade)
        {
            if (id != letterGrade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(letterGrade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LetterGradeExists(letterGrade.Id))
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
            return View(letterGrade);
        }

        // GET: LetterGrade/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var letterGrade = await _context.LetterGrades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (letterGrade == null)
            {
                return NotFound();
            }

            return View(letterGrade);
        }

        // POST: LetterGrade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var letterGrade = await _context.LetterGrades.FindAsync(id);
            _context.LetterGrades.Remove(letterGrade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LetterGradeExists(int id)
        {
            return _context.LetterGrades.Any(e => e.Id == id);
        }
    }
}
