﻿using KyoS.Common.Enums;
using KyoS.Web.Data;
using KyoS.Web.Data.Entities;
using KyoS.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KyoS.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IUserHelper _userHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper, IUserHelper userHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
        }

        public ClinicEntity ToClinicEntity(ClinicViewModel model, string path, bool isNew)
        {
            return new ClinicEntity
            {
                Id = isNew ? 0 : model.Id,
                LogoPath = path,
                Name = model.Name
            };
        }

        public ClinicViewModel ToClinicViewModel(ClinicEntity clinicEntity)
        {
            return new ClinicViewModel
            {
                Id = clinicEntity.Id,
                LogoPath = clinicEntity.LogoPath,
                Name = clinicEntity.Name
            };
        }

        public async Task<ThemeEntity> ToThemeEntity(ThemeViewModel model, bool isNew)
        {
            return new ThemeEntity
            {
                Id = isNew ? 0 : model.Id,
                Clinic = await _context.Clinics.FindAsync(model.IdClinic),
                Day = (model.DayId == 1) ? DayOfWeekType.Monday : (model.DayId == 2) ? DayOfWeekType.Tuesday :
                                    (model.DayId == 3) ? DayOfWeekType.Wednesday : (model.DayId == 4) ? DayOfWeekType.Thursday :
                                    (model.DayId == 5) ? DayOfWeekType.Friday : DayOfWeekType.Monday,
                Name = model.Name
            };
        }

        public ThemeViewModel ToThemeViewModel(ThemeEntity themeEntity)
        {
            return new ThemeViewModel
            {
                Id = themeEntity.Id,
                Name = themeEntity.Name,
                Day = themeEntity.Day,
                Days = _combosHelper.GetComboDays(),
                DayId = Convert.ToInt32(themeEntity.Day) + 1,
                IdClinic = themeEntity.Clinic.Id,
                Clinics = _combosHelper.GetComboClinics()
            };
        }

        public async Task<ActivityEntity> ToActivityEntity(ActivityViewModel model, bool isNew)
        {
            return new ActivityEntity
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Status = ActivityStatus.Pending,
                Theme = await _context.Themes.FindAsync(model.IdTheme)
            };
        }

        public ActivityViewModel ToActivityViewModel(ActivityEntity activityEntity)
        {
            return new ActivityViewModel
            {
                Id = activityEntity.Id,
                Name = activityEntity.Name,
                Themes = _combosHelper.GetComboThemes(),
                IdTheme = activityEntity.Theme.Id
            };
        }

        public async Task<NotePrototypeEntity> ToNotePrototypeEntity(NotePrototypeViewModel model, bool isNew)
        {
            return new NotePrototypeEntity
            {
                Id = isNew ? 0 : model.Id,
                Activity = await _context.Activities.FindAsync(model.IdActivity),
                AnswerClient = model.AnswerClient,
                AnswerFacilitator = model.AnswerFacilitator
            };
        }

        public NotePrototypeViewModel ToNotePrototypeViewModel(NotePrototypeEntity noteEntity)
        {
            return new NotePrototypeViewModel
            {
                Id = noteEntity.Id,
                AnswerClient = noteEntity.AnswerClient,
                AnswerFacilitator = noteEntity.AnswerFacilitator,
                IdActivity = noteEntity.Activity.Id,
                Activities = _combosHelper.GetComboActivities(),
                Classifications = noteEntity.Classifications,
                Clients = _combosHelper.GetComboClients(),
                Facilitators = _combosHelper.GetComboFacilitators()
            };
        }

        public async Task<FacilitatorEntity> ToFacilitatorEntity(FacilitatorViewModel model, string signaturePath, bool isNew)
        {
            return new FacilitatorEntity
            {
                Id = isNew ? 0 : model.Id,
                Clinic = await _context.Clinics.FindAsync(model.IdClinic),
                Codigo = model.Codigo,
                Name = model.Name,
                Status = StatusUtils.GetStatusByIndex(model.IdStatus),
                LinkedUser = _userHelper.GetUserNameById(model.IdUser),
                SignaturePath = signaturePath
            };
        }

        public FacilitatorViewModel ToFacilitatorViewModel(FacilitatorEntity facilitatorEntity, int idClinic)
        {
            return new FacilitatorViewModel
            {
                Id = facilitatorEntity.Id,
                Name = facilitatorEntity.Name,
                Codigo = facilitatorEntity.Codigo,
                IdClinic = facilitatorEntity.Clinic.Id,
                Clinics = _combosHelper.GetComboClinics(),
                IdStatus = (facilitatorEntity.Status == StatusType.Open) ? 1 : 2,
                StatusList = _combosHelper.GetComboClientStatus(),
                IdUser = _userHelper.GetIdByUserName(facilitatorEntity.LinkedUser),
                UserList = _combosHelper.GetComboUserNamesByRolesClinic(UserType.Facilitator, idClinic),
                SignaturePath = facilitatorEntity.SignaturePath
            };
        }

        public async Task<ClientEntity> ToClientEntity(ClientViewModel model, bool isNew, string photoPath, string signPath, string userId)
        {
            return new ClientEntity
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Gender = GenderUtils.GetGenderByIndex(model.IdGender),
                DateOfBirth = model.DateOfBirth,
                Code = model.Code,
                MedicaidID = model.MedicaidID,
                Clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Id == model.IdClinic),
                Status = StatusUtils.GetStatusByIndex(model.IdStatus),
                Email = model.Email,
                Telephone = model.Telephone,
                TelephoneSecondary = model.TelephoneSecondary,
                SSN = model.SSN,
                FullAddress = model.FullAddress,
                AlternativeAddress = model.AlternativeAddress,
                Country = model.Country,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Race = RaceUtils.GetRaceByIndex(model.IdRace),
                MaritalStatus = MaritalUtils.GetMaritalByIndex(model.IdMaritalStatus),
                Ethnicity = EthnicityUtils.GetEthnicityByIndex(model.IdEthnicity),
                PreferredLanguage = PreferredLanguageUtils.GetPreferredLanguageByIndex(model.IdPreferredLanguage),
                OtherLanguage = model.OtherLanguage,
                PhotoPath = photoPath,
                SignPath = signPath,
                Doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == model.IdDoctor),
                Psychiatrist = await _context.Psychiatrists.FirstOrDefaultAsync(p => p.Id == model.IdPsychiatrist),
                Referred = await _context.Referreds.FirstOrDefaultAsync(r => r.Id == model.IdReferred),
                LegalGuardian = await _context.LegalGuardians.FirstOrDefaultAsync(lg => lg.Id == model.IdLegalGuardian),
                EmergencyContact = await _context.EmergencyContacts.FirstOrDefaultAsync(ec => ec.Id == model.IdEmergencyContact),
                RelationShipOfLegalGuardian = RelationshipUtils.GetRelationshipByIndex(model.IdRelationship),
                CreatedBy = isNew ? userId : model.CreatedBy,
                CreatedOn = isNew ? DateTime.Now : model.CreatedOn,
                LastModifiedBy = !isNew ? userId : string.Empty,
                LastModifiedOn = !isNew ? DateTime.Now : Convert.ToDateTime(null)
            };
        }

        public ClientViewModel ToClientViewModel(ClientEntity clientEntity, string userId)
        {

            return new ClientViewModel
            {
                Id = clientEntity.Id,
                Name = clientEntity.Name,
                Code = clientEntity.Code,
                MedicaidID = clientEntity.MedicaidID,
                DateOfBirth = clientEntity.DateOfBirth,
                IdStatus = (clientEntity.Status == StatusType.Open) ? 1 : 2,
                StatusList = _combosHelper.GetComboClientStatus(),
                IdClinic = (clientEntity.Clinic != null) ? clientEntity.Clinic.Id : 0,
                Clinics = _combosHelper.GetComboClinics(),
                IdGender = (clientEntity.Gender == GenderType.Female) ? 1 : 2,
                GenderList = _combosHelper.GetComboGender(),
                CreatedBy = clientEntity.CreatedBy,
                CreatedOn = clientEntity.CreatedOn,
                LastModifiedBy = clientEntity.LastModifiedBy,
                LastModifiedOn = clientEntity.LastModifiedOn,
                Email = clientEntity.Email,
                Telephone = clientEntity.Telephone,
                TelephoneSecondary = clientEntity.TelephoneSecondary,
                SSN = clientEntity.SSN,
                FullAddress = clientEntity.FullAddress,
                AlternativeAddress = clientEntity.AlternativeAddress,
                Country = clientEntity.Country,
                City = clientEntity.City,
                State = clientEntity.State,
                ZipCode = clientEntity.ZipCode,
                IdRace = Convert.ToInt32(clientEntity.Race),
                Races = _combosHelper.GetComboRaces(),
                IdMaritalStatus = Convert.ToInt32(clientEntity.MaritalStatus),
                Maritals = _combosHelper.GetComboMaritals(),
                IdEthnicity = Convert.ToInt32(clientEntity.Ethnicity),
                Ethnicities = _combosHelper.GetComboEthnicities(),
                IdPreferredLanguage = Convert.ToInt32(clientEntity.PreferredLanguage),
                Languages = _combosHelper.GetComboLanguages(),
                OtherLanguage = clientEntity.OtherLanguage,
                PhotoPath = clientEntity.PhotoPath,
                SignPath = clientEntity.SignPath,
                IdRelationship = Convert.ToInt32(clientEntity.RelationShipOfLegalGuardian),
                Relationships = _combosHelper.GetComboRelationships(),
                IdReferred = (clientEntity.Referred != null) ? clientEntity.Referred.Id : 0,
                Referreds = _combosHelper.GetComboReferredsByClinic(userId),                
                IdEmergencyContact = (clientEntity.EmergencyContact != null) ? clientEntity.EmergencyContact.Id : 0,
                EmergencyContacts = _combosHelper.GetComboEmergencyContactsByClinic(userId),
                IdDoctor = (clientEntity.Doctor != null) ? clientEntity.Doctor.Id : 0,
                Doctors = _combosHelper.GetComboDoctorsByClinic(userId),
                IdPsychiatrist = (clientEntity.Psychiatrist != null) ? clientEntity.Psychiatrist.Id : 0,
                Psychiatrists = _combosHelper.GetComboPsychiatristsByClinic(userId),
                IdLegalGuardian = (clientEntity.LegalGuardian != null) ? clientEntity.LegalGuardian.Id : 0,
                LegalsGuardians = _combosHelper.GetComboLegalGuardiansByClinic(userId),
                DiagnosticTemp = _context.DiagnosticsTemp,
                DocumentTemp = _context.DocumentsTemp
            };
        }

        public async Task<SupervisorEntity> ToSupervisorEntity(SupervisorViewModel model, string signaturePath, bool isNew)
        {
            return new SupervisorEntity
            {
                Id = isNew ? 0 : model.Id,
                Clinic = await _context.Clinics.FindAsync(model.IdClinic),
                Code = model.Code,
                LinkedUser = _userHelper.GetUserNameById(model.IdUser),
                Name = model.Name,
                SignaturePath = signaturePath
            };
        }

        public SupervisorViewModel ToSupervisorViewModel(SupervisorEntity supervisorEntity, int idClinic)
        {
            return new SupervisorViewModel
            {
                Id = supervisorEntity.Id,
                Name = supervisorEntity.Name,
                Code = supervisorEntity.Code,
                IdClinic = supervisorEntity.Clinic.Id,
                Clinics = _combosHelper.GetComboClinics(),
                IdUser = _userHelper.GetIdByUserName(supervisorEntity.LinkedUser),
                UserList = _combosHelper.GetComboUserNamesByRolesClinic(UserType.Supervisor, idClinic),
                SignaturePath = supervisorEntity.SignaturePath
            };
        }

        public async Task<MTPEntity> ToMTPEntity(MTPViewModel model, bool isNew)
        {
            return new MTPEntity
            {
                Id = isNew ? 0 : model.Id,
                Client = await _context.Clients.FindAsync(model.IdClient),
                AdmisionDate = model.AdmisionDate,
                MTPDevelopedDate = model.MTPDevelopedDate,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                LevelCare = model.LevelCare,
                InitialDischargeCriteria = model.InitialDischargeCriteria,
                Modality = model.Modality,
                Frecuency = model.Frecuency,
                NumberOfMonths = model.NumberOfMonths,
                Active = isNew ? true : model.Active
            };
        }

        public MTPViewModel ToMTPViewModel(MTPEntity mtpEntity)
        {
            return new MTPViewModel
            {
                Id = mtpEntity.Id,
                IdClient = mtpEntity.Client.Id,
                Clients = _combosHelper.GetComboClients(),
                AdmisionDate = mtpEntity.AdmisionDate,
                MTPDevelopedDate = mtpEntity.MTPDevelopedDate,
                StartTime = mtpEntity.StartTime,
                EndTime = mtpEntity.EndTime,
                LevelCare = mtpEntity.LevelCare,
                InitialDischargeCriteria = mtpEntity.InitialDischargeCriteria,
                Modality = mtpEntity.Modality,
                Frecuency = mtpEntity.Frecuency,
                NumberOfMonths = mtpEntity.NumberOfMonths,                
                Setting = mtpEntity.Setting,
                Active = mtpEntity.Active
            };
        }        

        public async Task<GoalEntity> ToGoalEntity(GoalViewModel model, bool isNew)
        {
            return new GoalEntity
            {
                Id = isNew ? 0 : model.Id,
                Number = model.Number,
                Name = model.Name,
                AreaOfFocus = model.AreaOfFocus,
                MTP = await _context.MTPs.FindAsync(model.IdMTP)
            };
        }

        public GoalViewModel ToGoalViewModel(GoalEntity goalEntity)
        {
            return new GoalViewModel
            {
                Id = goalEntity.Id,
                Number = goalEntity.Number,
                MTP = goalEntity.MTP,
                IdMTP = goalEntity.MTP.Id,
                Name = goalEntity.Name,
                AreaOfFocus = goalEntity.AreaOfFocus
            };
        }

        public async Task<ObjetiveEntity> ToObjectiveEntity(ObjectiveViewModel model, bool isNew)
        {
            return new ObjetiveEntity
            {
                Id = isNew ? 0 : model.Id,
                Objetive = model.Objetive,
                DateOpened = model.DateOpened,
                DateTarget = model.DateTarget,
                DateResolved = model.DateResolved,
                Description = model.Description,
                Intervention = model.Intervention,
                Goal = await _context.Goals.FindAsync(model.IdGoal)
            };
        }

        public ObjectiveViewModel ToObjectiveViewModel(ObjetiveEntity objectiveEntity)
        {
            return new ObjectiveViewModel
            {
                Id = objectiveEntity.Id,
                Objetive = objectiveEntity.Objetive,
                Goal = objectiveEntity.Goal,
                IdGoal = objectiveEntity.Goal.Id,
                DateOpened = objectiveEntity.DateOpened,
                DateResolved = objectiveEntity.DateResolved,
                DateTarget = objectiveEntity.DateTarget,
                Description = objectiveEntity.Description,
                Classifications = objectiveEntity.Classifications,
                Intervention = objectiveEntity.Intervention
            };
        }

        public async Task<GroupEntity> ToGroupEntity(GroupViewModel model, bool isNew)
        {
            return new GroupEntity
            {
                Id = isNew ? 0 : model.Id,
                Am = model.Am,
                Pm = model.Pm,
                Facilitator = await _context.Facilitators.FindAsync(model.IdFacilitator)
            };
        }

        public GroupViewModel ToGroupViewModel(GroupEntity groupEntity)
        {
            return new GroupViewModel
            {
                Id = groupEntity.Id,
                Facilitator = groupEntity.Facilitator,
                IdFacilitator = groupEntity.Facilitator.Id,
                Facilitators = _combosHelper.GetComboFacilitators(),
                Am = groupEntity.Am,
                Pm = groupEntity.Pm,
                Clients = groupEntity.Clients
            };
        }

        public async Task<PlanEntity> ToPlanEntity(PlanViewModel model, bool isNew)
        {
            return new PlanEntity
            {

            };
        }

        public PlanViewModel ToPlanViewModel(PlanEntity planEntity)
        {
            return new PlanViewModel
            {
                Id = planEntity.Id,
                Text = planEntity.Text,
                Classifications = planEntity.Classifications
            };
        }

        public async Task<NoteEntity> ToNoteEntity(NoteViewModel model, bool isNew)
        {
            NoteEntity entity = await _context.Notes.FirstOrDefaultAsync(n => n.Workday_Cient.Id == model.Id);
            return new NoteEntity
            {
                Id = isNew ? 0 : entity.Id,
                Workday_Cient = await _context.Workdays_Clients.FindAsync(model.Id),
                PlanNote = model.PlanNote,
                Status = NoteStatus.Edition,
                OrientedX3 = model.OrientedX3,
                NotTime = model.NotTime,
                NotPlace = model.NotPlace,
                NotPerson = model.NotPerson,
                Present = model.Present,
                Adequate = model.Adequate,
                Limited = model.Limited,
                Impaired = model.Impaired,
                Faulty = model.Faulty,
                Euthymic = model.Euthymic,
                Congruent = model.Congruent,
                Negativistic = model.Negativistic,
                Depressed = model.Depressed,
                Euphoric = model.Euphoric,
                Optimistic = model.Optimistic,
                Anxious = model.Anxious,
                Hostile = model.Hostile,
                Withdrawn = model.Withdrawn,
                Irritable = model.Irritable,
                Dramatized = model.Dramatized,
                AdequateAC = model.AdequateAC,
                Inadequate = model.Inadequate,
                Fair = model.Fair,
                Unmotivated = model.Unmotivated,
                Motivated = model.Motivated,
                Guarded = model.Guarded,
                Normal = model.Normal,
                ShortSpanned = model.ShortSpanned,
                MildlyImpaired = model.MildlyImpaired,
                SeverelyImpaired = model.SeverelyImpaired                
            };
        }

        public NoteViewModel ToNoteViewModel(NoteEntity model)
        {
            return new NoteViewModel
            {
                Id = model.Id,
                PlanNote = model.PlanNote,
                Status = model.Status,
                Workday_Cient = model.Workday_Cient
            };
        }

        public Workday_ClientViewModel ToWorkdayClientViewModel(Workday_Client model)
        {
            return new Workday_ClientViewModel
            {
                Id = model.Id,
                Workday = model.Workday,
                Client = model.Client,
                Facilitator = model.Facilitator,
                Session = model.Session,
                Present = model.Present,
                Note = model.Note
            };
        }

        public async Task<MessageEntity> ToMessageEntity(MessageViewModel model, bool isNew)
        {

            return new MessageEntity
            {
                Id = isNew ? 0 : model.Id,
                Workday_Client = await _context.Workdays_Clients
                                               .Include(wc => wc.Facilitator)
                                               .FirstOrDefaultAsync(wc => wc.Id == model.IdWorkdayClient),
                Title = model.Title,
                Text = model.Text,
                DateCreated = DateTime.Now,
                Status = MessageStatus.NotRead
            };
        }

        public DoctorEntity ToDoctorEntity(DoctorViewModel model, bool isNew, string userId)
        {
            return new DoctorEntity
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                CreatedBy = isNew ? userId : model.CreatedBy,
                CreatedOn = isNew ? DateTime.Now : model.CreatedOn,
                LastModifiedBy = !isNew ? userId : string.Empty,
                LastModifiedOn = !isNew ? DateTime.Now : Convert.ToDateTime(null)
            };
        }

        public PsychiatristEntity ToPsychiatristEntity(PsychiatristViewModel model, bool isNew, string userId)
        {
            return new PsychiatristEntity
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                CreatedBy = isNew ? userId : model.CreatedBy,
                CreatedOn = isNew ? DateTime.Now : model.CreatedOn,
                LastModifiedBy = !isNew ? userId : string.Empty,
                LastModifiedOn = !isNew ? DateTime.Now : Convert.ToDateTime(null)
            };
        }

        public LegalGuardianEntity ToLegalGuardianEntity(LegalGuardianViewModel model, bool isNew, string userId)
        {
            return new LegalGuardianEntity
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                CreatedBy = isNew ? userId : model.CreatedBy,
                CreatedOn = isNew ? DateTime.Now : model.CreatedOn,
                LastModifiedBy = !isNew ? userId : string.Empty,
                LastModifiedOn = !isNew ? DateTime.Now : Convert.ToDateTime(null),
                Country = model.Country,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                TelephoneSecondary = model.TelephoneSecondary,
                AdressLine2 = model.AdressLine2
            };
        }

        public EmergencyContactEntity ToEmergencyContactEntity(EmergencyContactViewModel model, bool isNew, string userId)
        {
            return new EmergencyContactEntity
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                CreatedBy = isNew ? userId : model.CreatedBy,
                CreatedOn = isNew ? DateTime.Now : model.CreatedOn,
                LastModifiedBy = !isNew ? userId : string.Empty,
                LastModifiedOn = !isNew ? DateTime.Now : Convert.ToDateTime(null),
                Country = model.Country,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                TelephoneSecondary = model.TelephoneSecondary,
                AdressLine2 = model.AdressLine2
            };
        }

        public ReferredEntity ToReferredEntity(ReferredViewModel model, bool isNew, string userId)
        {
            return new ReferredEntity
            {
                Id = isNew ? 0 : model.Id,
                ReferredNote = model.ReferredNote,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                CreatedBy = isNew ? userId : model.CreatedBy,
                CreatedOn = isNew ? DateTime.Now : model.CreatedOn,
                LastModifiedBy = !isNew ? userId : string.Empty,
                LastModifiedOn = !isNew ? DateTime.Now : Convert.ToDateTime(null)
            };
        }

        public DiagnosticEntity ToDiagnosticEntity(DiagnosticViewModel model, bool isNew, string userId)
        {
            return new DiagnosticEntity
            {
                Id = isNew ? 0 : model.Id,
                Code = model.Code,
                Description = model.Description,
                CreatedBy = isNew ? userId : model.CreatedBy,
                CreatedOn = isNew ? DateTime.Now : model.CreatedOn,
                LastModifiedBy = !isNew ? userId : string.Empty,
                LastModifiedOn = !isNew ? DateTime.Now : Convert.ToDateTime(null)
            };
        }

        public DoctorViewModel ToDoctorViewModel(DoctorEntity model)
        {
            return new DoctorViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn
            };
        }

        public PsychiatristViewModel ToPsychiatristViewModel(PsychiatristEntity model)
        {
            return new PsychiatristViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn
            };
        }

        public ReferredViewModel ToReferredViewModel(ReferredEntity model)
        {
            return new ReferredViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                ReferredNote = model.ReferredNote,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn
            };
        }

        public LegalGuardianViewModel ToLegalGuardianViewModel(LegalGuardianEntity model)
        {
            return new LegalGuardianViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,
                LastModifiedBy = model.LastModifiedBy,
                LastModifiedOn = model.LastModifiedOn,
                Country = model.Country,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                TelephoneSecondary = model.TelephoneSecondary,
                AdressLine2 = model.AdressLine2
            };
        }

        public EmergencyContactViewModel ToEmergencyContactViewModel(EmergencyContactEntity model)
        {
            return new EmergencyContactViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Telephone = model.Telephone,
                Email = model.Email,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,
                LastModifiedBy = model.LastModifiedBy,
                LastModifiedOn = model.LastModifiedOn,
                Country = model.Country,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                TelephoneSecondary = model.TelephoneSecondary,
                AdressLine2 = model.AdressLine2
            };
        }

        public DiagnosticViewModel ToDiagnosticViewModel(DiagnosticEntity model)
        {
            return new DiagnosticViewModel
            {
                Id = model.Id,
                Code = model.Code,
                Description = model.Description,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,
                LastModifiedBy = model.LastModifiedBy,
                LastModifiedOn = model.LastModifiedOn
            };
        }
    }
}
