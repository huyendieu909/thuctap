using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCareAppointment.Data;
using HealthCareAppointment.Models;
using HealthCareAppointment.Data.UnitOfWork;
using System.Xml;

namespace HealthCareAppointment.MVC.Controllers
{
    public class UsersController : Controller
    {
        //private readonly HealthcareAppointmentContext _context;
        private readonly IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await unitOfWork.UserRepository.GetQuery().ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await unitOfWork.UserRepository.GetQuery()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["Role"] = new SelectList(Enum.GetValues(typeof(Users.RoleEnum)));
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Users users)
        {
            if (ModelState.IsValid)
            {
                users.Id = Guid.NewGuid();
                unitOfWork.UserRepository.Add(users);
                await unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Role"] = new SelectList(Enum.GetValues(typeof(Users.RoleEnum)), users.Role);
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await unitOfWork.UserRepository.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["Role"] = new SelectList(Enum.GetValues(typeof(Users.RoleEnum)), users.Role);
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,  Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.UserRepository.Update(users);
                    await unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
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
            ViewData["Role"] = new SelectList(Enum.GetValues(typeof(Users.RoleEnum)), users.Role);
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await unitOfWork.UserRepository.GetQuery()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var users = await unitOfWork.UserRepository.FindAsync(id);
            if (users != null)
            {
                await unitOfWork.UserRepository.DeleteAsync(users);
            }

            await unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(Guid id)
        {
            return unitOfWork.UserRepository.GetQuery().Any(e => e.Id == id);
        }
    }
}









//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using HealthCareAppointment.Data;
//using HealthCareAppointment.Models;

//namespace HealthCareAppointment.MVC.Controllers
//{
//    public class UsersController : Controller
//    {
//        private readonly HealthcareAppointmentContext _context;

//        public UsersController(HealthcareAppointmentContext context)
//        {
//            _context = context;
//        }

//        // GET: Users
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.Users.ToListAsync());
//        }

//        // GET: Users/Details/5
//        public async Task<IActionResult> Details(Guid? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var users = await _context.Users
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (users == null)
//            {
//                return NotFound();
//            }

//            return View(users);
//        }

//        // GET: Users/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Users/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Name,Email,DateOfBirth,Password,Role,Specialization")] Users users)
//        {
//            if (ModelState.IsValid)
//            {
//                users.Id = Guid.NewGuid();
//                _context.Add(users);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(users);
//        }

//        // GET: Users/Edit/5
//        public async Task<IActionResult> Edit(Guid? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var users = await _context.Users.FindAsync(id);
//            if (users == null)
//            {
//                return NotFound();
//            }
//            return View(users);
//        }

//        // POST: Users/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Email,DateOfBirth,Password,Role,Specialization")] Users users)
//        {
//            if (id != users.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(users);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!UsersExists(users.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(users);
//        }

//        // GET: Users/Delete/5
//        public async Task<IActionResult> Delete(Guid? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var users = await _context.Users
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (users == null)
//            {
//                return NotFound();
//            }

//            return View(users);
//        }

//        // POST: Users/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(Guid id)
//        {
//            var users = await _context.Users.FindAsync(id);
//            if (users != null)
//            {
//                _context.Users.Remove(users);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool UsersExists(Guid id)
//        {
//            return _context.Users.Any(e => e.Id == id);
//        }
//    }
//}
