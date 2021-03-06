﻿using KyoS.Common.Enums;
using KyoS.Web.Data;
using KyoS.Web.Data.Entities;
using KyoS.Web.Helpers;
using KyoS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KyoS.Web.Controllers
{
    [Authorize(Roles = "Admin, Mannager, Supervisor")]
    public class ThemesController : Controller
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;
        public ThemesController(DataContext context, ICombosHelper combosHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
                return View(await _context.Themes.Include(t => t.Clinic).OrderBy(t => t.Day).ToListAsync());
            else
            {
                UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
                                                             .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if (user_logged.Clinic == null)
                    return View(await _context.Themes.Include(t => t.Clinic).OrderBy(t => t.Day).ToListAsync());

                ClinicEntity clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Id == user_logged.Clinic.Id);
                if (clinic != null)
                    return View(await _context.Themes.Include(t => t.Clinic)
                                                          .Where(t => t.Clinic.Id == clinic.Id).OrderBy(f => f.Day).ToListAsync());
                else
                    return View(await _context.Themes.Include(t => t.Clinic).OrderBy(t => t.Day).ToListAsync());
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

            ThemeViewModel model = new ThemeViewModel();

            if (!User.IsInRole("Admin"))
            {
                UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user_logged.Clinic != null)
                {
                    ClinicEntity clinic = _context.Clinics.FirstOrDefault(c => c.Id == user_logged.Clinic.Id);
                    List<SelectListItem> list = new List<SelectListItem>();
                    list.Insert(0, new SelectListItem
                    {
                        Text = clinic.Name,
                        Value = $"{clinic.Id}"
                    });
                    model = new ThemeViewModel
                    {
                        Clinics = list,
                        IdClinic = clinic.Id,
                        Days = _combosHelper.GetComboDays()
                    };
                    return View(model);
                }
            }

            model = new ThemeViewModel
            {
                Days = _combosHelper.GetComboDays(),
                Clinics = _combosHelper.GetComboClinics(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThemeViewModel themeViewModel)
        {
            if (ModelState.IsValid)
            {
                DayOfWeekType day = (themeViewModel.DayId == 1) ? DayOfWeekType.Monday : (themeViewModel.DayId == 2) ? DayOfWeekType.Tuesday :
                                    (themeViewModel.DayId == 3) ? DayOfWeekType.Wednesday : (themeViewModel.DayId == 4) ? DayOfWeekType.Thursday :
                                    (themeViewModel.DayId == 5) ? DayOfWeekType.Friday : DayOfWeekType.Monday;
                ThemeEntity theme = await _context.Themes.FirstOrDefaultAsync((t => (t.Name == themeViewModel.Name && t.Day == day && t.Clinic.Id == themeViewModel.IdClinic)));
                if (theme == null)
                {
                    ThemeEntity themeEntity = await _converterHelper.ToThemeEntity(themeViewModel, true);

                    _context.Add(themeEntity);
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Create", new { id = 1 });
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, $"Already exists the theme: {themeEntity.Day.ToString()} - {themeEntity.Name}");
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
            return View(themeViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ThemeEntity themeEntity = await _context.Themes.FirstOrDefaultAsync(t => t.Id == id);
            if (themeEntity == null)
            {
                return NotFound();
            }

            _context.Themes.Remove(themeEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ThemeEntity themeEntity = await _context.Themes.Include(t => t.Clinic).FirstOrDefaultAsync(t => t.Id == id);
            if (themeEntity == null)
            {
                return NotFound();
            }

            ThemeViewModel themeViewModel = _converterHelper.ToThemeViewModel(themeEntity);

            if (!User.IsInRole("Admin"))
            {
                UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user_logged.Clinic != null)
                {
                    List<SelectListItem> list = new List<SelectListItem>();
                    list.Insert(0, new SelectListItem
                    {
                        Text = user_logged.Clinic.Name,
                        Value = $"{user_logged.Clinic.Id}"
                    });
                    themeViewModel.Clinics = list;
                }
            }

            return View(themeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ThemeViewModel themeViewModel)
        {
            if (id != themeViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ThemeEntity themeEntity = await _converterHelper.ToThemeEntity(themeViewModel, false);
                _context.Update(themeEntity);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, $"Already exists the theme: {themeEntity.Name}");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }
            }
            return View(themeViewModel);
        }
    }
}