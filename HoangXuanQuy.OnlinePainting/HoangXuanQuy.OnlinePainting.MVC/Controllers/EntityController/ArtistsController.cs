using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoangXuanQuy.OnlinePainting.Data.Context;
using HoangXuanQuy.OnlinePainting.Data.Models;
using HoangXuanQuy.OnlinePainting.Data.UnitOfWork;

namespace HoangXuanQuy.OnlinePainting.MVC.Controllers.EntityController
{
    public class ArtistsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArtistsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Artists
        public async Task<IActionResult> Index(string keyword)
        {
            var artist = _unitOfWork.Artists.GetQuery().Select(x => x);
            if (!String.IsNullOrEmpty(keyword))
            {
                artist = artist.Where(artist => artist.Name.Contains(keyword));
            }
            return View(await artist.ToListAsync());
        }
        //// GET: Artists
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Artists.ToListAsync());
        //}

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _unitOfWork.Artists.GetQuery()
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistId,Name,Bio,Website,BirthDate,Nationality")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Artists.AddAsync(artist);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _unitOfWork.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistId,Name,Bio,Website,BirthDate,Nationality")] Artist artist)
        {
            if (id != artist.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Artists.UpdateAsync(artist);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.ArtistId))
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
            return View(artist);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _unitOfWork.Artists.GetQuery()
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _unitOfWork.Artists.FindAsync(id);
            if (artist != null)
            {
                await _unitOfWork.Artists.DeleteAsync(artist);
            }

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
            return _unitOfWork.Artists.GetQuery().Any(e => e.ArtistId == id);
        }
    }
}
