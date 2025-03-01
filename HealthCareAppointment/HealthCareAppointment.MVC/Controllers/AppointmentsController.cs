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
using static HealthCareAppointment.Models.Appointment;

namespace HealthCareAppointment.MVC.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public AppointmentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var healthcareAppointmentContext = unitOfWork.AppointmentRepository.GetQuery().Include(a => a.Doctor).Include(a => a.Patient);
            return View(await healthcareAppointmentContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await unitOfWork.AppointmentRepository.GetQuery()
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusEnum)));
            ViewData["DoctorId"] = new SelectList(unitOfWork.UserRepository.GetQuery(x => x.Role == Users.RoleEnum.Doctor).ToList(), "Id", "Email");
            ViewData["PatientId"] = new SelectList(unitOfWork.UserRepository.GetQuery(x => x.Role == Users.RoleEnum.Patient).ToList(), "Id", "Email");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.Id = Guid.NewGuid();
                unitOfWork.AppointmentRepository.Add(appointment);
                await unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusEnum)));
            ViewData["DoctorId"] = new SelectList(unitOfWork.UserRepository.GetQuery(x => x.Role == Users.RoleEnum.Doctor).ToList(), "Id", "Email", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(unitOfWork.UserRepository.GetQuery(x => x.Role == Users.RoleEnum.Patient).ToList(), "Id", "Email", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await unitOfWork.AppointmentRepository.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusEnum)));
            ViewData["DoctorId"] = new SelectList(unitOfWork.UserRepository.GetQuery(x => x.Role == Users.RoleEnum.Doctor).ToList(), "Id", "Email", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(unitOfWork.UserRepository.GetQuery(x => x.Role == Users.RoleEnum.Patient).ToList(), "Id", "Email", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,  Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.AppointmentRepository.Update(appointment);
                    await unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StatusEnum)));
            ViewData["DoctorId"] = new SelectList(unitOfWork.UserRepository.GetQuery(x => x.Role == Users.RoleEnum.Doctor).ToList(), "Id", "Email", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(unitOfWork.UserRepository.GetQuery(x => x.Role == Users.RoleEnum.Patient).ToList(), "Id", "Email", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await unitOfWork.AppointmentRepository.GetQuery()
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appointment = await unitOfWork.AppointmentRepository.FindAsync(id);
            if (appointment != null)
            {
                await unitOfWork.AppointmentRepository.DeleteAsync(appointment);
            }

            await unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(Guid id)
        {
            return unitOfWork.AppointmentRepository.GetQuery().Any(e => e.Id == id);
        }
    }
}
