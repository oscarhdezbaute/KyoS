﻿using KyoS.Web.Data;
using KyoS.Web.Data.Entities;
using KyoS.Web.Helpers;
using KyoS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KyoS.Web.Controllers
{
    [Authorize(Roles = "Admin, Mannager")]
    public class DoctorsController : Controller
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;
        
        public DoctorsController(DataContext context, ICombosHelper combosHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }
        
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Doctors.OrderBy(d => d.Name).ToListAsync());
            }
            else
            {
                UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
                                                             .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if (user_logged.Clinic == null)
                {
                    return View(await _context.Doctors.OrderBy(d => d.Name).ToListAsync());
                }

                ClinicEntity clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Id == user_logged.Clinic.Id);

                if (clinic != null)
                {
                    List<DoctorEntity> doctors = await _context.Doctors.OrderBy(d => d.Name).ToListAsync();
                    List<DoctorEntity> doctors_by_clinic = new List<DoctorEntity>();
                    UserEntity user;
                    foreach (DoctorEntity item in doctors)
                    {
                        user = _context.Users.FirstOrDefault(u => u.Id == item.CreatedBy);
                        if (clinic.Users.Contains(user))
                        {
                            doctors_by_clinic.Add(item);
                        }
                    }
                    return View(doctors_by_clinic);
                }
                else
                {
                    return View(null);
                }
            }
        }

        public IActionResult Create(int id = 0)
        {
            if (id == 1)
            {
                ViewBag.Creado = "Y";
            }
            else
            {
                if (id == 2)
                {
                    ViewBag.Creado = "E";
                }
                else
                {
                    ViewBag.Creado = "N";
                }
            }

            DoctorViewModel model = new DoctorViewModel();            
            return View(model);           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel doctorViewModel)
        {
            if (ModelState.IsValid)
            {
                DoctorEntity doctor = await _context.Doctors.FirstOrDefaultAsync(c => c.Name == doctorViewModel.Name);
                if (doctor == null)
                {
                    UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                           .FirstOrDefault(u => u.UserName == User.Identity.Name);

                    DoctorEntity doctorEntity = _converterHelper.ToDoctorEntity(doctorViewModel, true, user_logged.Id);

                    _context.Add(doctorEntity);
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Create", new { id = 1 });
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, $"Already exists the client: {doctorEntity.Name}");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Create", new { id = 2 });
                }
            }
            return View(doctorViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DoctorEntity doctorEntity = await _context.Doctors.FirstOrDefaultAsync(c => c.Id == id);
            if (doctorEntity == null)
            {
                return NotFound();
            }

            DoctorViewModel doctorViewModel = _converterHelper.ToDoctorViewModel(doctorEntity);

            return View(doctorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorViewModel doctorViewModel)
        {
            if (id != doctorViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                           .FirstOrDefault(u => u.UserName == User.Identity.Name);

                DoctorEntity doctorEntity = _converterHelper.ToDoctorEntity(doctorViewModel, false, user_logged.Id);
                _context.Update(doctorEntity);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, $"Already exists the doctor: {doctorEntity.Name}");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }
            }
            return View(doctorViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DoctorEntity doctorEntity = await _context.Doctors.FirstOrDefaultAsync(c => c.Id == id);
            if (doctorEntity == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctorEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}