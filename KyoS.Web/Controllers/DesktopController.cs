﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KyoS.Common.Enums;
using KyoS.Web.Data;
using KyoS.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KyoS.Web.Controllers
{
    [Authorize]
    public class DesktopController : Controller
    {
        private readonly DataContext _context;
        public DesktopController(DataContext context)
        {
            _context = context;           
        }
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Facilitator"))
            {
                ViewBag.ApprovedNotes = _context.Workdays_Clients
                                                   .Include(wc => wc.Note)
                                                   .Count(wc => (wc.Facilitator.LinkedUser == User.Identity.Name 
                                                              && wc.Note.Status == NoteStatus.Approved)).ToString();

                ViewBag.PendingNotes = _context.Workdays_Clients
                                                  .Include(wc => wc.Note)
                                                  .Count(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                              && wc.Note.Status == NoteStatus.Pending)).ToString();

                ViewBag.InProgressNotes = _context.Workdays_Clients
                                                  .Include(wc => wc.Note)
                                                  .Count(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                              && wc.Note.Status == NoteStatus.Edition)).ToString();
                                
                List <Workday_Client> not_started_list = await _context.Workdays_Clients
                                                          .Include(wc => wc.Note)
                                                          .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                 && wc.Present == true)).ToListAsync();
                not_started_list = not_started_list.Where(wc => wc.Note == null).ToList();
                ViewBag.NotStartedNotes = not_started_list.Count.ToString();

                List<Workday_Client> notes_review_list = await _context.Workdays_Clients
                                                                 .Include(wc => wc.Messages)
                                                                 .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                            && wc.Note.Status == NoteStatus.Pending)).ToListAsync();
                notes_review_list = notes_review_list.Where(wc => wc.Messages.Count() > 0).ToList();
                ViewBag.NotesWithReview = notes_review_list.Count.ToString();

                ViewBag.NotPresentNotes = _context.Workdays_Clients
                                                  .Include(wc => wc.Note)
                                                  .Count(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                              && wc.Present == false)).ToString();
            }
            if (User.IsInRole("Supervisor"))
            {
                UserEntity user_logged = await _context.Users
                                                       .Include(u => u.Clinic)
                                                       .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                ViewBag.PendingActivities = _context.Activities
                                                    .Count(a => (a.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                              && a.Status == ActivityStatus.Pending)).ToString();

                ViewBag.PendingNotes = _context.Workdays_Clients
                                               .Count(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                              && wc.Note.Status == NoteStatus.Pending)).ToString();

                List<ClientEntity> client = await _context.Clients
                                                          .Include(c => c.MTPs)
                                                          .Where(c => c.Clinic.Id == user_logged.Clinic.Id).ToListAsync();
                client = client.Where(wc => wc.MTPs.Count == 0).ToList();
                ViewBag.MTPMissing = client.Count.ToString();

                List<Workday_Client> notes_review_list = await _context.Workdays_Clients
                                                                       .Include(wc => wc.Messages)
                                                                       .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                                               && wc.Note.Status == NoteStatus.Pending)).ToListAsync();
                notes_review_list = notes_review_list.Where(wc => wc.Messages.Count() > 0).ToList();
                ViewBag.NotesWithReview = notes_review_list.Count.ToString();
            }
            if (User.IsInRole("Mannager"))
            {
                UserEntity user_logged = await _context.Users
                                                       .Include(u => u.Clinic)
                                                       .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                ViewBag.PendingNotes = _context.Workdays_Clients
                                               .Count(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                              && wc.Note.Status == NoteStatus.Pending)).ToString();

                ViewBag.InProgressNotes = _context.Workdays_Clients
                                                  .Include(wc => wc.Note)
                                                  .Count(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                              && wc.Note.Status == NoteStatus.Edition)).ToString();

                List<Workday_Client> not_started_list = await _context.Workdays_Clients
                                                          .Include(wc => wc.Note)
                                                          .Where(wc => wc.Facilitator.Clinic.Id == user_logged.Clinic.Id).ToListAsync();
                not_started_list = not_started_list.Where(wc => (wc.Note == null && wc.Present == true)).ToList();
                ViewBag.NotStartedNotes = not_started_list.Count.ToString();

                List<ClientEntity> client = await _context.Clients
                                                          .Include(c => c.MTPs)
                                                          .Where(c => c.Clinic.Id == user_logged.Clinic.Id).ToListAsync();
                client = client.Where(wc => wc.MTPs.Count == 0).ToList();
                ViewBag.MTPMissing = client.Count.ToString();

                List<Workday_Client> notes_review_list = await _context.Workdays_Clients
                                                                      .Include(wc => wc.Messages)
                                                                      .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                                              && wc.Note.Status == NoteStatus.Pending)).ToListAsync();
                notes_review_list = notes_review_list.Where(wc => wc.Messages.Count() > 0).ToList();
                ViewBag.NotesWithReview = notes_review_list.Count.ToString();

                
                //-------------clients without documentation--------------//
                client = await _context.Clients
                                       .Where(c => c.Clinic.Id == user_logged.Clinic.Id).ToListAsync();
                int clients_without_doc = 0;                
                DocumentEntity doc_pe;
                DocumentEntity doc_intake;
                DocumentEntity doc_bio;
                DocumentEntity doc_mtp;
                DocumentEntity doc_fars;
                DocumentEntity doc_consent;
                foreach (var item in client)
                {
                    doc_pe = await _context.Documents
                                             .FirstOrDefaultAsync(d => (d.Description == DocumentDescription.Psychiatrist_evaluation
                                                                        && d.Client.Id == item.Id));
                    doc_intake = await _context.Documents
                                             .FirstOrDefaultAsync(d => (d.Description == DocumentDescription.Intake
                                                                        && d.Client.Id == item.Id));
                    doc_bio = await _context.Documents
                                             .FirstOrDefaultAsync(d => (d.Description == DocumentDescription.Bio
                                                                        && d.Client.Id == item.Id));
                    doc_mtp = await _context.Documents
                                             .FirstOrDefaultAsync(d => (d.Description == DocumentDescription.MTP
                                                                        && d.Client.Id == item.Id));
                    doc_fars = await _context.Documents
                                             .FirstOrDefaultAsync(d => (d.Description == DocumentDescription.Fars
                                                                        && d.Client.Id == item.Id));
                    doc_consent = await _context.Documents
                                             .FirstOrDefaultAsync(d => (d.Description == DocumentDescription.Consent
                                                                        && d.Client.Id == item.Id));

                    if (doc_pe == null || doc_intake == null || doc_bio == null || doc_mtp == null || doc_fars == null || doc_consent == null)
                    {
                        clients_without_doc++;
                    }
                }
                ViewBag.ClientsWithoutDoc = clients_without_doc.ToString();

                ViewBag.ApprovedNotes = _context.Workdays_Clients
                                                .Include(wc => wc.Note)
                                                .Count(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                              && wc.Note.Status == NoteStatus.Approved)).ToString();

                ViewBag.NotPresentNotes = _context.Workdays_Clients
                                                  .Include(wc => wc.Note)
                                                  .Count(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                              && wc.Present == false)).ToString();
            }
            return View();
        }
    }
}