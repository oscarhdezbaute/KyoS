﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KyoS.Common.Enums;
using KyoS.Web.Data;
using KyoS.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace KyoS.Web.Controllers
{
    [Authorize]
    public class TCMMessagesController : Controller
    {
        private readonly DataContext _context;

        public TCMMessagesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .ThenInclude(c => c.Setting)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (User.IsInRole("CaseManager"))
            {
                ViewBag.CountsMessagesNotes = _context.TCMMessages
                                                      .Count(m => (m.To == user_logged.UserName && m.Status == KyoS.Common.Enums.MessageStatus.NotRead
                                                                    && m.TCMNote != null && m.Notification == false))
                                                      .ToString();

                ViewBag.CountsMessagesTCMServicePlan = _context.TCMMessages
                                                               .Count(m => (m.To == user_logged.UserName && m.Status == KyoS.Common.Enums.MessageStatus.NotRead
                                                                            && m.TCMServicePlan != null && m.Notification == false))
                                                               .ToString();
            }
            
            return View();
        }

        [Authorize(Roles = "CaseManager")]
        public async Task<IActionResult> MessagesOfNotes(int id = 0)
        {
            if (User.IsInRole("CaseManager"))
            {
                return View(await _context.TCMNote
                                          .Include(wc => wc.TCMClient.Casemanager)
                                          .Include(wc => wc.TCMClient.Client)
                                          .Include(wc => wc.TCMMessages.Where(m => m.Notification == false))
                                          .Where(wc => (wc.TCMClient.Casemanager.LinkedUser == User.Identity.Name
                                                     && wc.TCMMessages.Count() > 0))
                                          .ToListAsync());
            }

            return View();
        }

        [Authorize(Roles = "CaseManager")]
        public async Task<IActionResult> MessagesOfServicePlan(int id = 0)
        {
            if (User.IsInRole("CaseManager"))
            {
                return View(await _context.TCMServicePlans
                                          .Include(wc => wc.TcmClient.Casemanager)
                                          .Include(wc => wc.TcmClient.Client)
                                          .Include(wc => wc.TCMMessages.Where(m => m.Notification == false))
                                          .Where(wc => (wc.TcmClient.Casemanager.LinkedUser == User.Identity.Name
                                                     && wc.TCMMessages.Count() > 0))
                                          .ToListAsync());
            }

            return View();
        }

        [Authorize(Roles = "CaseManager, TCMSupervisor, Manager")]
        public async Task<IActionResult> Notifications(int id = 0)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .ThenInclude(c => c.Setting)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            return View(await _context.TCMMessages
                                      .Include(m => m.TCMAddendum)
                                      .Include(m => m.TCMAssessment)
                                      .Include(m => m.TCMDischarge)
                                      .Include(m => m.TCMFarsForm)
                                      .Include(m => m.TCMNote)
                                      .Include(m => m.TCMServicePlan)
                                      .Include(m => m.TCMServicePlanReview)
                                      .Where(m => (m.To == user_logged.UserName && m.Notification == true))
                                      .ToListAsync());
        }

        [Authorize(Roles = "TCMSupervisor")]
        public async Task<IActionResult> Reviews(int id = 0)
        {
            TCMMessageEntity notification = await _context.TCMMessages
                                                          .Include(m => m.TCMNote)
                                                          .Include(m => m.TCMAddendum)
                                                          .Include(m => m.TCMAssessment)
                                                          .Include(m => m.TCMDischarge)
                                                          .Include(m => m.TCMFarsForm)
                                                          .Include(m => m.TCMServicePlan)
                                                          .Include(m => m.TCMServicePlanReview)
                                                          .FirstOrDefaultAsync(m => m.Id == id);

            if (notification == null)
            {
                return null;
            }

            if (notification.TCMNote != null)
            {
                return View(await _context.TCMMessages
                                          .Where(m => (m.TCMNote.Id == notification.TCMNote.Id && m.Notification == false))
                                          .ToListAsync());
            }

            if (notification.TCMAddendum != null)
            {
                return View(await _context.TCMMessages
                                          .Where(m => (m.TCMAddendum.Id == notification.TCMAddendum.Id && m.Notification == false))
                                          .ToListAsync());
            }

            if (notification.TCMAssessment != null)
            {
                return View(await _context.TCMMessages
                                          .Where(m => (m.TCMAssessment.Id == notification.TCMAssessment.Id && m.Notification == false))
                                          .ToListAsync());
            }

            if (notification.TCMDischarge != null)
            {
                return View(await _context.TCMMessages
                                          .Where(m => (m.TCMDischarge.Id == notification.TCMDischarge.Id && m.Notification == false))
                                          .ToListAsync());
            }

            if (notification.TCMFarsForm != null)
            {
                return View(await _context.TCMMessages
                                          .Where(m => (m.TCMFarsForm.Id == notification.TCMFarsForm.Id && m.Notification == false))
                                          .ToListAsync());
            }

            if (notification.TCMServicePlan != null)
            {
                return View(await _context.TCMMessages
                                          .Where(m => (m.TCMServicePlan.Id == notification.TCMServicePlan.Id && m.Notification == false))
                                          .ToListAsync());
            }

            if (notification.TCMServicePlanReview != null)
            {
                return View(await _context.TCMMessages
                                          .Where(m => (m.TCMServicePlanReview.Id == notification.TCMServicePlanReview.Id && m.Notification == false))
                                          .ToListAsync());
            }

            return null;
        }

    }
}
