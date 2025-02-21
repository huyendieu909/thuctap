using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoangXuanQuy.OnlinePainting.Data.Context;
using HoangXuanQuy.OnlinePainting.Data.Models;

namespace HoangXuanQuy.OnlinePainting.MVC.Controllers.EntityController
{
    public class PaintingsController : Controller
    {
        private readonly OnlinePaintingContext _context;

        public PaintingsController(OnlinePaintingContext context)
        {
            _context = context;
        }

        // GET: Paintings
        public async Task<IActionResult> Index()
        {
            var onlinePaintingContext = _context.Paintings.Include(p => p.Artist).Include(p => p.Category);
            return View(await onlinePaintingContext.ToListAsync());
        }

        // GET: Paintings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painting = await _context.Paintings
                .Include(p => p.Artist)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PaintingId == id);
            if (painting == null)
            {
                return NotFound();
            }

            return View(painting);
        }

        // GET: Paintings/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Bio");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Paintings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaintingId,Title,Description,Price,ImageUrl,CreatedDate,Dimensions,Medium,ArtistId,CategoryId")] Painting painting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(painting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Bio", painting.ArtistId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", painting.CategoryId);
            return View(painting);
        }

        // GET: Paintings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painting = await _context.Paintings.FindAsync(id);
            if (painting == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Bio", painting.ArtistId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", painting.CategoryId);
            return View(painting);
        }

        // POST: Paintings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaintingId,Title,Description,Price,ImageUrl,CreatedDate,Dimensions,Medium,ArtistId,CategoryId")] Painting painting)
        {
            if (id != painting.PaintingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(painting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaintingExists(painting.PaintingId))
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Bio", painting.ArtistId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", painting.CategoryId);
            return View(painting);
        }

        // GET: Paintings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painting = await _context.Paintings
                .Include(p => p.Artist)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PaintingId == id);
            if (painting == null)
            {
                return NotFound();
            }

            return View(painting);
        }

        // POST: Paintings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var painting = await _context.Paintings.FindAsync(id);
            if (painting != null)
            {
                _context.Paintings.Remove(painting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaintingExists(int id)
        {
            return _context.Paintings.Any(e => e.PaintingId == id);
        }
    }
}
