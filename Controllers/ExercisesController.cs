#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EdPlatformWebsite.Data;
using EdPlatformWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using IronPython.Hosting;
using EdPlatformWebsite.PythonCodeManagement;

namespace EdPlatformWebsite.Controllers
{
    [Authorize(Policy = "Administrators")]
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            ViewBag.Lessons = _context.Lessons.OrderBy(item => item.Number).ToDictionary(item => item.Id, item => item);
            return View(await _context.Exercises.ToListAsync());
        }

        #region Details
        // GET: Exercises/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id, string? result = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            ViewBag.Modules = await _context.Modules
                .OrderBy(item => item.Number)
                .Include(item => item.Lessons)
                .ThenInclude(item => item.Exercises)
                .ToListAsync();
            ViewBag.CurrentExercise = exercise;
            ViewBag.CurrentLesson = await _context.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == exercise.LessonId);
            if(result != null)
                ViewBag.Result = result;

            return View(exercise);
        }

        [HttpPost]
        public IActionResult CheckCode(int? id, string? code)
        {
            string s = "";
            MemoryStream ms = new MemoryStream();
            EventRaisingStreamWriter outputWr = new EventRaisingStreamWriter(ms);
            outputWr.StringWritten += new EventHandler<MyEvtArgs<string>>(sWr_StringWritten);
            void sWr_StringWritten(object sender, MyEvtArgs<string> e)
            {
                s += e.Value;
            }

            var engine = Python.CreateEngine();
            var source = engine.CreateScriptSourceFromString(code);
            var scope = engine.CreateScope();
            engine.Runtime.IO.SetOutput(ms, outputWr);
            //engine.Runtime.IO.SetErrorOutput(stream, txtWriter);
            source.Execute(scope);
            return RedirectToAction(nameof(Details), new { id = id, result = s });
        }
        #endregion

        #region Create
        // GET: Exercises/Create
        public IActionResult Create()
        {
            ViewBag.Lessons = _context.Lessons.OrderBy(item => item.Number).ToList();
            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Name,Description,LessonId")] Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exercise);
        }
        #endregion

        #region Edit
        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            ViewBag.Lessons = _context.Lessons.OrderBy(item => item.Number).ToList();
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Name,Description,LessonId")] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
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
            return View(exercise);
        }
        #endregion

        #region Delete
        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
