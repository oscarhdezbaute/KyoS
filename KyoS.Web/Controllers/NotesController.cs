﻿using AspNetCore.Reporting;
using KyoS.Common.Enums;
using KyoS.Common.Helpers;
using KyoS.Web.Data;
using KyoS.Web.Data.Entities;
using KyoS.Web.Helpers;
using KyoS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KyoS.Web.Controllers
{
    public class NotesController : Controller
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IDateHelper _dateHelper;
        private readonly ITranslateHelper _translateHelper;
        private readonly IWebHostEnvironment _webhostEnvironment;
        private readonly IReportHelper _reportHelper;
        private readonly IRenderHelper _renderHelper;
        private readonly IExportExcellHelper _exportExcelHelper;
        private readonly IFileHelper _fileHelper;

        public NotesController(DataContext context, ICombosHelper combosHelper, IConverterHelper converterHelper, IDateHelper dateHelper, ITranslateHelper translateHelper, IWebHostEnvironment webHostEnvironment, IImageHelper imageHelper, IReportHelper reportHelper, IRenderHelper renderHelper, IExportExcellHelper exportExcelHelper, IFileHelper fileHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _dateHelper = dateHelper;
            _translateHelper = translateHelper;
            _webhostEnvironment = webHostEnvironment;
            _imageHelper = imageHelper;
            _reportHelper = reportHelper;
            _renderHelper = renderHelper;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _exportExcelHelper = exportExcelHelper;
            _fileHelper = fileHelper;
        }
        
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> Index(int id = 0)
        {
            if (id == 1)
            {
                ViewBag.FinishEdition = "Y";
            }
           
            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            return View(await _context.Weeks

                                      .Include(w => w.Days)                                            
                                      .ThenInclude(d => d.Workdays_Clients)                                            
                                      .ThenInclude(wc => wc.Client)                          

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(g => g.Facilitator)

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.Note)

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.NoteP)

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.Schedule)

                                      .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                && w.Days.Where(d => (d.Service == ServiceType.PSR && d.Workdays_Clients.Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name).Count() > 0)).Count() > 0))
                                      .ToListAsync());            
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> IndividualNotes(int id = 0)
        {
            if (id == 1)
            {
                ViewBag.FinishEdition = "Y";
            }


            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            return View(await _context.Weeks
                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.Client)
                                      
                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(g => g.Facilitator)

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.IndividualNote)

                                      .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                && w.Days.Where(d => (d.Service == ServiceType.Individual && d.Workdays_Clients.Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name).Count() > 0)).Count() > 0))
                                      .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> GroupNotes(int id = 0)
        {
            if (id == 1)
            {
                ViewBag.FinishEdition = "Y";
            }


            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            return View(await _context.Weeks
                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.Client)                                      

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(g => g.Facilitator)

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.GroupNote)

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.Schedule)

                                      .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                && w.Days.Where(d => (d.Service == ServiceType.Group && d.Workdays_Clients.Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name).Count() > 0)).Count() > 0))                                               
                                            
                                      .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> Present(int id, int origin = 0)
        {
            Workday_Client workdayClient = await _context.Workdays_Clients
                                                         .Include(wc => wc.Workday)
                                                         .Include(wc => wc.Client)
                                                         .ThenInclude(c => c.Group)
                                                         .Include(wc => wc.Schedule)
                                                         .Include(g => g.Facilitator)
                                                         .FirstOrDefaultAsync(wc => wc.Id == id);
           
            Workday_ClientViewModel model = new Workday_ClientViewModel();

            model = new Workday_ClientViewModel
            {
                Id = workdayClient.Id,
                Workday = workdayClient.Workday,
                Client = (workdayClient.Client != null) ? workdayClient.Client : null,
                Facilitator = workdayClient.Facilitator,
                Session = workdayClient.Session,
                Present = workdayClient.Present,
                Note = workdayClient.Note,
                IndividualNote = workdayClient.IndividualNote
            };

            model.Origin = origin;
            model.CauseOfNotPresent = (string.IsNullOrEmpty(workdayClient.CauseOfNotPresent)) ?
                                            "The client was absent to PSR session today, because of a personal matter." : workdayClient.CauseOfNotPresent;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> Present(Workday_ClientViewModel model, IFormCollection form)
        {
            Workday_Client entity = await _context.Workdays_Clients
                                                  .Include(wc => wc.Workday)
                                                  .Include(wc => wc.Client)
                                                  .ThenInclude(c => c.Group)
                                                  .Include(wc => wc.Facilitator)
                                                  .FirstOrDefaultAsync(wc => wc.Id == model.Id);

            if (entity == null)
            {
                return RedirectToAction("Home/Error404");
            }

            switch (form["Present"])
            {
                case "present":
                    {
                        entity.Present = true;
                        entity.CauseOfNotPresent = string.Empty;
                        break;
                    }
                case "nopresent":
                    {
                        entity.Present = false;
                        entity.CauseOfNotPresent = model.CauseOfNotPresent;
                        break;
                    }
                default:
                    break;
            }

            _context.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
                if (model.Origin == 0)
                    return RedirectToAction(nameof(Index));
                if (model.Origin == 1)
                    return RedirectToAction(nameof(NotStartedNotes));
                if (model.Origin == 2)
                    return RedirectToAction(nameof(NotesInEdit));
                if (model.Origin == 3)
                    return RedirectToAction(nameof(NotStartedIndNotes));
                if (model.Origin == 4)
                    return RedirectToAction(nameof(IndividualNotes));
                if (model.Origin == 5)
                    return RedirectToAction(nameof(IndNotesInEdit));
                if (model.Origin == 6)
                    return RedirectToAction(nameof(GroupNotes));
                if (model.Origin == 7)
                    return RedirectToAction(nameof(NotStartedGroupNotes));
                if (model.Origin == 8)
                    return RedirectToAction(nameof(GroupNotesInEdit));
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, $"Already exists the element");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }

            return View(_converterHelper.ToWorkdayClientViewModel(entity,false));
        }

        //PSR Notes (Schema 1, 2, 4)
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditNote(int id, int error = 0, int origin = 0, string errorText = "")
        {
            FacilitatorEntity facilitator_logged = _context.Facilitators

                                                           .Include(f => f.Clinic)

                                                           .FirstOrDefault(f => f.LinkedUser == User.Identity.Name);

            NoteEntity note = await _context.Notes

                                            .Include(n => n.Workday_Cient)
                                            .ThenInclude(wc => wc.Client)
                                            .ThenInclude(c => c.Group)

                                            .Include(n => n.Workday_Cient)
                                            .ThenInclude(g => g.Facilitator)

                                            .Include(n => n.Notes_Activities)
                                            .ThenInclude(na => na.Activity)

                                            .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            if ((note == null) && (facilitator_logged.Clinic.Schema == Common.Enums.SchemaType.Schema3))
            {
                return RedirectToAction(nameof(EditNoteP), new { id = id, origin = origin});
            }

            NoteViewModel noteViewModel;
            List<ThemeEntity> topics;
            List<SelectListItem> list1 = new List<SelectListItem>();
            List<SelectListItem> list2 = new List<SelectListItem>();
            List<SelectListItem> list3 = new List<SelectListItem>();
            List<SelectListItem> list4 = new List<SelectListItem>();
            
            //la nota no tiene linkeado ningun goal
            if (error == 1)  
                ViewBag.Error = "0";

            //la nota no esta completa, faltan campos por editar
            if (error == 2)
                ViewBag.Error = "2";

            //la nota tiene problemas con el genero
            if (error == 4)
            {
                ViewBag.Error = "4";
                ViewBag.errorText = errorText;
            }

            //el cliente seleccionado tiene una nota ya creada de otro servicio en ese mismo horario
            if (error == 5)
            {
                ViewBag.Error = "5";
            }

            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)
                                                                           .ThenInclude(w => w.Workdays_Activities_Facilitators)
                                                                           .ThenInclude(waf => waf.Activity)
                                                                           .ThenInclude(a => a.Theme)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.MTPs)

                                                                           .Include(wc => wc.Facilitator)
                                                                           .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            //el dia no tiene actividad asociada para el facilitator logueado por lo tanto no se puede crear la nota
            if (workday_Client.Workday.Workdays_Activities_Facilitators.Where(waf => waf.Facilitator == facilitator_logged).Count() == 0)
            {
                ViewBag.Error = "1";
                noteViewModel = new NoteViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }

            //el cliente no tiene mtp activos
            if (workday_Client.Client.MTPs.Where(m => m.Active == true).Count() == 0)
            {
                ViewBag.Error = "3";
                noteViewModel = new NoteViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }

            topics = await _context.Themes.Where(t => t.Clinic.Id == workday_Client.Client.Clinic.Id)
                                          .ToListAsync();
            topics = topics.Where(t => t.Day.ToString() == workday_Client.Workday.Day)
                                            .ToList();

            int index = 0;
            foreach (ThemeEntity value in topics)
            {
                if (index == 0)
                {
                    list1.Insert(index, new SelectListItem
                    {
                        Text = value.Name,
                        Value = $"{value.Id}"
                    });
                    index = ++index;
                    continue;
                }
                if (index == 1)
                {
                    list2.Insert(0, new SelectListItem
                    {
                        Text = value.Name,
                        Value = $"{value.Id}"
                    });
                    index = ++index;
                    continue;
                }
                if (index == 2)
                {
                    list3.Insert(0, new SelectListItem
                    {
                        Text = value.Name,
                        Value = $"{value.Id}"
                    });
                    index = ++index;
                    continue;
                }
                if (index == 3)
                {
                    list4.Insert(0, new SelectListItem
                    {
                        Text = value.Name,
                        Value = $"{value.Id}"
                    });
                    index = ++index;
                    continue;
                }
            }

            //-----------se selecciona el primer MTP activo que tenga el cliente-----------//
            MTPEntity mtp = _context.MTPs.FirstOrDefault(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));

            List<Workday_Activity_Facilitator> activities = workday_Client.Workday
                                                                          .Workdays_Activities_Facilitators
                                                                          .Where(waf => waf.Facilitator == facilitator_logged)
                                                                          .ToList();

            //Evaluate setting for goals's classification
            SettingEntity setting = _context.Settings
                                            .FirstOrDefault(s => s.Clinic.Id == facilitator_logged.Clinic.Id);

            if (note == null)   //la nota no está creada
            {
                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.PSR);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }
                    
                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }                

                noteViewModel = new NoteViewModel
                {
                    Id = id,
                    Status = NoteStatus.Pending,    //es solo generico para la visualizacion del btn FinishEditing
                    Origin = origin,
                    Schema = workday_Client.Client.Clinic.Schema,
                    CodeBill = workday_Client.Client.Clinic.CodePSRTherapy,

                    //IdTopic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Id : 0,
                    Topic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    Activity1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    Goals1 = goals,
                    Objetives1 = objs,

                    //IdTopic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Id : 0,
                    Topic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Name : string.Empty,
                    IdActivity2 = (activities.Count > 1) ? activities[1].Activity.Id : 0,
                    Activity2 = (activities.Count > 1) ? activities[1].Activity.Name : string.Empty,
                    Goals2 = goals,
                    Objetives2 = objs,

                    //IdTopic3 = (activities.Count > 2) ? activities[2].Activity.Theme.Id : 0,
                    Topic3 = (activities.Count > 2) ? activities[2].Activity.Theme.Name : string.Empty,
                    IdActivity3 = (activities.Count > 2) ? activities[2].Activity.Id : 0,
                    Activity3 = (activities.Count > 2) ? activities[2].Activity.Name : string.Empty,
                    Goals3 = goals,
                    Objetives3 = objs,

                    //IdTopic4 = (activities.Count > 3) ? activities[3].Activity.Theme.Id : 0,
                    Topic4 = (activities.Count > 3) ? activities[3].Activity.Theme.Name : string.Empty,
                    IdActivity4 = (activities.Count > 3) ? activities[3].Activity.Id : 0,
                    Activity4 = (activities.Count > 3) ? activities[3].Activity.Name : string.Empty,
                    Goals4 = goals,
                    Objetives4 = objs,

                    Workday_Cient = workday_Client
                };
            }
            else
            {
                List<Note_Activity> note_Activity = await _context.Notes_Activities

                                                                  .Include(na => na.Activity)
                                                                  .ThenInclude(a => a.Theme)

                                                                  .Include(na => na.Objetive)
                                                                  .ThenInclude(o => o.Goal)

                                                                  .Where(na => na.Note.Id == note.Id)
                                                                  .ToListAsync();

                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.PSR);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }

                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }

                noteViewModel = new NoteViewModel
                {
                    Id = id,
                    Origin = origin,
                    Workday_Cient = workday_Client,
                    Schema = note.Schema,
                    PlanNote = note.PlanNote,
                    Status = note.Status,
                    CodeBill = workday_Client.CodeBill,

                    OrientedX3 = note.OrientedX3,
                    NotTime = note.NotTime,
                    NotPlace = note.NotPlace,
                    NotPerson = note.NotPerson,
                    Present = note.Present,
                    Adequate = note.Adequate,
                    Limited = note.Limited,
                    Impaired = note.Impaired,
                    Faulty = note.Faulty,
                    Euthymic = note.Euthymic,
                    Congruent = note.Congruent,
                    Negativistic = note.Negativistic,
                    Depressed = note.Depressed,
                    Euphoric = note.Euphoric,
                    Optimistic = note.Optimistic,
                    Anxious = note.Anxious,
                    Hostile = note.Hostile,
                    Withdrawn = note.Withdrawn,
                    Irritable = note.Irritable,
                    Dramatized = note.Dramatized,
                    AdequateAC = note.AdequateAC,
                    Inadequate = note.Inadequate,
                    Fair = note.Fair,
                    Unmotivated = note.Unmotivated,
                    Motivated = note.Motivated,
                    Guarded = note.Guarded,
                    Normal = note.Normal,
                    ShortSpanned = note.ShortSpanned,
                    MildlyImpaired = note.MildlyImpaired,
                    SeverelyImpaired = note.SeverelyImpaired,

                    //IdTopic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Id : 0,
                    Topic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    Activity1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    AnswerClient1 = note_Activity[0].AnswerClient,
                    AnswerFacilitator1 = note_Activity[0].AnswerFacilitator,
                    IdGoal1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Goal.Id : 0,
                    Goals1 = goals,
                    IdObjetive1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Id : 0,
                    //Paso el IdGoal1 como parametro
                    Objetives1 = _combosHelper.GetComboObjetives(((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) 
                                                                        ? note_Activity[0].Objetive.Goal.Id : 0),
                    Intervention1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Intervention : string.Empty,

                    //IdTopic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Id : 0,
                    Topic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Name : string.Empty,
                    IdActivity2 = (activities.Count > 1) ? activities[1].Activity.Id : 0,
                    Activity2 = (activities.Count > 1) ? activities[1].Activity.Name : string.Empty,
                    AnswerClient2 = note_Activity[1].AnswerClient,
                    AnswerFacilitator2 = note_Activity[1].AnswerFacilitator,
                    IdGoal2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Goal.Id : 0,
                    Goals2 = goals,
                    IdObjetive2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Id : 0,
                    //Paso el IdGoal2 como parametro
                    Objetives2 = _combosHelper.GetComboObjetives(((note_Activity.Count > 1) && (note_Activity[1].Objetive != null))
                                                                        ? note_Activity[1].Objetive.Goal.Id : 0),
                    Intervention2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Intervention : string.Empty,

                    //IdTopic3 = (activities.Count > 2) ? activities[2].Activity.Theme.Id : 0,
                    Topic3 = (activities.Count > 2) ? activities[2].Activity.Theme.Name : string.Empty,
                    IdActivity3 = (activities.Count > 2) ? activities[2].Activity.Id : 0,
                    Activity3 = (activities.Count > 2) ? activities[2].Activity.Name : string.Empty,
                    AnswerClient3 = note_Activity[2].AnswerClient,
                    AnswerFacilitator3 = note_Activity[2].AnswerFacilitator,
                    IdGoal3 = ((note_Activity.Count > 2) && (note_Activity[2].Objetive != null)) ? note_Activity[2].Objetive.Goal.Id : 0,
                    Goals3 = goals,
                    IdObjetive3 = ((note_Activity.Count > 2) && (note_Activity[2].Objetive != null)) ? note_Activity[2].Objetive.Id : 0,
                    //Paso el IdGoal3 como parametro
                    Objetives3 = _combosHelper.GetComboObjetives(((note_Activity.Count > 2) && (note_Activity[2].Objetive != null))
                                                                        ? note_Activity[2].Objetive.Goal.Id : 0),
                    Intervention3 = ((note_Activity.Count > 2) && (note_Activity[2].Objetive != null)) ? note_Activity[2].Objetive.Intervention : string.Empty,

                    //IdTopic4 = (activities.Count > 3) ? activities[3].Activity.Theme.Id : 0,
                    Topic4 = (activities.Count > 3) ? activities[3].Activity.Theme.Name : string.Empty,
                    IdActivity4 = (activities.Count > 3) ? activities[3].Activity.Id : 0,
                    Activity4 = (activities.Count > 3) ? activities[3].Activity.Name : string.Empty,
                    AnswerClient4 = note_Activity[3].AnswerClient,
                    AnswerFacilitator4 = note_Activity[3].AnswerFacilitator,
                    IdGoal4 = ((note_Activity.Count > 3) && (note_Activity[3].Objetive != null)) ? note_Activity[3].Objetive.Goal.Id : 0,
                    Goals4 = goals,
                    IdObjetive4 = ((note_Activity.Count > 3) && (note_Activity[3].Objetive != null)) ? note_Activity[3].Objetive.Id : 0,
                    //Paso el IdGoal4 como parametro
                    Objetives4 = _combosHelper.GetComboObjetives(((note_Activity.Count > 3) && (note_Activity[3].Objetive != null))
                                                                        ? note_Activity[3].Objetive.Goal.Id : 0),
                    Intervention4 = ((note_Activity.Count > 3) && (note_Activity[3].Objetive != null)) ? note_Activity[3].Objetive.Intervention : string.Empty,
                };
            }
            return View(noteViewModel);
        }

        //PSR Notes (Schema 1, 2, 4)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditNote(NoteViewModel model, IFormCollection form)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.MTPs)

                                                                           .Include(wc => wc.Facilitator)
                                                                           
                                                                           .FirstOrDefaultAsync(wc => wc.Id == model.Id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            NoteEntity noteEntity;
            if (ModelState.IsValid)
            {
                NoteEntity note = await _context.Notes.Include(n => n.Workday_Cient)
                                                      .ThenInclude(wc => wc.Messages)
                                                      .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);
                Note_Activity note_Activity;
                string progress;
                if (note == null)   //la nota no está creada
                {
                    //Verify the client is not present in other services of notes at the same time
                    if (this.VerifyNotesAtSameTime(workday_Client.Client.Id, workday_Client.Session, workday_Client.Workday.Date))
                    {
                        return RedirectToAction(nameof(EditNote), new { id = model.Id, error = 5, origin = model.Origin });
                    }

                    //actualizo el progress seleccionado en el plan
                    progress = (form["Progress"] == "SignificantProgress") ? "Significant progress " :
                                (form["Progress"] == "ModerateProgress") ? "Moderate progress " :
                                 (form["Progress"] == "MinimalProgress") ? "Minimal progress " :
                                  (form["Progress"] == "NoProgress") ? "No progress " :
                                   (form["Progress"] == "Regression") ? "Regression " :
                                    (form["Progress"] == "Decompensating") ? "Decompensating " :
                                     (form["Progress"] == "Unable") ? "Unable "
                                      : string.Empty;
                    progress = $"{progress}{RandomGenerator()}";
                    model.PlanNote = (model.PlanNote.Trim().Last() == '.') ? $"{progress}{model.PlanNote.Trim()}" : $"{progress}{model.PlanNote.Trim()}.";

                    noteEntity = await _converterHelper.ToNoteEntity(model, true);      
                    //Update plan progress
                    noteEntity.SignificantProgress = (form["Progress"] == "SignificantProgress") ? true : false;
                    noteEntity.ModerateProgress = (form["Progress"] == "ModerateProgress") ? true : false;
                    noteEntity.MinimalProgress = (form["Progress"] == "MinimalProgress") ? true : false;
                    noteEntity.NoProgress = (form["Progress"] == "NoProgress") ? true : false;
                    noteEntity.Regression = (form["Progress"] == "Regression") ? true : false;
                    noteEntity.Decompensating = (form["Progress"] == "Decompensating") ? true : false;
                    noteEntity.UnableToDetermine = (form["Progress"] == "Unable") ? true : false;
                    noteEntity.Setting = workday_Client.Client.MTPs.FirstOrDefault().Setting;
                    
                    //vinculo el mtp activo del cliente a la nota que se creará
                    Workday_Client workday_client = await _context.Workdays_Clients
                                                                  .Include(wd => wd.Client)
                                                                  .FirstOrDefaultAsync(wd => wd.Id == model.Id);
                    MTPEntity mtp = await _context.MTPs
                                                  .Include(n => n.MtpReviewList)
                                                  .FirstOrDefaultAsync(m => (m.Client.Id == workday_client.Client.Id && m.Active == true));

                    bool mtpExpired = false;
                    if (mtp != null)
                    {
                        noteEntity.MTPId = mtp.Id;
                        foreach (var item in mtp.MtpReviewList)
                        {
                            if (item.DataOfService.AddMonths(item.MonthOfTreatment) > workday_Client.Workday.Date)
                            {
                                mtpExpired = true;
                            }
                        }
                        if (mtp.AdmissionDateMTP.AddMonths(mtp.NumberOfMonths.Value) > workday_Client.Workday.Date)
                        {
                            mtpExpired = true;
                        }
                    }

                    if (mtpExpired == false)
                    {
                        return RedirectToAction("NotStartedNotes", "Notes", new { name = workday_Client.Facilitator.Name, id = 0, expired = 1 });
                    }

                    _context.Add(noteEntity);

                    bool bandera = false;
                    int idSchedule = 0;
                    if (workday_Client.Schedule != null)
                    {
                        bandera = true;
                        idSchedule = workday_Client.Schedule.Id;
                    }

                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == idSchedule).OrderBy(n => n.InitialTime).ToListAsync();

                    note_Activity = new Note_Activity
                    {
                        Note = noteEntity,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1),
                        AnswerClient = model.AnswerClient1.Trim(),
                        AnswerFacilitator = (model.AnswerFacilitator1.Trim().Last() == '.') ? model.AnswerFacilitator1.Trim() : $"{model.AnswerFacilitator1.Trim()}.",
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1),
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(0) : null
                    };
                    _context.Add(note_Activity);
                    note_Activity = new Note_Activity
                    {
                        Note = noteEntity,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity2),
                        AnswerClient = (model.AnswerClient2 != null) ? model.AnswerClient2.Trim() : string.Empty,
                        AnswerFacilitator = (model.AnswerFacilitator2 != null) ? ((model.AnswerFacilitator2.Trim().Last() == '.') ? model.AnswerFacilitator2.Trim() : $"{model.AnswerFacilitator2.Trim()}.") : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive2),
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(1) : null
                    };
                    _context.Add(note_Activity);
                    note_Activity = new Note_Activity
                    {
                        Note = noteEntity,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity3),
                        AnswerClient = (model.AnswerClient3 != null) ? model.AnswerClient3.Trim() : string.Empty,
                        AnswerFacilitator = (model.AnswerFacilitator3 != null) ? ((model.AnswerFacilitator3.Trim().Last() == '.') ? model.AnswerFacilitator3.Trim() : $"{model.AnswerFacilitator3.Trim()}.") : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive3),
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(2) : null
                    };
                    _context.Add(note_Activity);
                    note_Activity = new Note_Activity
                    {
                        Note = noteEntity,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity4),
                        AnswerClient = (model.AnswerClient4 != null) ? model.AnswerClient4.Trim() : string.Empty,
                        AnswerFacilitator = (model.AnswerFacilitator4 != null) ? ((model.AnswerFacilitator4.Trim().Last() == '.') ? model.AnswerFacilitator4.Trim() : $"{model.AnswerFacilitator4.Trim()}.") : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive4),
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(3) : null
                    };
                    _context.Add(note_Activity);

                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if(model.Origin == 0)
                            return RedirectToAction(nameof(Index));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedNotes));
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
                else    //la nota está creada y sólo se debe actualizar
                {
                    //I verify that the note has a selected goal
                    if (note.Status == NoteStatus.Pending)
                    {
                        if (model.IdObjetive1 == 0 && model.IdObjetive2 == 0 && model.IdObjetive3 == 0 && model.IdObjetive4 == 0)
                        {
                            return RedirectToAction(nameof(EditNote), new { id = model.Id, error = 1, origin = model.Origin });
                        }
                    }

                    note.PlanNote = (model.PlanNote.Trim().Last() == '.') ? model.PlanNote.Trim() : $"{model.PlanNote.Trim()}.";
                    note.OrientedX3 = model.OrientedX3;
                    note.NotTime = model.NotTime;
                    note.NotPlace = model.NotPlace;
                    note.NotPerson = model.NotPerson;
                    note.Present = model.Present;
                    note.Adequate = model.Adequate;
                    note.Limited = model.Limited;
                    note.Impaired = model.Impaired;
                    note.Faulty = model.Faulty;
                    note.Euthymic = model.Euthymic;
                    note.Congruent = model.Congruent;
                    note.Negativistic = model.Negativistic;
                    note.Depressed = model.Depressed;
                    note.Euphoric = model.Euphoric;
                    note.Optimistic = model.Optimistic;
                    note.Anxious = model.Anxious;
                    note.Hostile = model.Hostile;
                    note.Withdrawn = model.Withdrawn;
                    note.Irritable = model.Irritable;
                    note.Dramatized = model.Dramatized;
                    note.AdequateAC = model.AdequateAC;
                    note.Inadequate = model.Inadequate;
                    note.Fair = model.Fair;
                    note.Unmotivated = model.Unmotivated;
                    note.Motivated = model.Motivated;
                    note.Guarded = model.Guarded;
                    note.Normal = model.Normal;
                    note.ShortSpanned = model.ShortSpanned;
                    note.MildlyImpaired = model.MildlyImpaired;
                    note.SeverelyImpaired = model.SeverelyImpaired;

                    //actualizo el mtp activo del cliente a la nota que se creará
                    Workday_Client workday_client = await _context.Workdays_Clients
                                                                  .Include(wd => wd.Client)
                                                                  .FirstOrDefaultAsync(wd => wd.Id == model.Id);
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == workday_client.Client.Id && m.Active == true));
                    if (mtp != null)
                        note.MTPId = mtp.Id;

                    _context.Update(note);
                    List<Note_Activity> noteActivities_list = await _context.Notes_Activities
                                                                            .Where(na => na.Note.Id == note.Id)
                                                                            .ToListAsync();
                    foreach (Note_Activity item in noteActivities_list)
                    {
                        _context.Remove(item);
                    }

                    bool bandera = false;
                    int idSchedule = 0;
                    if (workday_Client.Schedule != null)
                    {
                        bandera = true;
                        idSchedule = workday_Client.Schedule.Id;
                    }

                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == idSchedule).OrderBy(n => n.InitialTime).ToListAsync();

                    note_Activity = new Note_Activity
                    {
                        Note = note,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1),
                        AnswerClient = model.AnswerClient1.Trim(),
                        AnswerFacilitator = (model.AnswerFacilitator1.Trim().Last() == '.') ? model.AnswerFacilitator1.Trim() : $"{model.AnswerFacilitator1.Trim()}.",
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1),
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(0) : null
                    };
                    _context.Add(note_Activity);                    
                    await _context.SaveChangesAsync();

                    note_Activity = new Note_Activity
                    {
                        Note = note,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity2),
                        AnswerClient = (model.AnswerClient2 != null) ? model.AnswerClient2.Trim() : string.Empty,
                        AnswerFacilitator = (model.AnswerFacilitator2 != null) ? ((model.AnswerFacilitator2.Trim().Last() == '.') ? model.AnswerFacilitator2.Trim() : $"{model.AnswerFacilitator2.Trim()}.") : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive2),
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(1) : null
                    };
                    _context.Add(note_Activity);
                    await _context.SaveChangesAsync();

                    note_Activity = new Note_Activity
                    {
                        Note = note,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity3),
                        AnswerClient = (model.AnswerClient3 != null) ? model.AnswerClient3.Trim() : string.Empty,
                        AnswerFacilitator = (model.AnswerFacilitator3 != null) ? ((model.AnswerFacilitator3.Trim().Last() == '.') ? model.AnswerFacilitator3.Trim() : $"{model.AnswerFacilitator3.Trim()}.") : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive3),
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(2) : null
                    };
                    _context.Add(note_Activity);
                    await _context.SaveChangesAsync();

                    note_Activity = new Note_Activity
                    {
                        Note = note,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity4),
                        AnswerClient = (model.AnswerClient4 != null) ? model.AnswerClient4.Trim() : string.Empty,
                        AnswerFacilitator = (model.AnswerFacilitator4 != null) ? ((model.AnswerFacilitator4.Trim().Last() == '.') ? model.AnswerFacilitator4.Trim() : $"{model.AnswerFacilitator4.Trim()}.") : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive4),
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(3) : null
                    };
                    _context.Add(note_Activity);

                    List<MessageEntity> messages = note.Workday_Cient.Messages.Where(m => (m.Status == MessageStatus.NotRead && m.Notification == false)).ToList();
                    //todos los mensajes no leidos que tiene el Workday_Client de la nota los pongo como leidos
                    foreach (MessageEntity value in messages)
                    {
                        value.Status = MessageStatus.Read;
                        value.DateRead = DateTime.Now;
                        _context.Update(value);

                        //I generate a notification to supervisor
                        MessageEntity notification = new MessageEntity
                        {
                            Workday_Client = workday_Client,
                            FarsForm = null,
                            MTPReview = null,
                            Addendum = null,
                            Discharge = null,
                            Title = "Update on reviewed PSR note",
                            Text = $"The PSR note of {workday_Client.ClientName} on {workday_Client.Workday.Date.ToShortDateString()} was rectified",
                            From = value.To,
                            To = value.From,
                            DateCreated = DateTime.Now,
                            Status = MessageStatus.NotRead,
                            Notification = true
                        };
                        _context.Add(notification);
                    }
                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }
                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(Index));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedNotes));
                        if (model.Origin == 2)
                            return RedirectToAction(nameof(NotesInEdit));
                        if (model.Origin == 3)
                            return RedirectToAction(nameof(PendingNotes));
                        if (model.Origin == 4)
                            return RedirectToAction(nameof(NotesWithReview));
                        if (model.Origin == 5)
                            return RedirectToAction("MessagesOfNotes", "Messages");
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
            }

            return View(model);
        }

        //PSR Notes (Schema 3)
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditNoteP(int id, int error = 0, int origin = 0, string errorText = "")
        {
            NotePViewModel noteViewModel;
            bool am = false;
            bool pm = false;
           
            //la nota no tiene linkeado ningun goal
            if (error == 1)
            ViewBag.Error = "0";

            //la nota no esta completa, faltan campos por editar
            if (error == 2)
                ViewBag.Error = "2";

            //la nota tiene problemas con el genero
            if (error == 4)
            {
                ViewBag.Error = "4";
                ViewBag.errorText = errorText;
            }

            //el cliente seleccionado tiene una nota ya creada de otro servicio en ese mismo horario
            if (error == 5)
            {
                ViewBag.Error = "5";
            }

            //la nota no tiene seleccionada las respuestas de clientes
            if (error == 6)
            {
                ViewBag.Error = "6";
            }

            Workday_Client workday_Client = await _context.Workdays_Clients
                                                          
                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Workdays_Activities_Facilitators)
                                                          .ThenInclude(waf => waf.Activity)
                                                          .ThenInclude(a => a.Theme)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.Clinic)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.Group)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.MTPs)

                                                          .Include(wc => wc.Facilitator)

                                                          .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (workday_Client.Session == "AM")
            {
                am = true;
                ViewData["session"] = "AM";
            }
            else
            {
                pm = true;
                ViewData["session"] = "PM";
            }

            FacilitatorEntity facilitator_logged = _context.Facilitators
                                                           .FirstOrDefault(f => f.LinkedUser == User.Identity.Name);

            //el dia no tiene actividad asociada para el facilitator logueado por lo tanto no se puede crear la nota
            List<Workday_Activity_Facilitator> workday_Activity_Facilitators = workday_Client.Workday.Workdays_Activities_Facilitators
                                                                                                     .Where(waf => waf.Facilitator == facilitator_logged)   
                                                                                                     .ToList();
            if (workday_Activity_Facilitators.Count() == 0)
            {
                ViewBag.Error = "1";
                noteViewModel = new NotePViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }
            else
            {
                if (am == true)
                {
                    
                    if (workday_Activity_Facilitators.Where(n => n.AM == am).Count() == 0)
                    {
                        ViewBag.Error = "1";
                        noteViewModel = new NotePViewModel
                        {
                            Id = workday_Client.Workday.Id,
                        };
                       
                        return View(noteViewModel);
                    }
                }
                if (pm == true)
                {
                    if (workday_Activity_Facilitators.Where(n => n.PM == pm).Count() == 0)
                    {
                        ViewBag.Error = "1";
                        noteViewModel = new NotePViewModel
                        {
                            Id = workday_Client.Workday.Id,
                        };
                        
                        return View(noteViewModel);
                    }
                }
            }

            //el cliente no tiene mtp activos
            if (workday_Client.Client.MTPs.Where(m => m.Active == true).Count() == 0)
            {
                ViewBag.Error = "3";
                noteViewModel = new NotePViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }

            NotePEntity note = await _context.NotesP

                                            .Include(n => n.Workday_Cient)
                                            .ThenInclude(wc => wc.Client)
                                            .ThenInclude(c => c.Group)

                                            .Include(n => n.Workday_Cient)
                                            .ThenInclude(g => g.Facilitator)

                                            .Include(n => n.NotesP_Activities)
                                            .ThenInclude(na => na.Activity)

                                            .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            //-----------se selecciona el primer MTP activo que tenga el cliente-----------//
            MTPEntity mtp = _context.MTPs
                                    .Include(n => n.MtpReviewList)
                                    .FirstOrDefault(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
            bool mtpExpired = false;
            if (mtp != null)
            {
                foreach (var item in mtp.MtpReviewList)
                {
                    if (item.DataOfService.AddMonths(item.MonthOfTreatment) > workday_Client.Workday.Date)
                    {
                        mtpExpired = true;
                    }
                }
                if (mtp.AdmissionDateMTP.AddMonths(mtp.NumberOfMonths.Value) > workday_Client.Workday.Date)
                {
                    mtpExpired = true;
                }
            }

            if (mtpExpired == false)
            {
                return RedirectToAction("NotStartedNotes", "Notes", new { name = workday_Client.Facilitator.Name, id = 0, expired = 1});
            }

            List<Workday_Activity_Facilitator> activities;
            if (am == true)
            {
                 activities = workday_Client.Workday
                                            .Workdays_Activities_Facilitators
                                            .Where(waf => (waf.Facilitator == facilitator_logged
                                                && waf.AM == true))
                                            .ToList();
            }
            else
            {
                activities = workday_Client.Workday
                                           .Workdays_Activities_Facilitators
                                           .Where(waf => (waf.Facilitator == facilitator_logged
                                               && waf.PM == true))
                                           .ToList();
            }

            //Evaluate setting for goals's classification
            SettingEntity setting = _context.Settings
                                            .FirstOrDefault(s => s.Clinic.Id == facilitator_logged.Clinic.Id);

            if (note == null)   //la nota no está creada
            {
                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.PSR);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }

                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }

                noteViewModel = new NotePViewModel
                {
                    Id = id,
                    Status = NoteStatus.Pending,    //es solo generico para la visualizacion del btn FinishEditing
                    Origin = origin,
                    Schema = workday_Client.Client.Clinic.Schema,
                    Setting = "53",
                    CodeBill = workday_Client.Client.Clinic.CodePSRTherapy,

                    Present1 = true,
                    Theme1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    Goals1 = goals,
                    Objetives1 = objs,
                    activityDailyLiving1 = activities[0].activityDailyLiving == null ? false : Convert.ToBoolean(activities[0].activityDailyLiving),
                    communityResources1 = activities[0].communityResources == null ? false : Convert.ToBoolean(activities[0].communityResources),
                    copingSkills1 = activities[0].copingSkills == null ? false : Convert.ToBoolean(activities[0].copingSkills),
                    diseaseManagement1 = activities[0].diseaseManagement == null ? false : Convert.ToBoolean(activities[0].diseaseManagement),
                    healthyLiving1 = activities[0].healthyLiving == null ? false : Convert.ToBoolean(activities[0].healthyLiving),
                    lifeSkills1 = activities[0].lifeSkills == null ? false : Convert.ToBoolean(activities[0].lifeSkills),
                    relaxationTraining1 = activities[0].relaxationTraining == null ? false : Convert.ToBoolean(activities[0].relaxationTraining),
                    socialSkills1 = activities[0].socialSkills == null ? false : Convert.ToBoolean(activities[0].socialSkills),
                    stressManagement1 = activities[0].stressManagement == null ? false : Convert.ToBoolean(activities[0].stressManagement),

                    Present2 = true,
                    Theme2 = (activities.Count > 1) ? activities[1].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention2 = (activities.Count > 1) ? activities[1].Activity.Name : string.Empty,
                    IdActivity2 = (activities.Count > 1) ? activities[1].Activity.Id : 0,
                    Goals2 = goals,
                    Objetives2 = objs,
                    activityDailyLiving2 = activities[1].activityDailyLiving == null ? false : Convert.ToBoolean(activities[1].activityDailyLiving),
                    communityResources2 = activities[1].communityResources == null ? false : Convert.ToBoolean(activities[1].communityResources),
                    copingSkills2 = activities[1].copingSkills == null ? false : Convert.ToBoolean(activities[1].copingSkills),
                    diseaseManagement2 = activities[1].diseaseManagement == null ? false : Convert.ToBoolean(activities[1].diseaseManagement),
                    healthyLiving2 = activities[1].healthyLiving == null ? false : Convert.ToBoolean(activities[1].healthyLiving),
                    lifeSkills2 = activities[1].lifeSkills == null ? false : Convert.ToBoolean(activities[1].lifeSkills),
                    relaxationTraining2 = activities[1].relaxationTraining == null ? false : Convert.ToBoolean(activities[1].relaxationTraining),
                    socialSkills2 = activities[1].socialSkills == null ? false : Convert.ToBoolean(activities[1].socialSkills),
                    stressManagement2 = activities[1].stressManagement == null ? false : Convert.ToBoolean(activities[1].stressManagement),

                    Present3 = true,
                    Theme3 = (activities.Count > 2) ? activities[2].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention3 = (activities.Count > 2) ? activities[2].Activity.Name : string.Empty,
                    IdActivity3 = (activities.Count > 2) ? activities[2].Activity.Id : 0,
                    Goals3 = goals,
                    Objetives3 = objs,
                    activityDailyLiving3 = activities[2].activityDailyLiving == null ? false : Convert.ToBoolean(activities[2].activityDailyLiving),
                    communityResources3 = activities[2].communityResources == null ? false : Convert.ToBoolean(activities[2].communityResources),
                    copingSkills3 = activities[2].copingSkills == null ? false : Convert.ToBoolean(activities[2].copingSkills),
                    diseaseManagement3 = activities[2].diseaseManagement == null ? false : Convert.ToBoolean(activities[2].diseaseManagement),
                    healthyLiving3 = activities[2].healthyLiving == null ? false : Convert.ToBoolean(activities[2].healthyLiving),
                    lifeSkills3 = activities[2].lifeSkills == null ? false : Convert.ToBoolean(activities[2].lifeSkills),
                    relaxationTraining3 = activities[2].relaxationTraining == null ? false : Convert.ToBoolean(activities[2].relaxationTraining),
                    socialSkills3 = activities[2].socialSkills == null ? false : Convert.ToBoolean(activities[2].socialSkills),
                    stressManagement3 = activities[2].stressManagement == null ? false : Convert.ToBoolean(activities[2].stressManagement),

                    Present4 = true,
                    Theme4 = (activities.Count > 3) ? activities[3].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention4 = (activities.Count > 3) ? activities[3].Activity.Name : string.Empty,
                    IdActivity4 = (activities.Count > 3) ? activities[3].Activity.Id : 0,
                    Goals4 = goals,
                    Objetives4 = objs,
                    activityDailyLiving4 = activities[3].activityDailyLiving == null ? false : Convert.ToBoolean(activities[3].activityDailyLiving),
                    communityResources4 = activities[3].communityResources == null ? false : Convert.ToBoolean(activities[3].communityResources),
                    copingSkills4 = activities[3].copingSkills == null ? false : Convert.ToBoolean(activities[3].copingSkills),
                    diseaseManagement4 = activities[3].diseaseManagement == null ? false : Convert.ToBoolean(activities[3].diseaseManagement),
                    healthyLiving4 = activities[3].healthyLiving == null ? false : Convert.ToBoolean(activities[3].healthyLiving),
                    lifeSkills4 = activities[3].lifeSkills == null ? false : Convert.ToBoolean(activities[3].lifeSkills),
                    relaxationTraining4 = activities[3].relaxationTraining == null ? false : Convert.ToBoolean(activities[3].relaxationTraining),
                    socialSkills4 = activities[3].socialSkills == null ? false : Convert.ToBoolean(activities[3].socialSkills),
                    stressManagement4 = activities[3].stressManagement == null ? false : Convert.ToBoolean(activities[3].stressManagement),

                    Workday_Cient = workday_Client,
                    MTPId = mtp.Id
                };
            }
            else
            {
                List<NoteP_Activity> note_Activity = await _context.NotesP_Activities

                                                                   .Include(na => na.Activity)
                                                                   .ThenInclude(a => a.Theme)

                                                                   .Include(na => na.Objetive)
                                                                   .ThenInclude(o => o.Goal)

                                                                   .Where(na => na.NoteP.Id == note.Id)
                                                                   .ToListAsync();

                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.PSR);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }

                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }

                noteViewModel = new NotePViewModel
                {
                    Id = id,
                    Origin = origin,
                    Workday_Cient = workday_Client,
                    Schema = note.Schema,
                    PlanNote = note.PlanNote,
                    Status = note.Status,
                    Title = note.Title,
                    Setting = note.Setting,
                    CodeBill = workday_Client.CodeBill,

                    //mental client status
                    Attentive = note.Attentive,
                    Depressed = note.Depressed,
                    Inattentive = note.Inattentive,
                    Angry = note.Angry,
                    Sad = note.Sad,
                    FlatAffect = note.FlatAffect,
                    Anxious = note.Anxious,
                    PositiveEffect = note.PositiveEffect,
                    Oriented1x = note.Oriented1x,
                    Oriented2x = note.Oriented2x,
                    Oriented3x = note.Oriented3x,
                    Impulsive = note.Impulsive,
                    Labile = note.Labile,
                    Withdrawn = note.Withdrawn,
                    RelatesWell = note.RelatesWell,
                    DecreasedEyeContact = note.DecreasedEyeContact,
                    AppropiateEyeContact = note.AppropiateEyeContact,   
                    
                    //progress
                    Minimal = note.Minimal,
                    Slow = note.Slow,
                    Steady = note.Steady,
                    GoodExcelent = note.GoodExcelent,
                    IncreasedDifficultiesNoted = note.IncreasedDifficultiesNoted,
                    Complicated = note.Complicated,
                    DevelopingInsight = note.DevelopingInsight,
                    LittleInsight = note.LittleInsight,
                    Aware = note.Aware,
                    AbleToGenerateAlternatives = note.AbleToGenerateAlternatives,
                    Initiates = note.Initiates,
                    ProblemSolved = note.ProblemSolved,
                    DemostratesEmpathy = note.DemostratesEmpathy,
                    UsesSessions = note.UsesSessions,
                    Variable = note.Variable,

                    Present1 = note_Activity[0].Present,
                    Theme1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    IdGoal1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Goal.Id : 0,
                    Goals1 = goals,
                    IdObjetive1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Id : 0,
                    Objetives1 = _combosHelper.GetComboObjetives(((note_Activity.Count > 0) && (note_Activity[0].Objetive != null))
                                                                        ? note_Activity[0].Objetive.Goal.Id : 0),
                    Intervention1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Intervention : string.Empty,

                    //Skill set addressed
                    activityDailyLiving1 = activities[0].activityDailyLiving == null ? false : Convert.ToBoolean(activities[0].activityDailyLiving),
                    communityResources1 = activities[0].communityResources == null ? false : Convert.ToBoolean(activities[0].communityResources),
                    copingSkills1 = activities[0].copingSkills == null ? false : Convert.ToBoolean(activities[0].copingSkills),
                    diseaseManagement1 = activities[0].diseaseManagement == null ? false : Convert.ToBoolean(activities[0].diseaseManagement),
                    healthyLiving1 = activities[0].healthyLiving == null ? false : Convert.ToBoolean(activities[0].healthyLiving),
                    lifeSkills1 = activities[0].lifeSkills == null ? false : Convert.ToBoolean(activities[0].lifeSkills),
                    relaxationTraining1 = activities[0].relaxationTraining == null ? false : Convert.ToBoolean(activities[0].relaxationTraining),
                    socialSkills1 = activities[0].socialSkills == null ? false : Convert.ToBoolean(activities[0].socialSkills),
                    stressManagement1 = activities[0].stressManagement == null ? false : Convert.ToBoolean(activities[0].stressManagement),  
                    
                    //Client's response
                    Cooperative1 = (note_Activity.Count > 0) ? note_Activity[0].Cooperative : false,
                    Assertive1 = (note_Activity.Count > 0) ? note_Activity[0].Assertive : false,
                    Passive1 = (note_Activity.Count > 0) ? note_Activity[0].Passive : false,
                    Variable1 = (note_Activity.Count > 0) ? note_Activity[0].Variable : false,
                    Uninterested1 = (note_Activity.Count > 0) ? note_Activity[0].Uninterested : false,
                    Engaged1 = (note_Activity.Count > 0) ? note_Activity[0].EngagedActive : false,
                    Distractible1 = (note_Activity.Count > 0) ? note_Activity[0].Distractible : false,
                    Confused1 = (note_Activity.Count > 0) ? note_Activity[0].Confused : false,
                    Aggresive1 = (note_Activity.Count > 0) ? note_Activity[0].Aggresive : false,
                    Resistant1 = (note_Activity.Count > 0) ? note_Activity[0].Resistant : false,
                    Other1 = (note_Activity.Count > 0) ? note_Activity[0].Other : false,

                    Present2 = note_Activity[1].Present,
                    Theme2 = (activities.Count > 1) ? activities[1].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention2 = (activities.Count > 1) ? activities[1].Activity.Name : string.Empty,
                    IdActivity2 = (activities.Count > 1) ? activities[1].Activity.Id : 0,
                    IdGoal2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Goal.Id : 0,
                    Goals2 = goals,
                    IdObjetive2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Id : 0,
                    Objetives2 = _combosHelper.GetComboObjetives(((note_Activity.Count > 1) && (note_Activity[1].Objetive != null))
                                                                        ? note_Activity[1].Objetive.Goal.Id : 0),
                    Intervention2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Intervention : string.Empty,

                    //Skill set addressed
                    activityDailyLiving2 = activities[1].activityDailyLiving == null ? false : Convert.ToBoolean(activities[1].activityDailyLiving),
                    communityResources2 = activities[1].communityResources == null ? false : Convert.ToBoolean(activities[1].communityResources),
                    copingSkills2 = activities[1].copingSkills == null ? false : Convert.ToBoolean(activities[1].copingSkills),
                    diseaseManagement2 = activities[1].diseaseManagement == null ? false : Convert.ToBoolean(activities[1].diseaseManagement),
                    healthyLiving2 = activities[1].healthyLiving == null ? false : Convert.ToBoolean(activities[1].healthyLiving),
                    lifeSkills2 = activities[1].lifeSkills == null ? false : Convert.ToBoolean(activities[1].lifeSkills),
                    relaxationTraining2 = activities[1].relaxationTraining == null ? false : Convert.ToBoolean(activities[1].relaxationTraining),
                    socialSkills2 = activities[1].socialSkills == null ? false : Convert.ToBoolean(activities[1].socialSkills),
                    stressManagement2 = activities[1].stressManagement == null ? false : Convert.ToBoolean(activities[1].stressManagement),

                    //Client's response
                    Cooperative2 = (note_Activity.Count > 1) ? note_Activity[1].Cooperative : false,
                    Assertive2 = (note_Activity.Count > 1) ? note_Activity[1].Assertive : false,
                    Passive2 = (note_Activity.Count > 1) ? note_Activity[1].Passive : false,
                    Variable2 = (note_Activity.Count > 1) ? note_Activity[1].Variable : false,
                    Uninterested2 = (note_Activity.Count > 1) ? note_Activity[1].Uninterested : false,
                    Engaged2 = (note_Activity.Count > 1) ? note_Activity[1].EngagedActive : false,
                    Distractible2 = (note_Activity.Count > 1) ? note_Activity[1].Distractible : false,
                    Confused2 = (note_Activity.Count > 1) ? note_Activity[1].Confused : false,
                    Aggresive2 = (note_Activity.Count > 1) ? note_Activity[1].Aggresive : false,
                    Resistant2 = (note_Activity.Count > 1) ? note_Activity[1].Resistant : false,
                    Other2 = (note_Activity.Count > 1) ? note_Activity[1].Other : false,

                    Present3 = note_Activity[2].Present,
                    Theme3 = (activities.Count > 2) ? activities[2].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention3 = (activities.Count > 2) ? activities[2].Activity.Name : string.Empty,
                    IdActivity3 = (activities.Count > 2) ? activities[2].Activity.Id : 0,
                    IdGoal3 = ((note_Activity.Count > 2) && (note_Activity[2].Objetive != null)) ? note_Activity[2].Objetive.Goal.Id : 0,
                    Goals3 = goals,
                    IdObjetive3 = ((note_Activity.Count > 2) && (note_Activity[2].Objetive != null)) ? note_Activity[2].Objetive.Id : 0,
                    Objetives3 = _combosHelper.GetComboObjetives(((note_Activity.Count > 2) && (note_Activity[2].Objetive != null))
                                                                        ? note_Activity[2].Objetive.Goal.Id : 0),
                    Intervention3 = ((note_Activity.Count > 2) && (note_Activity[2].Objetive != null)) ? note_Activity[2].Objetive.Intervention : string.Empty,

                    //Skill set addressed
                    activityDailyLiving3 = activities[2].activityDailyLiving == null ? false : Convert.ToBoolean(activities[2].activityDailyLiving),
                    communityResources3 = activities[2].communityResources == null ? false : Convert.ToBoolean(activities[2].communityResources),
                    copingSkills3 = activities[2].copingSkills == null ? false : Convert.ToBoolean(activities[2].copingSkills),
                    diseaseManagement3 = activities[2].diseaseManagement == null ? false : Convert.ToBoolean(activities[2].diseaseManagement),
                    healthyLiving3 = activities[2].healthyLiving == null ? false : Convert.ToBoolean(activities[2].healthyLiving),
                    lifeSkills3 = activities[2].lifeSkills == null ? false : Convert.ToBoolean(activities[2].lifeSkills),
                    relaxationTraining3 = activities[2].relaxationTraining == null ? false : Convert.ToBoolean(activities[2].relaxationTraining),
                    socialSkills3 = activities[2].socialSkills == null ? false : Convert.ToBoolean(activities[2].socialSkills),
                    stressManagement3 = activities[2].stressManagement == null ? false : Convert.ToBoolean(activities[2].stressManagement),

                    //Client's response
                    Cooperative3 = (note_Activity.Count > 2) ? note_Activity[2].Cooperative : false,
                    Assertive3 = (note_Activity.Count > 2) ? note_Activity[2].Assertive : false,
                    Passive3 = (note_Activity.Count > 2) ? note_Activity[2].Passive : false,
                    Variable3 = (note_Activity.Count > 2) ? note_Activity[2].Variable : false,
                    Uninterested3 = (note_Activity.Count > 2) ? note_Activity[2].Uninterested : false,
                    Engaged3 = (note_Activity.Count > 2) ? note_Activity[2].EngagedActive : false,
                    Distractible3 = (note_Activity.Count > 2) ? note_Activity[2].Distractible : false,
                    Confused3 = (note_Activity.Count > 2) ? note_Activity[2].Confused : false,
                    Aggresive3 = (note_Activity.Count > 2) ? note_Activity[2].Aggresive : false,
                    Resistant3 = (note_Activity.Count > 2) ? note_Activity[2].Resistant : false,
                    Other3 = (note_Activity.Count > 2) ? note_Activity[2].Other : false,

                    Present4 = note_Activity[3].Present,
                    Theme4 = (activities.Count > 3) ? activities[3].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention4 = (activities.Count > 3) ? activities[3].Activity.Name : string.Empty,
                    IdActivity4 = (activities.Count > 3) ? activities[3].Activity.Id : 0,
                    IdGoal4 = ((note_Activity.Count > 3) && (note_Activity[3].Objetive != null)) ? note_Activity[3].Objetive.Goal.Id : 0,
                    Goals4 = goals,
                    IdObjetive4 = ((note_Activity.Count > 3) && (note_Activity[3].Objetive != null)) ? note_Activity[3].Objetive.Id : 0,
                    Objetives4 = _combosHelper.GetComboObjetives(((note_Activity.Count > 3) && (note_Activity[3].Objetive != null))
                                                                        ? note_Activity[3].Objetive.Goal.Id : 0),
                    Intervention4 = ((note_Activity.Count > 3) && (note_Activity[3].Objetive != null)) ? note_Activity[3].Objetive.Intervention : string.Empty,

                    //Skill set addressed
                    activityDailyLiving4 = activities[3].activityDailyLiving == null ? false : Convert.ToBoolean(activities[3].activityDailyLiving),
                    communityResources4 = activities[3].communityResources == null ? false : Convert.ToBoolean(activities[3].communityResources),
                    copingSkills4 = activities[3].copingSkills == null ? false : Convert.ToBoolean(activities[3].copingSkills),
                    diseaseManagement4 = activities[3].diseaseManagement == null ? false : Convert.ToBoolean(activities[3].diseaseManagement),
                    healthyLiving4 = activities[3].healthyLiving == null ? false : Convert.ToBoolean(activities[3].healthyLiving),
                    lifeSkills4 = activities[3].lifeSkills == null ? false : Convert.ToBoolean(activities[3].lifeSkills),
                    relaxationTraining4 = activities[3].relaxationTraining == null ? false : Convert.ToBoolean(activities[3].relaxationTraining),
                    socialSkills4 = activities[3].socialSkills == null ? false : Convert.ToBoolean(activities[3].socialSkills),
                    stressManagement4 = activities[3].stressManagement == null ? false : Convert.ToBoolean(activities[3].stressManagement),

                    //Client's response
                    Cooperative4 = (note_Activity.Count > 3) ? note_Activity[3].Cooperative : false,
                    Assertive4 = (note_Activity.Count > 3) ? note_Activity[3].Assertive : false,
                    Passive4 = (note_Activity.Count > 3) ? note_Activity[3].Passive : false,
                    Variable4 = (note_Activity.Count > 3) ? note_Activity[3].Variable : false,
                    Uninterested4 = (note_Activity.Count > 3) ? note_Activity[3].Uninterested : false,
                    Engaged4 = (note_Activity.Count > 3) ? note_Activity[3].EngagedActive : false,
                    Distractible4 = (note_Activity.Count > 3) ? note_Activity[3].Distractible : false,
                    Confused4 = (note_Activity.Count > 3) ? note_Activity[3].Confused : false,
                    Aggresive4 = (note_Activity.Count > 3) ? note_Activity[3].Aggresive : false,
                    Resistant4 = (note_Activity.Count > 3) ? note_Activity[3].Resistant : false,
                    Other4 = (note_Activity.Count > 3) ? note_Activity[3].Other : false,

                    MTPId = mtp.Id
                };
            }
            return View(noteViewModel);
        }

        //PSR Notes (Schema 3)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditNoteP(NotePViewModel model, IFormCollection form)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.MTPs)

                                                                           .Include(wc => wc.Facilitator)
                                                                           .Include(wc => wc.Schedule)
                                                                           .ThenInclude(c => c.SubSchedules)

                                                                           .FirstOrDefaultAsync(wc => wc.Id == model.Id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            NotePEntity noteEntity;
            if (ModelState.IsValid)
            {
                //The user must select at least one client response per theme 
                if ((model.Present1 && !model.Cooperative1 && !model.Assertive1 && !model.Passive1 && !model.Variable1 && !model.Uninterested1 && !model.Engaged1 && !model.Distractible1 && !model.Confused1 && !model.Aggresive1 && !model.Resistant1 && !model.Other1)
                    || (model.Present2 && !model.Cooperative2 && !model.Assertive2 && !model.Passive2 && !model.Variable2 && !model.Uninterested2 && !model.Engaged2 && !model.Distractible2 && !model.Confused2 && !model.Aggresive2 && !model.Resistant2 && !model.Other2)
                    || (model.Present3 && !model.Cooperative3 && !model.Assertive3 && !model.Passive3 && !model.Variable3 && !model.Uninterested3 && !model.Engaged3 && !model.Distractible3 && !model.Confused3 && !model.Aggresive3 && !model.Resistant3 && !model.Other3)
                    || (model.Present4 && !model.Cooperative4 && !model.Assertive4 && !model.Passive4 && !model.Variable4 && !model.Uninterested4 && !model.Engaged4 && !model.Distractible4 && !model.Confused4 && !model.Aggresive4 && !model.Resistant4 && !model.Other4))
                {
                    return RedirectToAction(nameof(EditNoteP), new { id = model.Id, error = 6, origin = model.Origin });
                }

                NotePEntity note = await _context.NotesP.Include(n => n.Workday_Cient)
                                                        .ThenInclude(wc => wc.Messages)
                                                        .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);
                NoteP_Activity note_Activity;
                if (note == null)   //the note has not been created
                {
                    //Verify the client is not present in other services of notes at the same time
                    if (workday_Client.Schedule != null)
                    {
                        if (this.VerifyNotesAtSameTime(workday_Client.Client.Id, workday_Client.Session, workday_Client.Workday.Date, workday_Client.Schedule.InitialTime, workday_Client.Schedule.EndTime, workday_Client.Id))
                        {
                            return RedirectToAction(nameof(EditNoteP), new { id = model.Id, error = 5, origin = model.Origin });
                        }
                    }
                    else
                    {
                        if (this.VerifyNotesAtSameTime(workday_Client.Client.Id, workday_Client.Session, workday_Client.Workday.Date, workday_Client.Workday.Date, workday_Client.Workday.Date, workday_Client.Id))
                        {
                            return RedirectToAction(nameof(EditNoteP), new { id = model.Id, error = 5, origin = model.Origin });
                        }
                    }
                    //Verify the client is not present in other services of notes at the same time
                   

                    noteEntity = await _converterHelper.ToNotePEntity(model, true);
                    noteEntity.Setting = form["Setting"].ToString();

                    //vinculo el mtp activo del cliente a la nota que se creará
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
                    if (mtp != null)
                    { 
                        noteEntity.MTPId = mtp.Id;                        
                    }

                    //I verify that user selected at least a one progress, if no then I put minimal progress for default
                    if (!model.Minimal && !model.Slow && !model.Steady && !model.GoodExcelent && !model.IncreasedDifficultiesNoted && !model.Complicated && !model.DevelopingInsight
                                     && !model.LittleInsight && !model.Aware && !model.AbleToGenerateAlternatives && !model.Initiates && !model.ProblemSolved && !model.DemostratesEmpathy
                                     && !model.UsesSessions && !model.Variable)
                    {
                        noteEntity.Minimal = true;
                    }

                    //I verify that user selected at least a mental status observations, if no then I put attentive for default
                    if (!model.Attentive && !model.Depressed && !model.Inattentive && !model.Angry && !model.Sad && !model.FlatAffect && !model.Anxious
                                     && !model.PositiveEffect && !model.Oriented3x && !model.Oriented2x && !model.Oriented1x && !model.Impulsive && !model.Labile
                                     && !model.Withdrawn && !model.RelatesWell && !model.DecreasedEyeContact && !model.AppropiateEyeContact)
                    {
                        noteEntity.Attentive = true;
                    }
                                        
                    // I will calculate teh real units of the note
                    int realUnits = 0;
                    realUnits = (model.Present1) ? realUnits + 4 : realUnits;
                    realUnits = (model.Present2) ? realUnits + 4 : realUnits;
                    realUnits = (model.Present3) ? realUnits + 4 : realUnits;
                    realUnits = (model.Present4) ? realUnits + 4 : realUnits;
                    noteEntity.RealUnits = realUnits;

                    _context.Add(noteEntity);

                    bool bandera = false;
                    int idSchedule = 0;
                    if (workday_Client.Schedule != null)
                    {
                        bandera = true;
                        idSchedule = workday_Client.Schedule.Id;
                    }

                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == idSchedule).OrderBy(n => n.InitialTime).ToListAsync();
                    
                    note_Activity = new NoteP_Activity
                    {
                        NoteP = noteEntity,
                        Present = model.Present1,
                        Activity = (model.Present1) ? _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1) : null,
                        Objetive = (model.Present1) ? _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1) : null,
                        //Client's response
                        Cooperative = (model.Present1) ? model.Cooperative1 : false,
                        Assertive = (model.Present1) ? model.Assertive1 : false,
                        Passive = (model.Present1) ? model.Passive1 : false,
                        Variable = (model.Present1) ? model.Variable1 : false,
                        Uninterested = (model.Present1) ? model.Uninterested1 : false,
                        EngagedActive = (model.Present1) ? model.Engaged1 : false,
                        Distractible = (model.Present1) ? model.Distractible1 : false,
                        Confused = (model.Present1) ? model.Confused1 : false,
                        Aggresive = (model.Present1) ? model.Aggresive1 : false,
                        Resistant = (model.Present1) ? model.Resistant1 : false,
                        Other = (model.Present1) ? model.Other1 : false,
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(0) : null
                    };
                    _context.Add(note_Activity);
                    note_Activity = new NoteP_Activity
                    {
                        NoteP = noteEntity,
                        Present = model.Present2,
                        Activity = (model.Present2) ? _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity2) : null,
                        Objetive = (model.Present2) ? _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive2) : null,
                        //Client's response
                        Cooperative = (model.Present2) ? model.Cooperative2 : false,
                        Assertive = (model.Present2) ? model.Assertive2 : false,
                        Passive = (model.Present2) ? model.Passive2 : false,
                        Variable = (model.Present2) ? model.Variable2 : false,
                        Uninterested = (model.Present2) ? model.Uninterested2 : false,
                        EngagedActive = (model.Present2) ? model.Engaged2 : false,
                        Distractible = (model.Present2) ? model.Distractible2 : false,
                        Confused = (model.Present2) ? model.Confused2 : false,
                        Aggresive = (model.Present2) ? model.Aggresive2 : false,
                        Resistant = (model.Present2) ? model.Resistant2 : false,
                        Other = (model.Present2) ? model.Other2 : false,
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(1) : null
                    };
                    _context.Add(note_Activity);
                    note_Activity = new NoteP_Activity
                    {
                        NoteP = noteEntity,
                        Present = model.Present3,
                        Activity = (model.Present3) ? _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity3) : null,
                        Objetive = (model.Present3) ? _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive3) : null,
                        //Client's response
                        Cooperative = (model.Present3) ? model.Cooperative3 : false,
                        Assertive = (model.Present3) ? model.Assertive3 : false,
                        Passive = (model.Present3) ? model.Passive3 : false,
                        Variable = (model.Present3) ? model.Variable3 : false,
                        Uninterested = (model.Present3) ? model.Uninterested3 : false,
                        EngagedActive = (model.Present3) ? model.Engaged3 : false,
                        Distractible = (model.Present3) ? model.Distractible3 : false,
                        Confused = (model.Present3) ? model.Confused3 : false,
                        Aggresive = (model.Present3) ? model.Aggresive3 : false,
                        Resistant = (model.Present3) ? model.Resistant3 : false,
                        Other = (model.Present3) ? model.Other3 : false,
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(2) : null
                    };
                    _context.Add(note_Activity);
                    note_Activity = new NoteP_Activity
                    {
                        NoteP = noteEntity,
                        Present = model.Present4,
                        Activity = (model.Present4) ? _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity4) : null,
                        Objetive = (model.Present4) ? _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive4) : null,
                        //Client's response
                        Cooperative = (model.Present4) ? model.Cooperative4 : false,
                        Assertive = (model.Present4) ? model.Assertive4 : false,
                        Passive = (model.Present4) ? model.Passive4 : false,
                        Variable = (model.Present4) ? model.Variable4 : false,
                        Uninterested = (model.Present4) ? model.Uninterested4 : false,
                        EngagedActive = (model.Present4) ? model.Engaged4 : false,
                        Distractible = (model.Present4) ? model.Distractible4 : false,
                        Confused = (model.Present4) ? model.Confused4 : false,
                        Aggresive = (model.Present4) ? model.Aggresive4 : false,
                        Resistant = (model.Present4) ? model.Resistant4 : false,
                        Other = (model.Present4) ? model.Other4 : false,
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(3) : null
                    };
                    _context.Add(note_Activity);

                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(Index));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedNotes));
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
                else    //update note
                {
                    //I verify that the note has a selected goal
                    if (note.Status == NoteStatus.Pending)
                    {
                        if (model.IdObjetive1 == 0 && model.IdObjetive2 == 0 && model.IdObjetive3 == 0 && model.IdObjetive4 == 0)
                        {
                            return RedirectToAction(nameof(EditNoteP), new { id = model.Id, error = 1, origin = model.Origin });
                        }
                        else
                        {
                            if (!model.Present1 && model.IdObjetive1 != 0 && model.IdObjetive2 == 0 && model.IdObjetive3 == 0 && model.IdObjetive4 == 0)
                            {
                                return RedirectToAction(nameof(EditNoteP), new { id = model.Id, error = 1, origin = model.Origin });
                            }
                            else
                            {
                                if (!model.Present2 && model.IdObjetive2 != 0 && model.IdObjetive1 == 0 && model.IdObjetive3 == 0 && model.IdObjetive4 == 0)
                                {
                                    return RedirectToAction(nameof(EditNoteP), new { id = model.Id, error = 1, origin = model.Origin });
                                }
                                else
                                {
                                    if (!model.Present3 && model.IdObjetive3 != 0 && model.IdObjetive1 == 0 && model.IdObjetive2 == 0 && model.IdObjetive4 == 0)
                                    {
                                        return RedirectToAction(nameof(EditNoteP), new { id = model.Id, error = 1, origin = model.Origin });
                                    }
                                    else
                                    {
                                        if (!model.Present4 && model.IdObjetive4 != 0 && model.IdObjetive1 == 0 && model.IdObjetive2 == 0 && model.IdObjetive3 == 0)
                                        {
                                            return RedirectToAction(nameof(EditNoteP), new { id = model.Id, error = 1, origin = model.Origin });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    note.Title = model.Title;
                    note.PlanNote = model.PlanNote;
                    note.Setting = form["Setting"].ToString();

                    //mental client status
                    note.Attentive = model.Attentive;
                    note.Depressed = model.Depressed;
                    note.Inattentive = model.Inattentive;
                    note.Angry = model.Angry;
                    note.Sad = model.Sad;
                    note.FlatAffect = model.FlatAffect;
                    note.Anxious = model.Anxious;
                    note.PositiveEffect = model.PositiveEffect;
                    note.Oriented1x = model.Oriented1x;
                    note.Oriented2x = model.Oriented2x;
                    note.Oriented3x = model.Oriented3x;
                    note.Impulsive = model.Impulsive;
                    note.Labile = model.Labile;
                    note.Withdrawn = model.Withdrawn;
                    note.RelatesWell = model.RelatesWell;
                    note.DecreasedEyeContact = model.DecreasedEyeContact;
                    note.AppropiateEyeContact = model.AppropiateEyeContact;

                    //progress
                    note.Minimal = model.Minimal;
                    note.Slow = model.Slow;
                    note.Steady = model.Steady;
                    note.GoodExcelent = model.GoodExcelent;
                    note.IncreasedDifficultiesNoted = model.IncreasedDifficultiesNoted;
                    note.Complicated = model.Complicated;
                    note.DevelopingInsight = model.DevelopingInsight;
                    note.LittleInsight = model.LittleInsight;
                    note.Aware = model.Aware;
                    note.AbleToGenerateAlternatives = model.AbleToGenerateAlternatives;
                    note.Initiates = model.Initiates;
                    note.ProblemSolved = model.ProblemSolved;
                    note.DemostratesEmpathy = model.DemostratesEmpathy;
                    note.UsesSessions = model.UsesSessions;
                    note.Variable = model.Variable;
                    //I verify that user selected at least a one progress, if no then I put minimal progress for default
                    if (!note.Minimal && !note.Slow && !note.Steady && !note.GoodExcelent && !note.IncreasedDifficultiesNoted && !note.Complicated && !note.DevelopingInsight
                                     && !note.LittleInsight && !note.Aware && !note.AbleToGenerateAlternatives && !note.Initiates && !note.ProblemSolved && !note.DemostratesEmpathy
                                     && !note.UsesSessions && !note.Variable)
                    {
                        note.Minimal = true;
                    }

                    //I verify that user selected at least a mental status observations, if no then I put attentive for default
                    if (!note.Attentive && !note.Depressed && !note.Inattentive && !note.Angry && !note.Sad && !note.FlatAffect && !note.Anxious
                                     && !note.PositiveEffect && !note.Oriented3x && !note.Oriented2x && !note.Oriented1x && !note.Impulsive && !note.Labile
                                     && !note.Withdrawn && !note.RelatesWell && !note.DecreasedEyeContact && !note.AppropiateEyeContact)
                    {
                        note.Attentive = true;
                    }

                    //vinculo el mtp activo del cliente a la nota que se creará
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
                    if (mtp != null)
                    {
                        note.MTPId = mtp.Id;                        
                    }

                    // I will calculate the real units of the note
                    int realUnits = 0;
                    realUnits = (model.Present1) ? realUnits + 4 : realUnits;
                    realUnits = (model.Present2) ? realUnits + 4 : realUnits;
                    realUnits = (model.Present3) ? realUnits + 4 : realUnits;
                    realUnits = (model.Present4) ? realUnits + 4 : realUnits;
                    note.RealUnits = realUnits;

                    _context.Update(note);
                    List<NoteP_Activity> noteActivities_list = await _context.NotesP_Activities                                                                            
                                                                             .Where(na => na.NoteP.Id == note.Id)
                                                                             .ToListAsync();
                    _context.RemoveRange(noteActivities_list);
                    
                    bool bandera = false;
                    int idSchedule = 0;
                    if (workday_Client.Schedule != null)
                    {
                        bandera = true;
                        idSchedule = workday_Client.Schedule.Id;
                    }
                    
                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == idSchedule).OrderBy(n => n.InitialTime).ToListAsync();
                    

                    note_Activity = new NoteP_Activity
                    {
                        NoteP = note,
                        Present = model.Present1,
                        Activity = (model.Present1) ? _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1) : null,
                        Objetive = (model.Present1) ? _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1) : null,
                        //Client's response
                        Cooperative = (model.Present1) ? model.Cooperative1 : false,
                        Assertive = (model.Present1) ? model.Assertive1 : false,
                        Passive = (model.Present1) ? model.Passive1 : false,
                        Variable = (model.Present1) ? model.Variable1 : false,
                        Uninterested = (model.Present1) ? model.Uninterested1 : false,
                        EngagedActive = (model.Present1) ? model.Engaged1 : false,
                        Distractible = (model.Present1) ? model.Distractible1 : false,
                        Confused = (model.Present1) ? model.Confused1 : false,
                        Aggresive = (model.Present1) ? model.Aggresive1 : false,
                        Resistant = (model.Present1) ? model.Resistant1 : false,
                        Other = (model.Present1) ? model.Other1 : false,
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(0) : null
                    };
                    _context.Add(note_Activity);
                    await _context.SaveChangesAsync();

                    note_Activity = new NoteP_Activity
                    {
                        NoteP = note,
                        Present = model.Present2,
                        Activity = (model.Present2) ? _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity2) : null,
                        Objetive = (model.Present2) ? _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive2) : null,
                        //Client's response
                        Cooperative = (model.Present2) ? model.Cooperative2 : false, 
                        Assertive = (model.Present2) ? model.Assertive2 : false,
                        Passive = (model.Present2) ? model.Passive2 : false,
                        Variable = (model.Present2) ? model.Variable2 : false,
                        Uninterested = (model.Present2) ? model.Uninterested2 : false,
                        EngagedActive = (model.Present2) ? model.Engaged2 : false,
                        Distractible = (model.Present2) ? model.Distractible2 : false,
                        Confused = (model.Present2) ? model.Confused2 : false,
                        Aggresive = (model.Present2) ? model.Aggresive2 : false,
                        Resistant = (model.Present2) ? model.Resistant2 : false,
                        Other = (model.Present2) ? model.Other2 : false,
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(1) : null
                    };
                    _context.Add(note_Activity);
                    await _context.SaveChangesAsync();

                    note_Activity = new NoteP_Activity
                    {
                        NoteP = note,
                        Present = model.Present3,
                        Activity = (model.Present3) ? _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity3) : null,
                        Objetive = (model.Present3) ? _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive3) : null,
                        //Client's response
                        Cooperative = (model.Present3) ? model.Cooperative3 : false,
                        Assertive = (model.Present3) ? model.Assertive3 : false,
                        Passive = (model.Present3) ? model.Passive3 : false,
                        Variable = (model.Present3) ? model.Variable3 : false,
                        Uninterested = (model.Present3) ? model.Uninterested3 : false,
                        EngagedActive = (model.Present3) ? model.Engaged3 : false,
                        Distractible = (model.Present3) ? model.Distractible3 : false,
                        Confused = (model.Present3) ? model.Confused3 : false,
                        Aggresive = (model.Present3) ? model.Aggresive3 : false,
                        Resistant = (model.Present3) ? model.Resistant3 : false,
                        Other = (model.Present3) ? model.Other3 : false,
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(2) : null
                    };
                    _context.Add(note_Activity);
                    await _context.SaveChangesAsync();

                    note_Activity = new NoteP_Activity
                    {
                        NoteP = note,
                        Present = model.Present4,
                        Activity = (model.Present4) ? _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity4) : null,
                        Objetive = (model.Present4) ? _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive4) : null,
                        //Client's response
                        Cooperative = (model.Present4) ? model.Cooperative4 : false,
                        Assertive = (model.Present4) ? model.Assertive4 : false,
                        Passive = (model.Present4) ? model.Passive4 : false,
                        Variable = (model.Present4) ? model.Variable4 : false,
                        Uninterested = (model.Present4) ? model.Uninterested4 : false,
                        EngagedActive = (model.Present4) ? model.Engaged4 : false,
                        Distractible = (model.Present4) ? model.Distractible4 : false,
                        Confused = (model.Present4) ? model.Confused4 : false,
                        Aggresive = (model.Present4) ? model.Aggresive4 : false,
                        Resistant = (model.Present4) ? model.Resistant4 : false,
                        Other = (model.Present4) ? model.Other4 : false,
                        SubSchedule = bandera ? subSchedules.ElementAtOrDefault(3) : null
                    };
                    _context.Add(note_Activity);

                    List<MessageEntity> messages = note.Workday_Cient.Messages.Where(m => (m.Status == MessageStatus.NotRead && m.Notification == false)).ToList();
                    //todos los mensajes no leidos que tiene el Workday_Client de la nota los pongo como leidos
                    foreach (MessageEntity value in messages)
                    {                        
                        value.Status = MessageStatus.Read;
                        value.DateRead = DateTime.Now;
                        _context.Update(value);

                        //I generate a notification to supervisor
                        MessageEntity notification = new MessageEntity
                        {
                            Workday_Client = workday_Client,
                            FarsForm = null,
                            MTPReview = null,
                            Addendum = null,
                            Discharge = null,
                            Title = "Update on reviewed PSR note",
                            Text = $"The PSR note of {workday_Client.ClientName} on {workday_Client.Workday.Date.ToShortDateString()} was rectified",
                            From = value.To,
                            To = value.From,
                            DateCreated = DateTime.Now,
                            Status = MessageStatus.NotRead,
                            Notification = true
                        };
                        _context.Add(notification);                                                              
                    }

                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(Index));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedNotes));
                        if (model.Origin == 2)
                            return RedirectToAction(nameof(NotesInEdit));
                        if (model.Origin == 3)
                            return RedirectToAction(nameof(PendingNotes));
                        if (model.Origin == 4)
                            return RedirectToAction(nameof(NotesWithReview));
                        if (model.Origin == 5)
                            return RedirectToAction("MessagesOfNotes", "Messages");
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
            }

            return View(model);
        }

        //Individual Notes
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditIndNote(int id, int error = 0, int origin = 0, string errorText = "")
        {
            IndividualNoteViewModel individualNoteViewModel;
            
            //la nota no tiene linkeado ningun goal
            if (error == 1)
                ViewBag.Error = "0";

            //la nota no esta completa, faltan campos por editar
            if (error == 2)
                ViewBag.Error = "2";

            //la nota tiene problemas con el genero
            if (error == 4)
            {
                ViewBag.Error = "4";
                ViewBag.errorText = errorText;
            }

            //el cliente seleccionado tiene una nota ya creada en ese mismo horario
            if (error == 5)
            {
                ViewBag.Error = "5";                
            }

            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)
                                                                           
                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.MTPs)

                                                                           .Include(wc => wc.Facilitator)
                                                                           .ThenInclude(c => c.Clinic)  //es para saber la clinica cuando no existe la nota en individualNote
                                                                           .ThenInclude(c => c.Setting)

                                                                           .Include(wc => wc.Workday)
                                                                           .ThenInclude(w => w.Week)

                                                                           .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            FacilitatorEntity facilitator_logged = _context.Facilitators
                                                           .FirstOrDefault(f => f.LinkedUser == User.Identity.Name);

            //el cliente no tiene mtp activos
            if(workday_Client.Client != null)
            { 
                if (workday_Client.Client.MTPs.Where(m => m.Active == true).Count() == 0)
                {
                    ViewBag.Error = "3";
                    individualNoteViewModel = new IndividualNoteViewModel
                    {
                        Id = workday_Client.Workday.Id,
                    };
                    return View(individualNoteViewModel);
                }
            }

            IndividualNoteEntity note = await _context.IndividualNotes

                                                      .Include(n => n.Workday_Cient)
                                                      .ThenInclude(wc => wc.Client)
                                                      
                                                      .Include(n => n.Workday_Cient)
                                                      .ThenInclude(g => g.Facilitator)

                                                      .Include(n => n.Objective)
                                                      .ThenInclude(o => o.Goal)
                                                       
                                                      .Include(n => n.SubSchedule)

                                                      .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            //-----------se selecciona el primer MTP activo que tenga el cliente-----------//
            MTPEntity mtp = null;
            if (workday_Client.Client != null)
            {
                mtp = _context.MTPs
                              .FirstOrDefault(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
            }            

            IEnumerable<SelectListItem> goals = null;
            IEnumerable<SelectListItem> objs = null;
            if (mtp != null)
            {
                //Evaluate setting for goals's classification
                SettingEntity setting = _context.Settings
                                                .FirstOrDefault(s => s.Clinic.Id == facilitator_logged.Clinic.Id);

                if (setting != null)
                {
                    if (setting.MHClassificationOfGoals)
                        goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.Individual);
                    else
                        goals = _combosHelper.GetComboGoals(mtp.Id);
                }

                objs = _combosHelper.GetComboObjetives(0);
            }
            else
            {
                goals = _combosHelper.GetComboGoals(0);
                objs = _combosHelper.GetComboObjetives(0);
            }

            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);            

            if (note == null)   //la nota no está creada
            {
                individualNoteViewModel = new IndividualNoteViewModel
                {
                    Id = id,
                    Status = NoteStatus.Pending,    //es solo generico para la visualizacion del btn FinishEditing
                    Origin = origin,
                    Goals1 = goals,
                    Objetives1 = objs,
                    Workday_Cient = workday_Client,
                    Clients = _combosHelper.GetComboClientsForIndNotes(user_logged.Clinic.Id, workday_Client.Workday.Week.Id, facilitator_logged.Id),
                    IdClient = 0,
                    MTPId = 0,
                    CodeBill = user_logged.Clinic.CodeIndTherapy
                };
            }
            else
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Insert(0, new SelectListItem
                {
                    Text = note.Workday_Cient.Client.Name,
                    Value = $"{note.Workday_Cient.Client.Id}"
                });

                individualNoteViewModel = new IndividualNoteViewModel
                {
                    Id = id,
                    Workday_Cient = workday_Client,
                    IdClient = note.Workday_Cient.Client.Id,
                    Clients   = list,
                    Origin = origin,
                    SubjectiveData = note.SubjectiveData,
                    ObjectiveData = note.ObjectiveData,
                    Assessment = note.Assessment,
                    PlanNote = note.PlanNote,
                    Status = note.Status,
                    CodeBill = workday_Client.CodeBill,

                    Groomed = note.Groomed,
                    Unkempt = note.Unkempt,
                    Disheveled = note.Disheveled,
                    Meticulous = note.Meticulous,
                    Overbuild = note.Overbuild,
                    Other = note.Other,
                    Clear = note.Clear,
                    Pressured = note.Pressured,
                    Slurred = note.Slurred,
                    Slow = note.Slow,
                    Impaired = note.Impaired,
                    Poverty = note.Poverty,
                    Euthymic = note.Euthymic,
                    Depressed = note.Depressed,
                    Anxious = note.Anxious,
                    Fearful = note.Fearful,
                    Irritable = note.Irritable,
                    Labile = note.Labile,
                    WNL = note.WNL,
                    Guarded = note.Guarded,
                    Withdrawn = note.Withdrawn,
                    Hostile = note.Hostile,
                    Restless = note.Restless,
                    Impulsive = note.Impulsive,
                    WNL_Cognition = note.WNL_Cognition,
                    Blocked = note.Blocked,
                    Obsessive = note.Obsessive,
                    Paranoid = note.Paranoid,
                    Scattered = note.Scattered,
                    Psychotic = note.Psychotic,
                    Exceptional = note.Exceptional,
                    Steady = note.Steady,
                    Slow_Progress = note.Slow_Progress,
                    Regressing = note.Regressing,
                    Stable = note.Stable,
                    Maintain = note.Maintain,
                    CBT = note.CBT,
                    Psychodynamic = note.Psychodynamic,
                    BehaviorModification = note.BehaviorModification,
                    Other_Intervention = note.Other_Intervention,             

                    IdGoal1 = (note.Objective == null) ? 0 : note.Objective.Goal.Id,
                    Goals1 = goals,
                    IdObjetive1 = (note.Objective == null) ? 0 : note.Objective.Id,
                    //Paso el IdGoal1 como parametro
                    Objetives1 = _combosHelper.GetComboObjetives((note.Objective == null) ? 0 : note.Objective.Goal.Id),
                    Intervention1 = (note.Objective == null) ? string.Empty : note.Objective.Intervention,
                    MTPId = mtp.Id,
                    IdSubSchedule = (note.SubSchedule == null) ? 0: note.SubSchedule.Id
                };
            }

            return View(individualNoteViewModel);
        }

        //Individual Notes
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditIndNote(IndividualNoteViewModel model, IFormCollection form)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients

                                                          .Include(wc => wc.Workday)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.Clinic)                                                                          

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.MTPs)

                                                          .Include(wc => wc.Facilitator)

                                                          .Include(wc => wc.Schedule)
                                                          .ThenInclude(c => c.SubSchedules)

                                                          .FirstOrDefaultAsync(wc => wc.Id == model.Id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IndividualNoteEntity individualNoteEntity;
            if (ModelState.IsValid)
            {
                IndividualNoteEntity note = await _context.IndividualNotes

                                                          .Include(n => n.Workday_Cient)
                                                          .ThenInclude(wc => wc.Messages)

                                                          .Include(n => n.Objective)

                                                          .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);
               
                if (note == null)   //the note is not exist
                {
                    List<SubScheduleEntity> subSchedules = new List<SubScheduleEntity>();
                    SubScheduleEntity subScheduleEntity = new SubScheduleEntity();

                    if (workday_Client.Schedule != null)
                    {
                        subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == workday_Client.Schedule.Id).OrderBy(n => n.InitialTime).ToListAsync();
                       
                        string time = workday_Client.Session.Substring(0, 7);
                        foreach (var value in subSchedules)
                        {
                            if (value.InitialTime.ToShortTimeString().ToString().Contains(time) == true)
                            {
                                subScheduleEntity = value;
                            }
                        }

                    }

                    //note.SubSchedule = workday_Client.Schedule.SubSchedules.FirstOrDefault(n => n.Schedule.SubSchedules.Where(m => m.InitialTime.ToString().Contains(time) == true).Count() > 0);
                    //Verify the client is not present in other services of notes at the same time
                    if (this.VerifyNotesAtSameTime(model.IdClient, workday_Client.Session, workday_Client.Workday.Date, subScheduleEntity.InitialTime, subScheduleEntity.EndTime, workday_Client.Id))
                    {
                        return RedirectToAction(nameof(EditIndNote), new { id = model.Id, error = 5, origin = model.Origin });
                    }

                    model.PlanNote = (model.PlanNote.Trim().Last() == '.') ? model.PlanNote.Trim() : $"{model.PlanNote.Trim()}.";
                    model.SubjectiveData = (model.SubjectiveData.Trim().Last() == '.') ? model.SubjectiveData.Trim() : $"{model.SubjectiveData.Trim()}.";
                    model.ObjectiveData = (model.ObjectiveData.Trim().Last() == '.') ? model.ObjectiveData.Trim() : $"{model.ObjectiveData.Trim()}.";
                    model.Assessment = (model.Assessment.Trim().Last() == '.') ? model.Assessment.Trim() : $"{model.Assessment.Trim()}.";

                    individualNoteEntity = await _converterHelper.ToIndividualNoteEntity(model, true);
                    //Update plan progress
                    individualNoteEntity.Exceptional = (form["Progress"] == "Exceptional") ? true : false;
                    individualNoteEntity.Steady = (form["Progress"] == "Steady") ? true : false;
                    individualNoteEntity.Slow_Progress = (form["Progress"] == "Slow") ? true : false;
                    individualNoteEntity.Regressing = (form["Progress"] == "Regressing") ? true : false;
                    individualNoteEntity.Stable = (form["Progress"] == "Stable") ? true : false;
                    individualNoteEntity.Maintain = (form["Progress"] == "Maintain") ? true : false;

                    //vinculo el mtp activo del cliente a la nota que se creará
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == model.IdClient && m.Active == true));
                    if (mtp != null)
                        individualNoteEntity.MTPId = mtp.Id;

                    //Update selected client in Workday_Client
                    Workday_Client workday_client = await _context.Workdays_Clients  
                                                                  .Include(n => n.Schedule)
                                                                  .ThenInclude(n => n.SubSchedules)
                                                                  .FirstOrDefaultAsync(wd => wd.Id == model.Id);
                    workday_client.Client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == model.IdClient);

                    if (subScheduleEntity.Id == 0)
                    {
                        individualNoteEntity.SubSchedule = null;
                    }
                    else 
                    {
                        individualNoteEntity.SubSchedule = subScheduleEntity;
                    }
                    
                    if (workday_client.CodeBill != model.CodeBill)
                    {
                        workday_client.CodeBill = model.CodeBill;
                    }

                    _context.Update(workday_client);

                    //Create individual note
                    _context.Add(individualNoteEntity);
                    
                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(IndividualNotes));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedIndNotes));
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
                else    //the note exist, and must be updated
                {
                    //I verify that the note has a selected goal
                    if (note.Status == NoteStatus.Pending)
                    {
                        if (model.IdObjetive1 == 0)
                        {
                            return RedirectToAction(nameof(EditIndNote), new { id = model.Id, error = 1, origin = model.Origin });
                        }
                    }

                    note.PlanNote = (model.PlanNote.Trim().Last() == '.') ? model.PlanNote.Trim() : $"{model.PlanNote.Trim()}.";
                    note.SubjectiveData = (model.SubjectiveData.Trim().Last() == '.') ? model.SubjectiveData.Trim() : $"{model.SubjectiveData.Trim()}.";
                    note.ObjectiveData = (model.ObjectiveData.Trim().Last() == '.') ? model.ObjectiveData.Trim() : $"{model.ObjectiveData.Trim()}.";
                    note.Assessment = (model.Assessment.Trim().Last() == '.') ? model.Assessment.Trim() : $"{model.Assessment.Trim()}.";

                    note.Groomed = model.Groomed;
                    note.Unkempt = model.Unkempt;
                    note.Disheveled = model.Disheveled;
                    note.Meticulous = model.Meticulous;
                    note.Overbuild = model.Overbuild;
                    note.Other = model.Other;
                    note.Clear = model.Clear;
                    note.Pressured = model.Pressured;
                    note.Slurred = model.Slurred;
                    note.Slow = model.Slow;
                    note.Impaired = model.Impaired;
                    note.Poverty = model.Poverty;
                    note.Euthymic = model.Euthymic;
                    note.Depressed = model.Depressed;
                    note.Anxious = model.Anxious;
                    note.Fearful = model.Fearful;
                    note.Irritable = model.Irritable;
                    note.Labile = model.Labile;
                    note.WNL = model.WNL;
                    note.Guarded = model.Guarded;
                    note.Withdrawn = model.Withdrawn;
                    note.Hostile = model.Hostile;
                    note.Restless = model.Restless;
                    note.Impulsive = model.Impulsive;
                    note.WNL_Cognition = model.WNL_Cognition;
                    note.Blocked = model.Blocked;
                    note.Obsessive = model.Obsessive;
                    note.Paranoid = model.Paranoid;
                    note.Scattered = model.Scattered;
                    note.Psychotic = model.Psychotic;           
                    note.CBT = model.CBT;
                    note.Psychodynamic = model.Psychodynamic;
                    note.BehaviorModification = model.BehaviorModification;
                    note.Other_Intervention = model.Other_Intervention;  
                    

                    note.Objective = (model.IdObjetive1 != 0) ? await _context.Objetives.FirstOrDefaultAsync(o => o.Id == model.IdObjetive1) : null;

                    //vinculo el mtp activo del cliente a la nota que se creará
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == model.IdClient && m.Active == true));
                    if (mtp != null)
                        note.MTPId = mtp.Id;

                    note.SubSchedule = await _context.SubSchedule.FindAsync(model.IdSubSchedule);
                    _context.Update(note);

                    //Update selected client in Workday_Client
                    Workday_Client workday_client = await _context.Workdays_Clients
                                                                  
                                                                  .FirstOrDefaultAsync(wd => wd.Id == model.Id);
                    workday_client.Client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == model.IdClient);

                   

                    if (workday_client.CodeBill != model.CodeBill)
                    {
                        workday_client.CodeBill = model.CodeBill;
                    }

                    _context.Update(workday_client);                    
                    
                    List<MessageEntity> messages = note.Workday_Cient.Messages.Where(m => (m.Status == MessageStatus.NotRead && m.Notification == false)).ToList();
                    //todos los mensajes no leidos que tiene el Workday_Client de la nota los pongo como leidos
                    foreach (MessageEntity value in messages)
                    {
                        value.Status = MessageStatus.Read;
                        value.DateRead = DateTime.Now;
                        _context.Update(value);

                        //I generate a notification to supervisor
                        MessageEntity notification = new MessageEntity
                        {
                            Workday_Client = workday_Client,
                            FarsForm = null,
                            MTPReview = null,
                            Addendum = null,
                            Discharge = null,
                            Title = "Update on reviewed individual note",
                            Text = $"The individual note of {workday_Client.ClientName} on {workday_Client.Workday.Date.ToShortDateString()} was rectified",
                            From = value.To,
                            To = value.From,
                            DateCreated = DateTime.Now,
                            Status = MessageStatus.NotRead,
                            Notification = true
                        };
                        _context.Add(notification);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(IndividualNotes));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedIndNotes));
                        if (model.Origin == 2)
                            return RedirectToAction(nameof(IndNotesInEdit));
                        if (model.Origin == 3)
                            return RedirectToAction(nameof(PendingIndNotes));
                        if (model.Origin == 4)
                            return RedirectToAction(nameof(IndNotesWithReview));
                        if (model.Origin == 5)
                            return RedirectToAction("MessagesOfNotes", "Messages");
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
            }

            return View(model);
        }

        //Group Notes
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditGroupNote(int id, int error = 0, int origin = 0, string errorText = "")
        {
            GroupNoteViewModel noteViewModel;
            
            //la nota no tiene linkeado ningun goal
            if (error == 1)
                ViewBag.Error = "0";

            //la nota no esta completa, faltan campos por editar
            if (error == 2)
                ViewBag.Error = "2";

            //la nota tiene problemas con el genero
            if (error == 4)
            {
                ViewBag.Error = "4";
                ViewBag.errorText = errorText;
            }

            //el cliente seleccionado tiene una nota ya creada de otro servicio en ese mismo horario
            if (error == 5)
            {
                ViewBag.Error = "5";
            }

            Workday_Client workday_Client = await _context.Workdays_Clients

                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Workdays_Activities_Facilitators)
                                                          .ThenInclude(waf => waf.Activity)
                                                          .ThenInclude(a => a.Theme)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.Clinic)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.Group)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.MTPs)

                                                          .Include(wc => wc.Facilitator)
                                                          .Include(wc => wc.Schedule)

                                                          .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            FacilitatorEntity facilitator_logged = _context.Facilitators
                                                           .FirstOrDefault(f => f.LinkedUser == User.Identity.Name);

            //el dia no tiene actividad asociada para el facilitator logueado por lo tanto no se puede crear la nota
            if (workday_Client.Workday.Workdays_Activities_Facilitators.Where(waf => waf.Facilitator == facilitator_logged).Count() == 0)
            {
                ViewBag.Error = "1";
                noteViewModel = new GroupNoteViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }

            //el cliente no tiene mtp activos
            if (workday_Client.Client.MTPs.Where(m => m.Active == true).Count() == 0)
            {
                ViewBag.Error = "3";
                noteViewModel = new GroupNoteViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }

            GroupNoteEntity note = await _context.GroupNotes

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(wc => wc.Client)
                                                 .ThenInclude(c => c.Group)

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(g => g.Facilitator)

                                                 .Include(n => n.GroupNotes_Activities)
                                                 .ThenInclude(na => na.Activity)

                                                 .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            GroupNote2Entity note2 = await _context.GroupNotes2

                                                   .Include(n => n.Workday_Cient)
                                                
                                                   .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            if (note == null && note2 == null)
            {
                if (workday_Client.Client.Clinic.SchemaGroup == SchemaTypeGroup.Schema2)
                {
                    return RedirectToAction(nameof(EditGroupNote2), new { id = id, error = errorText, origin = origin, errorText = errorText });
                }
                if (workday_Client.Client.Clinic.SchemaGroup == SchemaTypeGroup.Schema3)
                {
                    return RedirectToAction(nameof(EditGroupNote3), new { id = id, error = errorText, origin = origin, errorText = errorText });
                }
            }


            if ((note2 != null))
            {
                if (note2.Schema == SchemaTypeGroup.Schema2)
                {
                    return RedirectToAction(nameof(EditGroupNote2), new { id = id, error = error, origin = origin, errorText = errorText });
                }
                if (note2.Schema == SchemaTypeGroup.Schema3)
                {
                    return RedirectToAction(nameof(EditGroupNote3), new { id = id, error = error, origin = origin, errorText = errorText });
                }
            }
            //-----------se selecciona el primer MTP activo que tenga el cliente-----------//
            MTPEntity mtp = _context.MTPs.FirstOrDefault(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));

            List<Workday_Activity_Facilitator> activities = workday_Client.Workday
                                                                          .Workdays_Activities_Facilitators
                                                                          .Where(waf => waf.Facilitator == facilitator_logged)
                                                                          .ToList();

            //Evaluate setting for goals's classification
            SettingEntity setting = _context.Settings
                                            .FirstOrDefault(s => s.Clinic.Id == facilitator_logged.Clinic.Id);

            if (note == null)   //la nota no está creada
            {
                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.Group);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }

                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }

                noteViewModel = new GroupNoteViewModel
                {
                    Id = id,
                    Status = NoteStatus.Pending,    //es solo generico para la visualizacion del btn FinishEditing
                    Origin = origin,      
                    CodeBill = workday_Client.Client.Clinic.CodeGroupTherapy,

                    //IdTopic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Id : 0,
                    Topic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    Activity1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    Goals1 = goals,
                    Objetives1 = objs,

                    //IdTopic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Id : 0,
                    Topic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Name : string.Empty,
                    IdActivity2 = (activities.Count > 1) ? activities[1].Activity.Id : 0,
                    Activity2 = (activities.Count > 1) ? activities[1].Activity.Name : string.Empty,
                    Goals2 = goals,
                    Objetives2 = objs,                    

                    Workday_Cient = workday_Client
                };
            }
            else
            {
                List<GroupNote_Activity> note_Activity = await _context.GroupNotes_Activities

                                                                       .Include(na => na.Activity)
                                                                       .ThenInclude(a => a.Theme)

                                                                       .Include(na => na.Objetive)
                                                                       .ThenInclude(o => o.Goal)

                                                                       .Where(na => na.GroupNote.Id == note.Id)
                                                                       .ToListAsync();

                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.Group);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }

                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }

                noteViewModel = new GroupNoteViewModel
                {
                    Id = id,
                    Origin = origin,
                    Workday_Cient = workday_Client,
                    PlanNote = note.PlanNote,
                    Status = note.Status,
                    CodeBill = workday_Client.CodeBill,

                    Groomed = note.Groomed,
                    Unkempt = note.Unkempt,
                    Disheveled = note.Disheveled,
                    Meticulous = note.Meticulous,
                    Overbuild = note.Overbuild,
                    Other = note.Other,
                    Clear = note.Clear,
                    Pressured = note.Pressured,
                    Slurred = note.Slurred,
                    Slow = note.Slow,
                    Impaired = note.Impaired,
                    Poverty = note.Poverty,
                    Euthymic = note.Euthymic,
                    Depressed = note.Depressed,
                    Anxious = note.Anxious,
                    Fearful = note.Fearful,
                    Irritable = note.Irritable,
                    Labile = note.Labile,
                    WNL = note.WNL,
                    Guarded = note.Guarded,
                    Withdrawn = note.Withdrawn,
                    Hostile = note.Hostile,
                    Restless = note.Restless,
                    Impulsive = note.Impulsive,
                    WNL_Cognition = note.WNL_Cognition,
                    Blocked = note.Blocked,
                    Obsessive = note.Obsessive,
                    Paranoid = note.Paranoid,
                    Scattered = note.Scattered,
                    Psychotic = note.Psychotic,
                    Exceptional = note.Exceptional,
                    Steady = note.Steady,
                    Slow_Progress = note.Slow_Progress,
                    Regressing = note.Regressing,
                    Stable = note.Stable,
                    Maintain = note.Maintain,
                    CBT = note.CBT,
                    Psychodynamic = note.Psychodynamic,
                    BehaviorModification = note.BehaviorModification,
                    Other_Intervention = note.Other_Intervention,

                    Topic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    Activity1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    AnswerClient1 = note_Activity[0].AnswerClient,
                    AnswerFacilitator1 = note_Activity[0].AnswerFacilitator,
                    IdGoal1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Goal.Id : 0,
                    Goals1 = goals,
                    IdObjetive1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Id : 0,
                    //Paso el IdGoal1 como parametro
                    Objetives1 = _combosHelper.GetComboObjetives(((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Goal.Id : 0),
                    Intervention1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Intervention : string.Empty,

                    Topic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Name : string.Empty,
                    IdActivity2 = (activities.Count > 1) ? activities[1].Activity.Id : 0,
                    Activity2 = (activities.Count > 1) ? activities[1].Activity.Name : string.Empty,
                    AnswerClient2 = note_Activity[1].AnswerClient,
                    AnswerFacilitator2 = note_Activity[1].AnswerFacilitator,
                    IdGoal2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Goal.Id : 0,
                    Goals2 = goals,
                    IdObjetive2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Id : 0,
                    //Paso el IdGoal2 como parametro
                    Objetives2 = _combosHelper.GetComboObjetives(((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Goal.Id : 0),
                    Intervention2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Intervention : string.Empty                    
                };
            }
            return View(noteViewModel);
        }

        //Group Notes
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditGroupNote(GroupNoteViewModel model, IFormCollection form)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.MTPs)

                                                                           .Include(wc => wc.Facilitator)
                                                                           .Include(wc => wc.Schedule)

                                                                           .FirstOrDefaultAsync(wc => wc.Id == model.Id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }
                        
            if (ModelState.IsValid)
            {
                GroupNoteEntity note = await _context.GroupNotes
                                                
                                                     .Include(n => n.Workday_Cient)
                                                     .ThenInclude(wc => wc.Messages)
                                                
                                                     .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);
                
                GroupNote_Activity note_Activity;
                GroupNoteEntity groupNoteEntity;
                if (note == null)   //la nota no está creada
                {
                    //Verify the client is not present in other services of notes at the same time
                    if (this.VerifyNotesAtSameTime(workday_Client.Client.Id, workday_Client.Session, workday_Client.Workday.Date))
                    {
                        return RedirectToAction(nameof(EditGroupNote), new { id = model.Id, error = 5, origin = model.Origin });
                    }

                    groupNoteEntity = await _converterHelper.ToGroupNoteEntity(model, true);

                    //Update plan progress
                    groupNoteEntity.Exceptional = (form["Progress"] == "Exceptional") ? true : false;
                    groupNoteEntity.Steady = (form["Progress"] == "Steady") ? true : false;
                    groupNoteEntity.Slow_Progress = (form["Progress"] == "Slow") ? true : false;
                    groupNoteEntity.Regressing = (form["Progress"] == "Regressing") ? true : false;
                    groupNoteEntity.Stable = (form["Progress"] == "Stable") ? true : false;
                    groupNoteEntity.Maintain = (form["Progress"] == "Maintain") ? true : false;            

                    //vinculo el mtp activo del cliente a la nota que se creará
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
                    if (mtp != null)
                        groupNoteEntity.MTPId = mtp.Id;

                    _context.Add(groupNoteEntity);

                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == workday_Client.Schedule.Id).OrderBy(n => n.InitialTime).ToListAsync();

                    note_Activity = new GroupNote_Activity
                    {
                        GroupNote = groupNoteEntity,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1),
                        AnswerClient = model.AnswerClient1.Trim(),
                        AnswerFacilitator = (model.AnswerFacilitator1.Trim().Last() == '.') ? model.AnswerFacilitator1.Trim() : $"{model.AnswerFacilitator1.Trim()}.",
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1),
                        SubSchedule = subSchedules.ElementAtOrDefault(0)
                    };
                    _context.Add(note_Activity);
                    note_Activity = new GroupNote_Activity
                    {
                        GroupNote = groupNoteEntity,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity2),
                        AnswerClient = (model.AnswerClient2 != null) ? model.AnswerClient2.Trim() : string.Empty,
                        AnswerFacilitator = (model.AnswerFacilitator2 != null) ? ((model.AnswerFacilitator2.Trim().Last() == '.') ? model.AnswerFacilitator2.Trim() : $"{model.AnswerFacilitator2.Trim()}.") : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive2),
                        SubSchedule = subSchedules.ElementAtOrDefault(1)
                    };
                    _context.Add(note_Activity);

                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(GroupNotes));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedGroupNotes));
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
                else    //la nota está creada y sólo se debe actualizar
                {
                    //I verify that the note has a selected goal
                    if (note.Status == NoteStatus.Pending)
                    {
                        if (model.IdObjetive1 == 0 && model.IdObjetive2 == 0)
                        {
                            return RedirectToAction(nameof(EditGroupNote), new { id = model.Id, error = 1, origin = model.Origin });
                        }
                    }

                    note.PlanNote = (model.PlanNote.Trim().Last() == '.') ? model.PlanNote.Trim() : $"{model.PlanNote.Trim()}.";

                    note.Groomed = model.Groomed;
                    note.Unkempt = model.Unkempt;
                    note.Disheveled = model.Disheveled;
                    note.Meticulous = model.Meticulous;
                    note.Overbuild = model.Overbuild;
                    note.Other = model.Other;
                    note.Clear = model.Clear;
                    note.Pressured = model.Pressured;
                    note.Slurred = model.Slurred;
                    note.Slow = model.Slow;
                    note.Impaired = model.Impaired;
                    note.Poverty = model.Poverty;
                    note.Euthymic = model.Euthymic;
                    note.Depressed = model.Depressed;
                    note.Anxious = model.Anxious;
                    note.Fearful = model.Fearful;
                    note.Irritable = model.Irritable;
                    note.Labile = model.Labile;
                    note.WNL = model.WNL;
                    note.Guarded = model.Guarded;
                    note.Withdrawn = model.Withdrawn;
                    note.Hostile = model.Hostile;
                    note.Restless = model.Restless;
                    note.Impulsive = model.Impulsive;
                    note.WNL_Cognition = model.WNL_Cognition;
                    note.Blocked = model.Blocked;
                    note.Obsessive = model.Obsessive;
                    note.Paranoid = model.Paranoid;
                    note.Scattered = model.Scattered;
                    note.Psychotic = model.Psychotic;
                    note.CBT = model.CBT;
                    note.Psychodynamic = model.Psychodynamic;
                    note.BehaviorModification = model.BehaviorModification;
                    note.Other_Intervention = model.Other_Intervention;

                    //actualizo el mtp activo del cliente a la nota que se creará                   
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
                    if (mtp != null)
                        note.MTPId = mtp.Id;

                    _context.Update(note);

                    List<GroupNote_Activity> noteActivities_list = await _context.GroupNotes_Activities
                                                                                 .Where(na => na.GroupNote.Id == note.Id)
                                                                                 .ToListAsync();
                    
                    _context.RemoveRange(noteActivities_list);

                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == workday_Client.Schedule.Id).OrderBy(n => n.InitialTime).ToListAsync();

                    note_Activity = new GroupNote_Activity
                    {
                        GroupNote = note,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1),
                        AnswerClient = model.AnswerClient1.Trim(),
                        AnswerFacilitator = (model.AnswerFacilitator1.Trim().Last() == '.') ? model.AnswerFacilitator1.Trim() : $"{model.AnswerFacilitator1.Trim()}.",
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1),
                        SubSchedule = subSchedules.ElementAtOrDefault(0)
                    };
                    _context.Add(note_Activity);
                    await _context.SaveChangesAsync();

                    note_Activity = new GroupNote_Activity
                    {
                        GroupNote = note,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity2),
                        AnswerClient = (model.AnswerClient2 != null) ? model.AnswerClient2.Trim() : string.Empty,
                        AnswerFacilitator = (model.AnswerFacilitator2 != null) ? ((model.AnswerFacilitator2.Trim().Last() == '.') ? model.AnswerFacilitator2.Trim() : $"{model.AnswerFacilitator2.Trim()}.") : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive2),
                        SubSchedule = subSchedules.ElementAtOrDefault(1)
                    };
                    _context.Add(note_Activity);
                    
                    List<MessageEntity> messages = note.Workday_Cient.Messages.Where(m => (m.Status == MessageStatus.NotRead && m.Notification == false)).ToList();
                    //todos los mensajes no leidos que tiene el Workday_Client de la nota los pongo como leidos
                    foreach (MessageEntity value in messages)
                    {
                        value.Status = MessageStatus.Read;
                        value.DateRead = DateTime.Now;
                        _context.Update(value);

                        //I generate a notification to supervisor
                        MessageEntity notification = new MessageEntity
                        {
                            Workday_Client = workday_Client,
                            FarsForm = null,
                            MTPReview = null,
                            Addendum = null,
                            Discharge = null,
                            Title = "Update on reviewed group note",
                            Text = $"The group note of {workday_Client.ClientName} on {workday_Client.Workday.Date.ToShortDateString()} was rectified",
                            From = value.To,
                            To = value.From,
                            DateCreated = DateTime.Now,
                            Status = MessageStatus.NotRead,
                            Notification = true
                        };
                        _context.Add(notification);
                    }

                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(GroupNotes));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedGroupNotes));
                        if (model.Origin == 2)
                            return RedirectToAction(nameof(GroupNotesInEdit));
                        if (model.Origin == 3)
                            return RedirectToAction(nameof(PendingGroupNotes));
                        if (model.Origin == 4)
                            return RedirectToAction(nameof(GroupNotesWithReview));
                        if (model.Origin == 5)
                            return RedirectToAction("MessagesOfNotes", "Messages");
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditGroupNote2(int id, int error = 0, int origin = 0, string errorText = "")
        {
            GroupNote2ViewModel noteViewModel;

            //la nota no tiene linkeado ningun goal
            if (error == 1)
                ViewBag.Error = "0";

            //la nota no esta completa, faltan campos por editar
            if (error == 2)
                ViewBag.Error = "2";

            //la nota tiene problemas con el genero
            if (error == 4)
            {
                ViewBag.Error = "4";
                ViewBag.errorText = errorText;
            }

            //el cliente seleccionado tiene una nota ya creada de otro servicio en ese mismo horario
            if (error == 5)
            {
                ViewBag.Error = "5";
            }

            Workday_Client workday_Client = await _context.Workdays_Clients

                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Workdays_Activities_Facilitators)
                                                          .ThenInclude(waf => waf.Activity)
                                                          .ThenInclude(a => a.Theme)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.Clinic)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.Group)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.MTPs)

                                                          .Include(wc => wc.Facilitator)
                                                          .Include(wc => wc.Schedule)

                                                          .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            FacilitatorEntity facilitator_logged = _context.Facilitators
                                                           .FirstOrDefault(f => f.LinkedUser == User.Identity.Name);

            //el dia no tiene actividad asociada para el facilitator logueado por lo tanto no se puede crear la nota
            if (workday_Client.Workday.Workdays_Activities_Facilitators.Where(waf => waf.Facilitator == facilitator_logged).Count() == 0)
            {
                ViewBag.Error = "1";
                noteViewModel = new GroupNote2ViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }

            //el cliente no tiene mtp activos
            if (workday_Client.Client.MTPs.Where(m => m.Active == true).Count() == 0)
            {
                ViewBag.Error = "3";
                noteViewModel = new GroupNote2ViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }

            GroupNote2Entity note = await _context.GroupNotes2

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(wc => wc.Client)
                                                 .ThenInclude(c => c.Group)

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(g => g.Facilitator)

                                                 .Include(n => n.GroupNotes2_Activities)
                                                 .ThenInclude(na => na.Activity)

                                                 .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            //-----------se selecciona el primer MTP activo que tenga el cliente-----------//
            MTPEntity mtp = _context.MTPs.FirstOrDefault(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));

            List<Workday_Activity_Facilitator> activities = workday_Client.Workday
                                                                          .Workdays_Activities_Facilitators
                                                                          .Where(waf => waf.Facilitator == facilitator_logged)
                                                                          .ToList();

            //Evaluate setting for goals's classification
            SettingEntity setting = _context.Settings
                                            .FirstOrDefault(s => s.Clinic.Id == facilitator_logged.Clinic.Id);

            if (note == null)   //la nota no está creada
            {
                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.Group);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }

                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }

                noteViewModel = new GroupNote2ViewModel
                {
                    Id = id,
                    Status = NoteStatus.Pending,    //es solo generico para la visualizacion del btn FinishEditing
                    Origin = origin,
                    CodeBill = workday_Client.Client.Clinic.CodeGroupTherapy,
                    GroupLeaderFacilitatorAbout = workday_Client.Workday.Workdays_Activities_Facilitators.ElementAt(0).Activity.Theme.Name,

                    //IdTopic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Id : 0,
                    Topic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    Activity1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    Goals1 = goals,
                    Objetives1 = objs,

                    //IdTopic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Id : 0,
                    Topic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Name : string.Empty,
                    IdActivity2 = (activities.Count > 1) ? activities[1].Activity.Id : 0,
                    Activity2 = (activities.Count > 1) ? activities[1].Activity.Name : string.Empty,
                    Goals2 = goals,
                    Objetives2 = objs,

                    Workday_Cient = workday_Client,
                    Schema = workday_Client.Client.Clinic.SchemaGroup,
                    Workday_Client_FK = workday_Client.Id
                };
            }
            else
            {
                List<GroupNote2_Activity> note_Activity = await _context.GroupNotes2_Activities

                                                                       .Include(na => na.Activity)
                                                                       .ThenInclude(a => a.Theme)

                                                                       .Include(na => na.Objetive)
                                                                       .ThenInclude(o => o.Goal)

                                                                       .Where(na => na.GroupNote2.Id == note.Id)
                                                                       .ToListAsync();

                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.Group);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }

                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }

                noteViewModel = new GroupNote2ViewModel
                {
                    Id = id,
                    Origin = origin,
                    Workday_Cient = workday_Client,
                    Status = note.Status,
                    CodeBill = workday_Client.CodeBill,
                    Schema = note.Schema,
                    
                    Other = note.Other,
                    Impaired = note.Impaired,
                    Euthymic = note.Euthymic,
                    Depressed = note.Depressed,
                    Anxious = note.Anxious,
                    Irritable = note.Irritable,
                    Guarded = note.Guarded,
                    Withdrawn = note.Withdrawn,
                    Hostile = note.Hostile,
                    Adequated = note.Adequated,
                    Assigned = note.Assigned,
                    AssignedTopicOf = note.AssignedTopicOf,
                    Congruent = note.Congruent,
                    Descompensating = note.Descompensating,
                    Developing = note.Developing,
                    Dramatic = note.Dramatic,
                    Euphoric = note.Euphoric,
                    Expressing = note.Expressing,
                    Facilitated = note.Facilitated,
                    Fair = note.Fair,
                    FairAttitude = note.FairAttitude,
                    Faulty = note.Faulty,
                    Getting = note.Getting,
                    GroupLeaderFacilitator = note.GroupLeaderFacilitator,
                    GroupLeaderFacilitatorAbout = note.GroupLeaderFacilitatorAbout,
                    GroupLeaderProviderPsychoeducation = note.GroupLeaderProviderPsychoeducation,
                    GroupLeaderProviderSupport = note.GroupLeaderProviderSupport,
                    Inadequated = note.Inadequated,
                    InsightAdequate = note.InsightAdequate,
                    Involved = note.Involved,
                    Kept = note.Kept,
                    LearningAbout = note.LearningAbout,
                    LearningFrom = note.LearningFrom,
                    Limited = note.Limited,
                    MildlyImpaired = note.MildlyImpaired,
                    MinimalProgress = note.MinimalProgress,
                    ModerateProgress = note.ModerateProgress,
                    Motivated = note.Motivated,
                    Negativistic = note.Negativistic,
                    NoProgress = note.NoProgress,
                    Normal = note.Normal,
                    NotToPerson = note.NotToPerson,
                    NotToPlace = note.NotToPlace,
                    NotToTime = note.NotToTime,
                    Optimistic = note.Optimistic,
                    Oriented = note.Oriented,
                    OtherExplain = note.OtherExplain,
                    Providing = note.Providing,
                    Received = note.Received,
                    Regression = note.Regression,
                    SevereryImpaired = note.SevereryImpaired,
                    Sharing = note.Sharing,
                    Short = note.Short,
                    SignificantProgress = note.SignificantProgress,
                    UnableToDetermine = note.UnableToDetermine,
                    Unmotivated = note.Unmotivated,
                    Workday_Client_FK = workday_Client.Id,



                    Topic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    Activity1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    AnswerClient1 = note_Activity[0].AnswerClient,
                    IdGoal1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Goal.Id : 0,
                    Goals1 = goals,
                    IdObjetive1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Id : 0,
                    //Paso el IdGoal1 como parametro
                    Objetives1 = _combosHelper.GetComboObjetives(((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Goal.Id : 0),
                    Intervention1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Intervention : string.Empty,

                    Topic2 = (activities.Count > 1) ? activities[1].Activity.Theme.Name : string.Empty,
                    IdActivity2 = (activities.Count > 1) ? activities[1].Activity.Id : 0,
                    Activity2 = (activities.Count > 1) ? activities[1].Activity.Name : string.Empty,
                    AnswerClient2 = note_Activity[1].AnswerClient,
                    IdGoal2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Goal.Id : 0,
                    Goals2 = goals,
                    IdObjetive2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Id : 0,
                    //Paso el IdGoal2 como parametro
                    Objetives2 = _combosHelper.GetComboObjetives(((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Goal.Id : 0),
                    Intervention2 = ((note_Activity.Count > 1) && (note_Activity[1].Objetive != null)) ? note_Activity[1].Objetive.Intervention : string.Empty
                };
            }
            return View(noteViewModel);
        }

        //Group Notes
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditGroupNote2(GroupNote2ViewModel model, IFormCollection form)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.MTPs)

                                                                           .Include(wc => wc.Facilitator)
                                                                           .Include(wc => wc.Schedule)

                                                                           .FirstOrDefaultAsync(wc => wc.Id == model.Id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (ModelState.IsValid)
            {
                GroupNote2Entity note = await _context.GroupNotes2

                                                      .Include(n => n.Workday_Cient)
                                                      .ThenInclude(wc => wc.Messages)

                                                      .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);

                GroupNote2_Activity note_Activity;
                GroupNote2Entity groupNoteEntity;
                if (note == null)   //la nota no está creada
                {
                    //Verify the client is not present in other services of notes at the same time
                    if (this.VerifyNotesAtSameTime(workday_Client.Client.Id, workday_Client.Session, workday_Client.Workday.Date))
                    {
                        return RedirectToAction(nameof(EditGroupNote), new { id = model.Id, error = 5, origin = model.Origin });
                    }

                    groupNoteEntity = await _converterHelper.ToGroupNote2Entity(model, true);

                    //Update plan progress
                    /*groupNoteEntity.SignificantProgress = (form["Progress"] == "SignificantProgress") ? true : false;
                    groupNoteEntity.ModerateProgress = (form["Progress"] == "ModerateProgress") ? true : false;
                    groupNoteEntity.MinimalProgress = (form["Progress"] == "MinimalProgress") ? true : false;
                    groupNoteEntity.NoProgress = (form["Progress"] == "NoProgress") ? true : false;
                    groupNoteEntity.Regression = (form["Progress"] == "Regression") ? true : false;
                    groupNoteEntity.Descompensating = (form["Progress"] == "Decompensating") ? true : false;
                    groupNoteEntity.UnableToDetermine = (form["Progress"] == "Unabled") ? true : false;*/

                    //vinculo el mtp activo del cliente a la nota que se creará
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
                    if (mtp != null)
                        groupNoteEntity.MTPId = mtp.Id;

                    _context.Add(groupNoteEntity);

                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == workday_Client.Schedule.Id).OrderBy(n => n.InitialTime).ToListAsync();

                    note_Activity = new GroupNote2_Activity
                    {
                        GroupNote2 = groupNoteEntity,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1),
                        AnswerClient = model.AnswerClient1.Trim(),
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1),
                        SubSchedule = subSchedules.ElementAtOrDefault(0)
                    };
                    _context.Add(note_Activity);
                    note_Activity = new GroupNote2_Activity
                    {
                        GroupNote2 = groupNoteEntity,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity2),
                        AnswerClient = (model.AnswerClient2 != null) ? model.AnswerClient2.Trim() : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive2),
                        SubSchedule = subSchedules.ElementAtOrDefault(1)
                    };
                    _context.Add(note_Activity);

                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(GroupNotes));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedGroupNotes));
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
                else    //la nota está creada y sólo se debe actualizar
                {
                    //I verify that the note has a selected goal
                    if (note.Status == NoteStatus.Pending)
                    {
                        if (model.IdObjetive1 == 0 && model.IdObjetive2 == 0)
                        {
                            return RedirectToAction(nameof(EditGroupNote2), new { id = model.Id, error = 1, origin = model.Origin });
                        }
                    }

                    note.Adequated = model.Adequated;
                    note.Assigned = model.Assigned;
                    note.AssignedTopicOf = model.AssignedTopicOf;
                    note.Congruent = model.Congruent;
                    note.Descompensating = model.Descompensating;
                    note.Developing = model.Developing;
                    note.Dramatic = model.Dramatic;
                    note.Euphoric = model.Euphoric;
                    note.Expressing = model.Expressing;
                    note.Facilitated = model.Facilitated;
                    note.Fair = model.Fair;
                    note.FairAttitude = model.FairAttitude;
                    note.Faulty = model.Faulty;
                    note.Getting = model.Getting;
                    note.GroupLeaderFacilitator = model.GroupLeaderFacilitator;
                    note.GroupLeaderFacilitatorAbout = model.GroupLeaderFacilitatorAbout;
                    note.GroupLeaderProviderPsychoeducation = model.GroupLeaderProviderPsychoeducation;
                    note.GroupLeaderProviderSupport = model.GroupLeaderProviderSupport;
                    note.Inadequated = model.Inadequated;
                    note.InsightAdequate = model.InsightAdequate;
                    note.Involved = model.Involved;
                    note.Kept = model.Kept;
                    note.LearningAbout = model.LearningAbout;
                    note.LearningFrom = model.LearningFrom;
                    note.Limited = model.Limited;
                    note.MildlyImpaired = model.MildlyImpaired;
                    note.MinimalProgress = model.MinimalProgress;
                    note.ModerateProgress = model.ModerateProgress;
                    note.Motivated = model.Motivated;
                    note.Negativistic = model.Negativistic;
                    note.NoProgress = model.NoProgress;
                    note.Normal = model.Normal;
                    note.NotToPerson = model.NotToPerson;
                    note.NotToPlace = model.NotToPlace;
                    note.NotToTime = model.NotToTime;
                    note.Optimistic = model.Optimistic;
                    note.Oriented = model.Oriented;
                    note.OtherExplain = model.OtherExplain;
                    note.Providing = model.Providing;
                    note.Received = model.Received;
                    note.Regression = model.Regression;
                    note.SevereryImpaired = model.SevereryImpaired;
                    note.Sharing = model.Sharing;
                    note.Short = model.Short;
                    note.SignificantProgress = model.SignificantProgress;
                    note.UnableToDetermine = model.UnableToDetermine;
                    note.Unmotivated = model.Unmotivated;
                    note.MTPId = model.MTPId;
                    note.Other = model.Other;
                    note.Impaired = model.Impaired;
                    note.Euthymic = model.Euthymic;
                    note.Depressed = model.Depressed;
                    note.Anxious = model.Anxious;
                    note.Irritable = model.Irritable;
                    note.Guarded = model.Guarded;
                    note.Withdrawn = model.Withdrawn;
                    note.Hostile = model.Hostile;
                    note.Schema = model.Schema;
                    
                    
                    //actualizo el mtp activo del cliente a la nota que se creará                   
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
                    if (mtp != null)
                        note.MTPId = mtp.Id;

                    _context.Update(note);

                    List<GroupNote2_Activity> noteActivities_list = await _context.GroupNotes2_Activities
                                                                                 .Where(na => na.GroupNote2.Id == note.Id)
                                                                                 .ToListAsync();

                    _context.RemoveRange(noteActivities_list);

                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == workday_Client.Schedule.Id).OrderBy(n => n.InitialTime).ToListAsync();

                    note_Activity = new GroupNote2_Activity
                    {
                        GroupNote2 = note,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1),
                        AnswerClient = model.AnswerClient1.Trim(),
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1),
                        SubSchedule = subSchedules.ElementAtOrDefault(0)
                    };
                    _context.Add(note_Activity);
                    await _context.SaveChangesAsync();

                    note_Activity = new GroupNote2_Activity
                    {
                        GroupNote2 = note,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity2),
                        AnswerClient = (model.AnswerClient2 != null) ? model.AnswerClient2.Trim() : string.Empty,
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive2),
                        SubSchedule = subSchedules.ElementAtOrDefault(1)
                    };
                    _context.Add(note_Activity);

                    List<MessageEntity> messages = note.Workday_Cient.Messages.Where(m => (m.Status == MessageStatus.NotRead && m.Notification == false)).ToList();
                    //todos los mensajes no leidos que tiene el Workday_Client de la nota los pongo como leidos
                    foreach (MessageEntity value in messages)
                    {
                        value.Status = MessageStatus.Read;
                        value.DateRead = DateTime.Now;
                        _context.Update(value);

                        //I generate a notification to supervisor
                        MessageEntity notification = new MessageEntity
                        {
                            Workday_Client = workday_Client,
                            FarsForm = null,
                            MTPReview = null,
                            Addendum = null,
                            Discharge = null,
                            Title = "Update on reviewed group note",
                            Text = $"The group note of {workday_Client.ClientName} on {workday_Client.Workday.Date.ToShortDateString()} was rectified",
                            From = value.To,
                            To = value.From,
                            DateCreated = DateTime.Now,
                            Status = MessageStatus.NotRead,
                            Notification = true
                        };
                        _context.Add(notification);
                    }

                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(GroupNotes));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedGroupNotes));
                        if (model.Origin == 2)
                            return RedirectToAction(nameof(GroupNotesInEdit));
                        if (model.Origin == 3)
                            return RedirectToAction(nameof(PendingGroupNotes));
                        if (model.Origin == 4)
                            return RedirectToAction(nameof(GroupNotesWithReview));
                        if (model.Origin == 5)
                            return RedirectToAction("MessagesOfNotes", "Messages");
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditGroupNote3(int id, int error = 0, int origin = 0, string errorText = "")
        {
            GroupNote3ViewModel noteViewModel;

            //la nota no tiene linkeado ningun goal
            if (error == 1)
                ViewBag.Error = "0";

            //la nota no esta completa, faltan campos por editar
            if (error == 2)
                ViewBag.Error = "2";

            //la nota tiene problemas con el genero
            if (error == 4)
            {
                ViewBag.Error = "4";
                ViewBag.errorText = errorText;
            }

            //el cliente seleccionado tiene una nota ya creada de otro servicio en ese mismo horario
            if (error == 5)
            {
                ViewBag.Error = "5";
            }

            Workday_Client workday_Client = await _context.Workdays_Clients

                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Workdays_Activities_Facilitators)
                                                          .ThenInclude(waf => waf.Activity)
                                                          .ThenInclude(a => a.Theme)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.Clinic)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.Group)

                                                          .Include(wc => wc.Client)
                                                          .ThenInclude(c => c.MTPs)

                                                          .Include(wc => wc.Facilitator)
                                                          .Include(wc => wc.Schedule)

                                                          .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            FacilitatorEntity facilitator_logged = _context.Facilitators
                                                           .FirstOrDefault(f => f.LinkedUser == User.Identity.Name);

            //el dia no tiene actividad asociada para el facilitator logueado por lo tanto no se puede crear la nota
            if (workday_Client.Workday.Workdays_Activities_Facilitators.Where(waf => waf.Facilitator == facilitator_logged).Count() == 0)
            {
                ViewBag.Error = "1";
                noteViewModel = new GroupNote3ViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }

            //el cliente no tiene mtp activos
            if (workday_Client.Client.MTPs.Where(m => m.Active == true).Count() == 0)
            {
                ViewBag.Error = "3";
                noteViewModel = new GroupNote3ViewModel
                {
                    Id = workday_Client.Workday.Id,
                };
                return View(noteViewModel);
            }

            GroupNote2Entity note = await _context.GroupNotes2

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(wc => wc.Client)
                                                 .ThenInclude(c => c.Group)

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(g => g.Facilitator)

                                                 .Include(n => n.GroupNotes2_Activities)
                                                 .ThenInclude(na => na.Activity)

                                                 .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            //-----------se selecciona el primer MTP activo que tenga el cliente-----------//
            MTPEntity mtp = _context.MTPs.FirstOrDefault(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));

            List<Workday_Activity_Facilitator> activities = workday_Client.Workday
                                                                          .Workdays_Activities_Facilitators
                                                                          .Where(waf => waf.Facilitator == facilitator_logged)
                                                                          .ToList();

            //Evaluate setting for goals's classification
            SettingEntity setting = _context.Settings
                                            .FirstOrDefault(s => s.Clinic.Id == facilitator_logged.Clinic.Id);

            if (note == null)   //la nota no está creada
            {
                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.Group);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }

                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }

                noteViewModel = new GroupNote3ViewModel
                {
                    Id = id,
                    Status = NoteStatus.Pending,    //es solo generico para la visualizacion del btn FinishEditing
                    Origin = origin,
                    CodeBill = workday_Client.Client.Clinic.CodeGroupTherapy,
                    GroupLeaderFacilitatorAbout = workday_Client.Workday.Workdays_Activities_Facilitators.ElementAt(0).Activity.Theme.Name,

                    //IdTopic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Id : 0,
                    Topic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    Activity1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    Goals1 = goals,
                    Objetives1 = objs,

                    Workday_Cient = workday_Client,
                    Schema = workday_Client.Client.Clinic.SchemaGroup,
                    Workday_Client_FK = workday_Client.Id
                };
            }
            else
            {
                List<GroupNote2_Activity> note_Activity = await _context.GroupNotes2_Activities

                                                                       .Include(na => na.Activity)
                                                                       .ThenInclude(a => a.Theme)

                                                                       .Include(na => na.Objetive)
                                                                       .ThenInclude(o => o.Goal)

                                                                       .Where(na => na.GroupNote2.Id == note.Id)
                                                                       .ToListAsync();

                IEnumerable<SelectListItem> goals = null;
                IEnumerable<SelectListItem> objs = null;
                if (mtp != null)
                {
                    if (setting != null)
                    {
                        if (setting.MHClassificationOfGoals)
                            goals = _combosHelper.GetComboGoalsByService(mtp.Id, ServiceType.Group);
                        else
                            goals = _combosHelper.GetComboGoals(mtp.Id);
                    }

                    objs = _combosHelper.GetComboObjetives(0);
                }
                else
                {
                    goals = _combosHelper.GetComboGoals(0);
                    objs = _combosHelper.GetComboObjetives(0);
                }

                noteViewModel = new GroupNote3ViewModel
                {
                    Id = id,
                    Origin = origin,
                    Workday_Cient = workday_Client,
                    Status = note.Status,
                    CodeBill = workday_Client.CodeBill,
                    Schema = note.Schema,
                   
                    Other = note.Other,
                    Impaired = note.Impaired,
                    Euthymic = note.Euthymic,
                    Depressed = note.Depressed,
                    Anxious = note.Anxious,
                    Irritable = note.Irritable,
                    Guarded = note.Guarded,
                    Withdrawn = note.Withdrawn,
                    Hostile = note.Hostile,
                    Adequated = note.Adequated,
                    Assigned = note.Assigned,
                    AssignedTopicOf = note.AssignedTopicOf,
                    Congruent = note.Congruent,
                    Descompensating = note.Descompensating,
                    Developing = note.Developing,
                    Dramatic = note.Dramatic,
                    Euphoric = note.Euphoric,
                    Expressing = note.Expressing,
                    Facilitated = note.Facilitated,
                    Fair = note.Fair,
                    FairAttitude = note.FairAttitude,
                    Faulty = note.Faulty,
                    Getting = note.Getting,
                    GroupLeaderFacilitator = note.GroupLeaderFacilitator,
                    GroupLeaderFacilitatorAbout = note.GroupLeaderFacilitatorAbout,
                    GroupLeaderProviderPsychoeducation = note.GroupLeaderProviderPsychoeducation,
                    GroupLeaderProviderSupport = note.GroupLeaderProviderSupport,
                    Inadequated = note.Inadequated,
                    InsightAdequate = note.InsightAdequate,
                    Involved = note.Involved,
                    Kept = note.Kept,
                    LearningAbout = note.LearningAbout,
                    LearningFrom = note.LearningFrom,
                    Limited = note.Limited,
                    MildlyImpaired = note.MildlyImpaired,
                    MinimalProgress = note.MinimalProgress,
                    ModerateProgress = note.ModerateProgress,
                    Motivated = note.Motivated,
                    Negativistic = note.Negativistic,
                    NoProgress = note.NoProgress,
                    Normal = note.Normal,
                    NotToPerson = note.NotToPerson,
                    NotToPlace = note.NotToPlace,
                    NotToTime = note.NotToTime,
                    Optimistic = note.Optimistic,
                    Oriented = note.Oriented,
                    OtherExplain = note.OtherExplain,
                    Providing = note.Providing,
                    Received = note.Received,
                    Regression = note.Regression,
                    SevereryImpaired = note.SevereryImpaired,
                    Sharing = note.Sharing,
                    Short = note.Short,
                    SignificantProgress = note.SignificantProgress,
                    UnableToDetermine = note.UnableToDetermine,
                    Unmotivated = note.Unmotivated,
                    Workday_Client_FK = workday_Client.Id,



                    Topic1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    IdActivity1 = (activities.Count > 0) ? activities[0].Activity.Id : 0,
                    Activity1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,
                    AnswerClient1 = note_Activity[0].AnswerClient,
                    IdGoal1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Goal.Id : 0,
                    Goals1 = goals,
                    IdObjetive1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Id : 0,
                    //Paso el IdGoal1 como parametro
                    Objetives1 = _combosHelper.GetComboObjetives(((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Goal.Id : 0),
                    Intervention1 = ((note_Activity.Count > 0) && (note_Activity[0].Objetive != null)) ? note_Activity[0].Objetive.Intervention : string.Empty,

                };
            }
            return View(noteViewModel);
        }

        //Group Notes
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> EditGroupNote3(GroupNote3ViewModel model, IFormCollection form) 
        {                    
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.MTPs)

                                                                           .Include(wc => wc.Facilitator)
                                                                           .Include(wc => wc.Schedule)

                                                                           .FirstOrDefaultAsync(wc => wc.Id == model.Id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (ModelState.IsValid)
            {
                GroupNote2Entity note = await _context.GroupNotes2

                                                      .Include(n => n.Workday_Cient)
                                                      .ThenInclude(wc => wc.Messages)

                                                      .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);

                GroupNote2_Activity note_Activity;
                GroupNote2Entity groupNoteEntity;
                if (note == null)   //la nota no está creada
                {
                    //Verify the client is not present in other services of notes at the same time
                    if (this.VerifyNotesAtSameTime(workday_Client.Client.Id, workday_Client.Session, workday_Client.Workday.Date, workday_Client.Workday.Date, workday_Client.Workday.Date, workday_Client.Id))
                    {
                        return RedirectToAction(nameof(EditGroupNote), new { id = model.Id, error = 5, origin = model.Origin });
                    }

                    groupNoteEntity = await _converterHelper.ToGroupNote3Entity(model, true);

                    //Update plan progress
                    /*groupNoteEntity.SignificantProgress = (form["Progress"] == "SignificantProgress") ? true : false;
                    groupNoteEntity.ModerateProgress = (form["Progress"] == "ModerateProgress") ? true : false;
                    groupNoteEntity.MinimalProgress = (form["Progress"] == "MinimalProgress") ? true : false;
                    groupNoteEntity.NoProgress = (form["Progress"] == "NoProgress") ? true : false;
                    groupNoteEntity.Regression = (form["Progress"] == "Regression") ? true : false;
                    groupNoteEntity.Descompensating = (form["Progress"] == "Decompensating") ? true : false;
                    groupNoteEntity.UnableToDetermine = (form["Progress"] == "Unabled") ? true : false;*/

                    //vinculo el mtp activo del cliente a la nota que se creará
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
                    if (mtp != null)
                        groupNoteEntity.MTPId = mtp.Id;

                    _context.Add(groupNoteEntity);

                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == workday_Client.Schedule.Id).OrderBy(n => n.InitialTime).ToListAsync();

                    note_Activity = new GroupNote2_Activity
                    {
                        GroupNote2 = groupNoteEntity,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1),
                        AnswerClient = model.AnswerClient1.Trim(),
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1),
                        SubSchedule = subSchedules.ElementAtOrDefault(0)
                    };
                    _context.Add(note_Activity);
                   
                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(GroupNotes));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedGroupNotes));
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
                else    //la nota está creada y sólo se debe actualizar
                {
                    //I verify that the note has a selected goal
                    if (note.Status == NoteStatus.Pending)
                    {
                        if (model.IdObjetive1 == 0 )
                        {
                            return RedirectToAction(nameof(EditGroupNote3), new { id = model.Id, error = 1, origin = model.Origin });
                        }
                    }

                    note.Adequated = model.Adequated;
                    note.Assigned = model.Assigned;
                    note.AssignedTopicOf = model.AssignedTopicOf;
                    note.Congruent = model.Congruent;
                    note.Descompensating = model.Descompensating;
                    note.Developing = model.Developing;
                    note.Dramatic = model.Dramatic;
                    note.Euphoric = model.Euphoric;
                    note.Expressing = model.Expressing;
                    note.Facilitated = model.Facilitated;
                    note.Fair = model.Fair;
                    note.FairAttitude = model.FairAttitude;
                    note.Faulty = model.Faulty;
                    note.Getting = model.Getting;
                    note.GroupLeaderFacilitator = model.GroupLeaderFacilitator;
                    note.GroupLeaderFacilitatorAbout = model.GroupLeaderFacilitatorAbout;
                    note.GroupLeaderProviderPsychoeducation = model.GroupLeaderProviderPsychoeducation;
                    note.GroupLeaderProviderSupport = model.GroupLeaderProviderSupport;
                    note.Inadequated = model.Inadequated;
                    note.InsightAdequate = model.InsightAdequate;
                    note.Involved = model.Involved;
                    note.Kept = model.Kept;
                    note.LearningAbout = model.LearningAbout;
                    note.LearningFrom = model.LearningFrom;
                    note.Limited = model.Limited;
                    note.MildlyImpaired = model.MildlyImpaired;
                    note.MinimalProgress = model.MinimalProgress;
                    note.ModerateProgress = model.ModerateProgress;
                    note.Motivated = model.Motivated;
                    note.Negativistic = model.Negativistic;
                    note.NoProgress = model.NoProgress;
                    note.Normal = model.Normal;
                    note.NotToPerson = model.NotToPerson;
                    note.NotToPlace = model.NotToPlace;
                    note.NotToTime = model.NotToTime;
                    note.Optimistic = model.Optimistic;
                    note.Oriented = model.Oriented;
                    note.OtherExplain = model.OtherExplain;
                    note.Providing = model.Providing;
                    note.Received = model.Received;
                    note.Regression = model.Regression;
                    note.SevereryImpaired = model.SevereryImpaired;
                    note.Sharing = model.Sharing;
                    note.Short = model.Short;
                    note.SignificantProgress = model.SignificantProgress;
                    note.UnableToDetermine = model.UnableToDetermine;
                    note.Unmotivated = model.Unmotivated;
                    note.MTPId = model.MTPId;
                    note.Other = model.Other;
                    note.Impaired = model.Impaired;
                    note.Euthymic = model.Euthymic;
                    note.Depressed = model.Depressed;
                    note.Anxious = model.Anxious;
                    note.Irritable = model.Irritable;
                    note.Guarded = model.Guarded;
                    note.Withdrawn = model.Withdrawn;
                    note.Hostile = model.Hostile;
                    note.Schema = model.Schema;

                    //actualizo el mtp activo del cliente a la nota que se creará                   
                    MTPEntity mtp = await _context.MTPs.FirstOrDefaultAsync(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
                    if (mtp != null)
                        note.MTPId = mtp.Id;

                    _context.Update(note);

                    List<GroupNote2_Activity> noteActivities_list = await _context.GroupNotes2_Activities
                                                                                 .Where(na => na.GroupNote2.Id == note.Id)
                                                                                 .ToListAsync();

                    _context.RemoveRange(noteActivities_list);

                    List<SubScheduleEntity> subSchedules = await _context.SubSchedule.Where(n => n.Schedule.Id == workday_Client.Schedule.Id).OrderBy(n => n.InitialTime).ToListAsync();

                    note_Activity = new GroupNote2_Activity
                    {
                        GroupNote2 = note,
                        Activity = _context.Activities.FirstOrDefault(a => a.Id == model.IdActivity1),
                        AnswerClient = model.AnswerClient1.Trim(),
                        Objetive = _context.Objetives.FirstOrDefault(o => o.Id == model.IdObjetive1),
                        SubSchedule = subSchedules.ElementAtOrDefault(0)
                    };
                    _context.Add(note_Activity);
                    await _context.SaveChangesAsync();

                    List<MessageEntity> messages = note.Workday_Cient.Messages.Where(m => (m.Status == MessageStatus.NotRead && m.Notification == false)).ToList();
                    //todos los mensajes no leidos que tiene el Workday_Client de la nota los pongo como leidos
                    foreach (MessageEntity value in messages)
                    {
                        value.Status = MessageStatus.Read;
                        value.DateRead = DateTime.Now;
                        _context.Update(value);

                        //I generate a notification to supervisor
                        MessageEntity notification = new MessageEntity
                        {
                            Workday_Client = workday_Client,
                            FarsForm = null,
                            MTPReview = null,
                            Addendum = null,
                            Discharge = null,
                            Title = "Update on reviewed group note",
                            Text = $"The group note of {workday_Client.ClientName} on {workday_Client.Workday.Date.ToShortDateString()} was rectified",
                            From = value.To,
                            To = value.From,
                            DateCreated = DateTime.Now,
                            Status = MessageStatus.NotRead,
                            Notification = true
                        };
                        _context.Add(notification);
                    }

                    if (workday_Client.CodeBill != model.CodeBill)
                    {
                        workday_Client.CodeBill = model.CodeBill;
                        _context.Update(workday_Client);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (model.Origin == 0)
                            return RedirectToAction(nameof(GroupNotes));
                        if (model.Origin == 1)
                            return RedirectToAction(nameof(NotStartedGroupNotes));
                        if (model.Origin == 2)
                            return RedirectToAction(nameof(GroupNotesInEdit));
                        if (model.Origin == 3)
                            return RedirectToAction(nameof(PendingGroupNotes));
                        if (model.Origin == 4)
                            return RedirectToAction(nameof(GroupNotesWithReview));
                        if (model.Origin == 5)
                            return RedirectToAction("MessagesOfNotes", "Messages");
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Already exists the element");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }
                }
            }

            return View(model);
        }


        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> FinishEditing(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients
                                                          .Include(wc => wc.Client)
                                                          .FirstOrDefaultAsync(wc => wc.Id == id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            NoteEntity note = await _context.Notes.Include(n => n.Workday_Cient)
                                                  .ThenInclude(wc => wc.Facilitator)
                                                  .ThenInclude(f => f.Clinic)
                                                  .Include(n => n.Notes_Activities)
                                                  .ThenInclude(na => na.Objetive)
                                                  .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            bool exist = false;
            bool complete = true;
            string gender_problems = string.Empty;
            int index = 1;
            foreach (Note_Activity item in note.Notes_Activities)
            {
                if (item.Objetive != null)
                    exist = true;
                if (string.IsNullOrEmpty(item.AnswerClient) || string.IsNullOrEmpty(item.AnswerFacilitator))
                    complete = false;
                if(!string.IsNullOrEmpty(item.AnswerClient))
                { 
                  if(this.GenderEvaluation(workday_Client.Client.Gender, item.AnswerClient))
                    gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Client Answer #{index}" : $"{gender_problems}, Client Answer #{index}";
                }
                if (!string.IsNullOrEmpty(item.AnswerFacilitator))
                {
                  if(this.GenderEvaluation(workday_Client.Client.Gender, item.AnswerFacilitator))
                    gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Facilitator Answer #{index}" : $"{gender_problems}, Facilitator Answer #{index}";
                }                
                index ++;
            }

            if (this.GenderEvaluation(workday_Client.Client.Gender, note.PlanNote))
                gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Plan" : $"{gender_problems}, Plan";

            if (!exist)     //la nota no tiene goal relacionado
            {
                if(origin == 0)
                    return RedirectToAction(nameof(EditNote), new {id =  id, error = 1, origin = 0});                
                if (origin == 2)
                    return RedirectToAction(nameof(EditNote), new {id = id, error = 1, origin = 2});
            }

            if (note.Schema == Common.Enums.SchemaType.Schema2) //se debe validar que las respuestas a las 4 activiades esten completas
            {
                if (!complete)     //la nota no esta completa
                {
                    if (origin == 0)
                        return RedirectToAction(nameof(EditNote), new { id = id, error = 2, origin = 0 });
                    if (origin == 2)
                        return RedirectToAction(nameof(EditNote), new { id = id, error = 2, origin = 2 });
                }
            }

            if (!string.IsNullOrEmpty(gender_problems))     //la nota tiene problemas con el genero
            {
                if (origin == 0)
                    return RedirectToAction(nameof(EditNote), new { id = id, error = 4, origin = 0, errorText = gender_problems});
                if (origin == 2)
                    return RedirectToAction(nameof(EditNote), new { id = id, error = 4, origin = 2, errorText = gender_problems});
            }
            
            note.Status = NoteStatus.Pending;
            _context.Update(note);

            await _context.SaveChangesAsync();
            if (origin == 2)
                return RedirectToAction(nameof(NotesInEdit), new { id = 1 });
            else
                return RedirectToAction(nameof(Index), new { id = 1});                        
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> FinishEditingP(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients
                                                          .Include(wc => wc.Client)
                                                          .FirstOrDefaultAsync(wc => wc.Id == id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            NotePEntity note = await _context.NotesP
                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(wc => wc.Facilitator)
                                             .ThenInclude(f => f.Clinic)
                                             
                                             .Include(n => n.NotesP_Activities)
                                             .ThenInclude(na => na.Objetive)

                                             .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            string gender_problems = string.Empty;
            if (this.GenderEvaluation(workday_Client.Client.Gender, note.PlanNote))
                gender_problems = $"Client benefited from...";

            bool exist = false;            
            foreach (NoteP_Activity item in note.NotesP_Activities)
            {
                if (item.Objetive != null)
                    exist = true;                
            }

            if (!exist)     //la nota no tiene goal relacionado
            {
                if (origin == 0)
                    return RedirectToAction(nameof(EditNoteP), new { id = id, error = 1, origin = 0 });
                if (origin == 2)
                    return RedirectToAction(nameof(EditNoteP), new { id = id, error = 1, origin = 2 });
            }

            if (!string.IsNullOrEmpty(gender_problems))     //la nota tiene problemas con el genero
            {
                if (origin == 0)
                    return RedirectToAction(nameof(EditNoteP), new { id = id, error = 4, origin = 0, errorText = gender_problems });
                if (origin == 2)
                    return RedirectToAction(nameof(EditNoteP), new { id = id, error = 4, origin = 2, errorText = gender_problems });
            }

            note.Status = NoteStatus.Pending;
            _context.Update(note);

            await _context.SaveChangesAsync();
            if (origin == 2)
                return RedirectToAction(nameof(NotesInEdit), new { id = 1 });
            else
                return RedirectToAction(nameof(Index), new { id = 1 });
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> FinishEditingIN(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients
                                                          .Include(wc => wc.Client)
                                                          .FirstOrDefaultAsync(wc => wc.Id == id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IndividualNoteEntity note = await _context.IndividualNotes

                                                      .Include(n => n.Objective)

                                                      .Include(n => n.Workday_Cient)
                                                      .ThenInclude(wc => wc.Facilitator)
                                                      .ThenInclude(f => f.Clinic)
                                                      
                                                      .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            string gender_problems = string.Empty;
                      
            if (note.Objective == null) //la nota no tiene goal relacionado
            { 
                if (origin == 0)
                    return RedirectToAction(nameof(EditIndNote), new { id = id, error = 1, origin = 0 });
                if (origin == 2)
                    return RedirectToAction(nameof(EditIndNote), new { id = id, error = 1, origin = 2 });
            }                        
            
            if (this.GenderEvaluation(workday_Client.Client.Gender, note.SubjectiveData))
                gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Subjective Data" : $"{gender_problems}, Subjective Data";
            if (this.GenderEvaluation(workday_Client.Client.Gender, note.ObjectiveData))
                gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Objective Data" : $"{gender_problems}, Objective Data";
            if (this.GenderEvaluation(workday_Client.Client.Gender, note.Assessment))
                gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Assessment" : $"{gender_problems}, Assessment";
            if (this.GenderEvaluation(workday_Client.Client.Gender, note.PlanNote))
                gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Plan" : $"{gender_problems}, Plan";
            
            if (!string.IsNullOrEmpty(gender_problems))     //la nota tiene problemas con el genero
            {
                if (origin == 0)
                    return RedirectToAction(nameof(EditIndNote), new { id = id, error = 4, origin = 0, errorText = gender_problems });
                if (origin == 2)
                    return RedirectToAction(nameof(EditIndNote), new { id = id, error = 4, origin = 2, errorText = gender_problems });
            }

            note.Status = NoteStatus.Pending;
            _context.Update(note);

            await _context.SaveChangesAsync();
            if (origin == 2)
                return RedirectToAction(nameof(IndNotesInEdit), new { id = 1 });
            else
                return RedirectToAction(nameof(IndividualNotes), new { id = 1 });
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> FinishEditingGroup(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients
                                                          .Include(wc => wc.Client)
                                                          .FirstOrDefaultAsync(wc => wc.Id == id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            GroupNoteEntity note = await _context.GroupNotes

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(wc => wc.Facilitator)
                                                 .ThenInclude(f => f.Clinic)

                                                  .Include(n => n.GroupNotes_Activities)
                                                  .ThenInclude(na => na.Objetive)

                                                  .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            if (note == null) 
            {
                return RedirectToAction(nameof(FinishEditingGroup2), new { id = id, origin = origin });
            }

            bool exist = false;
            bool complete = true;
            string gender_problems = string.Empty;
            int index = 1;
            foreach (GroupNote_Activity item in note.GroupNotes_Activities)
            {
                if (item.Objetive != null)
                    exist = true;
                if (string.IsNullOrEmpty(item.AnswerClient) || string.IsNullOrEmpty(item.AnswerFacilitator))
                    complete = false;
                if (!string.IsNullOrEmpty(item.AnswerClient))
                {
                    if (this.GenderEvaluation(workday_Client.Client.Gender, item.AnswerClient))
                        gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Client Answer #{index}" : $"{gender_problems}, Client Answer #{index}";
                }
                if (!string.IsNullOrEmpty(item.AnswerFacilitator))
                {
                    if (this.GenderEvaluation(workday_Client.Client.Gender, item.AnswerFacilitator))
                        gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Facilitator Answer #{index}" : $"{gender_problems}, Facilitator Answer #{index}";
                }
                index++;
            }

            if (this.GenderEvaluation(workday_Client.Client.Gender, note.PlanNote))
                gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Plan" : $"{gender_problems}, Plan";

            if (!exist)     //la nota no tiene goal relacionado
            {
                if (origin == 0)
                    return RedirectToAction(nameof(EditGroupNote), new { id = id, error = 1, origin = 0 });
                if (origin == 2)
                    return RedirectToAction(nameof(EditGroupNote), new { id = id, error = 1, origin = 2 });
            }            

            if (!string.IsNullOrEmpty(gender_problems))     //la nota tiene problemas con el genero
            {
                if (origin == 0)
                    return RedirectToAction(nameof(EditGroupNote), new { id = id, error = 4, origin = 0, errorText = gender_problems });
                if (origin == 2)
                    return RedirectToAction(nameof(EditGroupNote), new { id = id, error = 4, origin = 2, errorText = gender_problems });
            }

            note.Status = NoteStatus.Pending;
            _context.Update(note);

            await _context.SaveChangesAsync();
            if (origin == 2)
                return RedirectToAction(nameof(GroupNotesInEdit), new { id = 1 });
            else
                return RedirectToAction(nameof(GroupNotes), new { id = 1 });
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> FinishEditingGroup2(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients
                                                          .Include(wc => wc.Client)
                                                          .FirstOrDefaultAsync(wc => wc.Id == id);
            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            GroupNote2Entity note = await _context.GroupNotes2

                                                  .Include(n => n.Workday_Cient)
                                                  .ThenInclude(wc => wc.Facilitator)
                                                  .ThenInclude(f => f.Clinic)

                                                  .Include(n => n.GroupNotes2_Activities)
                                                  .ThenInclude(na => na.Objetive)

                                                  .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            bool exist = false;
            bool complete = true;
            string gender_problems = string.Empty;
            int index = 1;
            foreach (GroupNote2_Activity item in note.GroupNotes2_Activities)
            {
                if (item.Objetive != null)
                    exist = true;
                if (string.IsNullOrEmpty(item.AnswerClient) || string.IsNullOrEmpty(item.AnswerFacilitator))
                    complete = false;
                if (!string.IsNullOrEmpty(item.AnswerClient))
                {
                    if (this.GenderEvaluation(workday_Client.Client.Gender, item.AnswerClient))
                        gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Client Answer #{index}" : $"{gender_problems}, Client Answer #{index}";
                }
               /* if (!string.IsNullOrEmpty(item.AnswerFacilitator))
                {
                    if (this.GenderEvaluation(workday_Client.Client.Gender, item.AnswerFacilitator))
                        gender_problems = string.IsNullOrEmpty(gender_problems) ? $"Facilitator Answer #{index}" : $"{gender_problems}, Facilitator Answer #{index}";
                }*/
                index++;
            }

           
            if (!exist)     //la nota no tiene goal relacionado
            {
                if (origin == 0)
                    return RedirectToAction(nameof(EditGroupNote), new { id = id, error = 1, origin = 0 });
                if (origin == 2)
                    return RedirectToAction(nameof(EditGroupNote), new { id = id, error = 1, origin = 2 });
            }

            if (!string.IsNullOrEmpty(gender_problems))     //la nota tiene problemas con el genero
            {
                if (origin == 0)
                    return RedirectToAction(nameof(EditGroupNote), new { id = id, error = 4, origin = 0, errorText = gender_problems });
                if (origin == 2)
                    return RedirectToAction(nameof(EditGroupNote), new { id = id, error = 4, origin = 2, errorText = gender_problems });
            }

            note.Status = NoteStatus.Pending;
            _context.Update(note);

            await _context.SaveChangesAsync();
            if (origin == 2)
                return RedirectToAction(nameof(GroupNotesInEdit), new { id = 1 });
            else
                return RedirectToAction(nameof(GroupNotes), new { id = 1 });
        }

        [Authorize(Roles = "Facilitator")]
        public JsonResult GetActivityList(int idTheme)
        {
            List<ActivityEntity> activities = _context.Activities.Where(a => a.Theme.Id == idTheme).ToList();

            return Json(new SelectList(activities, "Id", "Name"));
        }

        [Authorize(Roles = "Facilitator")]
        public JsonResult GetObjetiveList(int idGoal)
        {
            List<ObjetiveEntity> objetives = _context.Objetives.Where(o => (o.Goal.Id == idGoal && o.Compliment == false)).ToList();
            if (objetives.Count == 0)
            {
                objetives.Insert(0, new ObjetiveEntity
                {
                    Objetive = "[First select goal...]",
                    Id = 0
                });
            }
            return Json(new SelectList(objetives, "Id", "Objetive"));
        }

        [Authorize(Roles = "Facilitator")]
        public JsonResult GetIntervention(int idObjetive)
        {
            ObjetiveEntity objetive = _context.Objetives.FirstOrDefault(o => o.Id == idObjetive);
            string text = "Select goal and objective";
            if (objetive != null)
            {
                text = objetive.Intervention;
            }
            return Json(text);
        }

        [Authorize(Roles = "Facilitator")]
        public JsonResult GetGoalsList(int idClient)
        {
            MTPEntity mtp = _context.MTPs
                                    .FirstOrDefault(m => (m.Client.Id == idClient && m.Active == true));
            
            List<SelectListItem> list = new List<SelectListItem>();

            if (mtp != null)
            {
                list = _context.Goals.Where(g => (g.MTP.Id == mtp.Id && g.Service == ServiceType.PSR && g.Compliment == false)).Select(g => new SelectListItem
                {
                    Text = $"{g.Number}",
                    Value = $"{g.Id}"
                }).ToList();
                list.Insert(0, new SelectListItem
                {
                    Text = "[Select goal...]",
                    Value = "0"
                });
            }
            else
            {
                list.Add(new SelectListItem
                {
                    Text = "[Select goal...]",
                    Value = "0"
                });
            }         

            return Json(new SelectList(list, "Value", "Text"));
        }

        [Authorize(Roles = "Facilitator")]
        public JsonResult GetGoalsIndList(int idClient)
        {
            MTPEntity mtp = _context.MTPs
                                    .FirstOrDefault(m => (m.Client.Id == idClient && m.Active == true));

            List<SelectListItem> list = new List<SelectListItem>();

            if (mtp != null)
            {
                UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

                //Evaluate setting for goals's classification
                SettingEntity setting =  _context.Settings
                                                 .FirstOrDefault(s => s.Clinic.Id == user_logged.Clinic.Id);
                if (setting != null)
                {
                    if (setting.MHClassificationOfGoals)
                    {
                        list = _context.Goals.Where(g => (g.MTP.Id == mtp.Id && g.Service == ServiceType.Individual && g.Compliment == false)).Select(g => new SelectListItem
                        {
                            Text = $"{g.Number}",
                            Value = $"{g.Id}"
                        }).ToList();
                    }
                    else
                    {
                        list = _context.Goals.Where(g => (g.MTP.Id == mtp.Id && g.Compliment == false)).Select(g => new SelectListItem
                        {
                            Text = $"{g.Number}",
                            Value = $"{g.Id}"
                        }).ToList();
                    }
                }
                
                list.Insert(0, new SelectListItem
                {
                    Text = "[Select goal...]",
                    Value = "0"
                });
            }
            else
            {
                list.Add(new SelectListItem
                {
                    Text = "[Select goal...]",
                    Value = "0"
                });
            }

            return Json(new SelectList(list, "Value", "Text"));
        }

        [Authorize(Roles = "Facilitator")]
        public JsonResult GetGoalsGroupList(int idClient)
        {
            MTPEntity mtp = _context.MTPs
                                    .FirstOrDefault(m => (m.Client.Id == idClient && m.Active == true));

            List<SelectListItem> list = new List<SelectListItem>();

            if (mtp != null)
            {
                list = _context.Goals.Where(g => (g.MTP.Id == mtp.Id && g.Service == ServiceType.Group && g.Compliment == false)).Select(g => new SelectListItem
                {
                    Text = $"{g.Number}",
                    Value = $"{g.Id}"
                }).ToList();
                list.Insert(0, new SelectListItem
                {
                    Text = "[Select goal...]",
                    Value = "0"
                });
            }
            else
            {
                list.Add(new SelectListItem
                {
                    Text = "[Select goal...]",
                    Value = "0"
                });
            }

            return Json(new SelectList(list, "Value", "Text"));
        }

        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> NotesSupervision()
        {
            UserEntity user_logged = await _context.Users

                                                   .Include(u => u.Clinic)

                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            List<WeekEntity> weeks = (await _context.Weeks
                                                    
                                                    .Include(w => w.Days)
                                                    .ThenInclude(d => d.Workdays_Clients)
                                                    .ThenInclude(wc => wc.Client)
                                                    .ThenInclude(c => c.Group)

                                                    .Include(w => w.Days)
                                                    .ThenInclude(d => d.Workdays_Clients)
                                                    .ThenInclude(g => g.Facilitator)

                                                    .Include(w => w.Days)
                                                    .ThenInclude(d => d.Workdays_Clients)
                                                    .ThenInclude(wc => wc.Note)

                                                    .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                              && w.Days.Where(d => (d.Service == ServiceType.PSR && d.Workdays_Clients.Where(wc => wc.Note.Status == NoteStatus.Pending).Count() > 0)).Count() > 0))
                                                    .ToListAsync());
            
            return View(weeks);
        }

        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> IndNotesSupervision()
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            List<WeekEntity> weeks = (await _context.Weeks

                                                    .Include(w => w.Days)
                                                    .ThenInclude(d => d.Workdays_Clients)
                                                    .ThenInclude(wc => wc.Client)                                                          

                                                    .Include(w => w.Days)
                                                    .ThenInclude(d => d.Workdays_Clients)
                                                    .ThenInclude(g => g.Facilitator)

                                                    .Include(w => w.Days)
                                                    .ThenInclude(d => d.Workdays_Clients)
                                                    .ThenInclude(wc => wc.IndividualNote)
                                                    
                                                    .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                              && w.Days.Where(d => (d.Service == ServiceType.Individual && d.Workdays_Clients.Where(wc => wc.IndividualNote.Status == NoteStatus.Pending).Count() > 0)).Count() > 0))
                                                    .ToListAsync());

            return View(weeks);
        }

        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GroupNotesSupervision()
        {
            UserEntity user_logged = await _context.Users

                                                    .Include(u => u.Clinic)

                                                    .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            List<WeekEntity> weeks = (await _context.Weeks

                                                    .Include(w => w.Days)
                                                    .ThenInclude(d => d.Workdays_Clients)
                                                    .ThenInclude(wc => wc.Client)
                                                    .ThenInclude(c => c.Group)

                                                    .Include(w => w.Days)
                                                    .ThenInclude(d => d.Workdays_Clients)
                                                    .ThenInclude(g => g.Facilitator)

                                                    .Include(w => w.Days)
                                                    .ThenInclude(d => d.Workdays_Clients)
                                                    .ThenInclude(wc => wc.GroupNote)

                                                    .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                              && w.Days.Where(d => (d.Service == ServiceType.Group && d.Workdays_Clients.Where(wc => wc.GroupNote.Status == NoteStatus.Pending).Count() > 0)).Count() > 0))
                                                    .ToListAsync());

            return View(weeks);
        }

        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveNote(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Facilitator)

                                                                           .Include(wc => wc.NoteP)
                                                                           
                                                                           .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            //redirect to ApproveNoteP due to the note is schema 3
            if (workday_Client.NoteP != null)
            {
                return RedirectToAction(nameof(ApproveNoteP), new { id = workday_Client.NoteP.Id, origin = origin });
            }

            NoteEntity note = await _context.Notes.Include(n => n.Workday_Cient)
                                                  .ThenInclude(wc => wc.Client)
                                                  .ThenInclude(c => c.Group)
                                                  .ThenInclude(g => g.Facilitator)

                                                  .Include(n => n.Notes_Activities)
                                                  .ThenInclude(na => na.Activity)

                                                  .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            NoteViewModel noteViewModel = null;
                        
            List<Note_Activity> note_Activity = await _context.Notes_Activities
                                                                .Include(na => na.Activity)
                                                                .ThenInclude(a => a.Theme)

                                                                .Include(n => n.Objetive)
                                                                .ThenInclude(o => o.Goal)

                                                                .Where(na => na.Note.Id == note.Id).ToListAsync();
            
            if ((note.Schema == Common.Enums.SchemaType.Schema1) || (note.Schema == Common.Enums.SchemaType.Schema2))
            {
                noteViewModel = new NoteViewModel
                {
                    Id = id,
                    Workday_Cient = workday_Client,
                    PlanNote = note.PlanNote,
                    Origin = origin,
                    Schema = note.Schema,
                    CodeBill = workday_Client.CodeBill,

                    OrientedX3 = note.OrientedX3,
                    NotTime = note.NotTime,
                    NotPlace = note.NotPlace,
                    NotPerson = note.NotPerson,
                    Present = note.Present,
                    Adequate = note.Adequate,
                    Limited = note.Limited,
                    Impaired = note.Impaired,
                    Faulty = note.Faulty,
                    Euthymic = note.Euthymic,
                    Congruent = note.Congruent,
                    Negativistic = note.Negativistic,
                    Depressed = note.Depressed,
                    Euphoric = note.Euphoric,
                    Optimistic = note.Optimistic,
                    Anxious = note.Anxious,
                    Hostile = note.Hostile,
                    Withdrawn = note.Withdrawn,
                    Irritable = note.Irritable,
                    Dramatized = note.Dramatized,
                    AdequateAC = note.AdequateAC,
                    Inadequate = note.Inadequate,
                    Fair = note.Fair,
                    Unmotivated = note.Unmotivated,
                    Motivated = note.Motivated,
                    Guarded = note.Guarded,
                    Normal = note.Normal,
                    ShortSpanned = note.ShortSpanned,
                    MildlyImpaired = note.MildlyImpaired,
                    SeverelyImpaired = note.SeverelyImpaired,

                    Topic1 = note_Activity[0].Activity.Theme.Name,
                    Activity1 = note_Activity[0].Activity.Name,
                    AnswerClient1 = note_Activity[0].AnswerClient,
                    AnswerFacilitator1 = note_Activity[0].AnswerFacilitator,
                    Goal1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Objetive : string.Empty,

                    Topic2 = note_Activity[1].Activity.Theme.Name,
                    Activity2 = note_Activity[1].Activity.Name,
                    AnswerClient2 = note_Activity[1].AnswerClient,
                    AnswerFacilitator2 = note_Activity[1].AnswerFacilitator,
                    Goal2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Objetive : string.Empty,

                    Topic3 = note_Activity[2].Activity.Theme.Name,
                    Activity3 = note_Activity[2].Activity.Name,
                    AnswerClient3 = note_Activity[2].AnswerClient,
                    AnswerFacilitator3 = note_Activity[2].AnswerFacilitator,
                    Goal3 = (note_Activity[2].Objetive != null) ? note_Activity[2].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive3 = (note_Activity[2].Objetive != null) ? note_Activity[2].Objetive.Objetive : string.Empty,

                    Topic4 = note_Activity[3].Activity.Theme.Name,
                    Activity4 = note_Activity[3].Activity.Name,
                    AnswerClient4 = note_Activity[3].AnswerClient,
                    AnswerFacilitator4 = note_Activity[3].AnswerFacilitator,
                    Goal4 = (note_Activity[3].Objetive != null) ? note_Activity[3].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive4 = (note_Activity[3].Objetive != null) ? note_Activity[3].Objetive.Objetive : string.Empty,
                };
            }
            if (note.Schema == Common.Enums.SchemaType.Schema4)
            {
                noteViewModel = new NoteViewModel
                {
                    Id = id,
                    Workday_Cient = workday_Client,
                    PlanNote = note.PlanNote,
                    Origin = origin,
                    Schema = note.Schema,
                    CodeBill = workday_Client.CodeBill,

                    OrientedX3 = note.OrientedX3,
                    NotTime = note.NotTime,
                    NotPlace = note.NotPlace,
                    NotPerson = note.NotPerson,
                    Present = note.Present,
                    Adequate = note.Adequate,
                    Limited = note.Limited,
                    Impaired = note.Impaired,
                    Faulty = note.Faulty,
                    Euthymic = note.Euthymic,
                    Congruent = note.Congruent,
                    Negativistic = note.Negativistic,
                    Depressed = note.Depressed,
                    Euphoric = note.Euphoric,
                    Optimistic = note.Optimistic,
                    Anxious = note.Anxious,
                    Hostile = note.Hostile,
                    Withdrawn = note.Withdrawn,
                    Irritable = note.Irritable,
                    Dramatized = note.Dramatized,
                    AdequateAC = note.AdequateAC,
                    Inadequate = note.Inadequate,
                    Fair = note.Fair,
                    Unmotivated = note.Unmotivated,
                    Motivated = note.Motivated,
                    Guarded = note.Guarded,
                    Normal = note.Normal,
                    ShortSpanned = note.ShortSpanned,
                    MildlyImpaired = note.MildlyImpaired,
                    SeverelyImpaired = note.SeverelyImpaired,

                    Topic1 = note_Activity[0].Activity.Theme.Name,
                    Activity1 = note_Activity[0].Activity.Name,
                    AnswerClient1 = note_Activity[0].AnswerClient,
                    AnswerFacilitator1 = note_Activity[0].AnswerFacilitator,
                    Goal1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Objetive : string.Empty,

                    Topic2 = note_Activity[1].Activity.Theme.Name,
                    Activity2 = note_Activity[1].Activity.Name,
                    AnswerClient2 = note_Activity[1].AnswerClient,
                    AnswerFacilitator2 = note_Activity[1].AnswerFacilitator,
                    Goal2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Objetive : string.Empty,

                    Topic3 = note_Activity[2].Activity.Theme.Name,
                    Activity3 = note_Activity[2].Activity.Name,
                    AnswerClient3 = note_Activity[2].AnswerClient,
                    AnswerFacilitator3 = note_Activity[2].AnswerFacilitator,
                    Goal3 = (note_Activity[2].Objetive != null) ? note_Activity[2].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive3 = (note_Activity[2].Objetive != null) ? note_Activity[2].Objetive.Objetive : string.Empty                    
                };
            }            
            
            return View(noteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveNote(NoteViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Home/Error404");
            }

            NoteEntity note = await _context.Notes
                                            .Include(n => n.Notes_Activities)
                                            .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);

            
            note.Status = NoteStatus.Approved;
            note.DateOfApprove = DateTime.Now;
            note.Supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.LinkedUser == User.Identity.Name);
            _context.Update(note);

            await _context.SaveChangesAsync();           
                
            if (model.Origin == 3)  ///viene de la pagina PendingNotes
                return RedirectToAction(nameof(PendingNotes));
            if (model.Origin == 4)  ///viene de la pagina NotesWithReview
                return RedirectToAction(nameof(NotesWithReview));
            if (model.Origin == 5)  ///viene de la pagina Notifications
                return RedirectToAction("Notifications", "Messages");

            return RedirectToAction(nameof(NotesSupervision));
        }

        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveNoteP(int id, int origin = 0)
        {
            NotePEntity note = await _context.NotesP

                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(wc => wc.Client)
                                             .ThenInclude(c => c.Group)
                                             .ThenInclude(g => g.Facilitator)
                                             
                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(wc => wc.Facilitator)

                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(w => w.Workday)
                                             .ThenInclude(wd => wd.Workdays_Activities_Facilitators)
                                             .ThenInclude(waf => waf.Activity)
                                             .ThenInclude(a => a.Theme)

                                             .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                return RedirectToAction("Home/Error404");
            }

            bool am = false;
            bool pm = false;

            if (note.Workday_Cient.Session == "AM")
            {
                am = true;
            }
            else
            {
                pm = true;
            }

            NotePViewModel noteViewModel = null;

            List<NoteP_Activity> note_Activity = await _context.NotesP_Activities

                                                               .Include(na => na.Activity)
                                                               .ThenInclude(a => a.Theme)

                                                               .Include(na => na.Objetive)
                                                               .ThenInclude(o => o.Goal)

                                                               .Where(na => na.NoteP.Id == note.Id)
                                                               .ToListAsync();

            List<Workday_Activity_Facilitator> activities;
            if (am == true)
            {
                activities = note.Workday_Cient.Workday
                                           .Workdays_Activities_Facilitators
                                           .Where(waf => (waf.Facilitator == note.Workday_Cient.Facilitator
                                               && waf.AM == true))
                                           .ToList();
            }
            else
            {
                activities = note.Workday_Cient.Workday
                                           .Workdays_Activities_Facilitators
                                           .Where(waf => (waf.Facilitator == note.Workday_Cient.Facilitator
                                               && waf.PM == true))
                                           .ToList();
            }

            if (note.Schema == Common.Enums.SchemaType.Schema3)
            {
                noteViewModel = new NotePViewModel
                {
                    Id = id,
                    Workday_Cient = note.Workday_Cient,
                    PlanNote = note.PlanNote,
                    Origin = origin,
                    Schema = note.Schema,
                    Setting = note.Setting,
                    CodeBill = note.Workday_Cient.CodeBill,

                    Title = note.Title,

                    //mental client status
                    Attentive = note.Attentive,
                    Depressed = note.Depressed,
                    Inattentive = note.Inattentive,
                    Angry = note.Angry,
                    Sad = note.Sad,
                    FlatAffect = note.FlatAffect,
                    Anxious = note.Anxious,
                    PositiveEffect = note.PositiveEffect,
                    Oriented1x = note.Oriented1x,
                    Oriented2x = note.Oriented2x,
                    Oriented3x = note.Oriented3x,
                    Impulsive = note.Impulsive,
                    Labile = note.Labile,
                    Withdrawn = note.Withdrawn,
                    RelatesWell = note.RelatesWell,
                    DecreasedEyeContact = note.DecreasedEyeContact,
                    AppropiateEyeContact = note.AppropiateEyeContact,

                    //progress
                    Minimal = note.Minimal,
                    Slow = note.Slow,
                    Steady = note.Steady,
                    GoodExcelent = note.GoodExcelent,
                    IncreasedDifficultiesNoted = note.IncreasedDifficultiesNoted,
                    Complicated = note.Complicated,
                    DevelopingInsight = note.DevelopingInsight,
                    LittleInsight = note.LittleInsight,
                    Aware = note.Aware,
                    AbleToGenerateAlternatives = note.AbleToGenerateAlternatives,
                    Initiates = note.Initiates,
                    ProblemSolved = note.ProblemSolved,
                    DemostratesEmpathy = note.DemostratesEmpathy,
                    UsesSessions = note.UsesSessions,
                    Variable = note.Variable,

                    Present1 = note_Activity[0].Present,
                    Theme1 = (activities.Count > 0) ? activities[0].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention1 = (activities.Count > 0) ? activities[0].Activity.Name : string.Empty,                   
                    Goal1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Objetive : string.Empty,
                    
                    //Skill set addressed
                    activityDailyLiving1 = activities[0].activityDailyLiving == null ? false : Convert.ToBoolean(activities[0].activityDailyLiving),
                    communityResources1 = activities[0].communityResources == null ? false : Convert.ToBoolean(activities[0].communityResources),
                    copingSkills1 = activities[0].copingSkills == null ? false : Convert.ToBoolean(activities[0].copingSkills),
                    diseaseManagement1 = activities[0].diseaseManagement == null ? false : Convert.ToBoolean(activities[0].diseaseManagement),
                    healthyLiving1 = activities[0].healthyLiving == null ? false : Convert.ToBoolean(activities[0].healthyLiving),
                    lifeSkills1 = activities[0].lifeSkills == null ? false : Convert.ToBoolean(activities[0].lifeSkills),
                    relaxationTraining1 = activities[0].relaxationTraining == null ? false : Convert.ToBoolean(activities[0].relaxationTraining),
                    socialSkills1 = activities[0].socialSkills == null ? false : Convert.ToBoolean(activities[0].socialSkills),
                    stressManagement1 = activities[0].stressManagement == null ? false : Convert.ToBoolean(activities[0].stressManagement),

                    //Client's response
                    Cooperative1 = (note_Activity.Count > 0) ? note_Activity[0].Cooperative : false,
                    Assertive1 = (note_Activity.Count > 0) ? note_Activity[0].Assertive : false,
                    Passive1 = (note_Activity.Count > 0) ? note_Activity[0].Passive : false,
                    Variable1 = (note_Activity.Count > 0) ? note_Activity[0].Variable : false,
                    Uninterested1 = (note_Activity.Count > 0) ? note_Activity[0].Uninterested : false,
                    Engaged1 = (note_Activity.Count > 0) ? note_Activity[0].EngagedActive : false,
                    Distractible1 = (note_Activity.Count > 0) ? note_Activity[0].Distractible : false,
                    Confused1 = (note_Activity.Count > 0) ? note_Activity[0].Confused : false,
                    Aggresive1 = (note_Activity.Count > 0) ? note_Activity[0].Aggresive : false,
                    Resistant1 = (note_Activity.Count > 0) ? note_Activity[0].Resistant : false,
                    Other1 = (note_Activity.Count > 0) ? note_Activity[0].Other : false,

                    Present2 = note_Activity[1].Present,
                    Theme2 = (activities.Count > 1) ? activities[1].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention2 = (activities.Count > 1) ? activities[1].Activity.Name : string.Empty,
                    Goal2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Objetive : string.Empty,

                    //Skill set addressed
                    activityDailyLiving2 = activities[1].activityDailyLiving == null ? false : Convert.ToBoolean(activities[1].activityDailyLiving),
                    communityResources2 = activities[1].communityResources == null ? false : Convert.ToBoolean(activities[1].communityResources),
                    copingSkills2 = activities[1].copingSkills == null ? false : Convert.ToBoolean(activities[1].copingSkills),
                    diseaseManagement2 = activities[1].diseaseManagement == null ? false : Convert.ToBoolean(activities[1].diseaseManagement),
                    healthyLiving2 = activities[1].healthyLiving == null ? false : Convert.ToBoolean(activities[1].healthyLiving),
                    lifeSkills2 = activities[1].lifeSkills == null ? false : Convert.ToBoolean(activities[1].lifeSkills),
                    relaxationTraining2 = activities[1].relaxationTraining == null ? false : Convert.ToBoolean(activities[1].relaxationTraining),
                    socialSkills2 = activities[1].socialSkills == null ? false : Convert.ToBoolean(activities[1].socialSkills),
                    stressManagement2 = activities[1].stressManagement == null ? false : Convert.ToBoolean(activities[1].stressManagement),

                    //Client's response
                    Cooperative2 = (note_Activity.Count > 1) ? note_Activity[1].Cooperative : false,
                    Assertive2 = (note_Activity.Count > 1) ? note_Activity[1].Assertive : false,
                    Passive2 = (note_Activity.Count > 1) ? note_Activity[1].Passive : false,
                    Variable2 = (note_Activity.Count > 1) ? note_Activity[1].Variable : false,
                    Uninterested2 = (note_Activity.Count > 1) ? note_Activity[1].Uninterested : false,
                    Engaged2 = (note_Activity.Count > 1) ? note_Activity[1].EngagedActive : false,
                    Distractible2 = (note_Activity.Count > 1) ? note_Activity[1].Distractible : false,
                    Confused2 = (note_Activity.Count > 1) ? note_Activity[1].Confused : false,
                    Aggresive2 = (note_Activity.Count > 1) ? note_Activity[1].Aggresive : false,
                    Resistant2 = (note_Activity.Count > 1) ? note_Activity[1].Resistant : false,
                    Other2 = (note_Activity.Count > 1) ? note_Activity[1].Other : false,

                    Present3 = note_Activity[2].Present,
                    Theme3 = (activities.Count > 2) ? activities[2].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention3 = (activities.Count > 2) ? activities[2].Activity.Name : string.Empty,
                    Goal3 = (note_Activity[2].Objetive != null) ? note_Activity[2].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive3 = (note_Activity[2].Objetive != null) ? note_Activity[2].Objetive.Objetive : string.Empty,

                    //Skill set addressed
                    activityDailyLiving3 = activities[2].activityDailyLiving == null ? false : Convert.ToBoolean(activities[2].activityDailyLiving),
                    communityResources3 = activities[2].communityResources == null ? false : Convert.ToBoolean(activities[2].communityResources),
                    copingSkills3 = activities[2].copingSkills == null ? false : Convert.ToBoolean(activities[2].copingSkills),
                    diseaseManagement3 = activities[2].diseaseManagement == null ? false : Convert.ToBoolean(activities[2].diseaseManagement),
                    healthyLiving3 = activities[2].healthyLiving == null ? false : Convert.ToBoolean(activities[2].healthyLiving),
                    lifeSkills3 = activities[2].lifeSkills == null ? false : Convert.ToBoolean(activities[2].lifeSkills),
                    relaxationTraining3 = activities[2].relaxationTraining == null ? false : Convert.ToBoolean(activities[2].relaxationTraining),
                    socialSkills3 = activities[2].socialSkills == null ? false : Convert.ToBoolean(activities[2].socialSkills),
                    stressManagement3 = activities[2].stressManagement == null ? false : Convert.ToBoolean(activities[2].stressManagement),

                    //Client's response
                    Cooperative3 = (note_Activity.Count > 2) ? note_Activity[2].Cooperative : false,
                    Assertive3 = (note_Activity.Count > 2) ? note_Activity[2].Assertive : false,
                    Passive3 = (note_Activity.Count > 2) ? note_Activity[2].Passive : false,
                    Variable3 = (note_Activity.Count > 2) ? note_Activity[2].Variable : false,
                    Uninterested3 = (note_Activity.Count > 2) ? note_Activity[2].Uninterested : false,
                    Engaged3 = (note_Activity.Count > 2) ? note_Activity[2].EngagedActive : false,
                    Distractible3 = (note_Activity.Count > 2) ? note_Activity[2].Distractible : false,
                    Confused3 = (note_Activity.Count > 2) ? note_Activity[2].Confused : false,
                    Aggresive3 = (note_Activity.Count > 2) ? note_Activity[2].Aggresive : false,
                    Resistant3 = (note_Activity.Count > 2) ? note_Activity[2].Resistant : false,
                    Other3 = (note_Activity.Count > 2) ? note_Activity[2].Other : false,

                    Present4 = note_Activity[3].Present,
                    Theme4 = (activities.Count > 3) ? activities[3].Activity.Theme.Name : string.Empty,
                    FacilitatorIntervention4 = (activities.Count > 3) ? activities[3].Activity.Name : string.Empty,
                    Goal4 = (note_Activity[3].Objetive != null) ? note_Activity[3].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive4 = (note_Activity[3].Objetive != null) ? note_Activity[3].Objetive.Objetive : string.Empty,

                    //Skill set addressed
                    activityDailyLiving4 = activities[3].activityDailyLiving == null ? false : Convert.ToBoolean(activities[3].activityDailyLiving),
                    communityResources4 = activities[3].communityResources == null ? false : Convert.ToBoolean(activities[3].communityResources),
                    copingSkills4 = activities[3].copingSkills == null ? false : Convert.ToBoolean(activities[3].copingSkills),
                    diseaseManagement4 = activities[3].diseaseManagement == null ? false : Convert.ToBoolean(activities[3].diseaseManagement),
                    healthyLiving4 = activities[3].healthyLiving == null ? false : Convert.ToBoolean(activities[3].healthyLiving),
                    lifeSkills4 = activities[3].lifeSkills == null ? false : Convert.ToBoolean(activities[3].lifeSkills),
                    relaxationTraining4 = activities[3].relaxationTraining == null ? false : Convert.ToBoolean(activities[3].relaxationTraining),
                    socialSkills4 = activities[3].socialSkills == null ? false : Convert.ToBoolean(activities[3].socialSkills),
                    stressManagement4 = activities[3].stressManagement == null ? false : Convert.ToBoolean(activities[3].stressManagement),

                    //Client's response
                    Cooperative4 = (note_Activity.Count > 3) ? note_Activity[3].Cooperative : false,
                    Assertive4 = (note_Activity.Count > 3) ? note_Activity[3].Assertive : false,
                    Passive4 = (note_Activity.Count > 3) ? note_Activity[3].Passive : false,
                    Variable4 = (note_Activity.Count > 3) ? note_Activity[3].Variable : false,
                    Uninterested4 = (note_Activity.Count > 3) ? note_Activity[3].Uninterested : false,
                    Engaged4 = (note_Activity.Count > 3) ? note_Activity[3].EngagedActive : false,
                    Distractible4 = (note_Activity.Count > 3) ? note_Activity[3].Distractible : false,
                    Confused4 = (note_Activity.Count > 3) ? note_Activity[3].Confused : false,
                    Aggresive4 = (note_Activity.Count > 3) ? note_Activity[3].Aggresive : false,
                    Resistant4 = (note_Activity.Count > 3) ? note_Activity[3].Resistant : false,
                    Other4 = (note_Activity.Count > 3) ? note_Activity[3].Other : false
                };
            }
            
            return View(noteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> ApproveNoteP(NoteViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Home/Error404");
            }

            NotePEntity note = await _context.NotesP

                                             .Include(n => n.NotesP_Activities)

                                             .FirstOrDefaultAsync(n => n.Id == model.Id);

            note.Status = NoteStatus.Approved;
            note.DateOfApprove = DateTime.Now;
            note.Supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.LinkedUser == User.Identity.Name);

            _context.Update(note);

            await _context.SaveChangesAsync();

            if (model.Origin == 3)  ///viene de la pagina PendingNotes
                return RedirectToAction(nameof(PendingNotes));
            if (model.Origin == 4)  ///viene de la pagina NotesWithReview
                return RedirectToAction(nameof(NotesWithReview));
            if (model.Origin == 5)  ///viene de la pagina Notifications
                return RedirectToAction("Notifications", "Messages");

            return RedirectToAction(nameof(NotesSupervision));
        }

        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveIndNote(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)                                                                           

                                                                           .Include(wc => wc.Facilitator)

                                                                           .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IndividualNoteEntity note = await _context.IndividualNotes
                                                      
                                                      .Include(n => n.Workday_Cient)
                                                      .ThenInclude(wc => wc.Client)                                                  

                                                      .Include(n => n.Workday_Cient)
                                                      .ThenInclude(wc => wc.Facilitator)   
                                                      
                                                      .Include(n => n.Objective)
                                                      .ThenInclude(o => o.Goal)

                                                      .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            IndividualNoteViewModel individualNoteViewModel = null;


            individualNoteViewModel = new IndividualNoteViewModel
            {
                Id = id,
                Workday_Cient = workday_Client,
                SubjectiveData = note.SubjectiveData,
                ObjectiveData = note.ObjectiveData,
                Assessment = note.Assessment,
                PlanNote = note.PlanNote,
                Origin = origin,
                CodeBill = workday_Client.CodeBill,

                Groomed = note.Groomed,
                Unkempt = note.Unkempt,
                Disheveled = note.Disheveled,
                Meticulous = note.Meticulous,
                Overbuild = note.Overbuild,
                Other = note.Other,
                Clear = note.Clear,
                Pressured = note.Pressured,
                Slurred = note.Slurred,
                Slow = note.Slow,
                Impaired = note.Impaired,
                Poverty = note.Poverty,
                Euthymic = note.Euthymic,
                Depressed = note.Depressed,
                Anxious = note.Anxious,
                Fearful = note.Fearful,
                Irritable = note.Irritable,
                Labile = note.Labile,
                WNL = note.WNL,
                Guarded = note.Guarded,
                Withdrawn = note.Withdrawn,
                Hostile = note.Hostile,
                Restless = note.Restless,
                Impulsive = note.Impulsive,
                WNL_Cognition = note.WNL_Cognition,
                Blocked = note.Blocked,
                Obsessive = note.Obsessive,
                Paranoid = note.Paranoid,
                Scattered = note.Scattered,
                Psychotic = note.Psychotic,
                Exceptional = note.Exceptional,
                Steady = note.Steady,
                Slow_Progress = note.Slow_Progress,
                Regressing = note.Regressing,
                Stable = note.Stable,
                Maintain = note.Maintain,
                CBT = note.CBT,
                Psychodynamic = note.Psychodynamic,
                BehaviorModification = note.BehaviorModification,
                Other_Intervention = note.Other_Intervention,

                Goal1 = (note.Objective == null) ? string.Empty : note.Objective.Goal.Number.ToString(),
                Objetive1 = (note.Objective == null) ? string.Empty : note.Objective.Objetive,                
                Intervention1 = (note.Objective == null) ? string.Empty : note.Objective.Intervention
            };           

            return View(individualNoteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveIndNote(IndividualNoteViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IndividualNoteEntity note = await _context.IndividualNotes                                            
                                                      .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);


            note.Status = NoteStatus.Approved;
            note.DateOfApprove = DateTime.Now;
            note.Supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.LinkedUser == User.Identity.Name);
            _context.Update(note);

            await _context.SaveChangesAsync();

            if (model.Origin == 5)  ///viene de la pagina PendingIndNotes
                return RedirectToAction(nameof(PendingIndNotes));
            if (model.Origin == 6)  ///viene de la pagina NotesWithReview
                return RedirectToAction(nameof(IndNotesWithReview));
            if (model.Origin == 7)  ///viene de la pagina Notifications
                return RedirectToAction("Notifications", "Messages");

            return RedirectToAction(nameof(IndNotesSupervision));
        }

        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveGroupNote(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Facilitator)

                                                                           .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            GroupNoteEntity note = await _context.GroupNotes

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(wc => wc.Client)
                                                 .ThenInclude(c => c.Group)
                                                 .ThenInclude(g => g.Facilitator)

                                                 .Include(n => n.GroupNotes_Activities)
                                                 .ThenInclude(na => na.Activity)

                                                 .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            GroupNote2Entity note2 = await _context.GroupNotes2

                                                   .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            if (note2 != null && note2.Schema == SchemaTypeGroup.Schema2)
            {
                return RedirectToAction(nameof(ApproveGroupNote2), new { id = id, origin = origin });
            }

            if (note2 != null && note2.Schema == SchemaTypeGroup.Schema3)
            {
                return RedirectToAction(nameof(ApproveGroupNote3), new { id = id, origin = origin });
            }

            GroupNoteViewModel noteViewModel = null;

            List<GroupNote_Activity> note_Activity = await _context.GroupNotes_Activities
                                                                   
                                                                   .Include(na => na.Activity)
                                                                   .ThenInclude(a => a.Theme)

                                                                   .Include(n => n.Objetive)
                                                                   .ThenInclude(o => o.Goal)

                                                                   .Where(na => na.GroupNote.Id == note.Id)
                                                                   .ToListAsync();

            noteViewModel = new GroupNoteViewModel
            {
                Id = id,
                Workday_Cient = workday_Client,
                PlanNote = note.PlanNote,
                Origin = origin,
                CodeBill = workday_Client.CodeBill,

                Groomed = note.Groomed,
                Unkempt = note.Unkempt,
                Disheveled = note.Disheveled,
                Meticulous = note.Meticulous,
                Overbuild = note.Overbuild,
                Other = note.Other,
                Clear = note.Clear,
                Pressured = note.Pressured,
                Slurred = note.Slurred,
                Slow = note.Slow,
                Impaired = note.Impaired,
                Poverty = note.Poverty,
                Euthymic = note.Euthymic,
                Depressed = note.Depressed,
                Anxious = note.Anxious,
                Fearful = note.Fearful,
                Irritable = note.Irritable,
                Labile = note.Labile,
                WNL = note.WNL,
                Guarded = note.Guarded,
                Withdrawn = note.Withdrawn,
                Hostile = note.Hostile,
                Restless = note.Restless,
                Impulsive = note.Impulsive,
                WNL_Cognition = note.WNL_Cognition,
                Blocked = note.Blocked,
                Obsessive = note.Obsessive,
                Paranoid = note.Paranoid,
                Scattered = note.Scattered,
                Psychotic = note.Psychotic,
                Exceptional = note.Exceptional,
                Steady = note.Steady,
                Slow_Progress = note.Slow_Progress,
                Regressing = note.Regressing,
                Stable = note.Stable,
                Maintain = note.Maintain,
                CBT = note.CBT,
                Psychodynamic = note.Psychodynamic,
                BehaviorModification = note.BehaviorModification,
                Other_Intervention = note.Other_Intervention,

                Topic1 = note_Activity[0].Activity.Theme.Name,
                Activity1 = note_Activity[0].Activity.Name,
                AnswerClient1 = note_Activity[0].AnswerClient,
                AnswerFacilitator1 = note_Activity[0].AnswerFacilitator,
                Goal1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Goal.Number.ToString() : string.Empty,
                Objetive1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Objetive : string.Empty,

                Topic2 = note_Activity[1].Activity.Theme.Name,
                Activity2 = note_Activity[1].Activity.Name,
                AnswerClient2 = note_Activity[1].AnswerClient,
                AnswerFacilitator2 = note_Activity[1].AnswerFacilitator,
                Goal2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Goal.Number.ToString() : string.Empty,
                Objetive2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Objetive : string.Empty                
            };           

            return View(noteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveGroupNote(GroupNoteViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Home/Error404");
            }

            GroupNoteEntity note = await _context.GroupNotes                                                 

                                                 .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);

            if (note == null)
            {
                GroupNote2Entity note2 = await _context.GroupNotes2
                                                       .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);
                note2.Status = NoteStatus.Approved;
                note2.DateOfApprove = DateTime.Now;
                note2.Supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.LinkedUser == User.Identity.Name);
                _context.Update(note2);
            }


            note.Status = NoteStatus.Approved;
            note.DateOfApprove = DateTime.Now;
            note.Supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.LinkedUser == User.Identity.Name);
            _context.Update(note);

            await _context.SaveChangesAsync();

            if (model.Origin == 7)  //viene de la pagina PendingGroupNotes
                return RedirectToAction(nameof(PendingGroupNotes));
            if (model.Origin == 8)  //viene de la pagina GroupNotesWithReview
                return RedirectToAction(nameof(GroupNotesWithReview));
            if (model.Origin == 9)  ///viene de la pagina Notifications
                return RedirectToAction("Notifications", "Messages");

            return RedirectToAction(nameof(GroupNotesSupervision));
        }

        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveGroupNote2(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Facilitator)

                                                                           .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            GroupNote2Entity note = await _context.GroupNotes2

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(wc => wc.Client)
                                                 .ThenInclude(c => c.Group)
                                                 .ThenInclude(g => g.Facilitator)

                                                 .Include(n => n.GroupNotes2_Activities)
                                                 .ThenInclude(na => na.Activity)

                                                 .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            
            List<GroupNote2_Activity> note_Activity = await _context.GroupNotes2_Activities

                                                                   .Include(na => na.Activity)
                                                                   .ThenInclude(a => a.Theme)

                                                                   .Include(n => n.Objetive)
                                                                   .ThenInclude(o => o.Goal)

                                                                   .Where(na => na.GroupNote2.Id == note.Id)
                                                                   .ToListAsync();
            if (note_Activity.Count() == 2)
            {
                GroupNote2ViewModel noteViewModel = null;
                noteViewModel = new GroupNote2ViewModel
                {
                    Id = id,
                    Origin = origin,
                    Workday_Cient = workday_Client,
                    Status = note.Status,
                    CodeBill = workday_Client.CodeBill,

                    Other = note.Other,
                    Impaired = note.Impaired,
                    Euthymic = note.Euthymic,
                    Depressed = note.Depressed,
                    Anxious = note.Anxious,
                    Irritable = note.Irritable,
                    Guarded = note.Guarded,
                    Withdrawn = note.Withdrawn,
                    Hostile = note.Hostile,
                    Adequated = note.Adequated,
                    Assigned = note.Assigned,
                    AssignedTopicOf = note.AssignedTopicOf,
                    Congruent = note.Congruent,
                    Descompensating = note.Descompensating,
                    Developing = note.Developing,
                    Dramatic = note.Dramatic,
                    Euphoric = note.Euphoric,
                    Expressing = note.Expressing,
                    Facilitated = note.Facilitated,
                    Fair = note.Fair,
                    FairAttitude = note.FairAttitude,
                    Faulty = note.Faulty,
                    Getting = note.Getting,
                    GroupLeaderFacilitator = note.GroupLeaderFacilitator,
                    GroupLeaderFacilitatorAbout = note.GroupLeaderFacilitatorAbout,
                    GroupLeaderProviderPsychoeducation = note.GroupLeaderProviderPsychoeducation,
                    GroupLeaderProviderSupport = note.GroupLeaderProviderSupport,
                    Inadequated = note.Inadequated,
                    InsightAdequate = note.InsightAdequate,
                    Involved = note.Involved,
                    Kept = note.Kept,
                    LearningAbout = note.LearningAbout,
                    LearningFrom = note.LearningFrom,
                    Limited = note.Limited,
                    MildlyImpaired = note.MildlyImpaired,
                    MinimalProgress = note.MinimalProgress,
                    ModerateProgress = note.ModerateProgress,
                    Motivated = note.Motivated,
                    Negativistic = note.Negativistic,
                    NoProgress = note.NoProgress,
                    Normal = note.Normal,
                    NotToPerson = note.NotToPerson,
                    NotToPlace = note.NotToPlace,
                    NotToTime = note.NotToTime,
                    Optimistic = note.Optimistic,
                    Oriented = note.Oriented,
                    OtherExplain = note.OtherExplain,
                    Providing = note.Providing,
                    Received = note.Received,
                    Regression = note.Regression,
                    SevereryImpaired = note.SevereryImpaired,
                    Sharing = note.Sharing,
                    Short = note.Short,
                    SignificantProgress = note.SignificantProgress,
                    UnableToDetermine = note.UnableToDetermine,
                    Unmotivated = note.Unmotivated,

                    Topic1 = note_Activity[0].Activity.Theme.Name,
                    Activity1 = note_Activity[0].Activity.Name,
                    AnswerClient1 = note_Activity[0].AnswerClient,
                    Goal1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Objetive : string.Empty,

                    Topic2 = note_Activity[1].Activity.Theme.Name,
                    Activity2 = note_Activity[1].Activity.Name,
                    AnswerClient2 = note_Activity[1].AnswerClient,
                    Goal2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive2 = (note_Activity[1].Objetive != null) ? note_Activity[1].Objetive.Objetive : string.Empty
                };
                return View(noteViewModel);
            }
            if (note_Activity.Count() == 1)
            {
                return RedirectToAction(nameof(ApproveGroupNote3), new { id = id, origin = origin });
            }

            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveGroupNote2(GroupNote2ViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Home/Error404");
            }

            GroupNote2Entity note = await _context.GroupNotes2

                                                 .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);


            note.Status = NoteStatus.Approved;
            note.DateOfApprove = DateTime.Now;
            note.Supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.LinkedUser == User.Identity.Name);
            _context.Update(note);

            await _context.SaveChangesAsync();

            if (model.Origin == 7)  //viene de la pagina PendingGroupNotes
                return RedirectToAction(nameof(PendingGroupNotes));
            if (model.Origin == 8)  //viene de la pagina GroupNotesWithReview
                return RedirectToAction(nameof(GroupNotesWithReview));
            if (model.Origin == 9)  ///viene de la pagina Notifications
                return RedirectToAction("Notifications", "Messages");

            return RedirectToAction(nameof(GroupNotesSupervision));
        }

        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveGroupNote3(int id, int origin = 0)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients.Include(wc => wc.Workday)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Clinic)

                                                                           .Include(wc => wc.Client)
                                                                           .ThenInclude(c => c.Group)

                                                                           .Include(wc => wc.Facilitator)

                                                                           .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            GroupNote2Entity note = await _context.GroupNotes2

                                                 .Include(n => n.Workday_Cient)
                                                 .ThenInclude(wc => wc.Client)
                                                 .ThenInclude(c => c.Group)
                                                 .ThenInclude(g => g.Facilitator)

                                                 .Include(n => n.GroupNotes2_Activities)
                                                 .ThenInclude(na => na.Activity)

                                                 .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);


            List<GroupNote2_Activity> note_Activity = await _context.GroupNotes2_Activities

                                                                   .Include(na => na.Activity)
                                                                   .ThenInclude(a => a.Theme)

                                                                   .Include(n => n.Objetive)
                                                                   .ThenInclude(o => o.Goal)

                                                                   .Where(na => na.GroupNote2.Id == note.Id)
                                                                   .ToListAsync();
            GroupNote3ViewModel noteViewModel = null;

            if (note_Activity.Count() == 1)
            {
                noteViewModel = new GroupNote3ViewModel
                {
                    Id = id,
                    Origin = origin,
                    Workday_Cient = workday_Client,
                    Status = note.Status,
                    CodeBill = workday_Client.CodeBill,

                    Other = note.Other,
                    Impaired = note.Impaired,
                    Euthymic = note.Euthymic,
                    Depressed = note.Depressed,
                    Anxious = note.Anxious,
                    Irritable = note.Irritable,
                    Guarded = note.Guarded,
                    Withdrawn = note.Withdrawn,
                    Hostile = note.Hostile,
                    Adequated = note.Adequated,
                    Assigned = note.Assigned,
                    AssignedTopicOf = note.AssignedTopicOf,
                    Congruent = note.Congruent,
                    Descompensating = note.Descompensating,
                    Developing = note.Developing,
                    Dramatic = note.Dramatic,
                    Euphoric = note.Euphoric,
                    Expressing = note.Expressing,
                    Facilitated = note.Facilitated,
                    Fair = note.Fair,
                    FairAttitude = note.FairAttitude,
                    Faulty = note.Faulty,
                    Getting = note.Getting,
                    GroupLeaderFacilitator = note.GroupLeaderFacilitator,
                    GroupLeaderFacilitatorAbout = note.GroupLeaderFacilitatorAbout,
                    GroupLeaderProviderPsychoeducation = note.GroupLeaderProviderPsychoeducation,
                    GroupLeaderProviderSupport = note.GroupLeaderProviderSupport,
                    Inadequated = note.Inadequated,
                    InsightAdequate = note.InsightAdequate,
                    Involved = note.Involved,
                    Kept = note.Kept,
                    LearningAbout = note.LearningAbout,
                    LearningFrom = note.LearningFrom,
                    Limited = note.Limited,
                    MildlyImpaired = note.MildlyImpaired,
                    MinimalProgress = note.MinimalProgress,
                    ModerateProgress = note.ModerateProgress,
                    Motivated = note.Motivated,
                    Negativistic = note.Negativistic,
                    NoProgress = note.NoProgress,
                    Normal = note.Normal,
                    NotToPerson = note.NotToPerson,
                    NotToPlace = note.NotToPlace,
                    NotToTime = note.NotToTime,
                    Optimistic = note.Optimistic,
                    Oriented = note.Oriented,
                    OtherExplain = note.OtherExplain,
                    Providing = note.Providing,
                    Received = note.Received,
                    Regression = note.Regression,
                    SevereryImpaired = note.SevereryImpaired,
                    Sharing = note.Sharing,
                    Short = note.Short,
                    SignificantProgress = note.SignificantProgress,
                    UnableToDetermine = note.UnableToDetermine,
                    Unmotivated = note.Unmotivated,

                    Topic1 = note_Activity[0].Activity.Theme.Name,
                    Activity1 = note_Activity[0].Activity.Name,
                    AnswerClient1 = note_Activity[0].AnswerClient,
                    Goal1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Goal.Number.ToString() : string.Empty,
                    Objetive1 = (note_Activity[0].Objetive != null) ? note_Activity[0].Objetive.Objetive : string.Empty

                };
               
            }

            return View(noteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor, Facilitator")]
        public async Task<IActionResult> ApproveGroupNote3(GroupNote3ViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Home/Error404");
            }

            GroupNote2Entity note = await _context.GroupNotes2

                                                 .FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);


            note.Status = NoteStatus.Approved;
            note.DateOfApprove = DateTime.Now;
            note.Supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.LinkedUser == User.Identity.Name);
            _context.Update(note);

            await _context.SaveChangesAsync();

            if (model.Origin == 7)  //viene de la pagina PendingGroupNotes
                return RedirectToAction(nameof(PendingGroupNotes));
            if (model.Origin == 8)  //viene de la pagina GroupNotesWithReview
                return RedirectToAction(nameof(GroupNotesWithReview));
            if (model.Origin == 9)  ///viene de la pagina Notifications
                return RedirectToAction("Notifications", "Messages");

            return RedirectToAction(nameof(GroupNotesSupervision));
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> PrintNotes(int id)
        {
            WorkdayEntity workday = await _context.Workdays.FirstOrDefaultAsync(w => w.Id == id);

            if (workday == null)
            {
                return RedirectToAction("Home/Error404");
            }

            PrintNotesViewModel noteViewModel = new PrintNotesViewModel
            {
                DateOfPrint = workday.Date
            };

            return View(noteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> PrintNotes(PrintNotesViewModel model, IFormCollection form)
        {
            var meridian = (form["classifications"] == "First") ? "AM" : "PM";
            List<Workday_Client> list = await _context.Workdays_Clients
                                                      .Include(wc => wc.Client)
                                                      .ThenInclude(c => c.Group)

                                                      .Include(wc => wc.Facilitator)
                                                      
                                                      .Include(wc => wc.Note)
                                                      .ThenInclude(n => n.Supervisor)

                                                      .Include(wc => wc.Note)
                                                      .ThenInclude(n => n.Notes_Activities)

                                                      .Where(wc => (wc.Workday.Date == model.DateOfPrint 
                                                          && wc.Client.Group.Facilitator.LinkedUser == User.Identity.Name
                                                          && wc.Session == meridian
                                                          && wc.Note.Status == NoteStatus.Approved)).ToListAsync();
            if (list.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "There are not a approved notes on that date");
            }
            
            return View(model);
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> PrintWorkdaysNotes(int id)
        {
            WorkdayEntity workday = await _context.Workdays

                                                  .Include(w => w.Workdays_Clients)     
                                                  .ThenInclude(wc => wc.Facilitator)

                                                  .FirstOrDefaultAsync(w => w.Id == id);

            if (workday == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IEnumerable<Workday_Client> workdayClientList = workday.Workdays_Clients
                                                                   .Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name
                                                                             && wc.Workday.Id == workday.Id);
            Workday_Client workdayClient;
            List<FileContentResult> fileContentList = new List<FileContentResult>();
            foreach (var item in workdayClientList)
            {
                workdayClient = _context.Workdays_Clients

                                        .Include(wc => wc.Facilitator)

                                        .Include(wc => wc.Client)
                                        .ThenInclude(c => c.MTPs)
                                        .ThenInclude(m => m.Goals)
                                        .ThenInclude(g => g.Objetives)

                                        .Include(wc => wc.Client)
                                        .ThenInclude(c => c.Clients_Diagnostics)
                                        .ThenInclude(cd => cd.Diagnostic)

                                        .Include(wc => wc.Note)
                                        .ThenInclude(n => n.Supervisor)
                                        .ThenInclude(s => s.Clinic)

                                        .Include(wc => wc.Note)
                                        .ThenInclude(n => n.Notes_Activities)
                                        .ThenInclude(na => na.Activity)
                                        .ThenInclude(a => a.Theme)

                                        .Include(wc => wc.Note)
                                        .ThenInclude(n => n.Notes_Activities)
                                        .ThenInclude(na => na.Objetive)
                                        .ThenInclude(o => o.Goal)

                                        .Include(wc => wc.Note)
                                        .ThenInclude(n => n.Notes_Activities)

                                        .Include(wc => wc.Workday)

                                        .FirstOrDefault(wc => (wc.Id == item.Id));

                if ((workdayClient.Note != null) && (workdayClient.Note.Status == NoteStatus.Approved))
                {
                    if (workdayClient.Note.Supervisor.Clinic.Name == "DAVILA")
                    {
                        if (workdayClient.Note.Schema == Common.Enums.SchemaType.Schema1)
                        {
                            fileContentList.Add(DavilaNoteReportFCRSchema1(workdayClient));
                        }
                        if (workdayClient.Note.Schema == Common.Enums.SchemaType.Schema4)
                        {
                            Stream stream = _reportHelper.DavilaNoteReportSchema4(workdayClient); 
                            fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                        }
                    }
                    if (workdayClient.Note.Supervisor.Clinic.Name == "LARKIN BEHAVIOR")
                    {
                        if (workdayClient.Note.Schema == Common.Enums.SchemaType.Schema1)
                        {
                            fileContentList.Add(LarkinNoteReportFCRSchema1(workdayClient));
                        }
                        if (workdayClient.Note.Schema == Common.Enums.SchemaType.Schema2)
                        {
                            fileContentList.Add(LarkinNoteReportFCRSchema2(workdayClient));
                        }                        
                    }
                    if (workdayClient.Note.Supervisor.Clinic.Name == "SOL & VIDA")
                    {
                        fileContentList.Add(SolAndVidaNoteReportFCRSchema1(workdayClient));
                    }                    
                    if (workdayClient.Note.Supervisor.Clinic.Name == "ADVANCED GROUP MEDICAL CENTER")
                    {
                        fileContentList.Add(AdvancedGroupMCNoteReportFCRSchema2(workdayClient));                        
                    }
                    if (workdayClient.Note.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
                    {
                        fileContentList.Add(FloridaSocialHSNoteReportFCRSchema2(workdayClient));                        
                    }
                    if (workdayClient.Note.Supervisor.Clinic.Name == "ATLANTIC GROUP MEDICAL CENTER")
                    {
                        fileContentList.Add(AtlanticGroupMCNoteReportFCRSchema2(workdayClient));                        
                    }
                    if (workdayClient.Note.Supervisor.Clinic.Name == "DEMO CLINIC SCHEMA 1")
                    {
                        fileContentList.Add(DemoClinic1NoteReportFCRSchema1(workdayClient));                        
                    }
                    if (workdayClient.Note.Supervisor.Clinic.Name == "DEMO CLINIC SCHEMA 2")
                    {
                        fileContentList.Add(DemoClinic2NoteReportFCRSchema2(workdayClient));                        
                    }
                }                
            }
            
            return File(_fileHelper.Zip(fileContentList), "application/zip", $"{workday.Date.ToShortDateString()}.zip");
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> PrintWorkdaysNotesP(int id)
        {
            IEnumerable<Workday_Client> workdayClientList = await _context.Workdays_Clients

                                                                          .Include(wc => wc.Facilitator)

                                                                          .Include(wc => wc.Client)                                                                          

                                                                          .Include(wc => wc.NoteP)
                                                                            .ThenInclude(n => n.Supervisor)
                                                                            .ThenInclude(s => s.Clinic)

                                                                          .Include(wc => wc.NoteP)
                                                                            .ThenInclude(n => n.NotesP_Activities)
                                                                            .ThenInclude(na => na.Activity)
                                                                            .ThenInclude(a => a.Theme)

                                                                          .Include(wc => wc.NoteP)
                                                                            .ThenInclude(n => n.NotesP_Activities)
                                                                            .ThenInclude(na => na.Objetive)
                                                                            .ThenInclude(o => o.Goal)

                                                                          .Include(wc => wc.Workday)
                                                                            .ThenInclude(w => w.Workdays_Activities_Facilitators)
                                                                            .ThenInclude(waf => waf.Facilitator)

                                                                          .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                                        && wc.NoteP != null && wc.NoteP.Status == NoteStatus.Approved
                                                                                        && wc.Workday.Id == id))
                                                                          .ToListAsync();
            if (workdayClientList.Count() == 0)
            {
                return RedirectToAction("Home/Error404");
            }

            List<FileContentResult> fileContentList = new List<FileContentResult>();
            foreach (var item in workdayClientList)
            {                
                if (item.NoteP.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
                {
                    if (item.NoteP.Schema == Common.Enums.SchemaType.Schema3)
                    {
                        Stream stream = _reportHelper.FloridaSocialHSNoteReportSchema3(item);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{item.Client.Name}.pdf"));
                    }
                }
                if (item.NoteP.Supervisor.Clinic.Name == "DREAMS MENTAL HEALTH INC")
                {
                    if (item.NoteP.Schema == Common.Enums.SchemaType.Schema3)
                    {
                        Stream stream = _reportHelper.DreamsMentalHealthNoteReportSchema3(item);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{item.Client.Name}.pdf"));
                    }
                }
                if (item.NoteP.Supervisor.Clinic.Name == "COMMUNITY HEALTH THERAPY CENTER")
                {
                    if (item.NoteP.Schema == Common.Enums.SchemaType.Schema3)
                    {
                        Stream stream = _reportHelper.CommunityHTCNoteReportSchema3(item);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{item.Client.Name}.pdf"));
                    }
                }
            }
            
            return File(_fileHelper.Zip(fileContentList), "application/zip", $"{workdayClientList.First().Workday.Date.ToShortDateString()}.zip");
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> PrintWorkdaysIndNotes(int id)
        {
            WorkdayEntity workday = await _context.Workdays

                                                  .Include(w => w.Workdays_Clients)
                                                  .ThenInclude(wc => wc.Facilitator)

                                                  .FirstOrDefaultAsync(w => w.Id == id);

            if (workday == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IEnumerable<Workday_Client> workdayClientList = workday.Workdays_Clients
                                                                   .Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name
                                                                             && wc.Workday.Id == workday.Id);
            Workday_Client workdayClient;
            List<FileContentResult> fileContentList = new List<FileContentResult>();
            foreach (var item in workdayClientList)
            {
                workdayClient = _context.Workdays_Clients

                                        .Include(wc => wc.Facilitator)

                                        .Include(wc => wc.Client)
                                        .ThenInclude(c => c.MTPs)
                                        .ThenInclude(m => m.Goals)
                                        .ThenInclude(g => g.Objetives)

                                        .Include(wc => wc.Client)
                                        .ThenInclude(c => c.Clients_Diagnostics)
                                        .ThenInclude(cd => cd.Diagnostic)

                                        .Include(wc => wc.IndividualNote)
                                        .ThenInclude(n => n.Supervisor)
                                        .ThenInclude(s => s.Clinic)

                                        .Include(wc => wc.IndividualNote)
                                        .ThenInclude(n => n.Objective)

                                        .Include(wc => wc.Workday)

                                        .FirstOrDefault(wc => (wc.Id == item.Id));

                if ((workdayClient.IndividualNote != null) && (workdayClient.IndividualNote.Status == NoteStatus.Approved))
                {
                    if (workdayClient.IndividualNote.Supervisor.Clinic.Name == "DAVILA")
                    {                       
                       Stream stream = _reportHelper.DavilaIndNoteReportSchema1(workdayClient);
                       fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));                        
                    }
                    if (workdayClient.IndividualNote.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
                    {
                        Stream stream = _reportHelper.FloridaSocialHSIndNoteReportSchema1(workdayClient);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                    }
                    if (workdayClient.IndividualNote.Supervisor.Clinic.Name == "DREAMS MENTAL HEALTH INC")
                    {
                        Stream stream = _reportHelper.DreamsMentalHealthIndNoteReportSchema1(workdayClient);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                    }
                    if (workdayClient.IndividualNote.Supervisor.Clinic.Name == "COMMUNITY HEALTH THERAPY CENTER")
                    {
                        Stream stream = _reportHelper.CommunityHTCIndNoteReportSchema1(workdayClient);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                    }
                }
            }
            
            return File(_fileHelper.Zip(fileContentList), "application/zip", $"{workday.Date.ToShortDateString()}.zip");
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> PrintWorkdaysGroupNotes(int id)
        {
            WorkdayEntity workday = await _context.Workdays

                                                  .Include(w => w.Workdays_Clients)
                                                  .ThenInclude(wc => wc.Facilitator)

                                                  .FirstOrDefaultAsync(w => w.Id == id);

            if (workday == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IEnumerable<Workday_Client> workdayClientList = workday.Workdays_Clients
                                                                   .Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name
                                                                             && wc.Workday.Id == workday.Id);

            Workday_Client workdayClient;
            List<FileContentResult> fileContentList = new List<FileContentResult>();
            foreach (var item in workdayClientList)
            {
                workdayClient = _context.Workdays_Clients

                                        .Include(wc => wc.Facilitator)

                                        .Include(wc => wc.Client)
                                        .ThenInclude(c => c.MTPs)
                                        .ThenInclude(m => m.Goals)
                                        .ThenInclude(g => g.Objetives)

                                        .Include(wc => wc.GroupNote)
                                        .ThenInclude(n => n.Supervisor)
                                        .ThenInclude(s => s.Clinic)

                                        .Include(wc => wc.GroupNote)
                                        .ThenInclude(n => n.GroupNotes_Activities)
                                        .ThenInclude(na => na.Activity)
                                        .ThenInclude(a => a.Theme)

                                        .Include(wc => wc.GroupNote)
                                        .ThenInclude(n => n.GroupNotes_Activities)
                                        .ThenInclude(na => na.Objetive)
                                        .ThenInclude(o => o.Goal)

                                        .Include(wc => wc.Workday)

                                        .FirstOrDefault(wc => (wc.Id == item.Id));               

                if ((workdayClient.GroupNote != null) && (workdayClient.GroupNote.Status == NoteStatus.Approved))
                {
                    if (workdayClient.GroupNote.Supervisor.Clinic.Name == "DAVILA")
                    {
                        Stream stream = _reportHelper.DavilaGroupNoteReportSchema1(workdayClient);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                    }
                    if (workdayClient.GroupNote.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
                    {
                        Stream stream = _reportHelper.FloridaSocialHSGroupNoteReportSchema1(workdayClient);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                    }
                    if (workdayClient.GroupNote.Supervisor.Clinic.Name == "DREAMS MENTAL HEALTH INC")
                    {
                        Stream stream = _reportHelper.DreamsMentalHealthGroupNoteReportSchema1(workdayClient);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                    }                    
                }
            }
                        
            return File(_fileHelper.Zip(fileContentList), "application/zip", $"{workday.Date.ToShortDateString()}.zip");
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> PrintWorkdaysGroupNotes2(int id)
        {
            WorkdayEntity workday = await _context.Workdays

                                                  .Include(w => w.Workdays_Clients)
                                                  .ThenInclude(wc => wc.Facilitator)

                                                  .FirstOrDefaultAsync(w => w.Id == id);

            if (workday == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IEnumerable<Workday_Client> workdayClientList = workday.Workdays_Clients
                                                                   .Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name
                                                                             && wc.Workday.Id == workday.Id);

            Workday_Client workdayClient;
            List<FileContentResult> fileContentList = new List<FileContentResult>();
            foreach (var item in workdayClientList)
            {
                workdayClient = _context.Workdays_Clients

                                        .Include(wc => wc.Facilitator)

                                        .Include(wc => wc.Client)
                                        .ThenInclude(c => c.MTPs)
                                        .ThenInclude(m => m.Goals)
                                        .ThenInclude(g => g.Objetives)

                                        .Include(wc => wc.GroupNote2)
                                        .ThenInclude(n => n.Supervisor)
                                        .ThenInclude(s => s.Clinic)

                                        .Include(wc => wc.GroupNote2)
                                        .ThenInclude(n => n.GroupNotes2_Activities)
                                        .ThenInclude(na => na.Activity)
                                        .ThenInclude(a => a.Theme)

                                        .Include(wc => wc.GroupNote2)
                                        .ThenInclude(n => n.GroupNotes2_Activities)
                                        .ThenInclude(na => na.Objetive)
                                        .ThenInclude(o => o.Goal)

                                        .Include(wc => wc.Workday)

                                        .Include(wc => wc.Schedule)

                                        .FirstOrDefault(wc => (wc.Id == item.Id));

                if ((workdayClient.GroupNote2 != null) && (workdayClient.GroupNote2.Status == NoteStatus.Approved))
                {
                    if (workdayClient.GroupNote2.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
                    {
                        Stream stream = _reportHelper.FloridaSocialHSGroupNoteReportSchema3(workdayClient);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                    }
                    if (workdayClient.GroupNote2.Supervisor.Clinic.Name == "DREAMS MENTAL HEALTH INC")
                    {
                        Stream stream = _reportHelper.DreamsMentalHealthGroupNoteReportSchema3(workdayClient);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                    }
                    if (workdayClient.GroupNote2.Supervisor.Clinic.Name == "COMMUNITY HEALTH THERAPY CENTER")
                    {
                        Stream stream = _reportHelper.CommunityHTCGroupNoteReportSchema3(workdayClient);
                        fileContentList.Add(File(_reportHelper.ConvertStreamToByteArray(stream), "application/pdf", $"{workdayClient.Client.Name}.pdf"));
                    }
                }
            }

            return File(_fileHelper.Zip(fileContentList), "application/zip", $"{workday.Date.ToShortDateString()}.zip");
        }

        [Authorize(Roles = "Facilitator, Manager")]        
        public IActionResult PrintNote(int id)
        {
            Workday_Client workdayClient = _context.Workdays_Clients
                                                   .Include(wc => wc.Facilitator)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.MTPs)
                                                   .ThenInclude(m => m.Goals)
                                                   .ThenInclude(g => g.Objetives)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.Clients_Diagnostics)
                                                   .ThenInclude(cd => cd.Diagnostic)                                              

                                                   .Include(wc => wc.Note)
                                                   .ThenInclude(n => n.Supervisor)
                                                   .ThenInclude(s => s.Clinic)

                                                   .Include(wc => wc.Note)
                                                   .ThenInclude(n => n.Notes_Activities)
                                                   .ThenInclude(na => na.Activity)
                                                   .ThenInclude(a => a.Theme)

                                                   .Include(wc => wc.Note)
                                                   .ThenInclude(n => n.Notes_Activities)
                                                   .ThenInclude(na => na.Objetive)
                                                   .ThenInclude(o => o.Goal)

                                                   .Include(wc => wc.Workday)
                                                    
                                                   .FirstOrDefault(wc => (wc.Id == id && (wc.Note.Status == NoteStatus.Approved || wc.NoteP.Status == NoteStatus.Approved)));
            if (workdayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            //redirect to notes of schema 3
            if (workdayClient.Note == null)
            {
                return RedirectToAction("PrintNoteP", new {id =id });
            }
            
            if (workdayClient.Note.Supervisor.Clinic.Name == "DAVILA")
            {
                if (workdayClient.Note.Schema == Common.Enums.SchemaType.Schema1)
                {
                    return DavilaNoteReportSchema1(workdayClient);
                }
                if (workdayClient.Note.Schema == Common.Enums.SchemaType.Schema4)
                {
                    Stream stream = _reportHelper.DavilaNoteReportSchema4(workdayClient);
                    return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
                }
            }

            if (workdayClient.Note.Supervisor.Clinic.Name == "LARKIN BEHAVIOR")
            {
                if (workdayClient.Note.Schema == Common.Enums.SchemaType.Schema1)
                {
                    return LarkinNoteReportSchema1(workdayClient);
                }
                if (workdayClient.Note.Schema == Common.Enums.SchemaType.Schema2)
                {
                    return LarkinNoteReportSchema2(workdayClient);
                }
            }

            if (workdayClient.Note.Supervisor.Clinic.Name == "SOL & VIDA")
            {
                return SolAndVidaNoteReportSchema1(workdayClient);
            }            

            if (workdayClient.Note.Supervisor.Clinic.Name == "ADVANCED GROUP MEDICAL CENTER")
            {
                return AdvancedGroupMCNoteReportSchema2(workdayClient);
            }

            if (workdayClient.Note.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
            {
                return FloridaSocialHSNoteReportSchema2(workdayClient);
            }

            if (workdayClient.Note.Supervisor.Clinic.Name == "ATLANTIC GROUP MEDICAL CENTER")
            {
                return AtlanticGroupMCNoteReportSchema2(workdayClient);
            }

            if (workdayClient.Note.Supervisor.Clinic.Name == "DEMO CLINIC SCHEMA 1")
            {
                return DemoClinic1NoteReportSchema1(workdayClient);
            }

            if (workdayClient.Note.Supervisor.Clinic.Name == "DEMO CLINIC SCHEMA 2")
            {
                return DemoClinic2NoteReportSchema2(workdayClient);
            }

            return null;
        }

        [Authorize(Roles = "Facilitator, Manager")]
        public IActionResult PrintNoteP(int id)
        {
            Workday_Client workdayClient = _context.Workdays_Clients

                                                   .Include(wc => wc.Facilitator)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.MTPs)
                                                   .ThenInclude(m => m.Goals)
                                                   .ThenInclude(g => g.Objetives)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.Clients_Diagnostics)
                                                   .ThenInclude(cd => cd.Diagnostic)

                                                   .Include(wc => wc.NoteP)
                                                   .ThenInclude(n => n.Supervisor)
                                                   .ThenInclude(s => s.Clinic)

                                                   .Include(wc => wc.NoteP)
                                                   .ThenInclude(n => n.NotesP_Activities)
                                                   .ThenInclude(na => na.Activity)
                                                   .ThenInclude(a => a.Theme)

                                                   .Include(wc => wc.NoteP)
                                                   .ThenInclude(n => n.NotesP_Activities)
                                                   .ThenInclude(na => na.Objetive)
                                                   .ThenInclude(o => o.Goal)

                                                   .Include(wc => wc.Workday)
                                                   .ThenInclude(w => w.Workdays_Activities_Facilitators)
                                                   .ThenInclude(waf => waf.Facilitator)
                                                   
                                                   .FirstOrDefault(wc => (wc.Id == id && wc.NoteP.Status == NoteStatus.Approved));
            
            if (workdayClient == null || workdayClient.NoteP == null)
            {
                return RedirectToAction("Home/Error404");
            }            

            if (workdayClient.NoteP.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
            {
                if (workdayClient.NoteP.Schema == Common.Enums.SchemaType.Schema3)
                {
                    Stream stream;
                    if (!workdayClient.SharedSession)                    
                        stream = _reportHelper.FloridaSocialHSNoteReportSchema3(workdayClient);                            
                    else
                        stream = _reportHelper.FloridaSocialHSNoteReportSchema3SS(workdayClient);
                    return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
                }
            }

            if (workdayClient.NoteP.Supervisor.Clinic.Name == "DREAMS MENTAL HEALTH INC")
            {
                if (workdayClient.NoteP.Schema == Common.Enums.SchemaType.Schema3)
                {
                    Stream stream;
                    if (!workdayClient.SharedSession)
                        stream = _reportHelper.DreamsMentalHealthNoteReportSchema3(workdayClient);
                    else
                        stream = _reportHelper.DreamsMentalHealthNoteReportSchema3SS(workdayClient);
                    return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
                }
            }

            if (workdayClient.NoteP.Supervisor.Clinic.Name == "COMMUNITY HEALTH THERAPY CENTER")
            {
                if (workdayClient.NoteP.Schema == Common.Enums.SchemaType.Schema3)
                {
                    Stream stream;
                    if (!workdayClient.SharedSession)
                        stream = _reportHelper.CommunityHTCNoteReportSchema3(workdayClient);
                    else
                        stream = _reportHelper.CommunityHTCNoteReportSchema3SS(workdayClient);
                    return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
                }
            }

            return null;
        }

        [Authorize(Roles = "Facilitator, Manager")]
        public IActionResult PrintIndNote(int id)
        {
            Workday_Client workdayClient = _context.Workdays_Clients

                                                    .Include(wc => wc.Facilitator)

                                                    .Include(wc => wc.Client)
                                                    .ThenInclude(c => c.MTPs)
                                                    .ThenInclude(m => m.Goals)
                                                    .ThenInclude(g => g.Objetives)

                                                    .Include(wc => wc.Client)
                                                    .ThenInclude(c => c.Clients_Diagnostics)
                                                    .ThenInclude(cd => cd.Diagnostic)

                                                    .Include(wc => wc.IndividualNote)
                                                    .ThenInclude(n => n.Supervisor)
                                                    .ThenInclude(s => s.Clinic)

                                                    .Include(wc => wc.IndividualNote)
                                                    .ThenInclude(n => n.Objective)

                                                    .Include(wc => wc.Workday)

                                                    .FirstOrDefault(wc => (wc.Id == id && wc.IndividualNote.Status == NoteStatus.Approved));
            if (workdayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (workdayClient.IndividualNote.Supervisor.Clinic.Name == "DAVILA")
            {
                Stream stream = _reportHelper.DavilaIndNoteReportSchema1(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);                
            }
            if (workdayClient.IndividualNote.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
            {
                Stream stream = _reportHelper.FloridaSocialHSIndNoteReportSchema1(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.IndividualNote.Supervisor.Clinic.Name == "DREAMS MENTAL HEALTH INC")
            {
                Stream stream = _reportHelper.DreamsMentalHealthIndNoteReportSchema1(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.IndividualNote.Supervisor.Clinic.Name == "COMMUNITY HEALTH THERAPY CENTER")
            {
                Stream stream = _reportHelper.CommunityHTCIndNoteReportSchema1(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            return null;
        }

        [Authorize(Roles = "Facilitator, Manager")]
        public IActionResult PrintGroupNote(int id)
        {
            Workday_Client workdayClient = _context.Workdays_Clients

                                                   .Include(wc => wc.Facilitator)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.MTPs)
                                                   .ThenInclude(m => m.Goals)
                                                   .ThenInclude(g => g.Objetives)                                                   

                                                   .Include(wc => wc.GroupNote)
                                                   .ThenInclude(n => n.Supervisor)
                                                   .ThenInclude(s => s.Clinic)

                                                   .Include(wc => wc.GroupNote)
                                                   .ThenInclude(n => n.GroupNotes_Activities)
                                                   .ThenInclude(na => na.Activity)
                                                   .ThenInclude(a => a.Theme)

                                                   .Include(wc => wc.GroupNote)
                                                   .ThenInclude(n => n.GroupNotes_Activities)
                                                   .ThenInclude(na => na.Objetive)
                                                   .ThenInclude(o => o.Goal)

                                                   .Include(wc => wc.Workday)

                                                   .FirstOrDefault(wc => (wc.Id == id && (wc.GroupNote.Status == NoteStatus.Approved || wc.GroupNote2.Status == NoteStatus.Approved)));
            if (workdayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            //redirect to notes of schema 2 or squema 3 in Group Therapy
            if (workdayClient.GroupNote == null)
            {
                return RedirectToAction("PrintGroupNote2", new { id = id });
            }

            if (workdayClient.GroupNote.Supervisor.Clinic.Name == "DAVILA")
            {
                Stream stream = _reportHelper.DavilaGroupNoteReportSchema1(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.GroupNote.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
            {
                Stream stream = _reportHelper.FloridaSocialHSGroupNoteReportSchema1(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.GroupNote.Supervisor.Clinic.Name == "DREAMS MENTAL HEALTH INC")
            {
                Stream stream = _reportHelper.DreamsMentalHealthGroupNoteReportSchema1(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }

            return null;
        }

        [Authorize(Roles = "Facilitator, Manager")]
        public IActionResult PrintGroupNote2(int id)
        {
            Workday_Client workdayClient = _context.Workdays_Clients

                                                   .Include(wc => wc.Facilitator)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.MTPs)
                                                   .ThenInclude(m => m.Goals)
                                                   .ThenInclude(g => g.Objetives)

                                                   .Include(wc => wc.GroupNote2)
                                                   .ThenInclude(n => n.Supervisor)
                                                   .ThenInclude(s => s.Clinic)

                                                   .Include(wc => wc.GroupNote2)
                                                   .ThenInclude(n => n.GroupNotes2_Activities)
                                                   .ThenInclude(na => na.Activity)
                                                   .ThenInclude(a => a.Theme)

                                                   .Include(wc => wc.GroupNote2)
                                                   .ThenInclude(n => n.GroupNotes2_Activities)
                                                   .ThenInclude(na => na.Objetive)
                                                   .ThenInclude(o => o.Goal)

                                                   .Include(wc => wc.Workday)
                                                   
                                                   .Include(wc => wc.Schedule)

                                                   .FirstOrDefault(wc => (wc.Id == id && wc.GroupNote2.Status == NoteStatus.Approved));
            if (workdayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (workdayClient.GroupNote2.Supervisor.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
            {
                Stream stream = null;

                if (workdayClient.GroupNote2.Schema == SchemaTypeGroup.Schema2)
                {
                    stream = _reportHelper.FloridaSocialHSGroupNoteReportSchema2(workdayClient);
                }
                if (workdayClient.GroupNote2.Schema == SchemaTypeGroup.Schema3)
                {
                    stream = _reportHelper.FloridaSocialHSGroupNoteReportSchema3(workdayClient);
                }

                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.GroupNote2.Supervisor.Clinic.Name == "DREAMS MENTAL HEALTH INC")
            {
                Stream stream = null;

                if (workdayClient.GroupNote2.Schema == SchemaTypeGroup.Schema2)
                {
                    stream = _reportHelper.DreamsMentalHealthGroupNoteReportSchema2(workdayClient);
                }
                if (workdayClient.GroupNote2.Schema == SchemaTypeGroup.Schema3)
                {
                    stream = _reportHelper.DreamsMentalHealthGroupNoteReportSchema3(workdayClient);
                }

                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.GroupNote2.Supervisor.Clinic.Name == "COMMUNITY HEALTH THERAPY CENTER")
            {
                Stream stream = null;

                if (workdayClient.GroupNote2.Schema == SchemaTypeGroup.Schema2)
                {
                    stream = _reportHelper.CommunityHTCGroupNoteReportSchema2(workdayClient);
                }
                if (workdayClient.GroupNote2.Schema == SchemaTypeGroup.Schema3)
                {
                    stream = _reportHelper.CommunityHTCGroupNoteReportSchema3(workdayClient);
                }

                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            return null;
        }

        #region Davila        
        private IActionResult DavilaNoteReportSchema1(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteDAVILA0");
            Dictionary<string, string> parameters = new Dictionary<string, string>();            
            
            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal = string.Empty;
            var goal_text = string.Empty;
            var num_of_obj = string.Empty;
            var obj_text = string.Empty;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                        goal_text = item.Objetive.Goal.Name;
                        num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                        obj_text = item.Objetive.Description;
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();
            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("num_of_goal", num_of_goal);
            parameters.Add("goal_text", goal_text);
            parameters.Add("num_of_obj", num_of_obj);
            parameters.Add("obj_text", obj_text);
            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf"/*,
                        $"NoteOf_{workdayClient.Client.Name}_{workdayClient.Workday.Date.ToShortDateString()}.pdf"*/);
        }

        private FileContentResult DavilaNoteReportFCRSchema1(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteDAVILA0");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal = string.Empty;
            var goal_text = string.Empty;
            var num_of_obj = string.Empty;
            var obj_text = string.Empty;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                        goal_text = item.Objetive.Goal.Name;
                        num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                        obj_text = item.Objetive.Description;
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();
            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("num_of_goal", num_of_goal);
            parameters.Add("goal_text", goal_text);
            parameters.Add("num_of_obj", num_of_obj);
            parameters.Add("obj_text", obj_text);
            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf", $"{workdayClient.Client.Name}.pdf");
        }
        #endregion

        #region Larkin
        private IActionResult LarkinNoteReportSchema1(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);            
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteLARKINBEHAVIOR0");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal = string.Empty;
            var goal_text = string.Empty;
            var num_of_obj = string.Empty;
            var obj_text = string.Empty;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                        goal_text = item.Objetive.Goal.Name;
                        num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                        obj_text = item.Objetive.Description;
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();
            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("num_of_goal", num_of_goal);
            parameters.Add("goal_text", goal_text);
            parameters.Add("num_of_obj", num_of_obj);
            parameters.Add("obj_text", obj_text);
            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        private IActionResult LarkinNoteReportSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);            
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteLARKINBEHAVIOR1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        private FileContentResult LarkinNoteReportFCRSchema1(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteLARKINBEHAVIOR0");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal = string.Empty;
            var goal_text = string.Empty;
            var num_of_obj = string.Empty;
            var obj_text = string.Empty;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                        goal_text = item.Objetive.Goal.Name;
                        num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                        obj_text = item.Objetive.Description;
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();
            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("num_of_goal", num_of_goal);
            parameters.Add("goal_text", goal_text);
            parameters.Add("num_of_obj", num_of_obj);
            parameters.Add("obj_text", obj_text);
            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf", $"{workdayClient.Client.Name}.pdf");
        }

        private FileContentResult LarkinNoteReportFCRSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteLARKINBEHAVIOR1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf", $"{workdayClient.Client.Name}.pdf");
        }
        #endregion

        #region SolAndVida        
        private IActionResult SolAndVidaNoteReportSchema1(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteSolAndVida0");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            
            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal = string.Empty;
            var goal_text = string.Empty;
            var num_of_obj = string.Empty;
            var obj_text = string.Empty;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                        goal_text = item.Objetive.Goal.Name;
                        num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                        obj_text = item.Objetive.Description;
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();
            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("num_of_goal", num_of_goal);
            parameters.Add("goal_text", goal_text);
            parameters.Add("num_of_obj", num_of_obj);
            parameters.Add("obj_text", obj_text);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("setting", setting);
            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        private FileContentResult SolAndVidaNoteReportFCRSchema1(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteSolAndVida0");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal = string.Empty;
            var goal_text = string.Empty;
            var num_of_obj = string.Empty;
            var obj_text = string.Empty;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                        goal_text = item.Objetive.Goal.Name;
                        num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                        obj_text = item.Objetive.Description;
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();
            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("num_of_goal", num_of_goal);
            parameters.Add("goal_text", goal_text);
            parameters.Add("num_of_obj", num_of_obj);
            parameters.Add("obj_text", obj_text);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("setting", setting);
            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf", $"{workdayClient.Client.Name}.pdf");
        }
        #endregion

        #region Advanced      
        private IActionResult AdvancedGroupMCNoteReportSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteAdvancedGroupMC1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        private FileContentResult AdvancedGroupMCNoteReportFCRSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteAdvancedGroupMC1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf", $"{workdayClient.Client.Name}.pdf");
        }
        #endregion

        #region Florida Social Health Solution        
        private IActionResult FloridaSocialHSNoteReportSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteFloridaSocialHS1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        private FileContentResult FloridaSocialHSNoteReportFCRSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteFloridaSocialHS1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf", $"{workdayClient.Client.Name}.pdf");
        }
        #endregion

        #region Atlantic
        private IActionResult AtlanticGroupMCNoteReportSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteAtlanticGroupMC1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        private FileContentResult AtlanticGroupMCNoteReportFCRSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteAtlanticGroupMC1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf", $"{workdayClient.Client.Name}.pdf");
        }
        #endregion

        #region Demo
        private IActionResult DemoClinic1NoteReportSchema1(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteDemoClinic0");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal = string.Empty;
            var goal_text = string.Empty;
            var num_of_obj = string.Empty;
            var obj_text = string.Empty;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                        goal_text = item.Objetive.Goal.Name;
                        num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                        obj_text = item.Objetive.Description;
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();
            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("num_of_goal", num_of_goal);
            parameters.Add("goal_text", goal_text);
            parameters.Add("num_of_obj", num_of_obj);
            parameters.Add("obj_text", obj_text);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("setting", setting);
            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        private FileContentResult DemoClinic1NoteReportFCRSchema1(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteDemoClinic0");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal = string.Empty;
            var goal_text = string.Empty;
            var num_of_obj = string.Empty;
            var obj_text = string.Empty;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                        goal_text = item.Objetive.Goal.Name;
                        num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                        obj_text = item.Objetive.Description;
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (num_of_goal == string.Empty)
                    {
                        if (item.Objetive != null)
                        {
                            num_of_goal = $"GOAL #{item.Objetive.Goal.Number}:";
                            goal_text = item.Objetive.Goal.Name;
                            num_of_obj = $"OBJ {item.Objetive.Objetive}:";
                            obj_text = item.Objetive.Description;
                        }
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();
            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("num_of_goal", num_of_goal);
            parameters.Add("goal_text", goal_text);
            parameters.Add("num_of_obj", num_of_obj);
            parameters.Add("obj_text", obj_text);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("setting", setting);
            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf", $"{workdayClient.Client.Name}.pdf");
        }

        private IActionResult DemoClinic2NoteReportSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteDemoClinic1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf"/*,
                        $"NoteOf_{workdayClient.Client.Name}_{workdayClient.Workday.Date.ToShortDateString()}.pdf"*/);
        }

        private FileContentResult DemoClinic2NoteReportFCRSchema2(Workday_Client workdayClient)
        {
            //report
            string mimetype = "";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("KyoS.Web.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\Notes\\{1}.rdlc", fileDirPath, $"rptNoteDemoClinic1");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport report = new LocalReport(rdlcFilePath);

            //signatures images 
            byte[] stream1 = null;
            byte[] stream2 = null;
            string path;
            if (!string.IsNullOrEmpty(workdayClient.Note.Supervisor.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Note.Supervisor.SignaturePath)}");
                stream1 = _imageHelper.ImageToByteArray(path);
            }
            if (!string.IsNullOrEmpty(workdayClient.Facilitator.SignaturePath))
            {
                path = string.Format($"{_webhostEnvironment.WebRootPath}{_imageHelper.TrimPath(workdayClient.Facilitator.SignaturePath)}");
                stream2 = _imageHelper.ImageToByteArray(path);
            }

            //datasource
            List<Workday_Client> workdaysclients = new List<Workday_Client> { workdayClient };
            List<ClientEntity> clients = new List<ClientEntity> { workdayClient.Client };
            List<NoteEntity> notes = new List<NoteEntity> { workdayClient.Note };
            List<FacilitatorEntity> facilitators = new List<FacilitatorEntity> { workdayClient.Facilitator };
            List<SupervisorEntity> supervisors = new List<SupervisorEntity> { workdayClient.Note.Supervisor };
            List<ImageArray> images = new List<ImageArray> { new ImageArray { ImageStream1 = stream1, ImageStream2 = stream2 } };

            List<Note_Activity> notesactivities1 = new List<Note_Activity>();
            List<ActivityEntity> activities1 = new List<ActivityEntity>();
            List<ThemeEntity> themes1 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities2 = new List<Note_Activity>();
            List<ActivityEntity> activities2 = new List<ActivityEntity>();
            List<ThemeEntity> themes2 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities3 = new List<Note_Activity>();
            List<ActivityEntity> activities3 = new List<ActivityEntity>();
            List<ThemeEntity> themes3 = new List<ThemeEntity>();
            List<Note_Activity> notesactivities4 = new List<Note_Activity>();
            List<ActivityEntity> activities4 = new List<ActivityEntity>();
            List<ThemeEntity> themes4 = new List<ThemeEntity>();

            int i = 0;
            var num_of_goal1 = string.Empty;
            var goal_text1 = string.Empty;
            bool goal1 = false;
            var num_of_goal2 = string.Empty;
            var goal_text2 = string.Empty;
            bool goal2 = false;
            var num_of_goal3 = string.Empty;
            var goal_text3 = string.Empty;
            bool goal3 = false;
            var num_of_goal4 = string.Empty;
            var goal_text4 = string.Empty;
            bool goal4 = false;
            var num_of_goal5 = string.Empty;
            var goal_text5 = string.Empty;
            bool goal5 = false;
            var goal_obj_activity1 = string.Empty;
            var goal_obj_activity2 = string.Empty;
            var goal_obj_activity3 = string.Empty;
            var goal_obj_activity4 = string.Empty;

            MTPEntity mtp;
            if (workdayClient.Note.MTPId == null)   //la nota no tiene mtp relacionado, entonces se usa el primero que esté
                mtp = workdayClient.Client.MTPs.FirstOrDefault();
            else                                    //la nota tiene mtp relacionado    
                mtp = _context.MTPs.FirstOrDefault(m => m.Id == workdayClient.Note.MTPId);

            foreach (GoalEntity item in mtp.Goals.OrderBy(g => g.Number))
            {
                if (i == 0)
                {
                    num_of_goal1 = $"GOAL #{item.Number}:";
                    goal_text1 = item.Name;
                }
                if (i == 1)
                {
                    num_of_goal2 = $"GOAL #{item.Number}:";
                    goal_text2 = item.Name;
                }
                if (i == 2)
                {
                    num_of_goal3 = $"GOAL #{item.Number}:";
                    goal_text3 = item.Name;
                }
                if (i == 3)
                {
                    num_of_goal4 = $"GOAL #{item.Number}:";
                    goal_text4 = item.Name;
                }
                if (i == 4)
                {
                    num_of_goal5 = $"GOAL #{item.Number}:";
                    goal_text5 = item.Name;
                }
                i = ++i;
            }

            i = 0;
            foreach (Note_Activity item in workdayClient.Note.Notes_Activities)
            {
                if (i == 0)
                {
                    notesactivities1 = new List<Note_Activity> { item };
                    activities1 = new List<ActivityEntity> { item.Activity };
                    themes1 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity1 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 1)
                {
                    notesactivities2 = new List<Note_Activity> { item };
                    activities2 = new List<ActivityEntity> { item.Activity };
                    themes2 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity2 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 2)
                {
                    notesactivities3 = new List<Note_Activity> { item };
                    activities3 = new List<ActivityEntity> { item.Activity };
                    themes3 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity3 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                if (i == 3)
                {
                    notesactivities4 = new List<Note_Activity> { item };
                    activities4 = new List<ActivityEntity> { item.Activity };
                    themes4 = new List<ThemeEntity> { item.Activity.Theme };
                    if (item.Objetive != null)
                    {
                        if (item.Objetive.Goal.Number == 1)
                            goal1 = true;
                        if (item.Objetive.Goal.Number == 2)
                            goal2 = true;
                        if (item.Objetive.Goal.Number == 3)
                            goal3 = true;
                        if (item.Objetive.Goal.Number == 4)
                            goal4 = true;
                        if (item.Objetive.Goal.Number == 5)
                            goal5 = true;
                        goal_obj_activity4 = $"(Goal #{item.Objetive.Goal.Number}, Obj#{item.Objetive.Objetive}) ";
                    }
                }
                i = ++i;
            }

            report.AddDataSource("dsWorkdays_Clients", workdaysclients);
            report.AddDataSource("dsClients", clients);
            report.AddDataSource("dsNotes", notes);
            report.AddDataSource("dsFacilitators", facilitators);
            report.AddDataSource("dsSupervisors", supervisors);
            report.AddDataSource("dsNotesActivities1", notesactivities1);
            report.AddDataSource("dsActivities1", activities1);
            report.AddDataSource("dsThemes1", themes1);
            report.AddDataSource("dsNotesActivities2", notesactivities2);
            report.AddDataSource("dsActivities2", activities2);
            report.AddDataSource("dsThemes2", themes2);
            report.AddDataSource("dsNotesActivities3", notesactivities3);
            report.AddDataSource("dsActivities3", activities3);
            report.AddDataSource("dsThemes3", themes3);
            report.AddDataSource("dsNotesActivities4", notesactivities4);
            report.AddDataSource("dsActivities4", activities4);
            report.AddDataSource("dsThemes4", themes4);
            report.AddDataSource("dsImages", images);

            var date = $"{workdayClient.Workday.Date.DayOfWeek}, {workdayClient.Workday.Date.ToShortDateString()}";
            var dateFacilitator = workdayClient.Workday.Date.ToShortDateString();
            var dateSupervisor = workdayClient.Note.DateOfApprove.Value.ToShortDateString();

            i = 0;
            var diagnostic = string.Empty;
            foreach (var item in workdayClient.Client.Clients_Diagnostics)
            {
                if (i == 0)
                    diagnostic = item.Diagnostic.Code;
                else
                    diagnostic = $"{diagnostic}, {item.Diagnostic.Code}";
                i = ++i;
            }

            var setting = $"Setting: {workdayClient.Note.Setting}";

            parameters.Add("date", date);
            parameters.Add("dateFacilitator", dateFacilitator);
            parameters.Add("dateSupervisor", dateSupervisor);
            parameters.Add("diagnosis", diagnostic);
            parameters.Add("num_of_goal1", num_of_goal1);
            parameters.Add("goal_text1", goal_text1);
            parameters.Add("goal1", goal1.ToString());
            parameters.Add("goal_obj_activity1", goal_obj_activity1);
            parameters.Add("num_of_goal2", num_of_goal2);
            parameters.Add("goal_text2", goal_text2);
            parameters.Add("goal2", goal2.ToString());
            parameters.Add("goal_obj_activity2", goal_obj_activity2);
            parameters.Add("num_of_goal3", num_of_goal3);
            parameters.Add("goal_text3", goal_text3);
            parameters.Add("goal3", goal3.ToString());
            parameters.Add("goal_obj_activity3", goal_obj_activity3);
            parameters.Add("num_of_goal4", num_of_goal4);
            parameters.Add("goal_text4", goal_text4);
            parameters.Add("goal4", goal4.ToString());
            parameters.Add("goal_obj_activity4", goal_obj_activity4);
            parameters.Add("num_of_goal5", num_of_goal5);
            parameters.Add("goal_text5", goal_text5);
            parameters.Add("goal5", goal5.ToString());
            parameters.Add("setting", setting);

            var result = report.Execute(RenderType.Pdf, 1, parameters, mimetype);
            return File(result.MainStream, "application/pdf", $"{workdayClient.Client.Name}.pdf");
        }
        #endregion

        [Authorize(Roles = "Facilitator, Supervisor")]
        public async Task<IActionResult> MTPView(int id)
        {
            Workday_Client workday_Client = await _context.Workdays_Clients
                                                          .Include(wc => wc.Client)
                                                          .FirstOrDefaultAsync(wc => wc.Id == id);

            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            MTPEntity mtp = await _context.MTPs
                                          .FirstOrDefaultAsync(m => (m.Client.Id == workday_Client.Client.Id && m.Active == true));
            if (mtp == null)
            {
                return RedirectToAction("Home/Error404");
            }
            
            return RedirectToAction("Details", "MTPs", new {id = mtp.Id});
        }

        public JsonResult Translate(string text)
        {
            return Json(text = _translateHelper.TranslateText("es", "en", text));            
        }

        [Authorize(Roles = "Facilitator, Manager")]
        public async Task<IActionResult> NotStartedNotes(string name, int id = 0, int expired = 0)
        {
            if (expired == 1)
            {
                ViewBag.MtpExpired = "E";
            }

            if (name != string.Empty && id != 0)
            {
                return View(await _context.Workdays_Clients

                                          .Include(wc => wc.Note)
                                          .Include(wc => wc.NoteP)
                                          .Include(wc => wc.Facilitator)

                                          .Include(wc => wc.Client)
                                          .ThenInclude(c => c.Clinic)

                                          .Include(wc => wc.Workday)
                                          .ThenInclude(w => w.Week)

                                          .Where(wc => (wc.Facilitator.Name == name
                                                     && wc.Workday.Week.Id == id
                                                     && wc.Note == null && wc.NoteP == null && wc.Present == true
                                                     && wc.Workday.Service == ServiceType.PSR))
                                          .ToListAsync());
            }
            else
            { 
                return View(await _context.Workdays_Clients

                                          .Include(wc => wc.Note)
                                          .Include(wc => wc.NoteP)
                                          .Include(wc => wc.Facilitator)
                                          .Include(wc => wc.Client)
                                          .Include(wc => wc.Workday)
                                          .ThenInclude(w => w.Week)
                                          
                                          .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                     && wc.Note == null && wc.NoteP == null && wc.Present == true
                                                     && wc.Workday.Service == ServiceType.PSR))
                                          .ToListAsync());
            }
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> NotStartedIndNotes()
        {
            return View(await _context.Workdays_Clients.Include(wc => wc.IndividualNote)
                                                       .Include(wc => wc.Facilitator)
                                                       .Include(wc => wc.Client)
                                                       .Include(wc => wc.Workday)
                                                       .ThenInclude(w => w.Week)
                                                       .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                        && wc.IndividualNote == null && wc.Present == true
                                                                        && wc.Workday.Service == ServiceType.Individual))                                                       
                                                       .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> NotStartedGroupNotes()
        {
            return View(await _context.Workdays_Clients
                                      
                                      .Include(wc => wc.GroupNote)
                                      
                                      .Include(wc => wc.Facilitator)
                                      
                                      .Include(wc => wc.Client)
                                      
                                      .Include(wc => wc.Workday)
                                      .ThenInclude(w => w.Week)

                                      .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                 && (wc.GroupNote == null 
                                                 && wc.GroupNote2 == null)
                                                 && wc.Present == true
                                                 && wc.Workday.Service == ServiceType.Group))
                                      .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> NotesInEdit(int id  = 0)
        {
            if (id == 1)
            {
                ViewBag.FinishEdition = "Y";
            }
            return View(await _context.Workdays_Clients.Include(wc => wc.Note)
                                                       .Include(wc => wc.NoteP)
                                                       .Include(wc => wc.Facilitator)
                                                       .Include(wc => wc.Client)
                                                       .Include(wc => wc.Workday)
                                                       .ThenInclude(w => w.Week)
                                                       .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                        && (wc.Note.Status == NoteStatus.Edition || wc.NoteP.Status == NoteStatus.Edition)
                                                                        && wc.Workday.Service == ServiceType.PSR))
                                                       .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> IndNotesInEdit(int id = 0)
        {
            if (id == 1)
            {
                ViewBag.FinishEdition = "Y";
            }
            return View(await _context.Workdays_Clients.Include(wc => wc.IndividualNote)

                                                       .Include(wc => wc.Facilitator)

                                                       .Include(wc => wc.Client)

                                                       .Include(wc => wc.Workday)
                                                       .ThenInclude(w => w.Week)

                                                       .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                        && wc.IndividualNote.Status == NoteStatus.Edition
                                                                        && wc.Workday.Service == ServiceType.Individual))
                                                       .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> GroupNotesInEdit(int id = 0)
        {
            if (id == 1)
            {
                ViewBag.FinishEdition = "Y";
            }
            return View(await _context.Workdays_Clients.Include(wc => wc.IndividualNote)

                                                       .Include(wc => wc.Facilitator)

                                                       .Include(wc => wc.Client)

                                                       .Include(wc => wc.Workday)
                                                       .ThenInclude(w => w.Week)

                                                       .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                  && (wc.GroupNote.Status == NoteStatus.Edition
                                                                  || wc.GroupNote2.Status == NoteStatus.Edition)
                                                                  && wc.Workday.Service == ServiceType.Group))
                                                       .ToListAsync());
        }

        [Authorize(Roles = "Facilitator, Supervisor")]
        public async Task<IActionResult> PendingNotes(int id = 0, int error = 0)
        {
            if (User.IsInRole("Facilitator"))
            {
                return View(await _context.Workdays_Clients.Include(wc => wc.Note)

                                                           .Include(wc => wc.Facilitator)

                                                           .Include(wc => wc.Client)

                                                           .Include(wc => wc.Workday)
                                                           .ThenInclude(w => w.Week)

                                                           .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                                           .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                      && (wc.Note.Status == NoteStatus.Pending || wc.NoteP.Status == NoteStatus.Pending)
                                                                      && wc.Workday.Service == ServiceType.PSR))
                                                           .ToListAsync());
            }

            if (User.IsInRole("Supervisor"))
            {
                UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
                                                             .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if (user_logged.Clinic != null)
                {
                    ViewBag.Error = error.ToString();
                    return View(await _context.Workdays_Clients.Include(wc => wc.Note)

                                                               .Include(wc => wc.Facilitator)
                                                               .ThenInclude(f => f.Clinic)

                                                               .Include(wc => wc.Client)

                                                               .Include(wc => wc.Workday)
                                                               .ThenInclude(w => w.Week)

                                                               .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                                               .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                                          && (wc.Note.Status == NoteStatus.Pending || wc.NoteP.Status == NoteStatus.Pending)
                                                                          && wc.Workday.Service == ServiceType.PSR))
                                                               .ToListAsync());
                }
            }

            return View();
        }

        [Authorize(Roles = "Facilitator, Supervisor")]
        public async Task<IActionResult> PendingIndNotes(int id = 0)
        {
            if (User.IsInRole("Facilitator"))
            {
                return View(await _context.Workdays_Clients.Include(wc => wc.Note)

                                                           .Include(wc => wc.Facilitator)

                                                           .Include(wc => wc.Client)

                                                           .Include(wc => wc.Workday)
                                                           .ThenInclude(w => w.Week)

                                                           .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                                           .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                      && wc.IndividualNote.Status == NoteStatus.Pending
                                                                      && wc.Workday.Service == ServiceType.Individual))
                                                           .ToListAsync());
            }

            if (User.IsInRole("Supervisor"))
            {
                UserEntity user_logged = await _context.Users
                                                       .Include(u => u.Clinic)
                                                       .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if (user_logged.Clinic != null)
                {
                    return View(await _context.Workdays_Clients.Include(wc => wc.Note)

                                                               .Include(wc => wc.Facilitator)
                                                               .ThenInclude(f => f.Clinic)

                                                               .Include(wc => wc.Client)

                                                               .Include(wc => wc.Workday)
                                                               .ThenInclude(w => w.Week)

                                                               .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                                               .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                                          && wc.IndividualNote.Status == NoteStatus.Pending
                                                                          && wc.Workday.Service == ServiceType.Individual))
                                                               .ToListAsync());
                }
            }

            return View();
        }

        [Authorize(Roles = "Facilitator, Supervisor")]
        public async Task<IActionResult> PendingGroupNotes(int id = 0)
        {
            if (User.IsInRole("Facilitator"))
            {
                return View(await _context.Workdays_Clients.Include(wc => wc.Note)

                                                           .Include(wc => wc.Facilitator)

                                                           .Include(wc => wc.Client)

                                                           .Include(wc => wc.Workday)
                                                           .ThenInclude(w => w.Week)

                                                           .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                                           .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                      && (wc.GroupNote.Status == NoteStatus.Pending
                                                                      || wc.GroupNote2.Status == NoteStatus.Pending)
                                                                      && wc.Workday.Service == ServiceType.Group))
                                                           .ToListAsync());
            }

            if (User.IsInRole("Supervisor"))
            {
                UserEntity user_logged = await _context.Users
                                                       .Include(u => u.Clinic)
                                                       .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if (user_logged.Clinic != null)
                {
                    return View(await _context.Workdays_Clients.Include(wc => wc.Note)

                                                               .Include(wc => wc.Facilitator)
                                                               .ThenInclude(f => f.Clinic)

                                                               .Include(wc => wc.Client)

                                                               .Include(wc => wc.Workday)
                                                               .ThenInclude(w => w.Week)

                                                               .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                                               .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                                          && (wc.GroupNote.Status == NoteStatus.Pending
                                                                          || wc.GroupNote2.Status == NoteStatus.Pending)
                                                                          && wc.Workday.Service == ServiceType.Group))
                                                               .ToListAsync());
                }
            }

            return View();
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> ApprovedNotes(int id = 0)
        {
            return View(await _context.Workdays_Clients

                                      .Include(wc => wc.Note)
                                                       
                                      .Include(wc => wc.Facilitator)
                                                       
                                      .Include(wc => wc.Client)
                                                       
                                      .Include(wc => wc.Workday)                                                       
                                      .ThenInclude(w => w.Week)
                                                       
                                      .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                 && (wc.Note.Status == NoteStatus.Approved || wc.NoteP.Status == NoteStatus.Approved)
                                                 && wc.Workday.Service == ServiceType.PSR))
                                                       
                                      .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> ApprovedIndNotes(int id = 0)
        {
            return View(await _context.Workdays_Clients

                                      .Include(wc => wc.IndividualNote)
                                                       
                                      .Include(wc => wc.Facilitator)
                                                       
                                      .Include(wc => wc.Client)
                                                       
                                      .Include(wc => wc.Workday)                                                       
                                      .ThenInclude(w => w.Week)
                                                       
                                      .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name           
                                                 && wc.IndividualNote.Status == NoteStatus.Approved
                                                 && wc.Workday.Service == ServiceType.Individual))
                                      .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> ApprovedGroupNotes(int id = 0)
        {
            return View(await _context.Workdays_Clients

                                      .Include(wc => wc.GroupNote)

                                      .Include(wc => wc.Facilitator)

                                      .Include(wc => wc.Client)

                                      .Include(wc => wc.Workday)
                                      .ThenInclude(w => w.Week)

                                      .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                 && (wc.GroupNote.Status == NoteStatus.Approved
                                                    || wc.GroupNote2.Status == NoteStatus.Approved)
                                                 && wc.Workday.Service == ServiceType.Group))
                                      .ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApprovedNotesClinic(string dateInterval = "", int idFacilitator = 0, int idClient = 0)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            IQueryable<Workday_Client> query = null;

            if (dateInterval == string.Empty && idFacilitator == 0 && idClient == 0)
            {
                query = _context.Workdays_Clients

                                .Include(wc => wc.Note)

                                .Include(wc => wc.IndividualNote)

                                .Include(wc => wc.GroupNote)

                                .Include(wc => wc.NoteP)

                                .Include(wc => wc.Facilitator)

                                .Include(wc => wc.Client)

                                .Include(wc => wc.Workday)

                                .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                            && (wc.Note.Status == NoteStatus.Approved || wc.IndividualNote.Status == NoteStatus.Approved
                                                                                    || wc.GroupNote.Status == NoteStatus.Approved
                                                                                    || wc.NoteP.Status == NoteStatus.Approved)
                                            && wc.Workday.Date >= DateTime.Now.AddMonths(-1)));

                return View(new ApprovedNotesClinicViewModel
                                {
                                    DateIterval = $"{DateTime.Now.AddMonths(-1).ToShortDateString()} - {DateTime.Now.AddDays(6).ToShortDateString()}",
                                    IdFacilitator = 0,
                                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                                    IdClient = 0,
                                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                                    WorkDaysClients = query.ToList()
                                }
                           );                
            }

            query = _context.Workdays_Clients

                            .Include(wc => wc.Note)

                            .Include(wc => wc.IndividualNote)

                            .Include(wc => wc.GroupNote)

                            .Include(wc => wc.NoteP)

                            .Include(wc => wc.Facilitator)

                            .Include(wc => wc.Client)

                            .Include(wc => wc.Workday)

                            .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                        && (wc.Note.Status == NoteStatus.Approved || wc.IndividualNote.Status == NoteStatus.Approved
                                                                                  || wc.GroupNote.Status == NoteStatus.Approved
                                                                                  || wc.NoteP.Status == NoteStatus.Approved)));

            if (dateInterval != string.Empty)
            {
                string[] date = dateInterval.Split(" - ");
                query = query.Where(wc => (wc.Workday.Date >= Convert.ToDateTime(date[0]) && wc.Workday.Date <= Convert.ToDateTime(date[1])));
            }

            if (idFacilitator != 0)
                query = query.Where(wc => wc.Facilitator.Id == idFacilitator);

            if (idClient != 0)
                query = query.Where(wc => wc.Client.Id == idClient);

            try
            {
                ApprovedNotesClinicViewModel model = new ApprovedNotesClinicViewModel
                {
                    DateIterval = dateInterval,
                    IdFacilitator = idFacilitator,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = idClient,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    WorkDaysClients = query.ToList()
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ApprovedNotesClinic));
            }         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult ApprovedNotesClinic(ApprovedNotesClinicViewModel model)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ApprovedNotesClinic), new { dateInterval = model.DateIterval, idFacilitator = model.IdFacilitator, idClient = model.IdClient });
            }

            return View(model);
        }

        [Authorize(Roles = "Supervisor, CaseManager")]
        public IActionResult AddMessageEntity(int id = 0, int origin = 0)
        {
            if (id == 0)
            {
                return View(new MessageViewModel());
            }
            else
            {
                MessageViewModel model = new MessageViewModel()
                {
                    IdWorkdayClient = id,
                    Origin = origin
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> AddMessageEntity(MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                MessageEntity model = await _converterHelper.ToMessageEntity(messageViewModel, true);
                UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
                                                             .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                model.From = user_logged.UserName;
                model.To = model.Workday_Client.Facilitator.LinkedUser;
                _context.Add(model);
                await _context.SaveChangesAsync();                
            }
            if(messageViewModel.Origin == 3)
                return RedirectToAction("PendingNotes");
            if (messageViewModel.Origin == 4)
                return RedirectToAction("NotesWithReview");
            if (messageViewModel.Origin == 5)
                return RedirectToAction("PendingIndNotes");
            if (messageViewModel.Origin == 6)
                return RedirectToAction("IndNotesWithReview");
            if (messageViewModel.Origin == 7)
                return RedirectToAction("PendingGroupNotes");
            if (messageViewModel.Origin == 8)
                return RedirectToAction("GroupNotesWithReview");
            if (messageViewModel.Origin == 1)
                return RedirectToAction("IndNotesSupervision");

            return RedirectToAction("NotesSupervision");
        }

        [Authorize(Roles = "Facilitator, Supervisor")]
        public async Task<IActionResult> NotesWithReview(int id = 0)
        {
            if (User.IsInRole("Facilitator"))
            {
                return View(await _context.Workdays_Clients.Include(wc => wc.Note)

                                                           .Include(wc => wc.Facilitator)

                                                           .Include(wc => wc.Client)

                                                           .Include(wc => wc.Workday)
                                                           .ThenInclude(w => w.Week)

                                                           .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                                           .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                     && (wc.Note.Status == NoteStatus.Pending || wc.NoteP.Status == NoteStatus.Pending)
                                                                     && wc.Messages.Count() > 0
                                                                     && wc.Workday.Service == ServiceType.PSR))
                                                           .ToListAsync());
            }

            if (User.IsInRole("Supervisor"))
            {
                UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
                                                             .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if (user_logged.Clinic != null)
                {
                    return View(await _context.Workdays_Clients.Include(wc => wc.Note)

                                                               .Include(wc => wc.Facilitator)
                                                               .ThenInclude(f => f.Clinic)

                                                               .Include(wc => wc.Client)

                                                               .Include(wc => wc.Workday)
                                                               .ThenInclude(w => w.Week)

                                                               .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                                               .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                                         && (wc.Note.Status == NoteStatus.Pending || wc.NoteP.Status == NoteStatus.Pending)
                                                                         && wc.Messages.Count() > 0
                                                                         && wc.Workday.Service == ServiceType.PSR))
                                                               .ToListAsync());
                }
            }

            return View();
        }

        [Authorize(Roles = "Facilitator, Supervisor")]
        public async Task<IActionResult> IndNotesWithReview(int id = 0)
        {
            if (User.IsInRole("Facilitator"))
            {
                return View(await _context.Workdays_Clients
                                          .Include(wc => wc.IndividualNote)

                                          .Include(wc => wc.Facilitator)

                                          .Include(wc => wc.Client)

                                          .Include(wc => wc.Workday)
                                          .ThenInclude(w => w.Week)

                                          .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                          .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                     && wc.IndividualNote.Status == NoteStatus.Pending
                                                     && wc.Messages.Count() > 0
                                                     && wc.Workday.Service == ServiceType.Individual))
                                          .ToListAsync());
            }

            if (User.IsInRole("Supervisor"))
            {
                UserEntity user_logged = await _context.Users
                                                       .Include(u => u.Clinic)
                                                       .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (user_logged.Clinic != null)
                {
                    return View(await _context.Workdays_Clients
                                              .Include(wc => wc.IndividualNote)

                                              .Include(wc => wc.Facilitator)
                                              .ThenInclude(f => f.Clinic)

                                              .Include(wc => wc.Client)

                                              .Include(wc => wc.Workday)
                                              .ThenInclude(w => w.Week)

                                              .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                              .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                         && wc.IndividualNote.Status == NoteStatus.Pending
                                                         && wc.Messages.Count() > 0
                                                         && wc.Workday.Service == ServiceType.Individual))
                                              .ToListAsync());
                }
            }

            return View();
        }

        [Authorize(Roles = "Facilitator, Supervisor")]
        public async Task<IActionResult> GroupNotesWithReview(int id = 0)
        {
            if (User.IsInRole("Facilitator"))
            {
                return View(await _context.Workdays_Clients

                                          .Include(wc => wc.GroupNote)

                                          .Include(wc => wc.Facilitator)

                                          .Include(wc => wc.Client)

                                          .Include(wc => wc.Workday)
                                          .ThenInclude(w => w.Week)

                                          .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                          .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                     && (wc.GroupNote.Status == NoteStatus.Pending
                                                     || wc.GroupNote2.Status == NoteStatus.Pending)
                                                     && wc.Messages.Count() > 0
                                                     && wc.Workday.Service == ServiceType.Group))
                                          .ToListAsync());
            }

            if (User.IsInRole("Supervisor"))
            {
                UserEntity user_logged = await _context.Users
                                                       .Include(u => u.Clinic)
                                                       .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (user_logged.Clinic != null)
                {
                    return View(await _context.Workdays_Clients
                                              .Include(wc => wc.IndividualNote)

                                              .Include(wc => wc.Facilitator)
                                              .ThenInclude(f => f.Clinic)

                                              .Include(wc => wc.Client)

                                              .Include(wc => wc.Workday)
                                              .ThenInclude(w => w.Week)

                                              .Include(wc => wc.Messages.Where(m => m.Notification == false))

                                              .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                         && (wc.GroupNote.Status == NoteStatus.Pending
                                                            || wc.GroupNote2.Status == NoteStatus.Pending)
                                                         && wc.Messages.Count() > 0
                                                         && wc.Workday.Service == ServiceType.Group))
                                              .ToListAsync());
                }
            }

            return View();
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> NotNotesSummary()
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            List<FacilitatorEntity> facilitators = await _context.Facilitators
                                                                 .Where(f => f.Clinic.Id == user_logged.Clinic.Id) 
                                                                 .ToListAsync();
            List<NotesSummary> notStarted = new List<NotesSummary>();
            foreach (FacilitatorEntity item in facilitators)
            {
                int psr_cant = await _context.Workdays_Clients
                                             .CountAsync(wc => (wc.Facilitator.Id == item.Id 
                                                             && (wc.Note == null && wc.NoteP == null) && wc.Present == true
                                                             && wc.Workday.Service == ServiceType.PSR));

                int ind_cant = await _context.Workdays_Clients
                                             .CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                             && wc.IndividualNote == null && wc.Present == true
                                                             && wc.Workday.Service == ServiceType.Individual));

                int group_cant = await _context.Workdays_Clients
                                               .CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                               && wc.GroupNote == null && wc.Present == true
                                                               && wc.Workday.Service == ServiceType.Group));

                notStarted.Add(new NotesSummary {FacilitatorName = item.Name, PSRNotStarted = psr_cant, IndNotStarted = ind_cant, GroupNotStarted = group_cant });
            }
            
            return View(notStarted);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditingSummary()
        {
            UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
                                                         .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            List<FacilitatorEntity> facilitators = await _context.Facilitators
                                                                 .Where(f => f.Clinic.Id == user_logged.Clinic.Id)
                                                                 .ToListAsync();
            
            List<NotesSummary> editing = new List<NotesSummary>();
            foreach (FacilitatorEntity item in facilitators)
            {
                int psr_cant = await _context.Workdays_Clients
                                             .CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                             && (wc.Note.Status == NoteStatus.Edition || wc.NoteP.Status == NoteStatus.Edition)
                                                             && wc.Workday.Service == ServiceType.PSR));

                int ind_cant = await _context.Workdays_Clients
                                             .CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                             && wc.IndividualNote.Status == NoteStatus.Edition
                                                             && wc.Workday.Service == ServiceType.Individual));

                int group_cant = await _context.Workdays_Clients
                                               .CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                             && wc.Note.Status == NoteStatus.Edition
                                                             && wc.Workday.Service == ServiceType.Group));

                editing.Add(new NotesSummary { FacilitatorName = item.Name, PSREditing = psr_cant, IndEditing = ind_cant, GroupEditing = group_cant });
            }

            return View(editing);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PendingSummary()
        {
            UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
                                                         .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            List<FacilitatorEntity> facilitators = await _context.Facilitators
                                                                 .Where(f => f.Clinic.Id == user_logged.Clinic.Id)
                                                                 .ToListAsync();
            
            List<NotesSummary> pending = new List<NotesSummary>();
            foreach (FacilitatorEntity item in facilitators)
            {
                int psr_cant = await _context.Workdays_Clients
                                             .CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                             && (wc.Note.Status == NoteStatus.Pending || wc.NoteP.Status == NoteStatus.Pending) 
                                                             && wc.Workday.Service == ServiceType.PSR));

                int ind_cant = await _context.Workdays_Clients
                                             .CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                             && wc.IndividualNote.Status == NoteStatus.Pending
                                                             && wc.Workday.Service == ServiceType.Individual));

                int group_cant = await _context.Workdays_Clients
                                               .CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                               && wc.Note.Status == NoteStatus.Pending
                                                               && wc.Workday.Service == ServiceType.Group));

                pending.Add(new NotesSummary { FacilitatorName = item.Name, PSRPending = psr_cant, IndPending = ind_cant, GroupPending = group_cant });
            }

            return View(pending);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ReviewSummary()
        {
            UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
                                                         .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            List<FacilitatorEntity> facilitators = await _context.Facilitators
                                                                 .Where(f => f.Clinic.Id == user_logged.Clinic.Id)
                                                                 .ToListAsync();
           
            List<NotesSummary> review = new List<NotesSummary>();
            foreach (FacilitatorEntity item in facilitators)
            {
                int psr_cant = await _context.Workdays_Clients.CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                                              && (wc.Note.Status == NoteStatus.Pending || wc.NoteP.Status == NoteStatus.Pending) 
                                                                              && wc.Messages.Where(m => m.Notification == false).Count() > 0
                                                                              && wc.Workday.Service == ServiceType.PSR));

                int ind_cant = await _context.Workdays_Clients.CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                                              && wc.IndividualNote.Status == NoteStatus.Pending
                                                                              && wc.Messages.Where(m => m.Notification == false).Count() > 0
                                                                              && wc.Workday.Service == ServiceType.Individual));

                int group_cant = await _context.Workdays_Clients.CountAsync(wc => (wc.Facilitator.Id == item.Id
                                                                                && wc.Note.Status == NoteStatus.Pending
                                                                                && wc.Messages.Where(m => m.Notification == false).Count() > 0
                                                                                && wc.Workday.Service == ServiceType.Group));

                review.Add(new NotesSummary { FacilitatorName = item.Name, PSRReview = psr_cant, IndReview = ind_cant, GroupReview = group_cant});
            }

            return View(review);
        }

        public string RandomGenerator()
        {
            Random random = new Random();
            int value = random.Next(1, 5);
            string text = (value == 1) ? "detected. " :
                           (value == 2) ? "observed. " :
                            (value == 3) ? "reached. " :
                             (value == 4) ? "attained. " :
                              (value == 5) ? "done. " : string.Empty;
            return text;
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> NotPresentNotes(int id = 0)
        {
            return View(await _context.Workdays_Clients.Include(wc => wc.Note)
                                                       .Include(wc => wc.Facilitator)
                                                       .Include(wc => wc.Client)
                                                       .Include(wc => wc.Workday)
                                                       .ThenInclude(w => w.Week)
                                                       .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                                  && wc.Present == false
                                                                  && wc.Workday.Service == ServiceType.PSR))
                                                       .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> NotPresentIndNotes(int id = 0)
        {
            return View(await _context.Workdays_Clients

                                      .Include(wc => wc.IndividualNote)

                                      .Include(wc => wc.Facilitator)

                                      .Include(wc => wc.Client)

                                      .Include(wc => wc.Workday)
                                      .ThenInclude(w => w.Week)

                                      .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                 && wc.Present == false
                                                 && wc.Workday.Service == ServiceType.Individual))
                                      .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> NotPresentGroupNotes(int id = 0)
        {
            return View(await _context.Workdays_Clients

                                      .Include(wc => wc.GroupNote)

                                      .Include(wc => wc.Facilitator)

                                      .Include(wc => wc.Client)

                                      .Include(wc => wc.Workday)
                                      .ThenInclude(w => w.Week)

                                      .Where(wc => (wc.Facilitator.LinkedUser == User.Identity.Name
                                                 && wc.Present == false
                                                 && wc.Workday.Service == ServiceType.Group))
                                      .ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> NotPresentNotesClinic(string dateInterval = "", int idFacilitator = 0, int idClient = 0)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            IQueryable<Workday_Client> query = null;

            if (dateInterval == string.Empty && idFacilitator == 0 && idClient == 0)
            {
                query = _context.Workdays_Clients

                                .Include(wc => wc.Facilitator)

                                .Include(wc => wc.Client)

                                .Include(wc => wc.Workday)

                                .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                           && wc.Present == false
                                           && wc.Workday.Date >= DateTime.Now.AddMonths(-1)));

                return View(new NotPresentNotesClinicViewModel
                {
                    DateIterval = $"{DateTime.Now.AddMonths(-1).ToShortDateString()} - {DateTime.Now.AddDays(6).ToShortDateString()}",
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    WorkDaysClients = query.ToList()
                }
                           );
            }

            query = _context.Workdays_Clients

                            .Include(wc => wc.Facilitator)

                            .Include(wc => wc.Client)

                            .Include(wc => wc.Workday)

                            .Where(wc => (wc.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                       && wc.Present == false));

            if (dateInterval != string.Empty)
            {
                string[] date = dateInterval.Split(" - ");
                query = query.Where(wc => (wc.Workday.Date >= Convert.ToDateTime(date[0]) && wc.Workday.Date <= Convert.ToDateTime(date[1])));
            }

            if (idFacilitator != 0)
                query = query.Where(wc => wc.Facilitator.Id == idFacilitator);

            if (idClient != 0)
                query = query.Where(wc => wc.Client.Id == idClient);

            try
            {
                NotPresentNotesClinicViewModel model = new NotPresentNotesClinicViewModel
                {
                    DateIterval = dateInterval,
                    IdFacilitator = idFacilitator,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = idClient,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    WorkDaysClients = query.ToList()
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(NotPresentNotesClinic));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult NotPresentNotesClinic(NotPresentNotesClinicViewModel model)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(NotPresentNotesClinic), new { dateInterval = model.DateIterval, idFacilitator = model.IdFacilitator, idClient = model.IdClient });
            }

            return View(model);
        }

        [Authorize(Roles = "Facilitator, Manager")]
        public IActionResult PrintAbsenceNote(int id)
        {
            Workday_Client workdayClient = _context.Workdays_Clients
                                                          
                                                   .Include(wc => wc.Facilitator)
                                                   .ThenInclude(c => c.Clinic)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.Clinic)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.Group)

                                                   .Include(wc => wc.Workday)

                                                   .FirstOrDefault(wc => wc.Id == id);
            if (workdayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (workdayClient.Client.Clinic.Name == "DAVILA")
            {
                Stream stream = _reportHelper.DavilaAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Client.Clinic.Name == "LARKIN BEHAVIOR")
            {
                Stream stream = _reportHelper.LarkinAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Client.Clinic.Name == "SOL & VIDA")
            {
                Stream stream = _reportHelper.SolAndVidaAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Client.Clinic.Name == "DREAMS MENTAL HEALTH INC")
            {
                Stream stream = _reportHelper.DreamsMentalHealthAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Client.Clinic.Name == "ADVANCED GROUP MEDICAL CENTER")
            {
                Stream stream = _reportHelper.AdvancedGroupMCAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Client.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
            {
                Stream stream = _reportHelper.FloridaSocialHSAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Client.Clinic.Name == "ATLANTIC GROUP MEDICAL CENTER")
            {
                Stream stream = _reportHelper.AtlanticGroupMCAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Client.Clinic.Name == "DEMO CLINIC SCHEMA 1")
            {
                Stream stream = _reportHelper.DemoClinic1AbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Client.Clinic.Name == "DEMO CLINIC SCHEMA 2")
            {
                Stream stream = _reportHelper.DemoClinic2AbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Client.Clinic.Name == "COMMUNITY HEALTH THERAPY CENTER")
            {
                Stream stream = _reportHelper.CommunityHTCAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }

            return null;
        }

        [Authorize(Roles = "Facilitator, Manager")]
        public IActionResult PrintAbsenceIndNote(int id)
        {
            Workday_Client workdayClient = _context.Workdays_Clients

                                                   .Include(wc => wc.Facilitator)
                                                   .ThenInclude(c => c.Clinic)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.Clinic)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.Group)

                                                   .Include(wc => wc.Workday)

                                                   .FirstOrDefault(wc => wc.Id == id);
            if (workdayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (workdayClient.Facilitator.Clinic.Name == "DAVILA")
            {
                Stream stream = _reportHelper.DavilaAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "LARKIN BEHAVIOR")
            {
                Stream stream = _reportHelper.LarkinAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "SOL & VIDA")
            {
                Stream stream = _reportHelper.SolAndVidaAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "DREAMS MENTAL HEALTH INC")
            {
                Stream stream = _reportHelper.DreamsMentalHealthAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "ADVANCED GROUP MEDICAL CENTER")
            {
                Stream stream = _reportHelper.AdvancedGroupMCAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
            {
                Stream stream = _reportHelper.FloridaSocialHSAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "ATLANTIC GROUP MEDICAL CENTER")
            {
                Stream stream = _reportHelper.AtlanticGroupMCAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "DEMO CLINIC SCHEMA 1")
            {
                Stream stream = _reportHelper.DemoClinic1AbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "DEMO CLINIC SCHEMA 2")
            {
                Stream stream = _reportHelper.DemoClinic2AbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "COMMUNITY HEALTH THERAPY CENTER")
            {
                Stream stream = _reportHelper.CommunityHTCAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }

            return null;
        }

        [Authorize(Roles = "Facilitator, Manager")]
        public IActionResult PrintAbsenceGroupNote(int id)
        {
            Workday_Client workdayClient = _context.Workdays_Clients

                                                   .Include(wc => wc.Facilitator)
                                                   .ThenInclude(c => c.Clinic)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.Clinic)

                                                   .Include(wc => wc.Client)
                                                   .ThenInclude(c => c.Group)

                                                   .Include(wc => wc.Workday)

                                                   .FirstOrDefault(wc => wc.Id == id);
            if (workdayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (workdayClient.Facilitator.Clinic.Name == "DAVILA")
            {
                Stream stream = _reportHelper.DavilaAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "LARKIN BEHAVIOR")
            {
                Stream stream = _reportHelper.LarkinAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "SOL & VIDA")
            {
                Stream stream = _reportHelper.SolAndVidaAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "DREAMS MENTAL HEALTH INC")
            {
                Stream stream = _reportHelper.DreamsMentalHealthAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "ADVANCED GROUP MEDICAL CENTER")
            {
                Stream stream = _reportHelper.AdvancedGroupMCAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "FLORIDA SOCIAL HEALTH SOLUTIONS")
            {
                Stream stream = _reportHelper.FloridaSocialHSAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "ATLANTIC GROUP MEDICAL CENTER")
            {
                Stream stream = _reportHelper.AtlanticGroupMCAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "DEMO CLINIC SCHEMA 1")
            {
                Stream stream = _reportHelper.DemoClinic1AbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "DEMO CLINIC SCHEMA 2")
            {
                Stream stream = _reportHelper.DemoClinic2AbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
            if (workdayClient.Facilitator.Clinic.Name == "COMMUNITY HEALTH THERAPY CENTER")
            {
                Stream stream = _reportHelper.CommunityHTCAbsenceNoteReport(workdayClient);
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }

            return null;
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> PrintIndividualSign(int id, int idWeek, ServiceType service)
        {
            List<Workday_Client> workdayClientList = await _context.Workdays_Clients

                                                                   .Include(wc => wc.Facilitator)
                                                                    .ThenInclude(c => c.Clinic)

                                                                   .Include(wc => wc.Client)                                                  

                                                                   .Include(wc => wc.Workday)

                                                                   .Where(wc => (wc.Client.Id == id && wc.Workday.Week.Id == idWeek 
                                                                              && wc.Workday.Service == service)).ToListAsync();
            if (workdayClientList.Count() == 0)
            {
                return RedirectToAction("Home/Error404");
            }

            Stream stream = _reportHelper.PrintIndividualSign(workdayClientList);            
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);            
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> PrintIndividualSignGroup(int id, int idWeek)
        {
            List<Workday_Client> workdayClientList = await _context.Workdays_Clients

                                                                   .Include(wc => wc.Facilitator)
                                                                    .ThenInclude(c => c.Clinic)

                                                                   .Include(wc => wc.Client)

                                                                   .Include(wc => wc.Workday)

                                                                   .Where(wc => (wc.Client.Id == id && wc.Workday.Week.Id == idWeek && wc.Workday.Service == ServiceType.Group)).ToListAsync();
            if (workdayClientList.Count() == 0)
            {
                return RedirectToAction("Home/Error404");
            }

            Stream stream = _reportHelper.PrintIndividualSignGroup(workdayClientList);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillingReport(string dateInterval = "")    
        {                     
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (dateInterval != string.Empty)
            {
                string[] date = dateInterval.Split(" - ");

                IQueryable<WeekEntity> query = _context.Weeks

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(g => g.Facilitator)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.Note)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.NoteP)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.GroupNote)
                                                        .ThenInclude(wc => wc.GroupNotes_Activities)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.GroupNote2)
                                                        .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.Client)
                                                        .ThenInclude(c => c.Clients_Diagnostics)
                                                        .ThenInclude(cd => cd.Diagnostic)

                                                        .Include(w => w.Clinic)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(d => d.Schedule)
                                                        .ThenInclude(d => d.SubSchedules)

                                                        .Where(w => (w.Clinic.Id == user_logged.Clinic.Id && w.InitDate >= Convert.ToDateTime(date[0]) && w.FinalDate <= Convert.ToDateTime(date[1])));

                try
                {
                    BillingReportViewModel model = new BillingReportViewModel
                    {
                        DateIterval = dateInterval,
                        IdFacilitator = 0,
                        Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                        IdClient = 0,
                        Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                        Weeks = query.ToList()
                    };

                    return View(model);
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(BillingReport));
                }                
            }
            else
            {
                IQueryable<WeekEntity> query = _context.Weeks

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(g => g.Facilitator)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.Note)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.NoteP)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.GroupNote)
                                                        .ThenInclude(wc => wc.GroupNotes_Activities)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.GroupNote2)
                                                        .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(wc => wc.Client)
                                                        .ThenInclude(c => c.Clients_Diagnostics)
                                                        .ThenInclude(cd => cd.Diagnostic)

                                                        .Include(w => w.Clinic)

                                                        .Include(w => w.Days)
                                                        .ThenInclude(d => d.Workdays_Clients)
                                                        .ThenInclude(d => d.Schedule)
                                                        .ThenInclude(d => d.SubSchedules)

                                                        .Where(w => (w.Clinic.Id == user_logged.Clinic.Id && w.InitDate >= DateTime.Now.AddMonths(-3) && w.FinalDate <= DateTime.Now.AddDays(6)));                                              

                BillingReportViewModel model = new BillingReportViewModel
                {
                    DateIterval = $"{DateTime.Now.AddMonths(-3).ToShortDateString()} - {DateTime.Now.AddDays(6).ToShortDateString()}",
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    Weeks = query.ToList()
                };

                return View(model);
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult BillingReport(BillingReportViewModel model)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(BillingReport), new { dateInterval = model.DateIterval });
            }

            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillingReport1(int idWeek = 0)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (idWeek != 0)
            {
                IQueryable<WeekEntity> query = _context.Weeks

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(g => g.Facilitator)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Note)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.NoteP)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.GroupNote)
                                                       .ThenInclude(wc => wc.GroupNotes_Activities)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.GroupNote2)
                                                       .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Client)
                                                       .ThenInclude(c => c.Clients_Diagnostics)
                                                       .ThenInclude(cd => cd.Diagnostic)

                                                       .Include(w => w.Clinic)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(d => d.Schedule)
                                                       .ThenInclude(d => d.SubSchedules)

                                                       .Where(w => (w.Clinic.Id == user_logged.Clinic.Id && w.Id == idWeek));

                try
                {
                    BillingReport1ViewModel model = new BillingReport1ViewModel
                    {
                        IdFacilitator = 0,
                        Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                        IdClient = 0,
                        Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                        Weeks = query.ToList(),
                        IdWeek = idWeek,
                        WeeksListName = _combosHelper.GetComboWeeksNameByClinic(user_logged.Clinic.Id)
                    };

                    return View(model);
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(BillingReport));
                }
            }
            else
            {
                int max = _context.Weeks
                                  .Where(m => m.Clinic.Id == user_logged.Clinic.Id)
                                  .Max(m => m.Id);

                IQueryable<WeekEntity> query = _context.Weeks

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(g => g.Facilitator)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Note)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.NoteP)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.GroupNote)
                                                       .ThenInclude(wc => wc.GroupNotes_Activities)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.GroupNote2)
                                                       .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Client)
                                                       .ThenInclude(c => c.Clients_Diagnostics)
                                                       .ThenInclude(cd => cd.Diagnostic)

                                                       .Include(w => w.Clinic)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(d => d.Schedule)
                                                       .ThenInclude(d => d.SubSchedules)

                                                       .Where(w => (w.Clinic.Id == user_logged.Clinic.Id && w.Id == max));
                
                BillingReport1ViewModel model = new BillingReport1ViewModel
                {
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    Weeks = query.ToList(),
                    IdWeek = max,
                    WeeksListName = _combosHelper.GetComboWeeksNameByClinic(user_logged.Clinic.Id)
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult BillingReport1(BillingReport1ViewModel model)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(BillingReport1), new { idWeek = model.IdWeek });
            }

            return View(model);
        }


        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillingWeek(int id, int billed = 0)
        {
            UserEntity user_logged = await _context.Users

                                                   .Include(u => u.Clinic)

                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
            {
                return RedirectToAction("Home/Error404");
            }

            WeekEntity week = _context.Weeks
                                      .FirstOrDefault(w => w.Id == id);

            if (billed == 0)
            {
                List<ClientEntity> clients = await _context.Clients
                                                           
                                                           .Include(c => c.Clients_Diagnostics)
                                                           .ThenInclude(cd => cd.Diagnostic)

                                                           .Include(c => c.Clients_HealthInsurances)
                                                           .ThenInclude(c => c.HealthInsurance)

                                                           .Include(wc => wc.MTPs)
                                                           .Include(wc => wc.Bio)

                                                           .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                              && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                          && wc.BilledDate == null
                                                                                          && wc.Hold == false
                                                                                          && wc.Workday.Week.Id == id).Count() > 0)
                                                                ||(wc.MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate 
                                                                                   && n.AdmissionDateMTP <= week.FinalDate
                                                                                   && n.BilledDate == null).Count() > 0)
                                                                ||(wc.Bio.DateBio >= week.InitDate && wc.Bio.DateBio <= week.FinalDate && wc.Bio.BilledDate == null))))

                                                           .ToListAsync();
                
                ViewData["idWeek"] = id;
                ViewData["range"] = week.InitDate.ToLongDateString() + " - " + week.FinalDate.ToLongDateString();
                
                int cantUnit = 0;
                int money = 0;
                int temp = 0;
                int document = 0;

                for (int i = 0; i < clients.Count(); i++)
                {
                    if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate && n.AdmissionDateMTP <= week.FinalDate && n.BilledDate == null).Count() == 0)
                    {
                        clients[i].MTPs = null;
                    }
                    else
                    {
                        temp = clients[i].MTPs.Sum(n => n.Units);
                        cantUnit += temp;
                        money += temp * 15;
                        temp = 0;
                        document++;
                    }

                    if (clients[i].Bio != null)
                    {
                        if (clients[i].Bio.DateBio >= week.InitDate && clients[i].Bio.DateBio <= week.FinalDate && clients[i].Bio.BilledDate == null)
                        {
                            temp = clients[i].Bio.Units;
                            cantUnit += temp;
                            money += temp * 15;
                            temp = 0;
                            document++;
                        }
                        else
                        {
                            clients[i].Bio = null;
                        }
                    }                    

                    clients[i].Workdays_Clients = _context.Workdays_Clients
                                                          .Include(wc => wc.Note)

                                                          .Include(wc => wc.NoteP)
                                                          
                                                          .Include(wc => wc.IndividualNote)
                                                             
                                                          .Include(wc => wc.GroupNote)

                                                          .Include(wc => wc.GroupNote2)
                                                          .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Week)

                                                          .Include(wc => wc.Facilitator)

                                                          .Include(wc => wc.Schedule)
                                                          .ThenInclude(wc => wc.SubSchedules)
                                                              
                                                          .Where(wc => wc.Present == true
                                                                    && wc.BilledDate == null
                                                                    && wc.Hold == false
                                                                    && wc.Workday.Week.Id == id
                                                                    && wc.Client.Id == clients[i].Id).ToList();
                   
                       

                }

                foreach (var client in clients)
                {
                    foreach (var item in client.Workdays_Clients.Where(n => n.BilledDate == null))
                    {
                        if (item.Note != null)
                        {
                            cantUnit += 16;
                            money += 16 * 9;
                            document++;
                        }
                        else
                        {
                            if (item.NoteP != null)
                            {
                                cantUnit += item.NoteP.RealUnits;
                                money += item.NoteP.RealUnits * 9;
                                document++;
                            }
                            else
                            {
                                if (item.IndividualNote != null)
                                {
                                    cantUnit += 4;
                                    money += 4 * 12;
                                    document++;
                                }
                                else
                                {
                                    if (item.Workday.Service == ServiceType.Group)
                                    {
                                        if (item.GroupNote2 != null)
                                        {
                                            cantUnit += item.GroupNote2.GroupNotes2_Activities.Count() * 4;
                                            money += item.GroupNote2.GroupNotes2_Activities.Count() * 4 * 7;
                                            document++;
                                        }
                                        else
                                        {
                                            cantUnit += item.Schedule.SubSchedules.Count() * 4;
                                            money += item.Schedule.SubSchedules.Count() * 4 * 7;
                                            document++;
                                        }
                                    }
                                    else
                                    {
                                        cantUnit += 16;
                                        money += 16 * 9;
                                        document++;
                                    }

                                }
                            }
                        }
                    }
                }
               
                ViewData["Units"] = cantUnit;
                ViewData["Money"] = money;
                ViewData["Billed"] = billed;
                ViewData["Document"] = document++;

                return View(clients);
            }
            else
            {
                List<ClientEntity> clients = await _context.Clients
                                                           
                                                           .Include(c => c.Clients_Diagnostics)
                                                           .ThenInclude(cd => cd.Diagnostic)

                                                           .Include(c => c.Clients_HealthInsurances)
                                                           .ThenInclude(c => c.HealthInsurance)

                                                           .Include(wc => wc.MTPs)
                                                           .Include(wc => wc.Bio)

                                                           .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                              && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                          && wc.BilledDate != null
                                                                                          && wc.Hold == false
                                                                                          && wc.Workday.Week.Id == id).Count() > 0)
                                                                || (wc.MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate
                                                                                    && n.AdmissionDateMTP <= week.FinalDate
                                                                                    && n.BilledDate != null).Count() > 0)
                                                                || (wc.Bio.DateBio >= week.InitDate && wc.Bio.DateBio <= week.FinalDate && wc.Bio.BilledDate != null))))

                                                           .ToListAsync();
               
                ViewData["idWeek"] = id;
                ViewData["range"] = week.InitDate.ToLongDateString() + " - " + week.FinalDate.ToLongDateString();
               
                int cantUnit = 0;
                int money = 0;
                int temp = 0;
                int document = 0;

                for (int i = 0; i < clients.Count(); i++)
                {
                    if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate && n.AdmissionDateMTP <= week.FinalDate && n.BilledDate != null).Count() == 0)
                    {
                        clients[i].MTPs = null;
                    }
                    else
                    {
                        temp = clients[i].MTPs.Sum(n => n.Units);
                        cantUnit += temp;
                        money += temp * 15;
                        temp = 0;
                        document++;
                    }

                    if (clients[i].Bio != null)
                    {
                        if (clients[i].Bio.DateBio >= week.InitDate && clients[i].Bio.DateBio <= week.FinalDate && clients[i].Bio.BilledDate != null)
                        {
                            temp = clients[i].Bio.Units;
                            cantUnit += temp;
                            money += temp * 15;
                            temp = 0;
                            document++;
                        }
                        else
                        {
                            clients[i].Bio = null;
                        }
                    }                    

                    clients[i].Workdays_Clients = _context.Workdays_Clients
                                                          .Include(wc => wc.Note)

                                                          .Include(wc => wc.NoteP)

                                                          .Include(wc => wc.IndividualNote)

                                                          .Include(wc => wc.GroupNote)

                                                          .Include(wc => wc.GroupNote2)
                                                          .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Week)

                                                          .Include(wc => wc.Facilitator)

                                                          .Include(wc => wc.Schedule)
                                                          .ThenInclude(wc => wc.SubSchedules)

                                                          .Where(wc => wc.Present == true
                                                                    && wc.BilledDate != null
                                                                    && wc.Hold == false
                                                                    && wc.Workday.Week.Id == id
                                                                    && wc.Client.Id == clients[i].Id).ToList();
                   
                }

                foreach (var client in clients)
                {
                    foreach (var item in client.Workdays_Clients.Where(n => n.BilledDate != null))
                    {
                        if (item.Note != null)
                        {
                            cantUnit += 16;
                            money += 16 * 9;
                            document++;
                        }
                        else
                        {
                            if (item.NoteP != null)
                            {
                                cantUnit += item.NoteP.RealUnits;
                                money += item.NoteP.RealUnits * 9;
                                document++;
                            }
                            else
                            {
                                if (item.IndividualNote != null)
                                {
                                    cantUnit += 4;
                                    money += 4 * 12;
                                    document++;
                                }
                                else
                                {
                                    if (item.Workday.Service == ServiceType.Group)
                                    {
                                        if (item.GroupNote2 != null)
                                        {
                                            cantUnit += item.GroupNote2.GroupNotes2_Activities.Count() * 4;
                                            money += item.GroupNote2.GroupNotes2_Activities.Count() * 4 * 7;
                                            document++;
                                        }
                                        else
                                        {
                                            cantUnit += item.Schedule.SubSchedules.Count() * 4;
                                            money += item.Schedule.SubSchedules.Count() * 4 * 7;
                                            document++;
                                        }
                                    }
                                    else
                                    {
                                        cantUnit += 16;
                                        money += 16 * 9;
                                        document++;
                                    }
                                }
                            }
                        }
                    }
                }
                
                ViewData["Units"] = cantUnit;
                ViewData["Money"] = money;
                ViewData["Billed"] = billed;
                ViewData["Document"] = document++;

                return View(clients);
            }
           
        }

        [Authorize(Roles = "Manager")]
        public IActionResult BillNote(int id, int week = 0, int abilled = 0)
        {
            BillViewModel model = new BillViewModel { Id = id, BilledDate = DateTime.Now };
            ViewData["week"] = week;
            ViewData["Billed"] = abilled;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillNote(BillViewModel model, int week = 0, int abilled = 0)
        {
            UserEntity user_logged = await _context.Users
                                                 .Include(u => u.Clinic)
                                                 .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            int idWeek = week;
            if (ModelState.IsValid)
            {
                Workday_Client workday_client;
                if (week == 0)
                {

                    workday_client = await _context.Workdays_Clients
                                                   .Include(wc => wc.Workday)
                                                   .ThenInclude(w => w.Week)
                                                   .ThenInclude(we => we.Clinic)

                                                   .Where(wc => wc.Id == model.Id)
                                                   .FirstOrDefaultAsync();

                    workday_client.BilledDate = model.BilledDate;
                    _context.Update(workday_client);
                    await _context.SaveChangesAsync();

                    idWeek = workday_client.Workday.Week.Id;
                }
                else
                {
                    workday_client = await _context.Workdays_Clients
                                                   .Include(wc => wc.Workday)
                                                   .ThenInclude(w => w.Week)
                                                   .ThenInclude(we => we.Clinic)
                                                   .Include(wc => wc.Client)
                                                   .Where(wc => wc.Client.Id == model.Id
                                                        && wc.Workday.Week.Id == week)
                                                   .FirstOrDefaultAsync();
                    List<Workday_Client> w_clients = new List<Workday_Client>();
                    if (abilled == 0)
                    {
                        w_clients = await _context.Workdays_Clients
                                                  .Where(w => w.Workday.Week.Id == workday_client.Workday.Week.Id
                                                        && w.Client.Id == workday_client.Client.Id
                                                        && w.BilledDate == null)
                                                  .ToListAsync();

                    }
                    else 
                    {
                        w_clients = await _context.Workdays_Clients
                                                  .Where(w => w.Workday.Week.Id == workday_client.Workday.Week.Id
                                                        && w.Client.Id == workday_client.Client.Id
                                                        && w.BilledDate != null)
                                                  .ToListAsync();

                    }
                    foreach (var item in w_clients)
                    {
                        item.BilledDate = model.BilledDate;
                        _context.Update(item);
                        await _context.SaveChangesAsync();
                    }
                    
                }

                List<ClientEntity> clients = new List<ClientEntity>();

                if (abilled == 0)
                {
                    clients = await _context.Clients
                                            
                                            .Include(c => c.Clients_Diagnostics)
                                            .ThenInclude(cd => cd.Diagnostic)

                                            .Include(c => c.Clients_HealthInsurances)
                                            .ThenInclude(c => c.HealthInsurance)

                                            .Include(wc => wc.MTPs)
                                            .Include(wc => wc.Bio)

                                            .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                     && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                 && wc.BilledDate == null
                                                                                 && wc.Hold == false
                                                                                 && wc.Workday.Week.Id == workday_client.Workday.Week.Id).Count() > 0)
                                                       || (wc.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate
                                                                           && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate
                                                                           && n.BilledDate == null).Count() > 0)
                                                       || (wc.Bio.DateBio >= workday_client.Workday.Week.InitDate 
                                                                           && wc.Bio.DateBio <= workday_client.Workday.Week.FinalDate 
                                                                           && wc.Bio.BilledDate == null))))
                                            .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate && n.BilledDate == null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }

                        if (clients[i].Bio != null)
                        {
                            if (clients[i].Bio.DateBio != null)
                            {
                                if (clients[i].Bio.DateBio >= workday_client.Workday.Week.InitDate && clients[i].Bio.DateBio <= workday_client.Workday.Week.FinalDate && clients[i].Bio.BilledDate == null)
                                {

                                }
                                else
                                {
                                    clients[i].Bio = null;
                                }
                            }
                        }

                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                        .Include(wc => wc.Note)

                                                        .Include(wc => wc.NoteP)

                                                        .Include(wc => wc.IndividualNote)

                                                        .Include(wc => wc.GroupNote)

                                                        .Include(wc => wc.GroupNote2)
                                                        .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                        .Include(wc => wc.Workday)
                                                        .ThenInclude(w => w.Week)

                                                        .Include(wc => wc.Facilitator)

                                                        .Include(wc => wc.Schedule)
                                                        .ThenInclude(wc => wc.SubSchedules)

                                                        .Where(wc => wc.Present == true
                                                                  && wc.BilledDate == null
                                                                  && wc.Hold == false
                                                                  && wc.Workday.Week.Id == workday_client.Workday.Week.Id
                                                                  && wc.Client.Id == clients[i].Id)
                                                        .ToList();
                    }

                }
                else
                {
                    clients = await _context.Clients
                                           
                                           .Include(c => c.Clients_Diagnostics)
                                           .ThenInclude(cd => cd.Diagnostic)

                                           .Include(c => c.Clients_HealthInsurances)
                                           .ThenInclude(c => c.HealthInsurance)

                                           .Include(wc => wc.MTPs)
                                           .Include(wc => wc.Bio)

                                           .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                    && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                && wc.BilledDate != null
                                                                                && wc.Hold == false
                                                                                && wc.Workday.Week.Id == workday_client.Workday.Week.Id).Count() > 0)
                                                      || (wc.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate
                                                                          && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate
                                                                          && n.BilledDate != null).Count() > 0)
                                                      || (wc.Bio.DateBio >= workday_client.Workday.Week.InitDate
                                                                           && wc.Bio.DateBio <= workday_client.Workday.Week.FinalDate
                                                                           && wc.Bio.BilledDate != null))))
                                           .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate && n.BilledDate != null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }

                        if (clients[i].Bio != null)
                        {
                            if (clients[i].Bio.DateBio != null)
                            {
                                if (clients[i].Bio.DateBio >= workday_client.Workday.Week.InitDate && clients[i].Bio.DateBio <= workday_client.Workday.Week.FinalDate && clients[i].Bio.BilledDate != null)
                                {

                                }
                                else
                                {
                                    clients[i].Bio = null;
                                }
                            }
                        }

                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                         .Include(wc => wc.Note)

                                                         .Include(wc => wc.NoteP)

                                                         .Include(wc => wc.IndividualNote)

                                                         .Include(wc => wc.GroupNote)

                                                         .Include(wc => wc.GroupNote2)
                                                         .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                         .Include(wc => wc.Workday)
                                                         .ThenInclude(w => w.Week)

                                                         .Include(wc => wc.Facilitator)

                                                         .Include(wc => wc.Schedule)
                                                         .ThenInclude(wc => wc.SubSchedules)

                                                         .Where(wc => wc.Present == true
                                                                   && wc.BilledDate != null
                                                                   && wc.Hold == false
                                                                   && wc.Workday.Week.Id == workday_client.Workday.Week.Id
                                                                   && wc.Client.Id == clients[i].Id)
                                                         .ToList();

                    }

                }

                ViewData["Billed"] = abilled;
                ViewData["idWeek"] = idWeek;

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingWeek", clients) });
            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "BillNote", model) });
        }

        [Authorize(Roles = "Manager")]
        public IActionResult PaymentReceived(int id, int week = 0)
        {
            PaymentReceivedViewModel model = new PaymentReceivedViewModel { Id = id, PaymentDate = DateTime.Now };
            ViewData["week"] = week;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PaymentReceived(PaymentReceivedViewModel model, int week = 0)
        {
            UserEntity user_logged = await _context.Users
                                                  .Include(u => u.Clinic)
                                                  .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                Workday_Client workday_client;
                List<ClientEntity> clients = new List<ClientEntity>();
                if (week == 0)
                {

                    workday_client = await _context.Workdays_Clients

                                                   .Include(wc => wc.Workday)
                                                   .ThenInclude(w => w.Week)
                                                   .ThenInclude(we => we.Clinic)

                                                   .Where(wc => wc.Id == model.Id)
                                                   .FirstOrDefaultAsync();

                    workday_client.PaymentDate = model.PaymentDate;
                    workday_client.DeniedBill = false;
                    _context.Update(workday_client);
                    await _context.SaveChangesAsync();

                    clients = await _context.Clients
                                            
                                            .Include(c => c.Clients_Diagnostics)
                                            .ThenInclude(cd => cd.Diagnostic)

                                            .Include(c => c.Clients_HealthInsurances)
                                            .ThenInclude(c => c.HealthInsurance)
                                            
                                            .Include(wc => wc.MTPs)
                                            .Include(wc => wc.Bio)

                                            .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                     && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                 && wc.BilledDate != null
                                                                                 && wc.Hold == false
                                                                                 && wc.Workday.Week.Id == workday_client.Workday.Week.Id).Count() > 0)
                                                       || (wc.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate
                                                                           && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate
                                                                           && n.BilledDate != null).Count() > 0)
                                                       || (wc.Bio.DateBio >= workday_client.Workday.Week.InitDate
                                                                           && wc.Bio.DateBio <= workday_client.Workday.Week.FinalDate
                                                                           && wc.Bio.BilledDate != null))))
                                            .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate && n.BilledDate != null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }

                        if (clients[i].Bio != null)
                        {
                            if (clients[i].Bio.DateBio != null)
                            {
                                if (clients[i].Bio.DateBio >= workday_client.Workday.Week.InitDate && clients[i].Bio.DateBio <= workday_client.Workday.Week.FinalDate && clients[i].Bio.BilledDate != null)
                                {

                                }
                                else
                                {
                                    clients[i].Bio = null;
                                }
                            }
                        }

                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                        .Include(wc => wc.Note)

                                                        .Include(wc => wc.NoteP)

                                                        .Include(wc => wc.IndividualNote)

                                                        .Include(wc => wc.GroupNote)

                                                        .Include(wc => wc.GroupNote2)
                                                        .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                        .Include(wc => wc.Workday)
                                                        .ThenInclude(w => w.Week)

                                                        .Include(wc => wc.Facilitator)

                                                        .Include(wc => wc.Schedule)
                                                        .ThenInclude(wc => wc.SubSchedules)

                                                        .Where(wc => wc.Present == true
                                                                  && wc.BilledDate != null
                                                                  && wc.Hold == false
                                                                  && wc.Workday.Week.Id == workday_client.Workday.Week.Id
                                                                  && wc.Client.Id == clients[i].Id)
                                                        .ToList();
                    }

                    ViewData["Billed"] = "1";
                    ViewData["idWeek"] = workday_client.Workday.Week.Id;

                }
                else
                {
                    workday_client = await _context.Workdays_Clients
                                                   .Include(wc => wc.Workday)
                                                   .ThenInclude(w => w.Week)
                                                   .ThenInclude(we => we.Clinic)
                                                   .Include(wc => wc.Client)
                                                   .Where(wc => wc.Client.Id == model.Id
                                                        && wc.Workday.Week.Id == week
                                                        && wc.BilledDate != null)
                                                   .FirstOrDefaultAsync();

                    List<Workday_Client> w_clients = await _context.Workdays_Clients
                                                                   .Where(w => w.Workday.Week.Id == workday_client.Workday.Week.Id
                                                                         && w.Client.Id == workday_client.Client.Id
                                                                         && w.BilledDate != null)
                                                                   .ToListAsync();
                    foreach (var item in w_clients)
                    {
                        item.PaymentDate = model.PaymentDate;
                        _context.Update(item);
                        await _context.SaveChangesAsync();
                    }

                    clients = await _context.Clients
                                           
                                            .Include(c => c.Clients_Diagnostics)
                                            .ThenInclude(cd => cd.Diagnostic)

                                            .Include(c => c.Clients_HealthInsurances)
                                            .ThenInclude(c => c.HealthInsurance)

                                            .Include(wc => wc.MTPs)
                                            .Include(wc => wc.Bio)

                                            .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                     && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                 && wc.BilledDate != null
                                                                                 && wc.Hold == false
                                                                                 && wc.Workday.Week.Id == workday_client.Workday.Week.Id).Count() > 0)
                                                      || (wc.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate
                                                                          && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate
                                                                          && n.BilledDate != null).Count() > 0)
                                                      || (wc.Bio.DateBio >= workday_client.Workday.Week.InitDate
                                                                           && wc.Bio.DateBio <= workday_client.Workday.Week.FinalDate
                                                                           && wc.Bio.BilledDate != null))))
                                           .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate && n.BilledDate != null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }

                        if (clients[i].Bio != null)
                        {
                            if (clients[i].Bio.DateBio != null)
                            {
                                if (clients[i].Bio.DateBio >= workday_client.Workday.Week.InitDate && clients[i].Bio.DateBio <= workday_client.Workday.Week.FinalDate && clients[i].Bio.BilledDate != null)
                                {

                                }
                                else
                                {
                                    clients[i].Bio = null;
                                }
                            }
                        }

                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                        .Include(wc => wc.Note)

                                                        .Include(wc => wc.NoteP)

                                                        .Include(wc => wc.IndividualNote)

                                                        .Include(wc => wc.GroupNote)

                                                        .Include(wc => wc.GroupNote2)
                                                        .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                        .Include(wc => wc.Workday)
                                                        .ThenInclude(w => w.Week)

                                                        .Include(wc => wc.Facilitator)

                                                        .Include(wc => wc.Schedule)
                                                        .ThenInclude(wc => wc.SubSchedules)

                                                        .Where(wc => wc.Present == true
                                                                  && wc.BilledDate != null
                                                                  && wc.Hold == false
                                                                  && wc.Workday.Week.Id == workday_client.Workday.Week.Id
                                                                  && wc.Client.Id == clients[i].Id)
                                                        .ToList();
                    }


                    ViewData["Billed"] = "1";
                    ViewData["idWeek"] = week;

                }

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingWeek", clients) });

            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "PaymentReceived", model) });
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> NotesSummaryDetails(string dateInterval = "")
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (dateInterval != string.Empty)
            {
                string[] date = dateInterval.Split(" - ");

                IQueryable<WeekEntity> query = _context.Weeks

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Client)
                                                       .ThenInclude(c => c.Group)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(g => g.Facilitator)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Note)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.NoteP)

                                                       .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                 && w.Days.Where(d => d.Service == ServiceType.PSR).Count() > 0
                                                                 && w.InitDate >= Convert.ToDateTime(date[0]) && w.FinalDate <= Convert.ToDateTime(date[1])));                                             

                
                BillingReportViewModel model = new BillingReportViewModel
                {
                    DateIterval = dateInterval,
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    Weeks = query.ToList()
                };
                ViewData["setting"] = _context.Settings.First(c => c.Clinic.Id == user_logged.Clinic.Id).MHProblems;
                return View(model);                
            }
            else
            {
                IQueryable<WeekEntity> query = _context.Weeks

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(wc => wc.Client)
                                                      .ThenInclude(c => c.Group)

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(g => g.Facilitator)

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(wc => wc.Note)

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(wc => wc.NoteP)

                                                      .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                && w.Days.Where(d => d.Service == ServiceType.PSR).Count() > 0
                                                                && w.InitDate >= DateTime.Now.AddMonths(-3) && w.FinalDate <= DateTime.Now.AddDays(6)));                                                         

                BillingReportViewModel model = new BillingReportViewModel
                {
                    DateIterval = $"{DateTime.Now.AddMonths(-3).ToShortDateString()} - {DateTime.Now.AddDays(6).ToShortDateString()}",
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    Weeks = query.ToList()
                };
                ViewData["setting"] = _context.Settings.First(c => c.Clinic.Id == user_logged.Clinic.Id).MHProblems;
                return View(model);
            }                                   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult NotesSummaryDetails(BillingReportViewModel model)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(NotesSummaryDetails), new { dateInterval = model.DateIterval });
            }

            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> IndNotesSummaryDetails(string dateInterval = "")
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (dateInterval != string.Empty)
            {
                string[] date = dateInterval.Split(" - ");

                IQueryable<WeekEntity> query = _context.Weeks

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Client)
                                                       .ThenInclude(c => c.Group)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(g => g.Facilitator)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Note)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.NoteP)

                                                       .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                 && w.Days.Where(d => d.Service == ServiceType.Individual).Count() > 0
                                                                 && w.InitDate >= Convert.ToDateTime(date[0]) && w.FinalDate <= Convert.ToDateTime(date[1])));


                BillingReportViewModel model = new BillingReportViewModel
                {
                    DateIterval = dateInterval,
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    Weeks = query.ToList()
                };

                return View(model);
            }
            else
            {
                IQueryable<WeekEntity> query = _context.Weeks

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(wc => wc.Client)
                                                      .ThenInclude(c => c.Group)

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(g => g.Facilitator)

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(wc => wc.Note)

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(wc => wc.NoteP)

                                                      .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                && w.Days.Where(d => d.Service == ServiceType.Individual).Count() > 0
                                                                && w.InitDate >= DateTime.Now.AddMonths(-3) && w.FinalDate <= DateTime.Now.AddDays(6)));

                BillingReportViewModel model = new BillingReportViewModel
                {
                    DateIterval = $"{DateTime.Now.AddMonths(-3).ToShortDateString()} - {DateTime.Now.AddDays(6).ToShortDateString()}",
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    Weeks = query.ToList()
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult IndNotesSummaryDetails(BillingReportViewModel model)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(IndNotesSummaryDetails), new { dateInterval = model.DateIterval });
            }

            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GroupNotesSummaryDetails(string dateInterval = "")
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (dateInterval != string.Empty)
            {
                string[] date = dateInterval.Split(" - ");

                IQueryable<WeekEntity> query = _context.Weeks

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Client)
                                                       .ThenInclude(c => c.Group)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(g => g.Facilitator)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Note)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.NoteP)

                                                       .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                 && w.Days.Where(d => d.Service == ServiceType.Group).Count() > 0
                                                                 && w.InitDate >= Convert.ToDateTime(date[0]) && w.FinalDate <= Convert.ToDateTime(date[1])));


                BillingReportViewModel model = new BillingReportViewModel
                {
                    DateIterval = dateInterval,
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    Weeks = query.ToList()
                };
                
                return View(model);
            }
            else
            {
                IQueryable<WeekEntity> query = _context.Weeks

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(wc => wc.Client)
                                                      .ThenInclude(c => c.Group)

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(g => g.Facilitator)

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(wc => wc.Note)

                                                      .Include(w => w.Days)
                                                      .ThenInclude(d => d.Workdays_Clients)
                                                      .ThenInclude(wc => wc.NoteP)

                                                      .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                && w.Days.Where(d => d.Service == ServiceType.Group).Count() > 0
                                                                && w.InitDate >= DateTime.Now.AddMonths(-3) && w.FinalDate <= DateTime.Now.AddDays(6)));

                BillingReportViewModel model = new BillingReportViewModel
                {
                    DateIterval = $"{DateTime.Now.AddMonths(-3).ToShortDateString()} - {DateTime.Now.AddDays(6).ToShortDateString()}",
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    Weeks = query.ToList()
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult GroupNotesSummaryDetails(BillingReportViewModel model)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(GroupNotesSummaryDetails), new { dateInterval = model.DateIterval });
            }

            return View(model);
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> IndividualSignInSheet()
        {
            UserEntity user_logged = await _context.Users.Include(u => u.Clinic)
                                                         .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
                return View(null);

            return View(await _context.Weeks
                                      .Include(w => w.Days)
                                        .ThenInclude(d => d.Workdays_Clients)
                                        .ThenInclude(wc => wc.Client)

                                      .Include(w => w.Days)
                                        .ThenInclude(d => d.Workdays_Clients)
                                        .ThenInclude(g => g.Facilitator)

                                      .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                && w.Days.Where(d => d.Service == ServiceType.PSR).Count() > 0))
                                      .ToListAsync());
        }

        [Authorize(Roles = "Facilitator")]
        public async Task<IActionResult> IndividualSignInSheetGroup()
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
                return View(null);

            return View(await _context.Weeks
                                      .Include(w => w.Days)
                                        .ThenInclude(d => d.Workdays_Clients)
                                        .ThenInclude(wc => wc.Client)                                            

                                      .Include(w => w.Days)
                                        .ThenInclude(d => d.Workdays_Clients)
                                        .ThenInclude(g => g.Facilitator)                                  

                                      .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                && w.Days.Where(d => d.Service == ServiceType.Group).Count() > 0))
                                      .ToListAsync());
        }

        #region Utils funtions
        private bool GenderEvaluation(GenderType gender, string text)
        {
            if (gender == GenderType.Female)
            {
                return text.Contains(" he ") || text.Contains(" He ") || text.Contains(" his ") || text.Contains(" His ") || text.Contains(" him ") ||
                       text.Contains(" him.") || text.Contains("himself") || text.Contains("Himself") || text.Contains(" oldman") || text.Contains(" wife");
            }
            else
            {
                return text.Contains(" she ") || text.Contains(" She ") || text.Contains(" her.") || text.Contains(" her ") || text.Contains(" Her ") ||
                       text.Contains("herself") || text.Contains("Herself") || text.Contains(" oldwoman") || text.Contains(" husband");
            }
        }        

        private bool VerifyNotesAtSameTime(int idClient, string session, DateTime date, DateTime initialTime = new DateTime(), DateTime endTime = new DateTime(), int idWordayClient = 0)
        {
            Workday_Client wordayClient =  _context.Workdays_Clients
                                                   .Include(n => n.Schedule)
                                                   .ThenInclude(n => n.SubSchedules)
                                                   .Include(n => n.Workday)
                                                   .FirstOrDefault(n => n.Id == idWordayClient);
            //Individual notes
            if (session == "8.00 - 9.00 AM" || session == "9.05 - 10.05 AM" || session == "10.15 - 11.15 AM" || session == "11.20 - 12.20 PM")
            {
                if (_context.Workdays_Clients
                            .Where(wc => (wc.Client.Id == idClient && wc.Session == "AM" && wc.Workday.Date == date))
                            .Count() > 0)
                    return true;
                return false;

            }
            else
            {
                if (session == "12.45 - 1.45 PM" || session == "1.50 - 2.50 PM" || session == "3.00 - 4.00 PM" || session == "4.05 - 5.05 PM")
                {
                    if (_context.Workdays_Clients
                                .Where(wc => (wc.Client.Id == idClient && wc.Session == "PM" && wc.Workday.Date == date))
                                .Count() > 0)
                        return true;
                    return false;
                }
                else
                {

                    //PSR notes
                    if (session == "AM")
                    {
                        if (wordayClient.Schedule != null)
                        {
                            if (_context.Workdays_Clients
                                        .Where(wc => (wc.Client.Id == idClient
                                            && wc.Session.Contains("AM") == true
                                            && wc.Workday.Date == date
                                            && wc.Id != idWordayClient
                                            && ((wc.Schedule.InitialTime >= initialTime
                                                && wc.Schedule.InitialTime <= endTime)
                                                ||
                                                (wc.Schedule.EndTime >= initialTime
                                                && wc.Schedule.EndTime <= endTime))))
                                        .Count() > 0)
                                return true;

                        }
                        else
                        {
                            if (_context.Workdays_Clients
                                           .Where(wc => (wc.Client.Id == idClient
                                               && wc.Session.Contains("AM") == true
                                               && wc.Workday.Date == date
                                               && wc.Id != idWordayClient))
                                           .Count() > 0)
                            {
                                DateTime initial = new DateTime().AddHours(8.0);
                                DateTime end = new DateTime().AddHours(12.30);

                                if ((initialTime.TimeOfDay < initial.TimeOfDay && endTime.TimeOfDay < initial.TimeOfDay)
                                     || (initialTime.TimeOfDay > end.TimeOfDay && endTime.TimeOfDay > end.TimeOfDay))
                                {
                                    return false;
                                }
                                return true;

                            }
                                return false;
                        }
                        return false;
                    }
                    else
                    {
                        if (session == "PM")
                        {
                            if (wordayClient.Schedule != null)
                            {
                                if (_context.Workdays_Clients
                                            .Where(wc => (wc.Client.Id == idClient
                                                && wc.Session.Contains("PM") == true
                                                && wc.Workday.Date == date
                                                && wc.Id != idWordayClient
                                                && ((wc.Schedule.InitialTime >= initialTime
                                                    && wc.Schedule.InitialTime <= endTime)
                                                    ||
                                                    (wc.Schedule.EndTime >= initialTime
                                                    && wc.Schedule.EndTime <= endTime))))
                                            .Count() > 0)
                                    return true;

                            }
                            else
                            {
                                if (_context.Workdays_Clients
                                               .Where(wc => (wc.Client.Id == idClient
                                                   && wc.Session.Contains("PM") == true
                                                   && wc.Workday.Date == date
                                                   && wc.Id != idWordayClient))
                                               .Count() > 0)
                                {
                                    DateTime initial = new DateTime().AddHours(12.30);
                                    DateTime end = new DateTime().AddHours(05.30);

                                    if ((initialTime.TimeOfDay < initial.TimeOfDay && endTime.TimeOfDay < initial.TimeOfDay)
                                         || (initialTime.TimeOfDay > end.TimeOfDay && endTime.TimeOfDay > end.TimeOfDay))
                                    {
                                        return false;
                                    }
                                    return true;

                                }
                                return false;
                            }
                            return false;
                        }
                        else
                        {
                            List<Workday_Client> temp = _context.Workdays_Clients.Where(n => n.Workday.Date == date
                                                                && ((n.Schedule.InitialTime >= initialTime
                                                                && n.Schedule.InitialTime <= endTime)
                                                                    || (n.Schedule.EndTime >= initialTime
                                                                       && n.Schedule.EndTime <= endTime)
                                                                    || (n.Schedule.InitialTime <= initialTime
                                                                       && n.Schedule.EndTime >= initialTime)
                                                                    || (n.Schedule.InitialTime <= endTime
                                                                       && n.Schedule.EndTime >= endTime))
                                                               && n.Id != idWordayClient
                                                               && n.Client.Id == idClient)
                                                            .ToList();


                            if (temp.Count() > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                            
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ChangeNotes(string dateInterval = "")
        {           
            UserEntity user_logged = await _context.Users

                                                   .Include(u => u.Clinic)
                                                   .ThenInclude(c => c.Setting)

                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
         
            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            if (dateInterval != string.Empty)
            {
                string[] date = dateInterval.Split(" - ");

                IQueryable<WeekEntity> query = _context.Weeks

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Client)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(g => g.Facilitator)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Note)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.NoteP)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Schedule)

                                                       .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                 && w.Days.Where(d => (d.Service == ServiceType.PSR)).Count() > 0
                                                                 && w.InitDate >= Convert.ToDateTime(date[0]) && w.FinalDate <= Convert.ToDateTime(date[1])));                                                
                                
                BillingReportViewModel model = new BillingReportViewModel
                {
                    DateIterval = dateInterval,
                    IdFacilitator = 0,
                    Facilitators = null,
                    IdClient = 0,
                    Clients = null,
                    Weeks = query.ToList()
                };

                return View(model);                
            }
            else
            {
                IQueryable<WeekEntity> query = _context.Weeks

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Client)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(g => g.Facilitator)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Note)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.NoteP)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Schedule)

                                                       .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                 && w.Days.Where(d => (d.Service == ServiceType.PSR)).Count() > 0
                                                                 && w.InitDate >= DateTime.Now.AddMonths(-1) && w.FinalDate <= DateTime.Now.AddDays(6)));                                                        

                BillingReportViewModel model = new BillingReportViewModel
                {
                    DateIterval = $"{DateTime.Now.AddMonths(-1).ToShortDateString()} - {DateTime.Now.AddDays(6).ToShortDateString()}",
                    IdFacilitator = 0,
                    Facilitators = null,
                    IdClient = 0,
                    Clients = null,
                    Weeks = query.ToList()
                };

                return View(model);
            }           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult ChangeNotes(BillingReportViewModel model)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ChangeNotes), new { dateInterval = model.DateIterval });
            }

            return View(model);
        }

        [Authorize(Roles = "Manager, Facilitator")]
        public async Task<IActionResult> ChangeSession(int? id = 0, string dateInterval = "")
        {
            if (id == null)
            {
                return RedirectToAction("Home/Error404");
            }

            Workday_Client wordayClient = await _context.Workdays_Clients
                                                        .Include(n => n.Facilitator)
                                                        .Include(n => n.Client)
                                                        .Include(n => n.Workday)
                                                        .Include(n => n.Schedule)
                                                        .FirstOrDefaultAsync(c => c.Id == id);
            if (wordayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            Workday_ClientViewModel wordayClientViewModel = _converterHelper.ToWorkdayClientViewModel(wordayClient, false);
            wordayClientViewModel.DateInterval = dateInterval;

            if (wordayClientViewModel.Session == "AM")
            {
                ViewData["am"] = "true" ;
            }
            else
            {
                ViewData["am"] = "false";
            }            
            
            return View(wordayClientViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Facilitator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeSession(int id, Workday_ClientViewModel workdayClientModel, IFormCollection form)
        {
            if (id != workdayClientModel.Id)
            {
                return RedirectToAction("Home/Error404");
            }

            switch (form["Meridian"])
            {
                case "Am":
                    {
                        workdayClientModel.Session = "AM";
                        break;
                    }
                case "Pm":
                    {
                        workdayClientModel.Session = "PM";
                        break;
                    }
                default:
                    break;
            }
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            Workday_Client wordayClient = await _context.Workdays_Clients
                                                        .FirstOrDefaultAsync(c => c.Id == workdayClientModel.Id);

            wordayClient.Session = workdayClientModel.Session;
            if (workdayClientModel.IdSchedule > 0)
            {
                wordayClient.Schedule = await _context.Schedule.FirstOrDefaultAsync(s => s.Id == workdayClientModel.IdSchedule);
                wordayClient.Session = wordayClient.Schedule.Session;
            }
            
            _context.Update(wordayClient);
            try
            {
                await _context.SaveChangesAsync();

                if (User.IsInRole("Facilitator"))
                {
                    List<WeekEntity> week = await _context.Weeks

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(wc => wc.Client)

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(g => g.Facilitator)

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(wc => wc.Note)

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(wc => wc.NoteP)

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(wc => wc.Schedule)

                                                          .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                    && w.Days.Where(d => (d.Service == ServiceType.PSR && d.Workdays_Clients.Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name).Count() > 0)).Count() > 0))
                                                          .ToListAsync();

                    return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_ViewNotes", week) });
                }
                if (User.IsInRole("Manager"))
                {
                    string[] date = workdayClientModel.DateInterval.Split(" - ");
                    List<WeekEntity> week = await _context.Weeks

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(wc => wc.Client)

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(g => g.Facilitator)

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(wc => wc.Note)

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(wc => wc.NoteP)

                                                          .Include(w => w.Days)
                                                          .ThenInclude(d => d.Workdays_Clients)
                                                          .ThenInclude(wc => wc.Schedule)

                                                          .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                    && w.Days.Where(d => (d.Service == ServiceType.PSR)).Count() > 0
                                                                    && w.InitDate >= Convert.ToDateTime(date[0]) && w.FinalDate <= Convert.ToDateTime(date[1])))
                                                          .ToListAsync();

                    BillingReportViewModel model = new BillingReportViewModel
                    {
                        DateIterval = workdayClientModel.DateInterval,
                        IdFacilitator = 0,
                        Facilitators = null,
                        IdClient = 0,
                        Clients = null,
                        Weeks = week
                    };

                    return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_ViewNotesForChanges", model) });
                }
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, $"Already exists the note: {wordayClient.ClientName}");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            return View(workdayClientModel);
        }

        [Authorize(Roles = "Manager, Facilitator")]
        public async Task<IActionResult> ChangeSessionByManager(int? id = 0)
        {
            if (id == null)
            {
                return RedirectToAction("Home/Error404");
            }

            Workday_Client wordayClient = await _context.Workdays_Clients
                                                        .Include(n => n.Facilitator)
                                                        .Include(n => n.Client)
                                                        .Include(n => n.Workday)
                                                        .FirstOrDefaultAsync(c => c.Id == id);
            if (wordayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            Workday_ClientViewModel wordayClientViewModel = _converterHelper.ToWorkdayClientViewModel(wordayClient, false);
            if (wordayClientViewModel.Session == "AM")
            {
                ViewData["am"] = "true";
            }
            else
            {
                ViewData["am"] = "false";
            }


            return View(wordayClientViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Facilitator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeSessionByManager(int id, Workday_ClientViewModel workdayClientModel, IFormCollection form)
        {
            if (id != workdayClientModel.Id)
            {
                return RedirectToAction("Home/Error404");
            }

            if (ModelState.IsValid)
            {
                switch (form["Meridian"])
                {
                    case "Am":
                        {
                            workdayClientModel.Session = "AM";
                            break;
                        }
                    case "Pm":
                        {
                            workdayClientModel.Session = "PM";
                            break;
                        }
                    default:
                        break;
                }
                UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                       .FirstOrDefault(u => u.UserName == User.Identity.Name);

                Workday_Client wordayClient = await _context.Workdays_Clients
                                                            .Include(n => n.Facilitator)
                                                            .Include(n => n.Client)
                                                            .Include(n => n.Workday)
                                                            .FirstOrDefaultAsync(c => c.Id == workdayClientModel.Id);
                wordayClient.Session = workdayClientModel.Session;
                _context.Update(wordayClient);
                try
                {
                    await _context.SaveChangesAsync();

                    List<WeekEntity> weeks = await _context.Weeks

                                                           .Include(w => w.Days)
                                                           .ThenInclude(d => d.Workdays_Clients)
                                                           .ThenInclude(wc => wc.Client)

                                                           .Include(w => w.Days)
                                                           .ThenInclude(d => d.Workdays_Clients)
                                                           .ThenInclude(g => g.Facilitator)

                                                           .Include(w => w.Days)
                                                           .ThenInclude(d => d.Workdays_Clients)
                                                           .ThenInclude(wc => wc.Note)

                                                           .Include(w => w.Days)
                                                           .ThenInclude(d => d.Workdays_Clients)
                                                           .ThenInclude(wc => wc.NoteP)

                                                           .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                     && w.Days.Where(d => (d.Service == ServiceType.PSR)).Count() > 0))
                                                           .ToListAsync();

                    return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_ViewNotesForChanges", weeks) });
                }
                catch (System.Exception ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, $"Already exists the note: {wordayClient.ClientName}");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }
            }
            return View(workdayClientModel);
        }

        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> ApproveNotePBatch(int id, int origin = 0)
        {
            NotePEntity note = await _context.NotesP

                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(wc => wc.Client)
                                             
                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(wc => wc.Facilitator)

                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(w => w.Workday)
                                             
                                             .FirstOrDefaultAsync(n => n.Workday_Cient.Id == id);

            if (note == null)
            {
                return RedirectToAction("Home/Error404");
            }
            else
            {
                List<NotePEntity> notelist = await _context.NotesP
                                                           .Include(n => n.Workday_Cient)
                                                           .ThenInclude(wc => wc.Client)

                                                           .Include(n => n.Workday_Cient)
                                                           .ThenInclude(wc => wc.Facilitator)

                                                           .Include(n => n.Workday_Cient)
                                                           .ThenInclude(wc => wc.Messages)

                                                           .Include(n => n.Workday_Cient)
                                                           .ThenInclude(w => w.Workday)
                                             
                                                           .Where(n => (n.Workday_Cient.Facilitator.Id == note.Workday_Cient.Facilitator.Id
                                                                    && n.Status == NoteStatus.Pending
                                                                    && n.Workday_Cient.Workday.Id == note.Workday_Cient.Workday.Id
                                                                    && n.Workday_Cient.Messages.Count() == 0))
                                                           .ToListAsync();
                if (notelist.Count() == 0)
                {
                    return RedirectToAction("PendingNotes", "Notes",new {id = id , error = 1});
                }
                return View(notelist);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> ApproveNotePBatch(int id = 0)
        {
            NotePEntity note = await _context.NotesP

                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(wc => wc.Client)

                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(wc => wc.Facilitator)

                                             .Include(n => n.Workday_Cient)
                                             .ThenInclude(w => w.Workday)

                                             .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                return RedirectToAction("Home/Error404");
            }

            List<NotePEntity> model = await _context.NotesP
                                                    .Include(n => n.Workday_Cient)
                                                    .ThenInclude(wc => wc.Client)

                                                    .Include(n => n.Workday_Cient)
                                                    .ThenInclude(wc => wc.Facilitator)

                                                    .Include(n => n.Workday_Cient)
                                                    .ThenInclude(wc => wc.Messages)

                                                    .Include(n => n.Workday_Cient)
                                                    .ThenInclude(w => w.Workday)

                                                    .Where(n => (n.Workday_Cient.Facilitator.Id == note.Workday_Cient.Facilitator.Id
                                                             && n.Status == NoteStatus.Pending
                                                             && n.Workday_Cient.Workday.Id == note.Workday_Cient.Workday.Id
                                                             && n.Workday_Cient.Messages.Count() == 0))
                                                    .ToListAsync();

            foreach (var item in model)
            {
                item.Status = NoteStatus.Approved;
                item.DateOfApprove = DateTime.Now;
                item.Supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.LinkedUser == User.Identity.Name);
                _context.Update(item);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PendingNotes));
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PaymentReceivedToday(int id = 0, int week = 0)
        {
            if (week == 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients

                                                            .Include(wc => wc.Workday)
                                                            .ThenInclude(w => w.Week)
                                                            .ThenInclude(we => we.Clinic)

                                                            .Where(wc => wc.Id == id)
                                                            .FirstOrDefaultAsync();
                workday_client.PaymentDate = DateTime.Now;
                workday_client.DeniedBill = false;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingWeek", "Notes", new { id = workday_client.Workday.Week.Id, billed = 1 });
            }
            else
            {
                List<Workday_Client> workday_client = await _context.Workdays_Clients

                                                                        .Include(wc => wc.Workday)
                                                                        .ThenInclude(w => w.Week)
                                                                        .ThenInclude(we => we.Clinic)

                                                                        .Where(wc => wc.Client.Id == id
                                                                           && wc.Workday.Week.Id == week
                                                                           && wc.BilledDate != null)
                                                                        .ToListAsync();

                foreach (var item in workday_client)
                {
                    item.PaymentDate = DateTime.Now;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }


            }

            return RedirectToAction("BillingWeek", "Notes", new { id = week, billed = 1 });

        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillNoteToday(int id = 0, int week = 0)
        {
            if (week == 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients

                                                                  .Include(wc => wc.Workday)
                                                                  .ThenInclude(w => w.Week)
                                                                  .ThenInclude(we => we.Clinic)

                                                                  .Where(wc => wc.Id == id)
                                                                  .FirstOrDefaultAsync();
                workday_client.BilledDate = DateTime.Now;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingWeek", "Notes", new { id = workday_client.Workday.Week.Id });
            }
            else
            {
                List<Workday_Client> workday_client = await _context.Workdays_Clients

                                                                     .Include(wc => wc.Workday)
                                                                     .ThenInclude(w => w.Week)
                                                                     .ThenInclude(we => we.Clinic)

                                                                     .Where(wc => wc.Client.Id == id
                                                                        && wc.Workday.Week.Id == week)
                                                                     .ToListAsync();

                foreach (var item in workday_client)
                { 
                    item.BilledDate = DateTime.Now;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }


            }
            
            return RedirectToAction("BillingWeek", "Notes", new { id = week });

        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ClientForWeek(int id = 0, int weekId = 0, int facilitatorId = 0)
        {
            if (id == 1)
            {
                ViewBag.FinishEdition = "Y";
            }


            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

             List<WeekEntity> week = await _context.Weeks

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.Client)

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(g => g.Facilitator)

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.Note)

                                      .Include(w => w.Days)
                                      .ThenInclude(d => d.Workdays_Clients)
                                      .ThenInclude(wc => wc.NoteP)

                                      .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                && w.Days.Where(d => (d.Service == ServiceType.PSR && d.Workdays_Clients.Where(wc => (wc.Facilitator.Id == facilitatorId && wc.Workday.Week.Id == weekId)).Count() > 0)).Count() > 0
                                                ))
                                      .ToListAsync();

            ViewData["facilitatorId"] = facilitatorId;
            return View(week);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult PSRAssistanceModifications(bool update = false, int affected_rows = 0)
        {
            //success
            if (update)
            {
                ViewBag.Error = "1";
                ViewBag.Text = $"Succesfully updated {affected_rows} rows";
            }

            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            MultiSelectList facilitator_list;
            List<FacilitatorEntity> facilitators = _context.Facilitators
                                                           .Where(f => (f.Clinic.Id == user_logged.Clinic.Id && f.Status == StatusType.Open))
                                                           .OrderBy(c => c.Name).ToList();
            
            facilitator_list = new MultiSelectList(facilitators, "Id", "Name");
            ViewData["facilitators"] = facilitator_list;

            AssistanceModificationsViewModel model = new AssistanceModificationsViewModel
            {
                CauseOfNotPresent = "The facilitator did not work on that date." 
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PSRAssistanceModifications(AssistanceModificationsViewModel model, IFormCollection form)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(form["facilitators"].ToString())) 
            {
                string[] workdays = model.Workdays.ToString().Split(',');
                string[] facilitators = form["facilitators"].ToString().Split(',');
                IQueryable<Workday_Client> workdayClient;
                int rows_affected = 0;
                bool present = true;

                switch (form["Present"])
                {
                    case "present":
                        {
                            present = true;
                            break;
                        }
                    case "nopresent":
                        {
                            present = false;
                            break;
                        }
                    default:
                        break;
                }

                foreach (string value in workdays)
                {
                    foreach (string item in facilitators)
                    {
                        workdayClient = _context.Workdays_Clients
                                                 .Where(wc => (wc.Facilitator.Id == Convert.ToInt32(item)
                                                            && wc.Workday.Date == Convert.ToDateTime(value)
                                                            && wc.Workday.Service == ServiceType.PSR));

                        rows_affected += workdayClient.Count();

                        await workdayClient.ForEachAsync(wc => wc.Present = present);
                        await workdayClient.ForEachAsync(wc => wc.CauseOfNotPresent = (present == true) ? string.Empty : model.CauseOfNotPresent);

                        _context.UpdateRange(workdayClient);
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(PSRAssistanceModifications), new { update = true, affected_rows = rows_affected });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
                return RedirectToAction(nameof(PSRAssistanceModifications));
            }

            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);
            MultiSelectList facilitator_list;
            List<FacilitatorEntity> facilitatorsList = _context.Facilitators
                                                           .Where(f => (f.Clinic.Id == user_logged.Clinic.Id && f.Status == StatusType.Open))
                                                           .OrderBy(c => c.Name).ToList();

            facilitator_list = new MultiSelectList(facilitatorsList, "Id", "Name");
            ViewData["facilitators"] = facilitator_list;

            return View(model);            
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteNote(int? idWorkday_client)
        {
            UserEntity user_logged = await _context.Users
                                                  .Include(u => u.Clinic)
                                                  .ThenInclude(c => c.Setting)
                                                  .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (idWorkday_client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            Workday_Client workday_Client = await _context.Workdays_Clients
                                                          .Include(n => n.Client)
                                                          .FirstAsync(t => t.Id == idWorkday_client);


            if (workday_Client == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IndividualNoteEntity indNotes = _context.IndividualNotes
                                                    .Include(n => n.Workday_Cient)
                                                    .ThenInclude(n => n.Client)
                                                    .FirstOrDefault(t => t.Workday_Cient.Id == idWorkday_client);
            int idClient = 0;
            if (indNotes != null)
            {
                idClient = indNotes.Workday_Cient.Client.Id;
                _context.Remove(indNotes);
                workday_Client.Client = null;
                _context.Update(workday_Client);
                await _context.SaveChangesAsync();

                return RedirectToAction("ClientHistory", "Clients", new { idClient = idClient });
            }

            idClient = workday_Client.Client.Id;
            _context.Workdays_Clients.Remove(workday_Client);
            await _context.SaveChangesAsync();

            return RedirectToAction("ClientHistory", "Clients", new { idClient = idClient });
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ReturnTo(int? id, int clientId = 0, NoteStatus aStatus = NoteStatus.Edition)
        {
            if (id == null)
            {
                return RedirectToAction("Home/Error404");
            }

            NotePEntity notePEntity = await _context.NotesP
                                                  .FirstOrDefaultAsync(t => t.Workday_Client_FK == id);
            if (notePEntity == null)
            {
                NoteEntity noteEntity = await _context.Notes
                                                  .FirstOrDefaultAsync(t => t.Workday_Client_FK == id);
                if (noteEntity == null)
                {
                    return RedirectToAction("Home/Error404");
                }
                try
                {
                    noteEntity.Status = aStatus;
                    _context.Notes.Update(noteEntity);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
                }

                return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
            }

            try
            {
                notePEntity.Status = aStatus;
                _context.NotesP.Update(notePEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
            }

            return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ReturnIndNoteTo(int? id, int clientId = 0, NoteStatus aStatus = NoteStatus.Edition)
        {
            if (id == null)
            {
                return RedirectToAction("Home/Error404");
            }

            IndividualNoteEntity noteIndEntity = await _context.IndividualNotes
                                                               .FirstOrDefaultAsync(t => t.Workday_Client_FK == id);
            if (noteIndEntity == null)
            {
                return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
            }

            try
            {
                noteIndEntity.Status = aStatus;
                _context.IndividualNotes.Update(noteIndEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
            }

            return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ReturnGroupNoteTo(int? id, int clientId = 0, NoteStatus aStatus = NoteStatus.Edition)
        {
            if (id == null)
            {
                return RedirectToAction("Home/Error404");
            }

            GroupNoteEntity noteGroupEntity = await _context.GroupNotes
                                                               .FirstOrDefaultAsync(t => t.Workday_Client_FK == id);
            if (noteGroupEntity == null)
            {
                return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
            }
            else
            {
                GroupNote2Entity noteGroup2Entity = await _context.GroupNotes2
                                                                   .FirstOrDefaultAsync(t => t.Workday_Client_FK == id);
                if (noteGroup2Entity == null)
                {
                    return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
                }
            }

            try
            {
                noteGroupEntity.Status = aStatus;
                _context.GroupNotes.Update(noteGroupEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
            }

            return RedirectToAction("ClientHistory", "Clients", new { idClient = clientId });
        }

        [Authorize(Roles = "Manager, Supervisor, Facilitator")]
        public async Task<IActionResult> AuditGroups()
        {
            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic || !user_logged.Clinic.Setting.MHProblems)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            DateTime date = new DateTime();
            List<AuditGroups> auditGroup = new List<AuditGroups>();
            AuditGroups auditGroup_Temp = new AuditGroups();

            List<WorkdayEntity> workday = await _context.Workdays
                                                  .Include(n => n.Workdays_Clients)
                                                  .ThenInclude(n => n.Note)

                                                  .Include(n => n.Workdays_Clients)
                                                  .ThenInclude(n => n.NoteP)

                                                  .Include(n => n.Workdays_Clients)
                                                  .ThenInclude(n => n.Facilitator)

                                                  .Where(w => (w.Week.Clinic.Id == user_logged.Clinic.Id 
                                                       && w.Service == 0))
                                                  .ToListAsync();
            List<FacilitatorEntity> facilitatorList = await _context.Facilitators.ToListAsync();
            int countAM = 0;
            int countPM = 0;
            foreach (var item in workday)
            {
                foreach (var facilitator in facilitatorList)
                {
                    countAM = item.Workdays_Clients.Where(m => (m.Facilitator.Id == facilitator.Id && m.Session == "AM" && m.Present == true)).Count();
                    countPM = item.Workdays_Clients.Where(m => (m.Facilitator.Id == facilitator.Id && m.Session == "PM" && m.Present == true)).Count();
                    if (countAM > 12)
                    {
                        auditGroup_Temp.Date = item.Date.ToShortDateString();
                        auditGroup_Temp.Count = countAM;
                        auditGroup_Temp.Facilitator = facilitator.Name;
                        auditGroup_Temp.Session = "AM";
                        auditGroup_Temp.Active = 0;
                        auditGroup.Add(auditGroup_Temp);
                    }
                    else
                    {
                        if (countAM < 3 && countAM > 0)
                        {
                            auditGroup_Temp.Date = item.Date.ToShortDateString();
                            auditGroup_Temp.Count = countAM;
                            auditGroup_Temp.Facilitator = facilitator.Name;
                            auditGroup_Temp.Session = "AM";
                            auditGroup_Temp.Active = 1;
                            auditGroup.Add(auditGroup_Temp);
                        }
                    }

                    auditGroup_Temp = new AuditGroups();
                    countAM = 0;
                   
                    if (countPM > 12)
                    {
                        auditGroup_Temp.Date = item.Date.ToShortDateString();
                        auditGroup_Temp.Count = countPM;
                        auditGroup_Temp.Facilitator = facilitator.Name;
                        auditGroup_Temp.Session = "PM";
                        auditGroup_Temp.Active = 0;
                        auditGroup.Add(auditGroup_Temp);
                    }
                    else
                    {
                       if (countPM < 3 && countPM > 0)
                        {
                            auditGroup_Temp.Date = item.Date.ToShortDateString();
                            auditGroup_Temp.Count = countPM;
                            auditGroup_Temp.Facilitator = facilitator.Name;
                            auditGroup_Temp.Session = "PM";
                            auditGroup_Temp.Active = 1;
                            auditGroup.Add(auditGroup_Temp);
                        }
                    }
                    auditGroup_Temp = new AuditGroups();
                    countPM = 0;
                }
               

            }

            return View(auditGroup);
        }

        [Authorize(Roles = "Manager, Supervisor, Facilitator")]
        public async Task<IActionResult> AuditGoalsObjective(int idMtp = 0)
        {
            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic || !user_logged.Clinic.Setting.MHProblems)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            List<AuditGoalsObjective> auditGoalsObjetive_List = new List<AuditGoalsObjective>();
            AuditGoalsObjective auditGoalsObjetive = new AuditGoalsObjective();

            MTPEntity mtp = _context.MTPs
                                    .Include(n => n.Goals)
                                    .ThenInclude(n => n.Objetives)
                                    //.Include(n => n.Client)
                                    .FirstOrDefault(n => n.Id == idMtp);

            List<NotePEntity> note = await _context.NotesP
                                                   .Include(n => n.NotesP_Activities)
                                                   .ThenInclude(n => n.Objetive)
                                                   .ThenInclude(n => n.Goal)

                                                   .Where(w => (w.Workday_Cient.Workday.Week.Clinic.Id == user_logged.Clinic.Id
                                                        && w.MTPId == idMtp
                                                        && w.Status != NoteStatus.Edition))
                                                   .ToListAsync();
            int tempCount = 0;
            foreach (var goal in mtp.Goals.Where(n => n.Service == ServiceType.PSR))
            {
                foreach (var objective in goal.Objetives.OrderBy(n => n.Objetive))
                {
                    auditGoalsObjetive.NumberGoal = goal.Number;
                    auditGoalsObjetive.NumberObjective = objective.Objetive;
                    tempCount = note.Where(n => n.NotesP_Activities.FirstOrDefault(m => m.Objetive != null).Objetive.Id == objective.Id).Count();
                    auditGoalsObjetive.Count = tempCount;
                    auditGoalsObjetive.Porciento = Math.Round((tempCount * 100) / Convert.ToDouble(note.Count()),1);
                    if (tempCount == 0)
                    {
                        auditGoalsObjetive.Active = 0;
                    }
                    else
                    {
                        auditGoalsObjetive.Active = 2;
                    }
                    auditGoalsObjetive_List.Add(auditGoalsObjetive);
                    tempCount = 0;
                    auditGoalsObjetive = new AuditGoalsObjective();
                }
                
            }
           
            return View(auditGoalsObjetive_List);
        }

        [Authorize(Roles = "Manager, Supervisor, Facilitator")]
        public async Task<IActionResult> AuditGoalsObjectiveInd(int idMtp = 0)
        {
            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic || !user_logged.Clinic.Setting.MHProblems)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            List<AuditGoalsObjective> auditGoalsObjetive_List = new List<AuditGoalsObjective>();
            AuditGoalsObjective auditGoalsObjetive = new AuditGoalsObjective();

            MTPEntity mtp = _context.MTPs
                                    .Include(n => n.Goals)
                                    .ThenInclude(n => n.Objetives)
                                    //.Include(n => n.Client)
                                    .FirstOrDefault(n => n.Id == idMtp);

            List<IndividualNoteEntity> note = await _context.IndividualNotes
                                                   .Include(n => n.Objective)
                                                   .ThenInclude(n => n.Goal)

                                                   .Where(w => (w.Workday_Cient.Workday.Week.Clinic.Id == user_logged.Clinic.Id
                                                        && w.MTPId == idMtp
                                                        && w.Status != NoteStatus.Edition))
                                                   .ToListAsync();
            int tempCount = 0;
            foreach (var goal in mtp.Goals.Where(n => n.Service == ServiceType.Individual))
            {
                foreach (var objective in goal.Objetives.OrderBy(n => n.Objetive))
                {
                    auditGoalsObjetive.NumberGoal = goal.Number;
                    auditGoalsObjetive.NumberObjective = objective.Objetive;
                    tempCount = note.Where(n => n.Objective.Id == objective.Id).Count();
                    auditGoalsObjetive.Count = tempCount;
                    auditGoalsObjetive.Porciento = Math.Round((tempCount * 100) / Convert.ToDouble(note.Count()), 1);
                    if (tempCount == 0)
                    {
                        auditGoalsObjetive.Active = 0;
                    }
                    else
                    {
                        auditGoalsObjetive.Active = 2;
                    }
                    auditGoalsObjetive_List.Add(auditGoalsObjetive);
                    tempCount = 0;
                    auditGoalsObjetive = new AuditGoalsObjective();
                }

            }

            return View(auditGoalsObjetive_List);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> NotBill(int id = 0, int week = 0)
        {
            if (week == 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients

                                                                  .Include(wc => wc.Workday)
                                                                  .ThenInclude(w => w.Week)
                                                                  .ThenInclude(we => we.Clinic)

                                                                  .Where(wc => wc.Id == id)
                                                                  .FirstOrDefaultAsync();
                workday_client.BilledDate = null;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingWeek", "Notes", new { id = workday_client.Workday.Week.Id, billed = 1 });
            }
            else
            {
                List<Workday_Client> workday_client = await _context.Workdays_Clients

                                                                     .Include(wc => wc.Workday)
                                                                     .ThenInclude(w => w.Week)
                                                                     .ThenInclude(we => we.Clinic)

                                                                     .Where(wc => wc.Client.Id == id
                                                                        && wc.Workday.Week.Id == week)
                                                                     .ToListAsync();

                foreach (var item in workday_client)
                {
                    item.BilledDate = null;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }


            }

            return RedirectToAction("BillingWeek", "Notes", new { id = week, billed = 1 });

        }

        [Authorize(Roles = "Manager, Supervisor, Facilitator")]
        public async Task<IActionResult> ViewAllGoals(int idMtp = 0)
        {
            UserEntity user_logged = _context.Users

                                             .Include(u => u.Clinic)
                                             .ThenInclude(c => c.Setting)

                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user_logged.Clinic == null || user_logged.Clinic.Setting == null || !user_logged.Clinic.Setting.MentalHealthClinic || !user_logged.Clinic.Setting.MHProblems)
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            List<AllGoals> allGoals_List = new List<AllGoals>();
            AllGoals goal_temp = new AllGoals();
            List<AllObjectives> allObjective_List = new List<AllObjectives>();
            AllObjectives objective_temp = new AllObjectives();

            MTPEntity mtp = _context.MTPs
                                    .Include(n => n.Goals)
                                    .ThenInclude(n => n.Objetives)
                                    .Include(n => n.MtpReviewList)
                                    .FirstOrDefault(n => n.Id == idMtp);

            int month = 0;
            if (mtp.MtpReviewList.Count() > 0)
            {
                foreach (var review in mtp.MtpReviewList)
                {
                    month = month + review.MonthOfTreatment;
                }
            }

            foreach (var goal in mtp.Goals)
            {
                goal_temp.NumberGoal = goal.Number;
                goal_temp.Name = goal.Name;
                goal_temp.AreaFocus = goal.AreaOfFocus;
                goal_temp.Service = goal.Service;
                goal_temp.Compliment = goal.Compliment;
                goal_temp.AllObjectives = new List<AllObjectives>();

                foreach (var objective in goal.Objetives.OrderBy(n => n.Objetive))
                {
                    objective_temp.NumberObjective = objective.Objetive;
                    objective_temp.Description = objective.Description;
                    objective_temp.Intervention = objective.Intervention;
                    objective_temp.DateTarget = objective.DateTarget.AddMonths(month).ToShortDateString().ToString();

                    goal_temp.AllObjectives.Add(objective_temp);
                    objective_temp = new AllObjectives();

                }

                allGoals_List.Add(goal_temp);
                goal_temp = new AllGoals();
            }

            return View(allGoals_List);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillingClient(int idClient, int billed = 0)
        {
            UserEntity user_logged = await _context.Users

                                                   .Include(u => u.Clinic)

                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
            {
                return RedirectToAction("Home/Error404");
            }

            ClientEntity client = await _context.Clients
                                                .Include(c => c.Clients_Diagnostics)
                                                .ThenInclude(cd => cd.Diagnostic)

                                                .Include(c => c.Clients_HealthInsurances)
                                                .ThenInclude(c => c.HealthInsurance)

                                                .FirstOrDefaultAsync(n => n.Id == idClient);
            int cantUnit = 0;
            int money = 0;
            int documents = 0;
            string clientName = client.Name;
            string code = client.Code;
            string diagnostics = string.Empty;
            string medicaidId = client.MedicaidID;
            string birthdate = client.DateOfBirth.ToShortDateString();
            string healthInsurance = string.Empty;
            string memberID = string.Empty;

            if (client.Clients_Diagnostics.Where(n => n.Principal == true).Count() > 0)
            {
                diagnostics = client.Clients_Diagnostics.First(n => n.Principal == true).Diagnostic.Code;
            }

            if (client.Clients_HealthInsurances.Where(n => n.Active == true).Count() > 0)
            {
                healthInsurance = client.Clients_HealthInsurances.First(n => n.Active == true).HealthInsurance.Name;
                memberID = client.Clients_HealthInsurances.First(n => n.Active == true).MemberId;
            }

            if (billed == 0)
            {
                ClientEntity client_salida = await _context.Clients
                                                           .Include(wc => wc.Workdays_Clients)
                                                           .ThenInclude(wc => wc.Note)

                                                           .Include(wc => wc.Workdays_Clients)
                                                           .ThenInclude(wc => wc.NoteP)

                                                           .Include(wc => wc.Workdays_Clients)
                                                           .ThenInclude(wc => wc.IndividualNote)

                                                           .Include(wc => wc.Workdays_Clients)
                                                           .ThenInclude(wc => wc.GroupNote)

                                                           .Include(wc => wc.Workdays_Clients)
                                                           .ThenInclude(wc => wc.GroupNote2)
                                                           .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                           .Include(c => c.Clients_Diagnostics)
                                                           .ThenInclude(cd => cd.Diagnostic)

                                                           .Include(c => c.Clients_HealthInsurances)
                                                           .ThenInclude(c => c.HealthInsurance)

                                                           .Include(wc => wc.Workdays_Clients)
                                                           .ThenInclude(wc => wc.Workday)
                                                           .ThenInclude(w => w.Week)

                                                           .Include(wc => wc.Workdays_Clients)
                                                           .ThenInclude(wc => wc.Facilitator)

                                                           .Include(wc => wc.Workdays_Clients)
                                                           .ThenInclude(wc => wc.Schedule)
                                                           .ThenInclude(wc => wc.SubSchedules)

                                                           .Include(wc => wc.MTPs)
                                                           .Include(wc => wc.Bio)

                                                           .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id 
                                                                      && wc.Id == idClient 
                                                                      && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                                  && wc.BilledDate == null
                                                                                                  && wc.Hold == false).Count() > 0)
                                                                        || (wc.MTPs.Where(n => n.BilledDate == null).Count() > 0)
                                                                        || (wc.Bio.BilledDate == null))))
                                                           .FirstOrDefaultAsync();
                if (client_salida != null)
                {
                    foreach (var item in client_salida.MTPs)
                    {
                        if (item.BilledDate == null)
                        {
                            cantUnit += item.Units;
                            money += item.Units * 15;
                            documents++;
                        }
                    }

                    if (client_salida.Bio.BilledDate == null)
                    {
                        cantUnit += client_salida.Bio.Units;
                        money += client_salida.Bio.Units * 15;
                        documents++;
                    }

                    foreach (var item in client_salida.Workdays_Clients.Where(n => n.BilledDate == null))
                    {
                        if (item.Note != null)
                        {
                            cantUnit += 16;
                            money += 16 * 9;
                            documents++;
                        }
                        else
                        {
                            if (item.NoteP != null)
                            {
                                cantUnit += item.NoteP.RealUnits;
                                money += item.NoteP.RealUnits * 9;
                                documents++;
                            }
                            else
                            {
                                if (item.IndividualNote != null)
                                {
                                    cantUnit += 4;
                                    money += 4 * 12;
                                    documents++;
                                }
                                else
                                {
                                    if (item.Workday.Service == ServiceType.Group)
                                    {
                                        if (item.GroupNote2 != null)
                                        {
                                            cantUnit += item.GroupNote2.GroupNotes2_Activities.Count() * 4;
                                            money += item.GroupNote2.GroupNotes2_Activities.Count() * 4 * 7;
                                            documents++;
                                        }
                                        else
                                        {
                                            cantUnit += item.Schedule.SubSchedules.Count() * 4;
                                            money += item.Schedule.SubSchedules.Count() * 4 * 7;
                                            documents++;
                                        }
                                    }
                                    else
                                    {
                                        cantUnit += 16;
                                        money += 16 * 9;
                                        documents++;
                                    }
                                }
                            }
                        }
                    }
                   

                }
                ViewData["Units"] = cantUnit;
                ViewData["Money"] = money;
                ViewData["Billed"] = billed;
                ViewData["Client"] = idClient;
                ViewData["ClientName"] = clientName;
                ViewData["Code"] = code;
                ViewData["Diagnostics"] = diagnostics;
                ViewData["MedicaidId"] = medicaidId;
                ViewData["BirthDate"] = birthdate;
                ViewData["HealthInsurance"] = healthInsurance;
                ViewData["MemberID"] = memberID;
                ViewData["Documents"] = documents;
                if (documents == 0)
                {
                    ViewData["CountClient"] = 0;
                }
                else
                {
                    ViewData["CountClient"] = 1;
                }

                return View(client_salida);
            }
            else
            {
                ClientEntity client_salida = await _context.Clients
                                                    .Include(wc => wc.Workdays_Clients)
                                                    .ThenInclude(wc => wc.Note)

                                                    .Include(wc => wc.Workdays_Clients)
                                                    .ThenInclude(wc => wc.NoteP)

                                                    .Include(wc => wc.Workdays_Clients)
                                                    .ThenInclude(wc => wc.IndividualNote)

                                                    .Include(wc => wc.Workdays_Clients)
                                                    .ThenInclude(wc => wc.GroupNote)

                                                    .Include(wc => wc.Workdays_Clients)
                                                    .ThenInclude(wc => wc.GroupNote2)
                                                    .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                    .Include(c => c.Clients_Diagnostics)
                                                    .ThenInclude(cd => cd.Diagnostic)

                                                    .Include(c => c.Clients_HealthInsurances)
                                                    .ThenInclude(c => c.HealthInsurance)

                                                    .Include(wc => wc.Workdays_Clients)
                                                    .ThenInclude(wc => wc.Workday)
                                                    .ThenInclude(w => w.Week)

                                                    .Include(wc => wc.Workdays_Clients)
                                                    .ThenInclude(wc => wc.Facilitator)

                                                    .Include(wc => wc.Workdays_Clients)
                                                    .ThenInclude(wc => wc.Schedule)
                                                    .ThenInclude(wc => wc.SubSchedules)

                                                    .Include(wc => wc.MTPs)
                                                    .Include(wc => wc.Bio)

                                                    .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                        && wc.Id == idClient
                                                        && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                          && wc.BilledDate != null
                                                                                          && wc.Hold == false).Count() > 0)
                                                      || (wc.MTPs.Where(n => n.BilledDate != null).Count() > 0)
                                                      || (wc.Bio.BilledDate != null))));

                if (client_salida != null)
                {
                    foreach (var item in client_salida.MTPs)
                    {
                        if (item.BilledDate != null)
                        {
                            cantUnit += item.Units;
                            money += item.Units * 15;
                            documents++;
                        }
                    }

                    if (client_salida.Bio.BilledDate != null)
                    {
                        cantUnit += client_salida.Bio.Units;
                        money += client_salida.Bio.Units * 15;
                        documents++;
                    }

                    foreach (var item in client_salida.Workdays_Clients.Where(n => n.BilledDate != null))
                    {
                        if (item.Note != null)
                        {
                            cantUnit += 16;
                            money += 16 * 9;
                            documents++;
                        }
                        else
                        {
                            if (item.NoteP != null)
                            {
                                cantUnit += item.NoteP.RealUnits;
                                money += item.NoteP.RealUnits * 9;
                                documents++;
                            }
                            else
                            {
                                if (item.IndividualNote != null)
                                {
                                    cantUnit += 4;
                                    money += 4 * 12;
                                    documents++;
                                }
                                else
                                {
                                    if (item.Workday.Service == ServiceType.Group)
                                    {
                                        if (item.GroupNote2 != null)
                                        {
                                            cantUnit += item.GroupNote2.GroupNotes2_Activities.Count() * 4;
                                            money += item.GroupNote2.GroupNotes2_Activities.Count() * 4 * 7;
                                            documents++;
                                        }
                                        else
                                        {
                                            cantUnit += item.Schedule.SubSchedules.Count() * 4;
                                            money += item.Schedule.SubSchedules.Count() * 4 * 7;
                                            documents++;
                                        }
                                    }
                                    else
                                    {
                                        cantUnit += 16;
                                        money += 16 * 9;
                                        documents++;
                                    }
                                }
                            }
                        }
                    }
                }
                
                ViewData["Units"] = cantUnit;
                ViewData["Money"] = money;
                ViewData["Billed"] = billed;
                ViewData["Client"] = idClient;
                ViewData["ClientName"] = clientName;
                ViewData["Code"] = code;
                ViewData["Diagnostics"] = diagnostics;
                ViewData["MedicaidId"] = medicaidId;
                ViewData["BirthDate"] = birthdate;
                ViewData["HealthInsurance"] = healthInsurance;
                ViewData["MemberID"] = memberID;
                ViewData["Documents"] = documents;
                if (documents == 0)
                {
                    ViewData["CountClient"] = 0;
                }
                else
                {
                    ViewData["CountClient"] = 1;
                }

                return View(client_salida);
            }

        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillNoteTodayClient(int id = 0)
        {
            if (id != 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients
                                                              .Include(wc => wc.Client)
                                                             
                                                              .Where(wc => wc.Id == id)
                                                              .FirstOrDefaultAsync();
                
                workday_client.BilledDate = DateTime.Now;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingClient", "Notes", new { idClient = workday_client.Client.Id, biled = 0 });
            }

            return RedirectToAction("NotAuthorized", "Account");
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> NotClientBill(int id = 0)
        {
            if (id != 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients
                                                              .Include(n => n.Client)
                                                              .Where(wc => wc.Id == id)
                                                              .FirstOrDefaultAsync();
                
                workday_client.BilledDate = null;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingClient", "Notes", new { idClient = workday_client.Client.Id, billed = 1 });
            }

            return RedirectToAction("NotAuthorized", "Account");
        }

        [Authorize(Roles = "Manager")]
        public IActionResult BillNoteClient(int idWorkday, int abilled = 0)
        {
            BillViewModel model = new BillViewModel { Id = idWorkday, BilledDate = DateTime.Now };
            ViewData["Billed"] = abilled;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillNoteClient(BillViewModel model, int abilled = 0)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                Workday_Client workday_client;
                workday_client = await _context.Workdays_Clients
                                               .Include(wc => wc.Client)
                                               .Include(wc => wc.Workday)
                                               .ThenInclude(wc => wc.Week)

                                               .Where(wc => wc.Id == model.Id)
                                               .FirstOrDefaultAsync();

                workday_client.BilledDate = model.BilledDate;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                ClientEntity client = new ClientEntity();
                
                if (abilled == 0)
                {
                    client = await _context.Clients

                                       .Include(c => c.Clients_Diagnostics)
                                       .ThenInclude(cd => cd.Diagnostic)

                                       .Include(c => c.Clients_HealthInsurances)
                                       .ThenInclude(c => c.HealthInsurance)

                                       .Include(wc => wc.MTPs)
                                       .Include(wc => wc.Bio)

                                       .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                && wc.Id == workday_client.Client.Id
                                                && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                            && wc.BilledDate == null
                                                                            && wc.Hold == false).Count() > 0)
                                                  || (wc.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate
                                                                      && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate
                                                                      && n.BilledDate == null).Count() > 0)
                                                  || (wc.Bio.DateBio >= workday_client.Workday.Week.InitDate
                                                                      && wc.Bio.DateBio <= workday_client.Workday.Week.FinalDate
                                                                      && wc.Bio.BilledDate == null))));

                    if (client.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate && n.BilledDate == null).Count() == 0)
                    {
                        client.MTPs = null;
                    }

                    if (client.Bio != null)
                    {
                        if (client.Bio.DateBio != null)
                        {
                            client.Bio = null;
                        }
                    }

                    client.Workdays_Clients = _context.Workdays_Clients
                                                      .Include(wc => wc.Note)

                                                      .Include(wc => wc.NoteP)

                                                      .Include(wc => wc.IndividualNote)

                                                      .Include(wc => wc.GroupNote)

                                                      .Include(wc => wc.GroupNote2)
                                                      .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                      .Include(wc => wc.Workday)
                                                      .ThenInclude(w => w.Week)

                                                      .Include(wc => wc.Facilitator)

                                                      .Include(wc => wc.Schedule)
                                                      .ThenInclude(wc => wc.SubSchedules)

                                                      .Where(wc => wc.Present == true
                                                                && wc.BilledDate == null
                                                                && wc.Hold == false
                                                                && wc.Client.Id == client.Id)
                                                      .ToList();


                }
                else
                {
                    client = await _context.Clients

                                         .Include(c => c.Clients_Diagnostics)
                                         .ThenInclude(cd => cd.Diagnostic)

                                         .Include(c => c.Clients_HealthInsurances)
                                         .ThenInclude(c => c.HealthInsurance)

                                         .Include(wc => wc.MTPs)
                                         .Include(wc => wc.Bio)

                                         .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                  && wc.Id == workday_client.Client.Id
                                                  && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                              && wc.BilledDate != null
                                                                              && wc.Hold == false).Count() > 0)
                                                    || (wc.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate
                                                                        && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate
                                                                        && n.BilledDate != null).Count() > 0)
                                                    || (wc.Bio.DateBio >= workday_client.Workday.Week.InitDate
                                                                        && wc.Bio.DateBio <= workday_client.Workday.Week.FinalDate
                                                                        && wc.Bio.BilledDate != null))));

                    if (client.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate && n.BilledDate != null).Count() == 0)
                    {
                        client.MTPs = null;
                    }

                    if (client.Bio != null)
                    {
                        if (client.Bio.DateBio == null)
                        {
                            client.Bio = null;
                        }
                    }

                    client.Workdays_Clients = _context.Workdays_Clients
                                                      .Include(wc => wc.Note)

                                                      .Include(wc => wc.NoteP)

                                                      .Include(wc => wc.IndividualNote)

                                                      .Include(wc => wc.GroupNote)

                                                      .Include(wc => wc.GroupNote2)
                                                      .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                      .Include(wc => wc.Workday)
                                                      .ThenInclude(w => w.Week)

                                                      .Include(wc => wc.Facilitator)

                                                      .Include(wc => wc.Schedule)
                                                      .ThenInclude(wc => wc.SubSchedules)

                                                      .Where(wc => wc.Present == true
                                                                && wc.BilledDate != null
                                                                && wc.Hold == false
                                                                && wc.Client.Id == client.Id)
                                                      .ToList();

                }
                ViewData["Billed"] = abilled;

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingClient", client) });
            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "BillNoteClient", model) });
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PaymentReceivedTodayClient(int idWorkclient = 0)
        {
            if (idWorkclient != 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients
                                                              .Include(wc => wc.Client)
                      
                                                              .Where(wc => wc.Id == idWorkclient)
                                                              .FirstOrDefaultAsync();
                workday_client.PaymentDate = DateTime.Now;
                workday_client.DeniedBill = false;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingClient", "Notes", new { idClient = workday_client.Client.Id, billed = 1 });
            }

            return RedirectToAction("NotAuthorized", "Account");

        }

        [Authorize(Roles = "Manager")]
        public IActionResult PaymentReceivedClient(int id)
        {
            PaymentReceivedViewModel model = new PaymentReceivedViewModel { Id = id, PaymentDate = DateTime.Now };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PaymentReceivedClient(PaymentReceivedViewModel model)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
             if (ModelState.IsValid)
            {
                Workday_Client workday_client;
                ClientEntity client = new ClientEntity();
                workday_client = await _context.Workdays_Clients
                                               .Include(wc => wc.Client)
                                               .Include(wc => wc.Workday)
                                               .ThenInclude(w => w.Week)
                                               .ThenInclude(we => we.Clinic)

                                               .Where(wc => wc.Id == model.Id)
                                               .FirstOrDefaultAsync();

                workday_client.PaymentDate = model.PaymentDate;
                workday_client.DeniedBill = false;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                client = await _context.Clients

                                        .Include(c => c.Clients_Diagnostics)
                                        .ThenInclude(cd => cd.Diagnostic)

                                        .Include(c => c.Clients_HealthInsurances)
                                        .ThenInclude(c => c.HealthInsurance)

                                        .Include(wc => wc.MTPs)
                                        .Include(wc => wc.Bio)

                                        .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                 && wc.Id == workday_client.Client.Id
                                                 && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                             && wc.BilledDate != null
                                                                             && wc.Hold == false).Count() > 0)
                                                   || (wc.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate
                                                                       && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate
                                                                       && n.BilledDate != null).Count() > 0)
                                                   || (wc.Bio.DateBio >= workday_client.Workday.Week.InitDate
                                                                       && wc.Bio.DateBio <= workday_client.Workday.Week.FinalDate
                                                                       && wc.Bio.BilledDate != null))));

                if (client.MTPs.Where(n => n.AdmissionDateMTP >= workday_client.Workday.Week.InitDate && n.AdmissionDateMTP <= workday_client.Workday.Week.FinalDate && n.BilledDate != null).Count() == 0)
                {
                    client.MTPs = null;
                }

                if (client.Bio != null)
                {
                    if (client.Bio.DateBio == null)
                    {
                        client.Bio = null;
                    }
                }

                client.Workdays_Clients = _context.Workdays_Clients
                                                  .Include(wc => wc.Note)

                                                  .Include(wc => wc.NoteP)

                                                  .Include(wc => wc.IndividualNote)

                                                  .Include(wc => wc.GroupNote)

                                                  .Include(wc => wc.GroupNote2)
                                                  .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                  .Include(wc => wc.Workday)
                                                  .ThenInclude(w => w.Week)

                                                  .Include(wc => wc.Facilitator)

                                                  .Include(wc => wc.Schedule)
                                                  .ThenInclude(wc => wc.SubSchedules)

                                                  .Where(wc => wc.Present == true
                                                            && wc.BilledDate != null
                                                            && wc.Hold == false
                                                            && wc.Client.Id == client.Id)
                                                  .ToList();

                ViewData["Billed"] = "1";
                
                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingClient", client) });
            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "PaymentReceivedClient", model) });
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> NotClientPayment(int id = 0)
        {
            if (id != 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients
                                                              .Include(n => n.Client)
                                                              .Where(wc => wc.Id == id)
                                                              .FirstOrDefaultAsync();

                workday_client.PaymentDate = null;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingClient", "Notes", new { idClient = workday_client.Client.Id, billed = 1 });
            }

            return RedirectToAction("NotAuthorized", "Account");
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> NotPaymentReceived(int id = 0, int week = 0)
        {
            if (week == 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients
                                                              .Include(n => n.Client)
                                                              .Include(n => n.Workday)
                                                              .ThenInclude(n => n.Week)
                                                              .Where(wc => wc.Id == id)
                                                              .FirstOrDefaultAsync();

                workday_client.PaymentDate = null;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingWeek", "Notes", new { id = workday_client.Workday.Week.Id, billed = 1 });
            }
            else
            {
                List<Workday_Client> workday_client_List = await _context.Workdays_Clients

                                                                         .Include(wc => wc.Workday)
                                                                         .ThenInclude(w => w.Week)
                                                                         .ThenInclude(we => we.Clinic)

                                                                         .Where(wc => wc.Client.Id == id
                                                                              && wc.Workday.Week.Id == week
                                                                              && wc.BilledDate != null)
                                                                         .ToListAsync();

                foreach (var item in workday_client_List)
                {
                    item.PaymentDate = null;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }


            }

            return RedirectToAction("BillingWeek", "Notes", new { id = week, billed = 1 });
        }

        [Authorize(Roles = "Manager")]
        public IActionResult EXCEL(int idWeek, int all = 0)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            List<Workday_Client> workday_Client = new List<Workday_Client>();

            WeekEntity week = _context.Weeks.FirstOrDefault(w => w.Id == idWeek);
            
            string Periodo = "";
            string ReportName = "SuperBill Report " + week.InitDate.ToShortDateString() + " To " + week.FinalDate.ToShortDateString() + ".xlsx";
            string data = "";
            if (all == 0)
            {
                workday_Client = _context.Workdays_Clients
                                         .Include(f => f.Facilitator)
                                         .Include(c => c.Client)
                                         .Include(w => w.Workday)

                                         .Include(w => w.Client)
                                         .ThenInclude(w => w.Clients_Diagnostics)
                                         .ThenInclude(w => w.Diagnostic)

                                         .Include(w => w.Client)
                                         .ThenInclude(w => w.Clients_HealthInsurances)
                                         .ThenInclude(w => w.HealthInsurance)

                                         .Include(w => w.Note)
                                         .Include(w => w.NoteP)
                                         .Include(w => w.IndividualNote)
                                         .Include(w => w.GroupNote)

                                         .Where(n => n.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                               && n.Workday.Week.Id == idWeek
                                               && n.Present == true
                                               && n.Client != null
                                               && n.BilledDate == null)
                                         .OrderBy(n => n.Client.Name)
                                         .ThenBy(n => n.Workday.Date)
                                         .ToList();
                Periodo = week.InitDate.ToLongDateString() + " - " + week.FinalDate.ToLongDateString();
                data = "NOT BILLED";
            }
            else
            {
                workday_Client = _context.Workdays_Clients
                                             .Include(f => f.Facilitator)
                                             .Include(c => c.Client)
                                             .Include(w => w.Workday)

                                             .Include(w => w.Client)
                                             .ThenInclude(w => w.Clients_Diagnostics)
                                             .ThenInclude(w => w.Diagnostic)

                                             .Include(w => w.Client)
                                             .ThenInclude(w => w.Clients_HealthInsurances)
                                             .ThenInclude(w => w.HealthInsurance)

                                             .Include(w => w.Note)
                                             .Include(w => w.NoteP)
                                             .Include(w => w.IndividualNote)
                                             .Include(w => w.GroupNote)

                                             .Where(n => n.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                   && n.Workday.Week.Id == idWeek
                                                   && n.Present == true
                                                   && n.Client != null)
                                             .OrderBy(n => n.Client.Name)
                                             .ThenBy( n => n.Workday.Date)
                                             .ToList();

                Periodo = week.InitDate.ToLongDateString() + " - " + week.FinalDate.ToLongDateString();
                data = "ALL DATA";
            }
            

            byte[] content = _exportExcelHelper.ExportBillForWeekHelper(workday_Client, Periodo,week.Clinic.Name, data);

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ReportName);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult EXCELforClient(int idClient)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            List<Workday_Client> workday_Client = new List<Workday_Client>();

            ClientEntity client = _context.Clients.Include(n => n.Clinic).FirstOrDefault(c => c.Id == idClient);

            string Periodo = "";
            string ReportName = "SuperBill Report " + client.Name + ".xlsx";
            string data = "";
            
            workday_Client = _context.Workdays_Clients
                                     .Include(f => f.Facilitator)
                                     .Include(c => c.Client)
                                     .Include(w => w.Workday)

                                     .Include(w => w.Client)
                                     .ThenInclude(w => w.Clients_Diagnostics)
                                     .ThenInclude(w => w.Diagnostic)

                                     .Include(w => w.Client)
                                     .ThenInclude(w => w.Clients_HealthInsurances)
                                     .ThenInclude(w => w.HealthInsurance)

                                     .Include(w => w.Note)
                                     .Include(w => w.NoteP)
                                     .Include(w => w.IndividualNote)
                                     .Include(w => w.GroupNote)

                                     .Where(n => n.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                            && n.Present == true
                                            && n.Client != null
                                            && n.Client.Id == client.Id)
                                     .OrderBy(n => n.Client.Name)
                                     .ToList();
                Periodo = client.AdmisionDate.ToLongDateString() + " - " + client.DateOfClose.ToLongDateString();
                data = "ALL RECORD";
           
            byte[] content = _exportExcelHelper.ExportBillForWeekHelper(workday_Client, Periodo, client.Clinic.Name, data);

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ReportName);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeniedBillClient(int idWorkclient = 0)
        {
            if (idWorkclient != 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients
                                                              .Include(wc => wc.Client)

                                                              .Where(wc => wc.Id == idWorkclient)
                                                              .FirstOrDefaultAsync();
                workday_client.DeniedBill = true;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingClient", "Notes", new { idClient = workday_client.Client.Id, billed = 1 });
            }

            return RedirectToAction("NotAuthorized", "Account");

        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeniedBillToday(int id = 0, int week = 0)
        {
            if (week == 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients

                                                            .Include(wc => wc.Workday)
                                                            .ThenInclude(w => w.Week)
                                                            .ThenInclude(we => we.Clinic)

                                                            .Where(wc => wc.Id == id)
                                                            .FirstOrDefaultAsync();
                workday_client.DeniedBill = true;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                return RedirectToAction("BillingWeek", "Notes", new { id = workday_client.Workday.Week.Id, billed = 1 });
            }
            else
            {
                List<Workday_Client> workday_client = await _context.Workdays_Clients

                                                                        .Include(wc => wc.Workday)
                                                                        .ThenInclude(w => w.Week)
                                                                        .ThenInclude(we => we.Clinic)

                                                                        .Where(wc => wc.Client.Id == id
                                                                           && wc.Workday.Week.Id == week
                                                                           && wc.BilledDate != null)
                                                                        .ToListAsync();

                foreach (var item in workday_client)
                {
                    item.DeniedBill = true;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }


            }

            return RedirectToAction("BillingWeek", "Notes", new { id = week, billed = 1 });

        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> NotDeniedBill(int idWorkclient = 0, int client = 0)
        {
            if (idWorkclient != 0)
            {
                Workday_Client workday_client = await _context.Workdays_Clients
                                                              .Include(wc => wc.Client)
                                                              .Include(wc => wc.Workday)
                                                              .ThenInclude(wc => wc.Week)

                                                              .Where(wc => wc.Id == idWorkclient)
                                                              .FirstOrDefaultAsync();
                workday_client.DeniedBill = false;
                _context.Update(workday_client);
                await _context.SaveChangesAsync();

                if (client == 0)
                {
                    return RedirectToAction("BillingWeek", "Notes", new { id = workday_client.Workday.Week.Id, billed = 1 }); 
                }
                else
                {
                    return RedirectToAction("BillingClient", "Notes", new { idClient = workday_client.Client.Id, billed = 1 });
                }

                
            }

            return RedirectToAction("NotAuthorized", "Account");

        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillingReportHold(int idWeek = 0)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user_logged.Clinic == null)
            {
                return RedirectToAction("Home/Error404");
            }

            if (idWeek != 0)
            {
                IQueryable<WeekEntity> query = _context.Weeks

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(g => g.Facilitator)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Note)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.NoteP)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Client)
                                                       .ThenInclude(c => c.Clients_Diagnostics)
                                                       .ThenInclude(cd => cd.Diagnostic)

                                                       .Include(w => w.Clinic)

                                                       .Where(w => (w.Clinic.Id == user_logged.Clinic.Id && w.Id == idWeek));

                try
                {
                    BillingReport1ViewModel model = new BillingReport1ViewModel
                    {
                        IdFacilitator = 0,
                        Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                        IdClient = 0,
                        Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                        Weeks = query.ToList(),
                        IdWeek = idWeek,
                        WeeksListName = _combosHelper.GetComboWeeksNameByClinic(user_logged.Clinic.Id)
                    };

                    return View(model);
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(BillingReport));
                }
            }
            else
            {
                int max = _context.Weeks
                                  .Where(m => m.Clinic.Id == user_logged.Clinic.Id)
                                  .Max(m => m.Id);

                IQueryable<WeekEntity> query = _context.Weeks

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(g => g.Facilitator)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Note)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.NoteP)

                                                       .Include(w => w.Days)
                                                       .ThenInclude(d => d.Workdays_Clients)
                                                       .ThenInclude(wc => wc.Client)
                                                       .ThenInclude(c => c.Clients_Diagnostics)
                                                       .ThenInclude(cd => cd.Diagnostic)

                                                       .Include(w => w.Clinic)

                                                       .Where(w => (w.Clinic.Id == user_logged.Clinic.Id && w.Id == max));

                BillingReport1ViewModel model = new BillingReport1ViewModel
                {
                    IdFacilitator = 0,
                    Facilitators = _combosHelper.GetComboFacilitatorsByClinic(user_logged.Clinic.Id),
                    IdClient = 0,
                    Clients = _combosHelper.GetComboClientsByClinic(user_logged.Clinic.Id),
                    Weeks = query.ToList(),
                    IdWeek = max,
                    WeeksListName = _combosHelper.GetComboWeeksNameByClinic(user_logged.Clinic.Id)
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public IActionResult BillingReportHold(BillingReport1ViewModel model)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(BillingReportHold), new { idWeek = model.IdWeek });
            }

            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult EXCEL_Hold(int idWeek, int all = 0)
        {
            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            List<Workday_Client> workday_Client = new List<Workday_Client>();

            WeekEntity week = _context.Weeks.FirstOrDefault(w => w.Id == idWeek);

            string Periodo = "";
            string ReportName = "SuperBill Hold Report " + week.InitDate.ToShortDateString() + " To " + week.FinalDate.ToShortDateString() + ".xlsx";
            string data = "";
            if (all == 0)
            {
                workday_Client = _context.Workdays_Clients
                                         .Include(f => f.Facilitator)
                                         .Include(c => c.Client)
                                         .Include(w => w.Workday)

                                         .Include(w => w.Client)
                                         .ThenInclude(w => w.Clients_Diagnostics)
                                         .ThenInclude(w => w.Diagnostic)

                                         .Include(w => w.Client)
                                         .ThenInclude(w => w.Clients_HealthInsurances)
                                         .ThenInclude(w => w.HealthInsurance)

                                         .Include(w => w.Note)
                                         .Include(w => w.NoteP)
                                         .Include(w => w.IndividualNote)
                                         .Include(w => w.GroupNote)

                                         .Where(n => n.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                               && n.Workday.Week.Id == idWeek
                                               && n.Present == true
                                               && n.Client != null
                                               && n.Hold == true)
                                         .OrderBy(n => n.Client.Name)
                                         .ThenBy(n => n.Workday.Date)
                                         .ToList();
                Periodo = week.InitDate.ToLongDateString() + " - " + week.FinalDate.ToLongDateString();
                data = "NOT BILLED";
            }
            else
            {
                workday_Client = _context.Workdays_Clients
                                             .Include(f => f.Facilitator)
                                             .Include(c => c.Client)
                                             .Include(w => w.Workday)

                                             .Include(w => w.Client)
                                             .ThenInclude(w => w.Clients_Diagnostics)
                                             .ThenInclude(w => w.Diagnostic)

                                             .Include(w => w.Client)
                                             .ThenInclude(w => w.Clients_HealthInsurances)
                                             .ThenInclude(w => w.HealthInsurance)

                                             .Include(w => w.Note)
                                             .Include(w => w.NoteP)
                                             .Include(w => w.IndividualNote)
                                             .Include(w => w.GroupNote)

                                             .Where(n => n.Facilitator.Clinic.Id == user_logged.Clinic.Id
                                                   && n.Present == true
                                                   && n.Client != null
                                                   && n.Hold == true)
                                             .OrderBy(n => n.Client.Name)
                                             .ThenBy(n => n.Workday.Date)
                                             .ToList();
                Periodo = week.InitDate.ToLongDateString() + " - " + week.FinalDate.ToLongDateString();
                data = "ALL DATA";
            }


            byte[] content = _exportExcelHelper.ExportBillHoldForWeekHelper(workday_Client, Periodo, week.Clinic.Name, data);

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ReportName);
        }

        [Authorize(Roles = "Manager, Facilitator")]
        public async Task<IActionResult> ChangeSessionGroup(int? id = 0, string dateInterval = "")
        {
            if (id == null)
            {
                return RedirectToAction("Home/Error404");
            }

            Workday_Client wordayClient = await _context.Workdays_Clients
                                                        .Include(n => n.Facilitator)
                                                        .Include(n => n.Client)
                                                        .Include(n => n.Workday)
                                                        .Include(n => n.Schedule)
                                                        .FirstOrDefaultAsync(c => c.Id == id);
            if (wordayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            Workday_ClientViewModel wordayClientViewModel = _converterHelper.ToWorkdayClientViewModel(wordayClient, false);
            wordayClientViewModel.DateInterval = dateInterval;

            if (wordayClientViewModel.Session == "AM")
            {
                ViewData["am"] = "true";
            }
            else
            {
                ViewData["am"] = "false";
            }

            return View(wordayClientViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Facilitator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeSessionGroup(int id, Workday_ClientViewModel workdayClientModel, IFormCollection form)
        {
            if (id != workdayClientModel.Id)
            {
                return RedirectToAction("Home/Error404");
            }

            if (ModelState.IsValid)
            {
                switch (form["Meridian"])
                {
                    case "Am":
                        {
                            workdayClientModel.Session = "AM";
                            break;
                        }
                    case "Pm":
                        {
                            workdayClientModel.Session = "PM";
                            break;
                        }
                    default:
                        break;
                }
                UserEntity user_logged = _context.Users
                                                 .Include(u => u.Clinic)
                                                 .FirstOrDefault(u => u.UserName == User.Identity.Name);

                Workday_Client wordayClient = await _context.Workdays_Clients
                                                            .FirstOrDefaultAsync(c => c.Id == workdayClientModel.Id);

                wordayClient.Session = workdayClientModel.Session;
                wordayClient.Schedule = await _context.Schedule.FirstOrDefaultAsync(s => s.Id == workdayClientModel.IdSchedule);
                _context.Update(wordayClient);
                try
                {
                    await _context.SaveChangesAsync();

                    if (User.IsInRole("Facilitator"))
                    {
                        List<WeekEntity> week = await _context.Weeks

                                                              .Include(w => w.Days)
                                                              .ThenInclude(d => d.Workdays_Clients)
                                                              .ThenInclude(wc => wc.Client)

                                                              .Include(w => w.Days)
                                                              .ThenInclude(d => d.Workdays_Clients)
                                                              .ThenInclude(g => g.Facilitator)

                                                              .Include(w => w.Days)
                                                              .ThenInclude(d => d.Workdays_Clients)
                                                              .ThenInclude(wc => wc.GroupNote)

                                                              .Include(w => w.Days)
                                                              .ThenInclude(d => d.Workdays_Clients)
                                                              .ThenInclude(wc => wc.GroupNote2)

                                                              .Include(w => w.Days)
                                                              .ThenInclude(d => d.Workdays_Clients)
                                                              .ThenInclude(wc => wc.Schedule)

                                                              .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                        && w.Days.Where(d => (d.Service == ServiceType.Group && d.Workdays_Clients.Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name).Count() > 0)).Count() > 0))
                                                              .ToListAsync();

                        return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_ViewNotesGroup", week) });
                        
                    }
                    if (User.IsInRole("Manager"))
                    {
                        string[] date = workdayClientModel.DateInterval.Split(" - ");
                        List<WeekEntity> week = await _context.Weeks

                                                              .Include(w => w.Days)
                                                              .ThenInclude(d => d.Workdays_Clients)
                                                              .ThenInclude(wc => wc.Client)

                                                              .Include(w => w.Days)
                                                              .ThenInclude(d => d.Workdays_Clients)
                                                              .ThenInclude(g => g.Facilitator)

                                                              .Include(w => w.Days)
                                                              .ThenInclude(d => d.Workdays_Clients)
                                                              .ThenInclude(wc => wc.GroupNote)

                                                              .Include(w => w.Days)
                                                              .ThenInclude(d => d.Workdays_Clients)
                                                              .ThenInclude(wc => wc.GroupNote2)

                                                              .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                                        && w.Days.Where(d => (d.Service == ServiceType.PSR)).Count() > 0
                                                                        && w.InitDate >= Convert.ToDateTime(date[0]) && w.FinalDate <= Convert.ToDateTime(date[1])))
                                                              .ToListAsync();

                        BillingReportViewModel model = new BillingReportViewModel
                        {
                            DateIterval = workdayClientModel.DateInterval,
                            IdFacilitator = 0,
                            Facilitators = null,
                            IdClient = 0,
                            Clients = null,
                            Weeks = week
                        };

                        return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_ViewNotesForChanges", model) });
                    }
                }
                catch (System.Exception ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, $"Already exists the note: {wordayClient.ClientName}");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }
            }
            return View(workdayClientModel);
        }

        [Authorize(Roles = "Manager, Facilitator")]
        public async Task<IActionResult> ChangeSessionInd(int? id = 0, string dateInterval = "")
        {
            if (id == null)
            {
                return RedirectToAction("Home/Error404");
            }

            Workday_Client wordayClient = await _context.Workdays_Clients
                                                        .Include(n => n.Facilitator)
                                                        .Include(n => n.Client)
                                                        .Include(n => n.Workday)
                                                        .Include(n => n.Schedule)
                                                        .Include(n => n.Schedule)
                                                        .FirstOrDefaultAsync(c => c.Id == id);
            if (wordayClient == null)
            {
                return RedirectToAction("Home/Error404");
            }

            Workday_ClientViewModel wordayClientViewModel = _converterHelper.ToWorkdayClientViewModel(wordayClient, true);
            wordayClientViewModel.DateInterval = dateInterval;

            return View(wordayClientViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Facilitator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeSessionInd(int id, Workday_ClientViewModel workdayClientModel, IFormCollection form)
        {
            if (id != workdayClientModel.Id)
            {
                return RedirectToAction("Home/Error404");
            }

            UserEntity user_logged = _context.Users
                                             .Include(u => u.Clinic)
                                             .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (workdayClientModel.IdSchedule > 0)
            {
               
                Workday_Client wordayClient_origin = await _context.Workdays_Clients
                                                                   .Include(n => n.Workday)
                                                                   .Include(n => n.Client)
                                                                   .Include(n => n.Facilitator)
                                                                   .Include(n => n.IndividualNote)
                                                                   .ThenInclude(n => n.SubSchedule)
                                                                   .FirstOrDefaultAsync(c => c.Id == workdayClientModel.Id);
                
                SubScheduleEntity subScheduleSelecction = _context.SubSchedule.FirstOrDefault(n => n.Id == workdayClientModel.IdSchedule);
                string session = subScheduleSelecction.InitialTime.ToShortTimeString() + " - " + subScheduleSelecction.EndTime.ToShortTimeString();
                
                Workday_Client wordayClient_Change = await _context.Workdays_Clients
                                                                   .Include(n => n.IndividualNote)
                                                                   .Include(n => n.Client)
                                                                   .FirstOrDefaultAsync(c => c.Session == session 
                                                                        && c.Workday.Service == ServiceType.Individual 
                                                                        && c.Workday.Id == wordayClient_origin.Workday.Id
                                                                        && c.Facilitator.Id == wordayClient_origin.Facilitator.Id);
                
                IndividualNoteEntity individualNote = wordayClient_origin.IndividualNote;
                individualNote.SubSchedule = subScheduleSelecction;
                individualNote.Workday_Cient = wordayClient_Change;
                individualNote.Workday_Client_FK = wordayClient_Change.Id;

                wordayClient_Change.Client = wordayClient_origin.Client;
                wordayClient_Change.Present = true;

                wordayClient_origin.Client = null;
                wordayClient_origin.Present = false;

                _context.Update(wordayClient_Change);
                _context.Update(wordayClient_origin);
                _context.Update(individualNote);
                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (System.Exception ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, $"Already exists the note: {wordayClient_origin.ClientName}");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }
            }

            if (User.IsInRole("Facilitator"))
            {
                List<WeekEntity> week = await _context.Weeks

                                                 .Include(w => w.Days)
                                                 .ThenInclude(d => d.Workdays_Clients)
                                                 .ThenInclude(wc => wc.Client)

                                                 .Include(w => w.Days)
                                                 .ThenInclude(d => d.Workdays_Clients)
                                                 .ThenInclude(g => g.Facilitator)

                                                 .Include(w => w.Days)
                                                 .ThenInclude(d => d.Workdays_Clients)
                                                 .ThenInclude(wc => wc.IndividualNote)

                                                 .Include(w => w.Days)
                                                 .ThenInclude(d => d.Workdays_Clients)
                                                 .ThenInclude(wc => wc.Schedule)

                                                 .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                           && w.Days.Where(d => (d.Service == ServiceType.Individual && d.Workdays_Clients.Where(wc => wc.Facilitator.LinkedUser == User.Identity.Name).Count() > 0)).Count() > 0))
                                                 .ToListAsync();

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_ViewNotesInd", week) });

            }
            if (User.IsInRole("Manager"))
            {
                List<WeekEntity> week = await _context.Weeks

                                                 .Include(w => w.Days)
                                                 .ThenInclude(d => d.Workdays_Clients)
                                                 .ThenInclude(wc => wc.Client)

                                                 .Include(w => w.Days)
                                                 .ThenInclude(d => d.Workdays_Clients)
                                                 .ThenInclude(g => g.Facilitator)

                                                 .Include(w => w.Days)
                                                 .ThenInclude(d => d.Workdays_Clients)
                                                 .ThenInclude(wc => wc.IndividualNote)

                                                 .Include(w => w.Days)
                                                 .ThenInclude(d => d.Workdays_Clients)
                                                 .ThenInclude(wc => wc.Schedule)

                                                 .Where(w => (w.Clinic.Id == user_logged.Clinic.Id
                                                           && w.Days.Where(d => (d.Service == ServiceType.Individual)).Count() > 0))
                                                 .ToListAsync();

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_ViewNotesInd", week) });

            }

            return View(workdayClientModel);
        }

        #region MTP Bill
        [Authorize(Roles = "Manager")]
        public IActionResult BillMTP(int id, int week = 0, int abilled = 0, int idMtp = 0)
        {
            BillViewModel model = new BillViewModel { Id = id, BilledDate = DateTime.Now };
            ViewData["week"] = week;
            ViewData["Billed"] = abilled;
            ViewData["idMtp"] = idMtp;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillMTP(BillViewModel model, int idweek = 0, int abilled = 0, int idMtp = 0)
        {
            UserEntity user_logged = await _context.Users
                                                 .Include(u => u.Clinic)
                                                 .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                MTPEntity mtp;
                if (idweek > 0)
                {
                    mtp = await _context.MTPs
                                        .FirstOrDefaultAsync(n => n.Id == idMtp);

                    mtp.BilledDate = model.BilledDate;
                    _context.Update(mtp);
                    await _context.SaveChangesAsync();

                }

                WeekEntity week = _context.Weeks.FirstOrDefault(n => n.Id == idweek);
                List<ClientEntity> clients = new List<ClientEntity>();

                if (abilled == 0)
                {
                    clients = await _context.Clients
                                            
                                            .Include(c => c.Clients_Diagnostics)
                                            .ThenInclude(cd => cd.Diagnostic)

                                            .Include(c => c.Clients_HealthInsurances)
                                            .ThenInclude(c => c.HealthInsurance)

                                            .Include(wc => wc.MTPs)
                                            .Include(wc => wc.Bio)

                                            .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                     && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                 && wc.BilledDate == null
                                                                                 && wc.Hold == false
                                                                                 && wc.Workday.Week.Id == idweek).Count() > 0)
                                                       || (wc.MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate
                                                                           && n.AdmissionDateMTP <= week.FinalDate
                                                                           && n.BilledDate == null).Count() > 0)
                                                       || (wc.Bio.DateBio >= week.InitDate
                                                          && wc.Bio.DateBio <= week.FinalDate
                                                          && wc.Bio.BilledDate == null))))
                                               .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate && n.AdmissionDateMTP <= week.FinalDate && n.BilledDate == null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }

                        if (clients[i].Bio != null)
                        {
                            if (clients[i].Bio.DateBio >= week.InitDate && clients[i].Bio.DateBio <= week.FinalDate && clients[i].Bio.BilledDate == null)
                            {

                            }
                            else
                            {
                                clients[i].Bio = null;
                            }
                        }

                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                          .Include(wc => wc.Note)

                                                          .Include(wc => wc.NoteP)

                                                          .Include(wc => wc.IndividualNote)

                                                          .Include(wc => wc.GroupNote)

                                                          .Include(wc => wc.GroupNote2)
                                                          .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Week)

                                                          .Include(wc => wc.Facilitator)

                                                          .Include(wc => wc.Schedule)
                                                          .ThenInclude(wc => wc.SubSchedules)

                                                          .Where(wc => wc.Present == true
                                                                    && wc.BilledDate == null
                                                                    && wc.Hold == false
                                                                    && wc.Workday.Week.Id == week.Id
                                                                    && wc.Client.Id == clients[i].Id).ToList();
                    }

                }
                else
                {
                    clients = await _context.Clients
                                            
                                            .Include(c => c.Clients_Diagnostics)
                                            .ThenInclude(cd => cd.Diagnostic)

                                            .Include(c => c.Clients_HealthInsurances)
                                            .ThenInclude(c => c.HealthInsurance)

                                            .Include(wc => wc.MTPs)
                                            .Include(wc => wc.Bio)

                                            .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                     && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                 && wc.BilledDate != null
                                                                                 && wc.Hold == false
                                                                                 && wc.Workday.Week.Id == idweek).Count() > 0)
                                                       || (wc.MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate
                                                                           && n.AdmissionDateMTP <= week.FinalDate
                                                                           && n.BilledDate != null).Count() > 0)
                                                       || (wc.Bio.DateBio >= week.InitDate
                                                          && wc.Bio.DateBio <= week.FinalDate
                                                          && wc.Bio.BilledDate != null))))
                                            .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate && n.AdmissionDateMTP <= week.FinalDate && n.BilledDate != null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }

                        if (clients[i].Bio != null)
                        {
                            if (clients[i].Bio.DateBio >= week.InitDate && clients[i].Bio.DateBio <= week.FinalDate && clients[i].Bio.BilledDate != null)
                            {

                            }
                            else
                            {
                                clients[i].Bio = null;
                            }
                        }

                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                          .Include(wc => wc.Note)

                                                          .Include(wc => wc.NoteP)

                                                          .Include(wc => wc.IndividualNote)

                                                          .Include(wc => wc.GroupNote)

                                                          .Include(wc => wc.GroupNote2)
                                                          .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Week)

                                                          .Include(wc => wc.Facilitator)

                                                          .Include(wc => wc.Schedule)
                                                          .ThenInclude(wc => wc.SubSchedules)

                                                          .Where(wc => wc.Present == true
                                                                    && wc.BilledDate != null
                                                                    && wc.Hold == false
                                                                    && wc.Workday.Week.Id == week.Id
                                                                    && wc.Client.Id == clients[i].Id).ToList();
                    }

                }

                ViewData["Billed"] = abilled;
                ViewData["idWeek"] = idweek;

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingWeek", clients) });
            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "BillNote", model) });
        }

        [Authorize(Roles = "Manager")]
        public IActionResult PaymentReceivedMTP(int id, int week = 0, int idMtp = 0)
        {
            PaymentReceivedViewModel model = new PaymentReceivedViewModel { Id = id, PaymentDate = DateTime.Now };
            ViewData["week"] = week;
            ViewData["idMtp"] = idMtp;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PaymentReceivedMTP(PaymentReceivedViewModel model, int idWeek = 0, int idMtp = 0)
        {
            UserEntity user_logged = await _context.Users
                                                  .Include(u => u.Clinic)
                                                  .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                MTPEntity mtp;
                WeekEntity week = _context.Weeks.FirstOrDefault(n => n.Id == idWeek);
                List<ClientEntity> clients = new List<ClientEntity>();
                if (idWeek > 0)
                {

                    mtp = await _context.MTPs
                                        .FirstOrDefaultAsync(m => m.Id == idMtp);

                    mtp.PaymentDate = model.PaymentDate;
                    mtp.DeniedBill = false;
                    _context.Update(mtp);
                    await _context.SaveChangesAsync();

                    clients = await _context.Clients
                                           
                                            .Include(c => c.Clients_Diagnostics)
                                            .ThenInclude(cd => cd.Diagnostic)

                                            .Include(c => c.Clients_HealthInsurances)
                                            .ThenInclude(c => c.HealthInsurance)

                                            .Include(wc => wc.MTPs)
                                            .Include(wc => wc.Bio)

                                            .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                     && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                 && wc.BilledDate != null
                                                                                 && wc.Hold == false
                                                                                 && wc.Workday.Week.Id == idWeek).Count() > 0)
                                                       || (wc.MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate
                                                                           && n.AdmissionDateMTP <= week.FinalDate
                                                                           && n.BilledDate != null).Count() > 0)
                                                       || (wc.Bio.DateBio >= week.InitDate
                                                          && wc.Bio.DateBio <= week.FinalDate
                                                          && wc.Bio.BilledDate != null))))
                                            .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate && n.AdmissionDateMTP <= week.FinalDate && n.BilledDate != null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }

                        if (clients[i].Bio != null)
                        {
                            if (clients[i].Bio.DateBio >= week.InitDate && clients[i].Bio.DateBio <= week.FinalDate && clients[i].Bio.BilledDate != null)
                            {

                            }
                            else
                            {
                                clients[i].Bio = null;
                            }
                        }

                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                              .Include(wc => wc.Note)

                                                              .Include(wc => wc.NoteP)

                                                              .Include(wc => wc.IndividualNote)

                                                              .Include(wc => wc.GroupNote)

                                                              .Include(wc => wc.GroupNote2)
                                                              .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                              .Include(wc => wc.Workday)
                                                              .ThenInclude(w => w.Week)

                                                              .Include(wc => wc.Facilitator)

                                                              .Include(wc => wc.Schedule)
                                                              .ThenInclude(wc => wc.SubSchedules)

                                                              .Where(wc => wc.Present == true
                                                                        && wc.BilledDate != null
                                                                        && wc.Hold == false
                                                                        && wc.Workday.Week.Id == week.Id
                                                                        && wc.Client.Id == clients[i].Id).ToList();
                    }

                    ViewData["Billed"] = "1";
                    ViewData["idWeek"] = idWeek;

                }


                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingWeek", clients) });

            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "PaymentReceived", model) });
        }

        [Authorize(Roles = "Manager")]
        public IActionResult BillMTPClient(int idClient, int abilled = 0, int idMtp = 0)
        {
            BillViewModel model = new BillViewModel { Id = idClient, BilledDate = DateTime.Now };
            ViewData["Billed"] = abilled;
            ViewData["MTP"] = idMtp;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillMTPClient(BillViewModel model, int abilled = 0, int idMtp = 0)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                MTPEntity mtp = await _context.MTPs
                                              .FirstOrDefaultAsync(wc => wc.Id == idMtp);

                ClientEntity client;
                
                mtp.BilledDate = model.BilledDate;
                _context.Update(mtp);
                await _context.SaveChangesAsync();

                if (abilled == 0)
                {
                    client = await _context.Clients
                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Note)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.NoteP)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.IndividualNote)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.GroupNote)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.GroupNote2)
                                           .ThenInclude(wc => wc.GroupNotes2_Activities)

                                           .Include(c => c.Clients_Diagnostics)
                                           .ThenInclude(cd => cd.Diagnostic)

                                           .Include(c => c.Clients_HealthInsurances)
                                           .ThenInclude(c => c.HealthInsurance)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Workday)
                                           .ThenInclude(w => w.Week)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Facilitator)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Schedule)
                                           .ThenInclude(wc => wc.SubSchedules)

                                           .Include(wc => wc.MTPs)
                                           .Include(wc => wc.Bio)

                                           .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                    && wc.Id == model.Id
                                                    && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                && wc.BilledDate == null
                                                                                && wc.Hold == false).Count() > 0)
                                                      || (wc.MTPs.Where(n => n.BilledDate == null).Count() > 0)
                                                      || (wc.Bio.BilledDate == null))));

                }
                else
                {
                    client = await _context.Clients
                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Note)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.NoteP)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.IndividualNote)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.GroupNote)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.GroupNote2)
                                           .ThenInclude(wc => wc.GroupNotes2_Activities)

                                           .Include(c => c.Clients_Diagnostics)
                                           .ThenInclude(cd => cd.Diagnostic)

                                           .Include(c => c.Clients_HealthInsurances)
                                           .ThenInclude(c => c.HealthInsurance)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Workday)
                                           .ThenInclude(w => w.Week)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Facilitator)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Schedule)
                                           .ThenInclude(wc => wc.SubSchedules)

                                           .Include(wc => wc.MTPs)
                                           .Include(wc => wc.Bio)

                                           .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                    && wc.Id == model.Id
                                                    && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                && wc.BilledDate != null
                                                                                && wc.Hold == false).Count() > 0)
                                                      || (wc.MTPs.Where(n => n.BilledDate != null).Count() > 0)
                                                      || (wc.Bio.BilledDate == null))));

                }
                ViewData["Billed"] = abilled;

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingClient", client) });
            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "BillNoteClient", model) });
        }

        [Authorize(Roles = "Manager")]
        public IActionResult PaymentReceivedClientMTP(int id)
        {
            PaymentReceivedViewModel model = new PaymentReceivedViewModel { Id = id, PaymentDate = DateTime.Now };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PaymentReceivedClientMTP(PaymentReceivedViewModel model)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                MTPEntity mtp;
                mtp = await _context.MTPs
                                    .Include(n => n.Client)
                                    .FirstOrDefaultAsync(wc => wc.Id == model.Id);

                mtp.PaymentDate = model.PaymentDate;
                mtp.DeniedBill = false;
                _context.Update(mtp);
                await _context.SaveChangesAsync();

                ClientEntity client;
                client = await _context.Clients
                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.Note)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.NoteP)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.IndividualNote)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.GroupNote)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.GroupNote2)
                                       .ThenInclude(wc => wc.GroupNotes2_Activities)

                                       .Include(c => c.Clients_Diagnostics)
                                       .ThenInclude(cd => cd.Diagnostic)

                                       .Include(c => c.Clients_HealthInsurances)
                                       .ThenInclude(c => c.HealthInsurance)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.Workday)
                                       .ThenInclude(w => w.Week)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.Facilitator)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.Schedule)
                                       .ThenInclude(wc => wc.SubSchedules)

                                       .Include(wc => wc.MTPs)
                                       .Include(wc => wc.Bio)

                                       .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                    && wc.Id == mtp.Client.Id
                                                    && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                && wc.BilledDate != null
                                                                                && wc.Hold == false).Count() > 0)
                                                      || (wc.Bio.BilledDate != null)
                                                      || (wc.MTPs.Where(n => n.BilledDate != null).Count() > 0))));
                ViewData["Billed"] = "1";

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingClient", client) });
            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "PaymentReceivedClient", model) });
        }

        #endregion

        #region BIO Bill
        [Authorize(Roles = "Manager")]
        public IActionResult BillBIO(int id, int week = 0, int abilled = 0, int idBio = 0)
        {
            BillViewModel model = new BillViewModel { Id = id, BilledDate = DateTime.Now };
            ViewData["week"] = week;
            ViewData["Billed"] = abilled;
            ViewData["idBio"] = idBio;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillBIO(BillViewModel model, int idweek = 0, int abilled = 0, int idBio = 0)
        {
            UserEntity user_logged = await _context.Users
                                                 .Include(u => u.Clinic)
                                                 .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                BioEntity bio;
                if (idweek > 0)
                {
                    bio = await _context.Bio
                                        .FirstOrDefaultAsync(n => n.Id == idBio);

                    bio.BilledDate = model.BilledDate;
                    _context.Update(bio);
                    await _context.SaveChangesAsync();

                }

                WeekEntity week = _context.Weeks.FirstOrDefault(n => n.Id == idweek);
                List<ClientEntity> clients = new List<ClientEntity>();

                if (abilled == 0)
                {
                    clients = await _context.Clients

                                            .Include(c => c.Clients_Diagnostics)
                                            .ThenInclude(cd => cd.Diagnostic)

                                            .Include(c => c.Clients_HealthInsurances)
                                            .ThenInclude(c => c.HealthInsurance)

                                            .Include(wc => wc.Bio)
                                            .Include(wc => wc.MTPs)

                                            .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                     && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                 && wc.BilledDate == null
                                                                                 && wc.Hold == false
                                                                                 && wc.Workday.Week.Id == idweek).Count() > 0)
                                                       || (wc.Bio.DateBio >= week.InitDate
                                                          && wc.Bio.DateBio <= week.FinalDate
                                                          && wc.Bio.BilledDate == null)
                                                       || (wc.MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate
                                                                           && n.AdmissionDateMTP <= week.FinalDate
                                                                           && n.BilledDate == null).Count() > 0))))
                                            .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate && n.AdmissionDateMTP <= week.FinalDate && n.BilledDate == null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }
                        if (clients[i].Bio != null)
                        { 
                            if (clients[i].Bio.DateBio >= week.InitDate && clients[i].Bio.DateBio <= week.FinalDate && clients[i].Bio.BilledDate == null)
                            {
                        
                            }
                            else
                            {
                                clients[i].Bio = null;
                            }
                        }
                        
                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                          .Include(wc => wc.Note)

                                                          .Include(wc => wc.NoteP)

                                                          .Include(wc => wc.IndividualNote)

                                                          .Include(wc => wc.GroupNote)

                                                          .Include(wc => wc.GroupNote2)
                                                          .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Week)

                                                          .Include(wc => wc.Facilitator)

                                                          .Include(wc => wc.Schedule)
                                                          .ThenInclude(wc => wc.SubSchedules)

                                                          .Where(wc => wc.Present == true
                                                                    && wc.BilledDate == null
                                                                    && wc.Hold == false
                                                                    && wc.Workday.Week.Id == week.Id
                                                                    && wc.Client.Id == clients[i].Id).ToList();
                    }

                }
                else
                {
                    clients = await _context.Clients

                                            .Include(c => c.Clients_Diagnostics)
                                            .ThenInclude(cd => cd.Diagnostic)

                                            .Include(c => c.Clients_HealthInsurances)
                                            .ThenInclude(c => c.HealthInsurance)

                                            .Include(wc => wc.Bio)
                                            .Include(wc => wc.MTPs)

                                            .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                     && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                 && wc.BilledDate != null
                                                                                 && wc.Hold == false
                                                                                 && wc.Workday.Week.Id == idweek).Count() > 0)
                                                       || (wc.Bio.DateBio >= week.InitDate
                                                          && wc.Bio.DateBio <= week.FinalDate
                                                          && wc.Bio.BilledDate != null)
                                                       || (wc.MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate
                                                                           && n.AdmissionDateMTP <= week.FinalDate
                                                                           && n.BilledDate != null).Count() > 0))))
                                            .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate && n.AdmissionDateMTP <= week.FinalDate && n.BilledDate != null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }

                        if (clients[i].Bio != null)
                        {
                            if (clients[i].Bio.DateBio >= week.InitDate && clients[i].Bio.DateBio <= week.FinalDate && clients[i].Bio.BilledDate != null)
                            {

                            }
                            else
                            {
                                clients[i].MTPs = null;
                            }
                        }                        

                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                          .Include(wc => wc.Note)

                                                          .Include(wc => wc.NoteP)

                                                          .Include(wc => wc.IndividualNote)

                                                          .Include(wc => wc.GroupNote)

                                                          .Include(wc => wc.GroupNote2)
                                                          .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                          .Include(wc => wc.Workday)
                                                          .ThenInclude(w => w.Week)

                                                          .Include(wc => wc.Facilitator)

                                                          .Include(wc => wc.Schedule)
                                                          .ThenInclude(wc => wc.SubSchedules)

                                                          .Where(wc => wc.Present == true
                                                                    && wc.BilledDate != null
                                                                    && wc.Hold == false
                                                                    && wc.Workday.Week.Id == week.Id
                                                                    && wc.Client.Id == clients[i].Id).ToList();
                    }

                }

                ViewData["Billed"] = abilled;
                ViewData["idWeek"] = idweek;

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingWeek", clients) });
            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "BillNote", model) });
        }

        [Authorize(Roles = "Manager")]
        public IActionResult PaymentReceivedBIO(int id, int week = 0, int idBio = 0)
        {
            PaymentReceivedViewModel model = new PaymentReceivedViewModel { Id = id, PaymentDate = DateTime.Now };
            ViewData["week"] = week;
            ViewData["idBio"] = idBio;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PaymentReceivedBIO(PaymentReceivedViewModel model, int idWeek = 0, int idBio = 0)
        {
            UserEntity user_logged = await _context.Users
                                                  .Include(u => u.Clinic)
                                                  .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                BioEntity bio;
                WeekEntity week = _context.Weeks.FirstOrDefault(n => n.Id == idWeek);
                List<ClientEntity> clients = new List<ClientEntity>();
                if (idWeek > 0)
                {

                    bio = await _context.Bio
                                        .FirstOrDefaultAsync(m => m.Id == idBio);

                    bio.PaymentDate = model.PaymentDate;
                    bio.DeniedBill = false;
                    _context.Update(bio);
                    await _context.SaveChangesAsync();

                    clients = await _context.Clients

                                            .Include(c => c.Clients_Diagnostics)
                                            .ThenInclude(cd => cd.Diagnostic)

                                            .Include(c => c.Clients_HealthInsurances)
                                            .ThenInclude(c => c.HealthInsurance)

                                            .Include(wc => wc.Bio)
                                            .Include(wc => wc.MTPs)

                                            .Where(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                     && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                 && wc.BilledDate != null
                                                                                 && wc.Hold == false
                                                                                 && wc.Workday.Week.Id == idWeek).Count() > 0)
                                                       || (wc.Bio.DateBio >= week.InitDate
                                                          && wc.Bio.DateBio <= week.FinalDate
                                                          && wc.Bio.BilledDate != null)
                                                       || (wc.MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate
                                                                           && n.AdmissionDateMTP <= week.FinalDate
                                                                           && n.BilledDate != null).Count() > 0))))
                                            .ToListAsync();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (clients[i].Bio != null)
                        {
                            if (clients[i].Bio.DateBio >= week.InitDate && clients[i].Bio.DateBio <= week.FinalDate && clients[i].Bio.BilledDate != null)
                            {

                            }
                            else
                            {
                                clients[i].MTPs = null;
                            }
                        }

                        if (clients[i].MTPs.Where(n => n.AdmissionDateMTP >= week.InitDate && n.AdmissionDateMTP <= week.FinalDate && n.BilledDate != null).Count() == 0)
                        {
                            clients[i].MTPs = null;
                        }

                        clients[i].Workdays_Clients = _context.Workdays_Clients
                                                              .Include(wc => wc.Note)

                                                              .Include(wc => wc.NoteP)

                                                              .Include(wc => wc.IndividualNote)

                                                              .Include(wc => wc.GroupNote)

                                                              .Include(wc => wc.GroupNote2)
                                                              .ThenInclude(wc => wc.GroupNotes2_Activities)

                                                              .Include(wc => wc.Workday)
                                                              .ThenInclude(w => w.Week)

                                                              .Include(wc => wc.Facilitator)

                                                              .Include(wc => wc.Schedule)
                                                              .ThenInclude(wc => wc.SubSchedules)

                                                              .Where(wc => wc.Present == true
                                                                        && wc.BilledDate != null
                                                                        && wc.Hold == false
                                                                        && wc.Workday.Week.Id == week.Id
                                                                        && wc.Client.Id == clients[i].Id).ToList();
                    }

                    ViewData["Billed"] = "1";
                    ViewData["idWeek"] = idWeek;

                }


                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingWeek", clients) });

            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "PaymentReceived", model) });
        }

        [Authorize(Roles = "Manager")]
        public IActionResult BillBIOClient(int idClient, int abilled = 0, int idBio = 0)
        {
            BillViewModel model = new BillViewModel { Id = idClient, BilledDate = DateTime.Now };
            ViewData["Billed"] = abilled;
            ViewData["BIO"] = idBio;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BillBIOClient(BillViewModel model, int abilled = 0, int idBio = 0)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                BioEntity bio = await _context.Bio
                                              .FirstOrDefaultAsync(wc => wc.Id == idBio);

                ClientEntity client;

                bio.BilledDate = model.BilledDate;
                _context.Update(bio);
                await _context.SaveChangesAsync();

                if (abilled == 0)
                {
                    client = await _context.Clients
                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Note)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.NoteP)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.IndividualNote)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.GroupNote)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.GroupNote2)
                                           .ThenInclude(wc => wc.GroupNotes2_Activities)

                                           .Include(c => c.Clients_Diagnostics)
                                           .ThenInclude(cd => cd.Diagnostic)

                                           .Include(c => c.Clients_HealthInsurances)
                                           .ThenInclude(c => c.HealthInsurance)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Workday)
                                           .ThenInclude(w => w.Week)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Facilitator)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Schedule)
                                           .ThenInclude(wc => wc.SubSchedules)

                                           .Include(wc => wc.Bio)
                                           .Include(wc => wc.MTPs)

                                           .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                    && wc.Id == model.Id
                                                    && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                && wc.BilledDate == null
                                                                                && wc.Hold == false).Count() > 0)
                                                      || (wc.Bio.BilledDate == null)
                                                      || (wc.MTPs.Where(n => n.BilledDate == null).Count() > 0))));

                }
                else
                {
                    client = await _context.Clients
                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Note)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.NoteP)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.IndividualNote)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.GroupNote)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.GroupNote2)
                                           .ThenInclude(wc => wc.GroupNotes2_Activities)

                                           .Include(c => c.Clients_Diagnostics)
                                           .ThenInclude(cd => cd.Diagnostic)

                                           .Include(c => c.Clients_HealthInsurances)
                                           .ThenInclude(c => c.HealthInsurance)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Workday)
                                           .ThenInclude(w => w.Week)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Facilitator)

                                           .Include(wc => wc.Workdays_Clients)
                                           .ThenInclude(wc => wc.Schedule)
                                           .ThenInclude(wc => wc.SubSchedules)

                                           .Include(wc => wc.Bio)
                                           .Include(wc => wc.MTPs)

                                           .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                    && wc.Id == model.Id
                                                    && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                && wc.BilledDate != null
                                                                                && wc.Hold == false).Count() > 0)
                                                      || (wc.Bio.BilledDate != null)
                                                      || (wc.MTPs.Where(n => n.BilledDate != null).Count() > 0))));

                }
                ViewData["Billed"] = abilled;

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingClient", client) });
            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "BillNoteClient", model) });
        }

        [Authorize(Roles = "Manager")]
        public IActionResult PaymentReceivedClientBIO(int id)
        {
            PaymentReceivedViewModel model = new PaymentReceivedViewModel { Id = id, PaymentDate = DateTime.Now };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PaymentReceivedClientBIO(PaymentReceivedViewModel model)
        {
            UserEntity user_logged = await _context.Users
                                                   .Include(u => u.Clinic)
                                                   .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                BioEntity bio;
                bio = await _context.Bio
                                    .Include(n => n.Client)
                                    .FirstOrDefaultAsync(wc => wc.Id == model.Id);

                bio.PaymentDate = model.PaymentDate;
                bio.DeniedBill = false;
                _context.Update(bio);
                await _context.SaveChangesAsync();

                ClientEntity client;
                client = await _context.Clients
                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.Note)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.NoteP)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.IndividualNote)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.GroupNote)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.GroupNote2)
                                       .ThenInclude(wc => wc.GroupNotes2_Activities)

                                       .Include(c => c.Clients_Diagnostics)
                                       .ThenInclude(cd => cd.Diagnostic)

                                       .Include(c => c.Clients_HealthInsurances)
                                       .ThenInclude(c => c.HealthInsurance)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.Workday)
                                       .ThenInclude(w => w.Week)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.Facilitator)

                                       .Include(wc => wc.Workdays_Clients)
                                       .ThenInclude(wc => wc.Schedule)
                                       .ThenInclude(wc => wc.SubSchedules)

                                       .Include(wc => wc.Bio)
                                       .Include(wc => wc.MTPs)

                                       .FirstOrDefaultAsync(wc => (wc.Clinic.Id == user_logged.Clinic.Id
                                                    && wc.Id == bio.Client.Id
                                                    && ((wc.Workdays_Clients.Where(wc => wc.Present == true
                                                                                && wc.BilledDate != null
                                                                                && wc.Hold == false).Count() > 0)
                                                      || (wc.Bio.BilledDate != null)
                                                      || (wc.MTPs.Where(n => n.BilledDate != null).Count() > 0))));
                ViewData["Billed"] = "1";

                return Json(new { isValid = true, html = _renderHelper.RenderRazorViewToString(this, "_BillingClient", client) });
            }
            return Json(new { isValid = false, html = _renderHelper.RenderRazorViewToString(this, "PaymentReceivedClient", model) });
        }

        #endregion
    }
}