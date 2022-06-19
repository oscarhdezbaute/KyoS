﻿using KyoS.Common.Enums;
using KyoS.Web.Data;
using KyoS.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KyoS.Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly DataContext _context;
        
        public MessagesController(DataContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .ThenInclude(c => c.Setting)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (User.IsInRole("Facilitator"))
            {
                ViewBag.CountsMessagesNotes = _context.Messages
                                                      .Count(m => (m.To == user_logged.UserName && m.Status == KyoS.Common.Enums.MessageStatus.NotRead
                                                                && m.Workday_Client != null))
                                                      .ToString();
                
                ViewBag.CountsMessagesFars = _context.Messages
                                                     .Count(m => (m.To == user_logged.UserName && m.Status == KyoS.Common.Enums.MessageStatus.NotRead
                                                                                               && m.FarsForm != null))
                                                     .ToString();

                ViewBag.CountsMessagesMTPReview = _context.Messages
                                                          .Count(m => (m.To == user_logged.UserName && m.Status == KyoS.Common.Enums.MessageStatus.NotRead
                                                                                                    && m.MTPReview != null))
                                                          .ToString();
                
                ViewBag.CountsMessagesAddendum = _context.Messages
                                                         .Count(m => (m.To == user_logged.UserName && m.Status == KyoS.Common.Enums.MessageStatus.NotRead
                                                                                                   && m.Addendum != null))
                                                         .ToString();

                ViewBag.CountsMessagesDischarge = _context.Messages
                                                          .Count(m => (m.To == user_logged.UserName && m.Status == KyoS.Common.Enums.MessageStatus.NotRead
                                                                                                    && m.Discharge != null))
                                                          .ToString();

                ViewBag.Total = Convert.ToInt32(ViewBag.CountsMessagesNotes) + Convert.ToInt32(ViewBag.CountsMessagesFars) + Convert.ToInt32(ViewBag.CountsMessagesMTPReview) + Convert.ToInt32(ViewBag.CountsMessagesAddendum) + Convert.ToInt32(ViewBag.CountsMessagesDischarge);
            }
            return View();
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> MessagesOfNotes(int id = 0)
        {
            if (User.IsInRole("Facilitator"))
            {
                return View(await _context.Workdays_Clients

                                          .Include(wc => wc.Note)

                                          .Include(wc => wc.NoteP)

                                          .Include(wc => wc.IndividualNote)

                                          .Include(wc => wc.GroupNote)

                                          .Include(wc => wc.Facilitator)

                                          .Include(wc => wc.Client)

                                          .Include(wc => wc.Workday)
                                          .ThenInclude(w => w.Week)

                                          .Include(wc => wc.Messages)

                                          .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name                                                                     
                                                      && wc.Messages.Count() > 0))
                                          .ToListAsync());
            }

            //if (User.IsInRole("Supervisor"))
            //{
            //    UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
            //                                                 .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            //    if (user_logged.Clinic != null)
            //    {
            //        return View(await _context.Workdays_Clients.Include(wc => wc.Note)

            //                                                   .Include(wc => wc.Facilitator)
            //                                                   .ThenInclude(f => f.Clinic)

            //                                                   .Include(wc => wc.Client)

            //                                                   .Include(wc => wc.Workday)
            //                                                   .ThenInclude(w => w.Week)

            //                                                   .Include(wc => wc.Messages)

            //                                                   .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
            //                                                       && (wc.Note.Status == NoteStatus.Pending || wc.NoteP.Status == NoteStatus.Pending)
            //                                                          && wc.Messages.Count() > 0
            //                                                          && (wc.Workday.Service == ServiceType.PSR || wc.Workday.Service == ServiceType.Individual)))
            //                                                   .ToListAsync());
            //    }
            //}

            return View();
        }
    }
}