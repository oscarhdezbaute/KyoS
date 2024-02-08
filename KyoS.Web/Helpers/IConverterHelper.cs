﻿using KyoS.Web.Data.Entities;
using KyoS.Web.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        Task<ActivityEntity> ToActivityEntity(ActivityViewModel model, bool isNew, string userId);
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
        PlanViewModel ToPlanViewModel(PlanEntity model);
        Task<NoteEntity> ToNoteEntity(NoteViewModel model, bool isNew);
        NoteViewModel ToNoteViewModel(NoteEntity model);
        Task<NotePEntity> ToNotePEntity(NotePViewModel model, bool isNew);
        NotePViewModel ToNotePViewModel(NotePEntity model);
        Task<IndividualNoteEntity> ToIndividualNoteEntity(IndividualNoteViewModel model, bool isNew);
        Task<GroupNoteEntity> ToGroupNoteEntity(GroupNoteViewModel model, bool isNew);       
        Workday_ClientViewModel ToWorkdayClientViewModel(Workday_Client model, bool indTherapy);
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
        IntakeConsentForTreatmentEntity ToIntakeConsentForTreatmentEntity(IntakeConsentForTreatmentViewModel model, bool isNew);
        IntakeConsentForTreatmentViewModel ToIntakeConsentForTreatmentViewModel(IntakeConsentForTreatmentEntity model);
        IntakeConsentForReleaseEntity ToIntakeConsentForReleaseEntity(IntakeConsentForReleaseViewModel model, bool isNew);
        IntakeConsentForReleaseViewModel ToIntakeConsentForReleaseViewModel(IntakeConsentForReleaseEntity model);
        IntakeConsumerRightsEntity ToIntakeConsumerRightsEntity(IntakeConsumerRightsViewModel model, bool isNew);
        IntakeConsumerRightsViewModel ToIntakeConsumerRightsViewModel(IntakeConsumerRightsEntity model);
        IntakeAcknowledgementHippaEntity ToIntakeAcknoewledgementHippaEntity(IntakeAcknoewledgementHippaViewModel model, bool isNew);
        IntakeAcknoewledgementHippaViewModel ToIntakeAcknoewledgementHippaViewModel(IntakeAcknowledgementHippaEntity model);
        IntakeAccessToServicesEntity ToIntakeAccessToServicesEntity(IntakeAccessToServicesViewModel model, bool isNew);
        IntakeAccessToServicesViewModel ToIntakeAccessToServicesViewModel(IntakeAccessToServicesEntity model);
        IntakeOrientationChecklistEntity ToIntakeOrientationChecklistEntity(IntakeOrientationCheckListViewModel model, bool isNew);
        IntakeOrientationCheckListViewModel ToIntakeOrientationChecklistViewModel(IntakeOrientationChecklistEntity model);
        IntakeTransportationEntity ToIntakeTransportationEntity(IntakeTransportationViewModel model, bool isNew);
        IntakeTransportationViewModel ToIntakeTransportationViewModel(IntakeTransportationEntity model);
        IntakeConsentPhotographEntity ToIntakeConsentPhotographEntity(IntakeConsentPhotographViewModel model, bool isNew);
        IntakeConsentPhotographViewModel ToIntakeConsentPhotographViewModel(IntakeConsentPhotographEntity model);
        IntakeFeeAgreementEntity ToIntakeFeeAgreementEntity(IntakeFeeAgreementViewModel model, bool isNew);
        IntakeFeeAgreementViewModel ToIntakeFeeAgreementViewModel(IntakeFeeAgreementEntity model);
        IntakeTuberculosisEntity ToIntakeTuberculosisEntity(IntakeTuberculosisViewModel model, bool isNew);
        IntakeTuberculosisViewModel ToIntakeTuberculosisViewModel(IntakeTuberculosisEntity model);
        IntakeMedicalHistoryEntity ToIntakeMedicalHistoryEntity(IntakeMedicalHistoryViewModel model, bool isNew, string userId);
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
        TCMClientEntity ToTCMClientEntity(TCMClientViewModel model, bool isNew, string userId);
        TCMClientViewModel ToTCMClientViewModel(TCMClientEntity model);
        Task<TCMServicePlanEntity> ToTCMServicePlanEntity(TCMServicePlanViewModel model, bool isNew, string userId);
        TCMServicePlanViewModel ToTCMServicePlanViewModel(TCMServicePlanEntity model);
        TCMDomainEntity ToTCMDomainEntity(TCMDomainViewModel model, bool isNew, string origin, string userId);
        TCMDomainViewModel ToTCMDomainViewModel(TCMDomainEntity model);
        Task<TCMObjetiveEntity> ToTCMObjetiveEntity(TCMObjetiveViewModel model, bool isNew, int Origin, string userId);
        TCMObjetiveViewModel ToTCMObjetiveViewModel(TCMObjetiveEntity model);
        Task<TCMAdendumEntity> ToTCMAdendumEntity(TCMAdendumViewModel model, bool isNew, string userId);
        TCMAdendumViewModel ToTCMAdendumViewModel(TCMAdendumEntity model);
        Task<TCMServicePlanReviewEntity> ToTCMServicePlanReviewEntity(TCMServicePlanReviewViewModel model, bool isNew, string userId);
        TCMServicePlanReviewViewModel ToTCMServicePlanReviewViewModel(TCMServicePlanReviewEntity model);
        TCMServicePlanReviewDomainEntity ToTCMServicePlanReviewDomainEntity(TCMServicePlanReviewDomainViewModel model, bool isNew, string userId);
        TCMServicePlanReviewDomainViewModel ToTCMServicePlanReviewDomainViewModel(TCMServicePlanReviewDomainEntity model);
        Task<TCMDischargeEntity> ToTCMDischargeEntity(TCMDischargeViewModel model, bool isNew, string userId);
        TCMDischargeViewModel ToTCMDischargeViewModel(TCMDischargeEntity model);
        Task<TCMIntakeFormEntity> ToTCMIntakeFormEntity(TCMIntakeFormViewModel model, bool isNew, string userId);
        TCMIntakeFormViewModel ToTCMIntakeFormViewModel(TCMIntakeFormEntity model);
        TCMIntakeConsentForTreatmentEntity ToTCMIntakeConsentForTreatmentEntity(TCMIntakeConsentForTreatmentViewModel model, bool isNew, string userId);
        TCMIntakeConsentForTreatmentViewModel ToTCMIntakeConsentForTreatmentViewModel(TCMIntakeConsentForTreatmentEntity model);
        Task<TCMIntakeConsentForReleaseEntity> ToTCMIntakeConsentForReleaseEntity(TCMIntakeConsentForReleaseViewModel model, bool isNew, string userId);
        TCMIntakeConsentForReleaseViewModel ToTCMIntakeConsentForReleaseViewModel(TCMIntakeConsentForReleaseEntity model);
        TCMIntakeConsumerRightsEntity ToTCMIntakeConsumerRightsEntity(TCMIntakeConsumerRightsViewModel model, bool isNew, string userId);
        TCMIntakeConsumerRightsViewModel ToTCMIntakeConsumerRightsViewModel(TCMIntakeConsumerRightsEntity model);
        TCMIntakeAcknowledgementHippaEntity ToTCMIntakeAcknoewledgementHippaEntity(TCMIntakeAcknoewledgementHippaViewModel model, bool isNew, string userId);
        TCMIntakeAcknoewledgementHippaViewModel ToTCMIntakeAcknoewledgementHippaViewModel(TCMIntakeAcknowledgementHippaEntity model);
        TCMIntakeOrientationChecklistEntity ToTCMIntakeOrientationChecklistEntity(TCMIntakeOrientationCheckListViewModel model, bool isNew, string userId);
        TCMIntakeOrientationCheckListViewModel ToTCMIntakeOrientationChecklistViewModel(TCMIntakeOrientationChecklistEntity model);
        TCMIntakeAdvancedDirectiveEntity ToTCMIntakeAdvancedDirectiveEntity(TCMIntakeAdvancedDirectiveViewModel model, bool isNew, string userId);
        TCMIntakeAdvancedDirectiveViewModel ToTCMIntakeAdvancedDirectiveViewModel(TCMIntakeAdvancedDirectiveEntity model);
        TCMIntakeForeignLanguageEntity ToTCMIntakeForeignLanguageEntity(TCMIntakeForeignLanguageViewModel model, bool isNew, string userId);
        TCMIntakeForeignLanguageViewModel ToTCMIntakeForeignLanguageViewModel(TCMIntakeForeignLanguageEntity model);
        TCMIntakeWelcomeEntity ToTCMIntakeWelcomeEntity(TCMIntakeWelcomeViewModel model, bool isNew, string userId);
        TCMIntakeWelcomeViewModel ToTCMIntakeWelcomeViewModel(TCMIntakeWelcomeEntity model);
        TCMIntakeNonClinicalLogEntity ToTCMIntakeNonClinicalLogEntity(TCMIntakeNonClinicalLogViewModel model, bool isNew, string userId);
        TCMIntakeNonClinicalLogViewModel ToTCMIntakeNonClinicalLogViewModel(TCMIntakeNonClinicalLogEntity model);
        TCMIntakeMiniMentalEntity ToTCMIntakeMiniMenatalEntity(TCMIntakeMiniMentalViewModel model, bool isNew, string userId);
        TCMIntakeMiniMentalViewModel ToTCMIntakeMiniMenatalViewModel(TCMIntakeMiniMentalEntity model);
        TCMIntakeCoordinationCareEntity ToTCMIntakeCoordinationCareEntity(TCMIntakeCoordinationCareViewModel model, bool isNew, string userId);
        TCMIntakeCoordinationCareViewModel ToTCMIntakeCoordinationCareViewModel(TCMIntakeCoordinationCareEntity model);
        Task<TCMDischargeFollowUpEntity> ToTCMDischargeFollowUpEntity(TCMDischargeFollowUpViewModel model, bool isNew, string userId);
        TCMDischargeFollowUpViewModel ToTCMDischargeFollowUpViewModel(TCMDischargeFollowUpEntity model);
        Task<TCMIntakeAppendixJEntity> ToTCMIntakeAppendixJEntity(TCMIntakeAppendixJViewModel model, bool isNew, string userId);
        TCMIntakeAppendixJViewModel ToTCMIntakeAppendixJViewModel(TCMIntakeAppendixJEntity model);
        TCMIntakeInterventionLogEntity ToTCMIntakeInterventionLogEntity(TCMIntakeInterventionLogViewModel model, bool isNew, string userId);
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
        GoalsTempEntity ToGoalTempEntity(GoalsTempViewModel model, bool isNew);
        GoalsTempViewModel ToGoalTempViewModel(GoalsTempEntity model);
        Task<ObjectiveTempEntity> ToObjectiveTempEntity(ObjectiveTempViewModel model, bool isNew);
        ObjectiveTempViewModel ToObjectiveTempViewModel(ObjectiveTempEntity model);
        Task<BriefEntity> ToBriefEntity(BriefViewModel model, bool isNew, string userId);
        BriefViewModel ToBriefViewModel(BriefEntity model);
        Task<GroupNote2Entity> ToGroupNote2Entity(GroupNote2ViewModel model, bool isNew);
        Task<GroupNote2Entity> ToGroupNote3Entity(GroupNote3ViewModel model, bool isNew);
        ScheduleEntity ToScheduleEntity(ScheduleViewModel model, bool isNew, UserEntity user);
        ScheduleViewModel ToScheduleViewModel(ScheduleEntity model);
        SubScheduleEntity ToSubScheduleEntity(SubScheduleViewModel model, bool isNew, UserEntity user);
        SubScheduleViewModel ToSubScheduleViewModel(SubScheduleEntity model);
        Task<ManagerEntity> ToManagerEntity(ManagerViewModel model, string signaturePath, bool isNew);
        ManagerViewModel ToManagerViewModel(ManagerEntity model);
        ClientAuxiliarViewModel ToClientAUXViewModel(ClientEntity model, List<MTPReviewEntity> mtpr);
        TCMIntakeClientSignatureVerificationEntity ToTCMIntakeClientSignatureVerificationEntity(TCMIntakeClientSignatureVerificationViewModel model, bool isNew, string userId);
        TCMIntakeClientSignatureVerificationViewModel ToTCMIntakeClientSignatureVerificationViewModel(TCMIntakeClientSignatureVerificationEntity model);
        TCMIntakeClientIdDocumentVerificationEntity ToTCMIntakeClientIdDocumentVerificationEntity(TCMIntakeClientIdDocumentVerificationViewModel model, bool isNew, string userId);
        TCMIntakeClientIdDocumentVerificationViewModel ToTCMIntakeClientIdDocumentVerificationViewModel(TCMIntakeClientIdDocumentVerificationEntity model);
        TCMIntakePainScreenEntity ToTCMIntakePainScreenEntity(TCMIntakePainScreenViewModel model, bool isNew, string userId);
        TCMIntakePainScreenViewModel ToTCMIntakePainScreenViewModel(TCMIntakePainScreenEntity model);
        TCMIntakeColumbiaSuicideEntity ToTCMIntakeColumbiaSuicideEntity(TCMIntakeColumbiaSuicideViewModel model, bool isNew, string userId);
        TCMIntakeColumbiaSuicideViewModel ToTCMIntakeColumbiaSuicideViewModel(TCMIntakeColumbiaSuicideEntity model);
        TCMIntakePersonalWellbeingEntity ToTCMIntakePersonalWellbeingEntity(TCMIntakePersonalWellbeingViewModel model, bool isNew, string userId);
        TCMIntakePersonalWellbeingViewModel ToTCMIntakePersonalWellbeingViewModel(TCMIntakePersonalWellbeingEntity model);
        TCMIntakeNutritionalScreenEntity ToTCMIntakeNutritionalScreenEntity(TCMIntakeNutritionalScreenViewModel model, bool isNew, string userId);
        TCMIntakeNutritionalScreenViewModel ToTCMIntakeNutritionalScreenViewModel(TCMIntakeNutritionalScreenEntity model);
        Task<TCMDateBlockedEntity> ToTCMDateBlockedEntity(TCMDateBlockedViewModel model, bool isNew, string userId);
        TCMDateBlockedViewModel ToTCMDateBlockedViewModel(TCMDateBlockedEntity model);
        Task<CiteEntity> ToCiteEntity(CiteViewModel model, bool isNew, string userId);
        CiteViewModel ToCiteViewModel(CiteEntity model, int idClinic);
        BillDmsEntity ToBillDMSEntity(BillDmsViewModel model, bool isNew);
        BillDmsViewModel ToBillDMSViewModel(BillDmsEntity model);
        BillDmsPaidEntity ToBillDMSPaidEntity(BillDmsPaidViewModel model, bool isNew);
        BillDmsPaidViewModel ToBillDMSPaidModel(BillDmsPaidEntity model);
        Task<TCMReferralFormEntity> ToTCMReferralFormEntity(TCMReferralFormViewModel model, bool isNew, string userId);
        TCMReferralFormViewModel ToTCMReferralFormViewModel(TCMReferralFormEntity model);
        Task<TCMSupervisionTimeEntity> ToTCMSupervisionTimeEntity(TCMSupervisionTimeViewModel model, bool isNew, string userId);
        TCMSupervisionTimeViewModel ToTCMSupervisionTimeViewModel(TCMSupervisionTimeEntity model, int idClinic);
        Task<TCMSubServiceEntity> ToTCMSubServiceEntity(TCMSubServiceViewModel model, bool isNew, string userId);
        TCMSubServiceViewModel ToTCMSubServiceViewModel(TCMSubServiceEntity model);
        TCMTransferEntity ToTCMTransferEntity(TCMTransferViewModel model, bool isNew, string userId);
        TCMTransferViewModel ToTCMTransferViewModel(TCMTransferEntity model);
        IntakeConsentForTelehealthEntity ToIntakeConsentForTelehealthEntity(IntakeConsentForTelehealthViewModel model, bool isNew);
        IntakeConsentForTelehealthViewModel ToIntakeConsentForTelehealthViewModel(IntakeConsentForTelehealthEntity model);
        IntakeNoDuplicateServiceEntity ToIntakeNoDuplicateServiceEntity(IntakeNoDuplicateServiceViewModel model, bool isNew);
        IntakeNoDuplicateServiceViewModel ToIntakeNoDuplicateServiceViewModel(IntakeNoDuplicateServiceEntity model);
        IntakeAdvancedDirectiveEntity ToIntakeAdvancedDirectiveEntity(IntakeAdvancedDirectiveViewModel model, bool isNew, string userId);
        IntakeAdvancedDirectiveViewModel ToIntakeAdvancedDirectiveViewModel(IntakeAdvancedDirectiveEntity model);
        ReferralFormEntity ToReferralEntity(ReferralFormViewModel model, bool isNew, string userId);
        ReferralFormViewModel ToReferralViewModel(ReferralFormEntity model);
        Task<TCMIntakeAppendixIEntity> ToTCMIntakeAppendixIEntity(TCMIntakeAppendixIViewModel model, bool isNew, string userId);
        TCMIntakeAppendixIViewModel ToTCMIntakeAppendixIViewModel(TCMIntakeAppendixIEntity model);
        IntakeClientIdDocumentVerificationEntity ToIntakeClientIdDocumentVerificationEntity(IntakeClientIdDocumentVerificationViewModel model, bool isNew, string userId);
        IntakeClientIdDocumentVerificationViewModel ToIntakeClientIdDocumentVerificationViewModel(IntakeClientIdDocumentVerificationEntity model);
        IntakeForeignLanguageEntity ToIntakeForeignLanguageEntity(IntakeForeignLanguageViewModel model, bool isNew, string userId);
        IntakeForeignLanguageViewModel ToIntakeForeignLanguageViewModel(IntakeForeignLanguageEntity model);
        Task<SafetyPlanEntity> ToSafetyPlanEntity(SafetyPlanViewModel model, bool isNew, string userId);
        SafetyPlanViewModel ToSafetyPlanViewModel(SafetyPlanEntity model);
        Task<IncidentReportEntity> ToIncidentReportEntity(IncidentReportViewModel model, bool isNew, string userId);
        IncidentReportViewModel ToIncidentReportViewModel(IncidentReportEntity model);
        MeetingNoteEntity ToMeetingNoteEntity(MeetingNotesViewModel model, bool isNew);
        MeetingNotesViewModel ToMeetingNoteViewModel(MeetingNoteEntity model);
        MeetingNotes_Facilitator ToMeetingNoteFacilitatorEntity(MeetingNotesFacilitatorModel model, bool isNew);
        MeetingNotesFacilitatorModel ToMeetingNoteFacilitatorViewModel(MeetingNotes_Facilitator model);
        TCMPayStubEntity ToPayStubEntity(TCMNotePendingByPayStubViewModel model, bool isNew);
        TCMNotePendingByPayStubViewModel ToPayStubViewModel(TCMPayStubEntity model);
        TCMIntakeMedicalHistoryEntity ToTCMIntakeMedicalHistoryEntity(TCMIntakeMedicalHistoryViewModel model, bool isNew, string userId);
        TCMIntakeMedicalHistoryViewModel ToTCMIntakeMedicalHistoryViewModel(TCMIntakeMedicalHistoryEntity model);
    }
}
