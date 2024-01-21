using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppScheduler_v1.Areas.Identity.Data;
using AppScheduler_v1.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppScheduler_v1.Controllers
{
    [Authorize(Roles = "Patient,Doctor")]
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            // Get the ID of the logged-in user
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve appointments only for the logged-in user
            var userAppointments = _context.Appointments
                .Where(a => a.PatientId == loggedInUserId || a.DoctorId == loggedInUserId)
                .Include(a => a.Doctor)
                .Include(a => a.Patient);

            return View(await userAppointments.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
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
            // Get the list of users with the role "Doctor"
            var doctorRole = _context.Roles.FirstOrDefault(a => a.Name == "Doctor");
            var doctorUsers = _context.Users
                .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == doctorRole.Id))
                .ToList();
            // Get the id of the logged-in user who is trying to create
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Set ViewData for DoctorId with the list of doctor users
            ViewData["DoctorId"] = new SelectList(doctorUsers, "Id", "UserName");
            // Set ViewData for PatientId with the id of the logged-in user
            ViewData["PatientId"] = new SelectList(new List<string> { loggedInUserId });
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,DoctorId,Symptoms,Date,AppointmentTimeSlot")] Appointment appointment)
        {

            //Combine date and time from the model to form a DateTime
            appointment.Date = CombineDateAndTime(appointment.Date, appointment.AppointmentTimeSlot);

            // Check if there's an existing appointment at the chosen date and time slot
            var existingAppointment = _context.Appointments
                .FirstOrDefault(a => a.Date == appointment.Date && a.AppointmentTimeSlot == appointment.AppointmentTimeSlot);

            if (existingAppointment != null)
            {
                // Handle the case where the time slot is already booked
                ModelState.AddModelError("AppointmentTimeSlot", "Selected time slot is not available for the chosen date.");
            }
            else
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var doctorRole = _context.Roles.FirstOrDefault(a => a.Name == "Doctor");
            var doctorUsers = _context.Users
                .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == doctorRole.Id))
                .ToList();

            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ViewData["DoctorId"] = new SelectList(doctorUsers, "Id", "UserName");
            ViewData["PatientId"] = new SelectList(new List<string> { loggedInUserId });

            return View(appointment);
        }

        private DateTime CombineDateAndTime(DateTime date, AppointmentTime timeSlot)
        {
            //Get the display name of the enum value (e.g., "09:00 - 10:00"
            string timeSlotDisplayName = ((DisplayAttribute)timeSlot
                .GetType()
                .GetMember(timeSlot.ToString())[0]
                .GetCustomAttributes(typeof(DisplayAttribute), false)[0])
                .Name;

            //Parse the time part from the display name
            string timePart = timeSlotDisplayName.Split('-')[0].Trim();

            return DateTime.Parse($"{date:yyyy-MM-dd} {timePart}");
        }

        /*// GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,DoctorId,Symptoms,DateTime")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
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
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Users, "Id", "Id", appointment.PatientId);
            return View(appointment);
        }*/

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'AppDbContext.Appointments'  is null.");
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
          return (_context.Appointments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //Action for doctors to view appointments
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DoctorAppointments()
        {
            //get the ID of the logged in doctor 
            var loggedInDoctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //retrieve appointments only for the logged in doctor
            var doctorAppointments = _context.Appointments
                .Where(a => a.DoctorId == loggedInDoctorId)
                .Include(a => a.Patient)
                .ToList();

            return View(doctorAppointments);
        }
    }
}
