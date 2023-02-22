﻿using KyoS.Web.Data.Entities;
using KyoS.Web.Models;
using System.Threading.Tasks;

namespace KyoS.Web.Helpers
{
    public interface IConverterHelper
    {
        ClinicEntity ToClinicEntity(ClinicViewModel model, string path, bool isNew, string pathSignatureClinical);
        ClinicViewModel ToClinicViewModel(ClinicEntity model);
        Task<ThemeEntity> ToThemeEntity(ThemeViewModel model, bool isNew);
        Task<ThemeEntity> ToTheme3Entity(Theme3ViewModel model, bool isNew);
        ThemeViewModel ToThemeViewModel(ThemeEntity model);
        Theme3ViewModel ToTheme3ViewModel(ThemeEntity model);
        Task<ActivityEntity> ToActivityEntity(ActivityViewModel model, bool isNew);
        ActivityViewModel ToActivityViewModel(ActivityEntity model);
        Task<NotePrototypeEntity> ToNotePrototypeEntity(NotePrototypeViewModel model, bool isNew);
        NotePrototypeViewModel ToNotePrototypeViewModel(NotePrototypeEntity model);
        Task<FacilitatorEntity> ToFacilitatorEntity(FacilitatorViewModel model, string signaturePath, bool isNew);
        FacilitatorViewModel ToFacilitatorViewModel(FacilitatorEntity model, int idClinic);
        Task<ClientEntity> ToClientEntity(ClientViewModel model, bool isNew, string photoPath, string signPath, string userId);
        Task<ClientViewModel> ToClientViewModel(ClientEntity model, string userId);
        Task<SupervisorEntity> ToSupervisorEntity(SupervisorViewModel model, string signaturePath, bool isNew);
        SupervisorViewModel ToSupervisorViewModel(SupervisorEntity model, int idClinic);
        Task<MTPEntity> ToMTPEntity(MTPViewModel model, bool isNew, string userId);
        MTPViewModel ToMTPViewModel(MTPEntity model);       
        Task<GoalEntity> ToGoalEntity(GoalViewModel model, bool isNew);
        GoalViewModel ToGoalViewModel(GoalEntity model);
        Task<ObjetiveEntity> ToObjectiveEntity(ObjectiveViewModel model, bool isNew);
        ObjectiveViewModel ToObjectiveViewModel(ObjetiveEntity model);
        Task<GroupEntity> ToGroupEntity(GroupViewModel model, bool isNew);
        GroupViewModel ToGroupViewModel(GroupEntity model);
        Task<PlanEntity> ToPlanEntity(PlanViewModel model, bool isNew);
        PlanViewModel ToPlanViewModel(PlanEntity model);
        Task<NoteEntity> ToNoteEntity(NoteViewModel model, bool isNew);
        NoteViewModel ToNoteViewModel(NoteEntity model);
        Task<NotePEntity> ToNotePEntity(NotePViewModel model, bool isNew);
        NotePViewModel ToNotePViewModel(NotePEntity model);
        Task<IndividualNoteEntity> ToIndividualNoteEntity(IndividualNoteViewModel model, bool isNew);
        Task<GroupNoteEntity> ToGroupNoteEntity(GroupNoteViewModel model, bool isNew);       
        Workday_ClientViewModel ToWorkdayClientViewModel(Workday_Client model);
        Task<MessageEntity> ToMessageEntity(MessageViewModel model, bool isNew);
        DoctorEntity ToDoctorEntity(DoctorViewModel model, bool isNew, string userId);
        PsychiatristEntity ToPsychiatristEntity(PsychiatristViewModel model, bool isNew, string userId);
        LegalGuardianEntity ToLegalGuardianEntity(LegalGuardianViewModel model, bool isNew, string userId);
        EmergencyContactEntity ToEmergencyContactEntity(EmergencyContactViewModel model, bool isNew, string userId);
        ReferredEntity ToReferredEntity(ReferredViewModel model, bool isNew, string userId);
        DiagnosticEntity ToDiagnosticEntity(DiagnosticViewModel model, bool isNew, string userId);
        DoctorViewModel ToDoctorViewModel(DoctorEntity model);
        PsychiatristViewModel ToPsychiatristViewModel(PsychiatristEntity model);
        ReferredViewModel ToReferredViewModel(ReferredEntity model);
        LegalGuardianViewModel ToLegalGuardianViewModel(LegalGuardianEntity model);
        EmergencyContactViewModel ToEmergencyContactViewModel(EmergencyContactEntity model);
        DiagnosticViewModel ToDiagnosticViewModel(DiagnosticEntity model);
        Task<IncidentEntity> ToIncidentEntity(IncidentViewModel model, bool isNew, string userId);
        IncidentViewModel ToIncidentViewModel(IncidentEntity model);
        Task<HealthInsuranceEntity> ToHealthInsuranceEntity(HealthInsuranceViewModel model, bool isNew, string userId, string documentPath);
        HealthInsuranceViewModel ToHealthInsuranceViewModel(HealthInsuranceEntity model);
        Task<Client_HealthInsurance> ToClientHealthInsuranceEntity(UnitsAvailabilityViewModel model, bool isNew, string userId);
        UnitsAvailabilityViewModel ToClientHealthInsuranceViewModel(Client_HealthInsurance model,  int idClinic);
        Task<SettingEntity> ToSettingEntity(SettingViewModel model, bool isNew, string userId);
        SettingViewModel ToSettingViewModel(SettingEntity model);
        Task<IntakeScreeningEntity> ToIntakeEntity(IntakeScreeningViewModel model, bool isNew);
        IntakeScreeningViewModel ToIntakeViewModel(IntakeScreeningEntity model);
        Task<IntakeConsentForTreatmentEntity> ToIntakeConsentForTreatmentEntity(IntakeConsentForTreatmentViewModel model, bool isNew);
        IntakeConsentForTreatmentViewModel ToIntakeConsentForTreatmentViewModel(IntakeConsentForTreatmentEntity model);
        Task<IntakeConsentForReleaseEntity> ToIntakeConsentForReleaseEntity(IntakeConsentForReleaseViewModel model, bool isNew);
        IntakeConsentForReleaseViewModel ToIntakeConsentForReleaseViewModel(IntakeConsentForReleaseEntity model);
        Task<IntakeConsumerRightsEntity> ToIntakeConsumerRightsEntity(IntakeConsumerRightsViewModel model, bool isNew);
        IntakeConsumerRightsViewModel ToIntakeConsumerRightsViewModel(IntakeConsumerRightsEntity model);
        Task<IntakeAcknowledgementHippaEntity> ToIntakeAcknoewledgementHippaEntity(IntakeAcknoewledgementHippaViewModel model, bool isNew);
        IntakeAcknoewledgementHippaViewModel ToIntakeAcknoewledgementHippaViewModel(IntakeAcknowledgementHippaEntity model);
        Task<IntakeAccessToServicesEntity> ToIntakeAccessToServicesEntity(IntakeAccessToServicesViewModel model, bool isNew);
        IntakeAccessToServicesViewModel ToIntakeAccessToServicesViewModel(IntakeAccessToServicesEntity model);
        Task<IntakeOrientationChecklistEntity> ToIntakeOrientationChecklistEntity(IntakeOrientationCheckListViewModel model, bool isNew);
        IntakeOrientationCheckListViewModel ToIntakeOrientationChecklistViewModel(IntakeOrientationChecklistEntity model);
        Task<IntakeTransportationEntity> ToIntakeTransportationEntity(IntakeTransportationViewModel model, bool isNew);
        IntakeTransportationViewModel ToIntakeTransportationViewModel(IntakeTransportationEntity model);
        Task<IntakeConsentPhotographEntity> ToIntakeConsentPhotographEntity(IntakeConsentPhotographViewModel model, bool isNew);
        IntakeConsentPhotographViewModel ToIntakeConsentPhotographViewModel(IntakeConsentPhotographEntity model);
        Task<IntakeFeeAgreementEntity> ToIntakeFeeAgreementEntity(IntakeFeeAgreementViewModel model, bool isNew);
        IntakeFeeAgreementViewModel ToIntakeFeeAgreementViewModel(IntakeFeeAgreementEntity model);
        Task<IntakeTuberculosisEntity> ToIntakeTuberculosisEntity(IntakeTuberculosisViewModel model, bool isNew);
        IntakeTuberculosisViewModel ToIntakeTuberculosisViewModel(IntakeTuberculosisEntity model);
        Task<IntakeMedicalHistoryEntity> ToIntakeMedicalHistoryEntity(IntakeMedicalHistoryViewModel model, bool isNew);
        IntakeMedicalHistoryViewModel ToIntakeMedicalHistoryViewModel(IntakeMedicalHistoryEntity model);
        Task<DischargeEntity> ToDischargeEntity(DischargeViewModel model, bool isNew, string userId);
        DischargeViewModel ToDischargeViewModel(DischargeEntity model);
        Task<MedicationEntity> ToMedicationEntity(MedicationViewModel model, bool isNew);
        MedicationViewModel ToMedicationViewModel(MedicationEntity model);
        Task<FarsFormEntity> ToFarsFormEntity(FarsFormViewModel model, bool isNew, string userId);
        FarsFormViewModel ToFarsFormViewModel(FarsFormEntity model);
        Task<BioEntity> ToBioEntity(BioViewModel model, bool isNew, string userId);
        BioViewModel ToBioViewModel(BioEntity model);
        Task<Bio_BehavioralHistoryEntity> ToBio_BehaviorEntity(Bio_BehavioralHistoryViewModel model, bool isNew);
        Bio_BehavioralHistoryViewModel ToBio_BehaviorViewModel(Bio_BehavioralHistoryEntity model);
        Task<AdendumEntity> ToAdendumEntity(AdendumViewModel model, bool isNew, string userId);
        AdendumViewModel ToAdendumViewModel(AdendumEntity model);
        Task<MTPReviewEntity> ToMTPReviewEntity(MTPReviewViewModel model, bool isNew, string userId);
        MTPReviewViewModel ToMTPReviewViewModel(MTPReviewEntity model);
        Task<CaseMannagerEntity> ToCaseMannagerEntity(CaseMannagerViewModel model, string signaturePath, bool isNew);
        CaseMannagerViewModel ToCaseMannagerViewModel(CaseMannagerEntity model, int idClinic);
        Task<TCMSupervisorEntity> ToTCMsupervisorEntity(TCMSupervisorViewModel model, string signaturePath, bool isNew, string userId);
        TCMSupervisorViewModel ToTCMsupervisorViewModel(TCMSupervisorEntity model, int idClinic);
        Task<TCMServiceEntity> ToTCMServiceEntity(TCMServiceViewModel model, bool isNew, string userId);
        TCMServiceViewModel ToTCMServiceViewModel(TCMServiceEntity model, int idClinic);
        Task<TCMStageEntity> ToTCMStageEntity(TCMStageViewModel model, bool isNew, string userId);
        TCMStageViewModel ToTCMStageViewModel(TCMStageEntity model);
        Task<TCMClientEntity> ToTCMClientEntity(TCMClientViewModel model, bool isNew, string userId);
        TCMClientViewModel ToTCMClientViewModel(TCMClientEntity model);
        Task<TCMServicePlanEntity> ToTCMServicePlanEntity(TCMServicePlanViewModel model, bool isNew, string userId);
        TCMServicePlanViewModel ToTCMServicePlanViewModel(TCMServicePlanEntity model);
        Task<TCMDomainEntity> ToTCMDomainEntity(TCMDomainViewModel model, bool isNew, string origin, string userId);
        TCMDomainViewModel ToTCMDomainViewModel(TCMDomainEntity model);
        Task<TCMObjetiveEntity> ToTCMObjetiveEntity(TCMObjetiveViewModel model, bool isNew, int Origin, string userId);
        TCMObjetiveViewModel ToTCMObjetiveViewModel(TCMObjetiveEntity model);
        Task<TCMAdendumEntity> ToTCMAdendumEntity(TCMAdendumViewModel model, bool isNew, string userId);
        TCMAdendumViewModel ToTCMAdendumViewModel(TCMAdendumEntity model);
        Task<TCMServicePlanReviewEntity> ToTCMServicePlanReviewEntity(TCMServicePlanReviewViewModel model, bool isNew, string userId);
        TCMServicePlanReviewViewModel ToTCMServicePlanReviewViewModel(TCMServicePlanReviewEntity model);
        Task<TCMServicePlanReviewDomainEntity> ToTCMServicePlanReviewDomainEntity(TCMServicePlanReviewDomainViewModel model, bool isNew, string userId);
        TCMServicePlanReviewDomainViewModel ToTCMServicePlanReviewDomainViewModel(TCMServicePlanReviewDomainEntity model);
        Task<TCMDischargeEntity> ToTCMDischargeEntity(TCMDischargeViewModel model, bool isNew, string userId);
        TCMDischargeViewModel ToTCMDischargeViewModel(TCMDischargeEntity model);
        Task<TCMIntakeFormEntity> ToTCMIntakeFormEntity(TCMIntakeFormViewModel model, bool isNew, string userId);
        TCMIntakeFormViewModel ToTCMIntakeFormViewModel(TCMIntakeFormEntity model);
        Task<TCMIntakeConsentForTreatmentEntity> ToTCMIntakeConsentForTreatmentEntity(TCMIntakeConsentForTreatmentViewModel model, bool isNew, string userId);
        TCMIntakeConsentForTreatmentViewModel ToTCMIntakeConsentForTreatmentViewModel(TCMIntakeConsentForTreatmentEntity model);
        Task<TCMIntakeConsentForReleaseEntity> ToTCMIntakeConsentForReleaseEntity(TCMIntakeConsentForReleaseViewModel model, bool isNew, string userId);
        TCMIntakeConsentForReleaseViewModel ToTCMIntakeConsentForReleaseViewModel(TCMIntakeConsentForReleaseEntity model);
        Task<TCMIntakeConsumerRightsEntity> ToTCMIntakeConsumerRightsEntity(TCMIntakeConsumerRightsViewModel model, bool isNew, string userId);
        TCMIntakeConsumerRightsViewModel ToTCMIntakeConsumerRightsViewModel(TCMIntakeConsumerRightsEntity model);
        Task<TCMIntakeAcknowledgementHippaEntity> ToTCMIntakeAcknoewledgementHippaEntity(TCMIntakeAcknoewledgementHippaViewModel model, bool isNew, string userId);
        TCMIntakeAcknoewledgementHippaViewModel ToTCMIntakeAcknoewledgementHippaViewModel(TCMIntakeAcknowledgementHippaEntity model);
        Task<TCMIntakeOrientationChecklistEntity> ToTCMIntakeOrientationChecklistEntity(TCMIntakeOrientationCheckListViewModel model, bool isNew, string userId);
        TCMIntakeOrientationCheckListViewModel ToTCMIntakeOrientationChecklistViewModel(TCMIntakeOrientationChecklistEntity model);
        Task<TCMIntakeAdvancedDirectiveEntity> ToTCMIntakeAdvancedDirectiveEntity(TCMIntakeAdvancedDirectiveViewModel model, bool isNew, string userId);
        TCMIntakeAdvancedDirectiveViewModel ToTCMIntakeAdvancedDirectiveViewModel(TCMIntakeAdvancedDirectiveEntity model);
        Task<TCMIntakeForeignLanguageEntity> ToTCMIntakeForeignLanguageEntity(TCMIntakeForeignLanguageViewModel model, bool isNew, string userId);
        TCMIntakeForeignLanguageViewModel ToTCMIntakeForeignLanguageViewModel(TCMIntakeForeignLanguageEntity model);
        Task<TCMIntakeWelcomeEntity> ToTCMIntakeWelcomeEntity(TCMIntakeWelcomeViewModel model, bool isNew, string userId);
        TCMIntakeWelcomeViewModel ToTCMIntakeWelcomeViewModel(TCMIntakeWelcomeEntity model);
        Task<TCMIntakeNonClinicalLogEntity> ToTCMIntakeNonClinicalLogEntity(TCMIntakeNonClinicalLogViewModel model, bool isNew, string userId);
        TCMIntakeNonClinicalLogViewModel ToTCMIntakeNonClinicalLogViewModel(TCMIntakeNonClinicalLogEntity model);
        Task<TCMIntakeMiniMentalEntity> ToTCMIntakeMiniMenatalEntity(TCMIntakeMiniMentalViewModel model, bool isNew, string userId);
        TCMIntakeMiniMentalViewModel ToTCMIntakeMiniMenatalViewModel(TCMIntakeMiniMentalEntity model);
        Task<TCMIntakeCoordinationCareEntity> ToTCMIntakeCoordinationCareEntity(TCMIntakeCoordinationCareViewModel model, bool isNew, string userId);
        TCMIntakeCoordinationCareViewModel ToTCMIntakeCoordinationCareViewModel(TCMIntakeCoordinationCareEntity model);
        Task<TCMDischargeFollowUpEntity> ToTCMDischargeFollowUpEntity(TCMDischargeFollowUpViewModel model, bool isNew, string userId);
        TCMDischargeFollowUpViewModel ToTCMDischargeFollowUpViewModel(TCMDischargeFollowUpEntity model);
        Task<TCMIntakeAppendixJEntity> ToTCMIntakeAppendixJEntity(TCMIntakeAppendixJViewModel model, bool isNew, string userId);
        TCMIntakeAppendixJViewModel ToTCMIntakeAppendixJViewModel(TCMIntakeAppendixJEntity model);
        Task<TCMIntakeInterventionLogEntity> ToTCMIntakeInterventionLogEntity(TCMIntakeInterventionLogViewModel model, bool isNew, string userId);
        TCMIntakeInterventionLogViewModel ToTCMIntakeInterventionLogViewModel(TCMIntakeInterventionLogEntity model);
        Task<TCMIntakeInterventionEntity> ToTCMIntakeInterventionEntity(TCMIntakeInterventionViewModel model, bool isNew, string userId);
        TCMIntakeInterventionViewModel ToTCMIntakeInterventionViewModel(TCMIntakeInterventionEntity model);
        Task<TCMFarsFormEntity> ToTCMFarsFormEntity(TCMFarsFormViewModel model, bool isNew, string userId);
        TCMFarsFormViewModel ToTCMFarsFormViewModel(TCMFarsFormEntity model);
        Task<TCMAssessmentEntity> ToTCMAssessmentEntity(TCMAssessmentViewModel model, bool isNew, string userId);
        TCMAssessmentViewModel ToTCMAssessmentViewModel(TCMAssessmentEntity model);
        Task<TCMAssessmentHouseCompositionEntity> ToTCMAssessmentHouseCompositionEntity(TCMAssessmentHouseCompositionViewModel model, bool isNew, string userId);
        TCMAssessmentHouseCompositionViewModel ToTCMAssessmentHouseCompositionViewModel(TCMAssessmentHouseCompositionEntity model);
        Task<TCMAssessmentIndividualAgencyEntity> ToTCMAssessmenIndividualAgencyEntity(TCMAssessmentIndividualAgencyViewModel model, bool isNew, string userId);
        TCMAssessmentIndividualAgencyViewModel ToTCMAssessmentIndividualAgencyViewModel(TCMAssessmentIndividualAgencyEntity model);
        Task<TCMAssessmentMedicationEntity> ToTCMAssessmenMedicationEntity(TCMAssessmentMedicationViewModel model, bool isNew, string userId);
        TCMAssessmentMedicationViewModel ToTCMAssessmentMedicationViewModel(TCMAssessmentMedicationEntity model);
        Task<TCMAssessmentPastCurrentServiceEntity> ToTCMAssessmenPastCurrentServiceEntity(TCMAssessmentPastCurrentServiceViewModel model, bool isNew, string userId);
        TCMAssessmentPastCurrentServiceViewModel ToTCMAssessmentPastCurrentServiceViewModel(TCMAssessmentPastCurrentServiceEntity model);
        Task<TCMAssessmentHospitalEntity> ToTCMAssessmentHospitalEntity(TCMAssessmentHospitalViewModel model, bool isNew, string userId);
        TCMAssessmentHospitalViewModel ToTCMAssessmentHospitalViewModel(TCMAssessmentHospitalEntity model);
        Task<TCMAssessmentDrugEntity> ToTCMAssessmentDrugEntity(TCMAssessmentDrugViewModel model, bool isNew, string userId);
        TCMAssessmentDrugViewModel ToTCMAssessmentDrugViewModel(TCMAssessmentDrugEntity model);
        Task<TCMAssessmentMedicalProblemEntity> ToTCMAssessmentMedicalProblemEntity(TCMAssessmentMedicalProblemViewModel model, bool isNew, string userId);
        TCMAssessmentMedicalProblemViewModel ToTCMAssessmentMedicalProblemViewModel(TCMAssessmentMedicalProblemEntity model);
        Task<TCMAssessmentSurgeryEntity> ToTCMAssessmentSurgeryEntity(TCMAssessmentSurgeryViewModel model, bool isNew, string userId);
        TCMAssessmentSurgeryViewModel ToTCMAssessmentSurgeryViewModel(TCMAssessmentSurgeryEntity model);
        Task<TCMNoteEntity> ToTCMNoteEntity(TCMNoteViewModel model, bool isNew, string userId);
        TCMNoteViewModel ToTCMNoteViewModel(TCMNoteEntity model);
        Task<TCMNoteActivityEntity> ToTCMNoteActivityEntity(TCMNoteActivityViewModel model, bool isNew, string userId);
        TCMNoteActivityViewModel ToTCMNoteActivityViewModel(TCMNoteActivityEntity model);
        TCMNoteActivityTempEntity ToTCMNoteActivityTempEntity(TCMNoteActivityViewModel model, bool isNew, string userId);
        Task<DocumentsAssistantEntity> ToDocumentsAssistantEntity(DocumentsAssistantViewModel model, string signaturePath, bool isNew);
        DocumentsAssistantViewModel ToDocumentsAssistantViewModel(DocumentsAssistantEntity model, int idClinic);
        Task<TCMServiceActivityEntity> ToTCMServiceActivityEntity(TCMServiceActivityViewModel model, bool isNew, string userId);
        TCMServiceActivityViewModel ToTCMServiceActivityViewModel(TCMServiceActivityEntity model);
        Task<TCMMessageEntity> ToTCMMessageEntity(TCMMessageViewModel model, bool isNew);
        Task<GoalsTempEntity> ToGoalTempEntity(GoalsTempViewModel model, bool isNew);
        GoalsTempViewModel ToGoalTempViewModel(GoalsTempEntity model);
        Task<ObjectiveTempEntity> ToObjectiveTempEntity(ObjectiveTempViewModel model, bool isNew);
        ObjectiveTempViewModel ToObjectiveTempViewModel(ObjectiveTempEntity model);
        Task<BriefEntity> ToBriefEntity(BriefViewModel model, bool isNew, string userId);
        BriefViewModel ToBriefViewModel(BriefEntity model);
        Task<GroupNote2Entity> ToGroupNote2Entity(GroupNote2ViewModel model, bool isNew);
        Task<GroupNote2Entity> ToGroupNote3Entity(GroupNote3ViewModel model, bool isNew);
    }
}
