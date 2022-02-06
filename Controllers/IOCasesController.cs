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

namespace EdPlatformWebsite.Controllers
{
    [Authorize(Policy = "Administrators")]
    public class IOCasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IOCasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IOCases
        public async Task<IActionResult> Index()
        {
            ViewBag.Exercises = _context.Exercises.OrderBy(item => item.Number).ToDictionary(item => item.Id, item => item);
            return View(await _context.IOCases.ToListAsync());
        }

        // GET: IOCases/Create
        public IActionResult Create()
        {
            ViewBag.Exercises = _context.Exercises.OrderBy(item => item.Number).ToList();
            return View();
        }

        // POST: IOCases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Input,Output,ExerciseId")] IOCase iOCase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iOCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(iOCase);
        }

        // GET: IOCases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iOCase = await _context.IOCases.FindAsync(id);
            if (iOCase == null)
            {
                return NotFound();
            }
            ViewBag.Exercises = _context.Exercises.OrderBy(item => item.Number).ToList();
            return View(iOCase);
        }

        // POST: IOCases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Input,Output,ExerciseId")] IOCase iOCase)
        {
            if (id != iOCase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iOCase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IOCaseExists(iOCase.Id))
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
            return View(iOCase);
        }

        // GET: IOCases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iOCase = await _context.IOCases
                .FirstOrDefaultAsync(m => m.Id == id);
            if (iOCase == null)
            {
                return NotFound();
            }

            return View(iOCase);
        }

        // POST: IOCases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iOCase = await _context.IOCases.FindAsync(id);
            _context.IOCases.Remove(iOCase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IOCaseExists(int id)
        {
            return _context.IOCases.Any(e => e.Id == id);
        }
    }
}
