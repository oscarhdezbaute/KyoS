﻿using KyoS.Common.Enums;
using KyoS.Web.Data;
using KyoS.Web.Data.Entities;
using KyoS.Web.Models;
using System;
using System.Threading.Tasks;

namespace KyoS.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
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

        public ThemeEntity ToThemeEntity(ThemeViewModel model, bool isNew)
        {
            return new ThemeEntity
            {
                Id = isNew ? 0 : model.Id,
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
                DayId = Convert.ToInt32(themeEntity.Day) + 1
            };
        }

        public async Task<ActivityEntity>  ToActivityEntity(ActivityViewModel model, bool isNew)
        {
            return new ActivityEntity
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
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

        public async Task<NoteEntity> ToNoteEntity(NoteViewModel model, bool isNew)
        {
            return new NoteEntity
            {
                Id = isNew ? 0 : model.Id,
                Activity = await _context.Activities.FindAsync(model.IdActivity),
                AnswerClient = model.AnswerClient,
                AnswerFacilitator = model.AnswerFacilitator               
            };
        }

        public NoteViewModel ToNoteViewModel(NoteEntity noteEntity)
        {
            return new NoteViewModel
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

        public async Task<FacilitatorEntity> ToFacilitatorEntity(FacilitatorViewModel model, bool isNew)
        {
            return new FacilitatorEntity
            {
                Id = isNew ? 0 : model.Id,
                Clinic = await _context.Clinics.FindAsync(model.IdClinic),
                Codigo = model.Codigo,
                Name = model.Name                
            };
        }

        public FacilitatorViewModel ToFacilitatorViewModel(FacilitatorEntity facilitatorEntity)
        {
            return new FacilitatorViewModel
            {
                Id = facilitatorEntity.Id,
                Name = facilitatorEntity.Name,
                Codigo = facilitatorEntity.Codigo,
                IdClinic = facilitatorEntity.Clinic.Id,
                Clinics = _combosHelper.GetComboClinics()                
            };
        }

        public async Task<ClientEntity> ToClientEntity(ClientViewModel model, bool isNew)
        {
            ClinicEntity clinic = null;
            if (model.IdClinic != 0)
                clinic = await _context.Clinics.FindAsync(model.IdClinic);

            return new ClientEntity
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Gender = GenderUtils.GetGenderByIndex(model.IdGender),
                DateOfBirth = model.DateOfBirth,
                Code = model.Code,
                MedicalID = model.MedicalID,
                Clinic = clinic,
                Status = StatusUtils.GetStatusByIndex(model.IdStatus)
            };
        }

        public ClientViewModel ToClientViewModel(ClientEntity clientEntity)
        {
            return new ClientViewModel
            {
                Id = clientEntity.Id,
                Name = clientEntity.Name,
                Code = clientEntity.Code,
                MedicalID = clientEntity.MedicalID,
                DateOfBirth = clientEntity.DateOfBirth,
                IdStatus = (clientEntity.Status == StatusType.Open) ? 1 : 2,
                StatusList = _combosHelper.GetComboClientStatus(),
                IdClinic = (clientEntity.Clinic != null) ? clientEntity.Clinic.Id : 0,
                Clinics = _combosHelper.GetComboClinics(),
                IdGender = (clientEntity.Gender == GenderType.Female) ? 1 : 2,
                GenderList = _combosHelper.GetComboGender(),
            };
        }

        public async Task<SupervisorEntity> ToSupervisorEntity(SupervisorViewModel model, bool isNew)
        {
            return new SupervisorEntity
            {
                Id = isNew ? 0 : model.Id,
                Clinic = await _context.Clinics.FindAsync(model.IdClinic),
                Code = model.Code,
                Name = model.Name
            };
        }

        public SupervisorViewModel ToSupervisorViewModel(SupervisorEntity supervisorEntity)
        {
            return new SupervisorViewModel
            {
                Id = supervisorEntity.Id,
                Name = supervisorEntity.Name,
                Code = supervisorEntity.Code,
                IdClinic = supervisorEntity.Clinic.Id,
                Clinics = _combosHelper.GetComboClinics()
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
                NumberOfMonths = model.NumberOfMonths
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
                Diagnosis = mtpEntity.Diagnosis
            };
        }

        public async Task<DiagnosisEntity> ToDiagnosisEntity(DiagnosisViewModel model, bool isNew)
        {
            return new DiagnosisEntity
            {
                Id = isNew ? 0 : model.Id,
                Code = model.Code,
                Description = model.Description,
                MTP = await _context.MTPs.FindAsync(model.IdMTP)
            };
        }

        public DiagnosisViewModel ToDiagnosisViewModel(DiagnosisEntity diagnosisEntity)
        {
            return new DiagnosisViewModel
            {
                Id = diagnosisEntity.Id,
                MTP = diagnosisEntity.MTP,
                IdMTP = diagnosisEntity.MTP.Id,
                Code = diagnosisEntity.Code,
                Description = diagnosisEntity.Description
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
    }
}
