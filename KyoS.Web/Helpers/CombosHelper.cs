﻿using KyoS.Common.Enums;
using KyoS.Web.Data;
using KyoS.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KyoS.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            List<SelectListItem> list = new List<SelectListItem>
                                { new SelectListItem { Text = UserType.Facilitator.ToString(), Value = "1"},
                                  new SelectListItem { Text = UserType.Supervisor.ToString(), Value = "2"},
                                  new SelectListItem { Text = UserType.Mannager.ToString(), Value = "3"},
                                  new SelectListItem { Text = UserType.Admin.ToString(), Value = "4"}
            };
            
            list.Insert(0, new SelectListItem
            {
                Text = "[Select rol...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboUserNamesByRolesClinic(UserType userType, int idClinic)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (idClinic == 0)
            {
                list = _context.Users.Where(u => u.UserType == userType).Select(u => new SelectListItem
                {
                    Text = $"{u.UserName}",
                    Value = $"{u.Id}"
                }).ToList();
            }
            else
            {
                list = _context.Users.Where(u => (u.UserType == userType && u.Clinic.Id == idClinic)).Select(u => new SelectListItem
                {
                    Text = $"{u.UserName}",
                    Value = $"{u.Id}"
                }).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "[Select linked user...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDays()
        {
            List<SelectListItem> list = new List<SelectListItem>
                                { new SelectListItem { Text = DayOfWeekType.Monday.ToString(), Value = "1"},
                                  new SelectListItem { Text = DayOfWeekType.Tuesday.ToString(), Value = "2"},
                                  new SelectListItem { Text = DayOfWeekType.Wednesday.ToString(), Value = "3"},
                                  new SelectListItem { Text = DayOfWeekType.Thursday.ToString(), Value = "4"},
                                  new SelectListItem { Text = DayOfWeekType.Friday.ToString(), Value = "5"}};

            list.Insert(0, new SelectListItem
            {
                Text = "[Select day...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboThemes()
        {
            List<SelectListItem> list = _context.Themes.Select(t => new SelectListItem
            {
                Text = $"{t.Day.ToString()} - {t.Name}",
                Value = $"{t.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select topic...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboThemesByClinic(int idClinic)
        {
            List<SelectListItem> list = _context.Themes.Where(t => t.Clinic.Id == idClinic).Select(t => new SelectListItem
            {
                Text = $"{t.Day.ToString()} - {t.Name}",
                Value = $"{t.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select topic...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboFacilitators()
        {
            List<SelectListItem> list = _context.Facilitators.Select(f => new SelectListItem
            {
                Text = $"{f.Name}",
                Value = $"{f.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select facilitator...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboFacilitatorsByClinic(int idClinic)
        {
            List<SelectListItem> list = _context.Facilitators.Where(f => f.Clinic.Id == idClinic).Select(f => new SelectListItem
            {
                Text = $"{f.Name}",
                Value = $"{f.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select facilitator...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboClients()
        {
            List<SelectListItem> list = _context.Clients.Select(c => new SelectListItem
            {
                Text = $"{c.Name}",
                Value = $"{c.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select client...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboClientsByClinic(int idClinic)
        {
            List<SelectListItem> list = _context.Clients.Where(c => (c.Clinic.Id == idClinic/* && c.MTPs.Count == 0*/))
                                                        .Select(c => new SelectListItem
            {
                Text = $"{c.Name}",
                Value = $"{c.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select client...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboActivities()
        {
            List<SelectListItem> list = _context.Activities.Select(a => new SelectListItem
            {
                Text = $"{a.Id.ToString()} - {a.Name}",
                Value = $"{a.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select activity...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboActivitiesByTheme(int idTheme, int idFacilitator, DateTime date)
        {
            List<ActivityEntity> activities = _context.Activities

                                                      .Include(a => a.Workdays_Activities_Facilitators)
                                                      .ThenInclude(waf => waf.Workday)

                                                      .Include(a => a.Workdays_Activities_Facilitators)
                                                      .ThenInclude(waf => waf.Facilitator)

                                                      .Where(a => (a.Theme.Id == idTheme && a.Status == ActivityStatus.Approved))                                                      

                                                      .ToList();

            List<SelectListItem> list = new List<SelectListItem>();            
            Workday_Activity_Facilitator waf;
            TimeSpan daysCount;
            int weeks;
            
            foreach (ActivityEntity item in activities)
            {
                waf = item.Workdays_Activities_Facilitators.Where(waf => waf.Facilitator.Id == idFacilitator).LastOrDefault();
                if ((waf == null) || (date == waf.Workday.Date))    //Actividad no usada por el facilitator
                {
                    list.Insert(0, new SelectListItem
                    {
                        Text = $"{item.Id.ToString()} - NU - {item.Name}",
                        Value = $"{item.Id}"
                    });
                }
                else
                {
                    daysCount = date - waf.Workday.Date;                    
                    weeks = Convert.ToInt32(Math.Round(Convert.ToDouble(daysCount.Days) / 7, 0, MidpointRounding.AwayFromZero));
                    list.Insert(0, new SelectListItem
                    {
                        Text = $"{item.Id.ToString()} - {weeks}w - {item.Name}",
                        Value = $"{item.Id}"
                    });
                }                
            }

            list.Insert(0, new SelectListItem
            {
                Text = "[Select activity...]",
                Value = "0"
            });
            
            return list.OrderBy(l => Convert.ToInt32(l.Value));
        }

        public IEnumerable<SelectListItem> GetComboClassifications()
        {
            List<SelectListItem> list = new List<SelectListItem>
                                { new SelectListItem { Text = NoteClassification.Depressed.ToString(), Value = "1"},
                                  new SelectListItem { Text = NoteClassification.Negativistic.ToString(), Value = "2"},
                                  new SelectListItem { Text = NoteClassification.Sadness.ToString(), Value = "3"},
                                  new SelectListItem { Text = NoteClassification.Anxious.ToString(), Value = "4"},
                                  new SelectListItem { Text = NoteClassification.SleepProblems.ToString(), Value = "5"},
                                  new SelectListItem { Text = NoteClassification.Insomnia.ToString(), Value = "6" },
                                  new SelectListItem { Text = NoteClassification.Socialization.ToString(), Value = "7" },
                                  new SelectListItem { Text = NoteClassification.Isolation.ToString(), Value = "8" },
                                  new SelectListItem { Text = NoteClassification.Community.ToString(), Value = "9" },
                                  new SelectListItem { Text = NoteClassification.Motivation.ToString(), Value = "10" },
                                  new SelectListItem { Text = NoteClassification.Irritable.ToString(), Value = "11"},
                                  new SelectListItem { Text = NoteClassification.SelfEsteem.ToString(), Value = "12"},
                                  new SelectListItem { Text = NoteClassification.Concentration.ToString(), Value = "13"},
                                  new SelectListItem { Text = NoteClassification.Memory.ToString(), Value = "14"},
                                  new SelectListItem { Text = NoteClassification.Independent.ToString(), Value = "15"},
                                  new SelectListItem { Text = NoteClassification.MedicalManagenent.ToString(), Value = "16" },
                                  new SelectListItem { Text = NoteClassification.SelfCare.ToString(), Value = "17" },
                                  new SelectListItem { Text = NoteClassification.PositiveSelfTalk.ToString(), Value = "18" },
                                  new SelectListItem { Text = NoteClassification.NegativeSelfTalk.ToString(), Value = "19" }};


        list.Insert(0, new SelectListItem
            {
                Text = "[Select classification of note...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboClinics()
        {
            List<SelectListItem> list = _context.Clinics.Select(a => new SelectListItem
            {
                Text = $"{a.Name}",
                Value = $"{a.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select clinic...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboGender()
        {
            List<SelectListItem> list = new List<SelectListItem>
                                { new SelectListItem { Text = GenderType.Female.ToString(), Value = "1"},
                                  new SelectListItem { Text = GenderType.Male.ToString(), Value = "2"}};

            list.Insert(0, new SelectListItem
            {
                Text = "[Select gender...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboClientStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>
                                { new SelectListItem { Text = StatusType.Open.ToString(), Value = "1"},
                                  new SelectListItem { Text = StatusType.Close.ToString(), Value = "2"}};

            list.Insert(0, new SelectListItem
            {
                Text = "[Select status...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboGroups()
        {
            List<SelectListItem> list = _context.Groups.Select(g => new SelectListItem
            {
                Text = $"{g.Facilitator.Name} - {g.Meridian}",
                Value = $"{g.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select group...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboGoals(int idMTP)
        {
            List<SelectListItem> list = _context.Goals.Where(g => g.MTP.Id == idMTP).Select(g => new SelectListItem
            {
                Text = $"{g.Number}",
                Value = $"{g.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select goal...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboObjetives(int idGoal)
        {
            List<SelectListItem> list = _context.Objetives.Where(o => o.Goal.Id == idGoal).Select(o => new SelectListItem
            {
                Text = $"{o.Objetive}",
                Value = $"{o.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[First select goal...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboRelationships()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = RelationshipType.Unknown.ToString(), Value = "0"},
                new SelectListItem { Text = RelationshipType.Brother.ToString(), Value = "1"},
                new SelectListItem { Text = RelationshipType.Child.ToString(), Value = "2"},
                new SelectListItem { Text = RelationshipType.Daugther.ToString(), Value = "3"},
                new SelectListItem { Text = RelationshipType.Father.ToString(), Value = "4"},
                new SelectListItem { Text = RelationshipType.Friend.ToString(), Value = "5"},
                new SelectListItem { Text = RelationshipType.Guardian.ToString(), Value = "6"},
                new SelectListItem { Text = RelationshipType.Mother.ToString(), Value = "7"},
                new SelectListItem { Text = RelationshipType.Psychiatrist.ToString(), Value = "8"},
                new SelectListItem { Text = RelationshipType.Self.ToString(), Value = "9"},
                new SelectListItem { Text = RelationshipType.Sibling.ToString(), Value = "10"},
                new SelectListItem { Text = RelationshipType.Sister.ToString(), Value = "11"},
                new SelectListItem { Text = RelationshipType.Spouse.ToString(), Value = "12"},
                new SelectListItem { Text = RelationshipType.Son.ToString(), Value = "13"},
                new SelectListItem { Text = RelationshipType.Other.ToString(), Value = "14"}
            };
            
            return list;
        }

        public IEnumerable<SelectListItem> GetComboRaces()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = RaceType.Unknown.ToString(), Value = "0"},
                new SelectListItem { Text = RaceType.White.ToString(), Value = "1"},
                new SelectListItem { Text = RaceType.Black.ToString(), Value = "2"},
                new SelectListItem { Text = RaceType.NativeAmerican.ToString(), Value = "3"},
                new SelectListItem { Text = RaceType.AfricanAmerican.ToString(), Value = "4"},
                new SelectListItem { Text = RaceType.Asian.ToString(), Value = "5"},
                new SelectListItem { Text = RaceType.Other.ToString(), Value = "6"}                
            };

            return list;
        }

        public IEnumerable<SelectListItem> GetComboMaritals()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = MaritalStatus.Unknown.ToString(), Value = "0"},
                new SelectListItem { Text = MaritalStatus.Single.ToString(), Value = "1"},
                new SelectListItem { Text = MaritalStatus.Married.ToString(), Value = "2"},
                new SelectListItem { Text = MaritalStatus.Cohabiting.ToString(), Value = "3"},
                new SelectListItem { Text = MaritalStatus.Divorced.ToString(), Value = "4"},
                new SelectListItem { Text = MaritalStatus.Separated.ToString(), Value = "5"},
                new SelectListItem { Text = MaritalStatus.Widowed.ToString(), Value = "6"}
            };

            return list;
        }

        public IEnumerable<SelectListItem> GetComboEthnicities()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = EthnicityType.Unknown.ToString(), Value = "0"},
                new SelectListItem { Text = EthnicityType.HispanicLatino.ToString(), Value = "1"},
                new SelectListItem { Text = EthnicityType.NonHispanicLatino.ToString(), Value = "2"}
                
            };

            return list;
        }

        public IEnumerable<SelectListItem> GetComboLanguages()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = PreferredLanguage.English.ToString(), Value = "0"},
                new SelectListItem { Text = PreferredLanguage.Spanish.ToString(), Value = "1"},
                new SelectListItem { Text = PreferredLanguage.French.ToString(), Value = "2"},
                new SelectListItem { Text = PreferredLanguage.Portuguese.ToString(), Value = "3"}                
            };

            return list;
        }

        public IEnumerable<SelectListItem> GetComboReferredsByClinic(string idUser)
        {
            UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                   .FirstOrDefault(u => u.Id == idUser);

            ClinicEntity clinic = _context.Clinics.FirstOrDefault(c => c.Id == user_logged.Clinic.Id);

            List<ReferredEntity> referreds = _context.Referreds.OrderBy(d => d.Name).ToList();
            List<ReferredEntity> referreds_by_clinic = new List<ReferredEntity>();
            UserEntity user;
            foreach (ReferredEntity item in referreds)
            {
                user = _context.Users.FirstOrDefault(u => u.Id == item.CreatedBy);
                if (clinic.Users.Contains(user))
                {
                    referreds_by_clinic.Add(item);
                }
            }

            List<SelectListItem> list = new List<SelectListItem>();
            list = referreds_by_clinic.Select(r => new SelectListItem
             {
                 Text = $"{r.Name}",
                 Value = $"{r.Id}"
             }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = string.Empty,
                Value = "0"
            });

            return list.OrderBy(l => l.Text);
        }

        public IEnumerable<SelectListItem> GetComboEmergencyContactsByClinic(string idUser)
        {
            UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                   .FirstOrDefault(u => u.Id == idUser);

            ClinicEntity clinic = _context.Clinics.FirstOrDefault(c => c.Id == user_logged.Clinic.Id);

            List<EmergencyContactEntity> contacts = _context.EmergencyContacts.OrderBy(d => d.Name).ToList();
            List<EmergencyContactEntity> contacts_by_clinic = new List<EmergencyContactEntity>();
            UserEntity user;
            foreach (EmergencyContactEntity item in contacts)
            {
                user = _context.Users.FirstOrDefault(u => u.Id == item.CreatedBy);
                if (clinic.Users.Contains(user))
                {
                    contacts_by_clinic.Add(item);
                }
            }

            List<SelectListItem> list = new List<SelectListItem>();
            list = contacts_by_clinic.Select(ec => new SelectListItem
            {
                Text = $"{ec.Name}",
                Value = $"{ec.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = string.Empty,
                Value = "0"
            });

            return list.OrderBy(l => l.Text);
        }

        public IEnumerable<SelectListItem> GetComboDoctorsByClinic(string idUser)
        {
            UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                   .FirstOrDefault(u => u.Id == idUser);

            ClinicEntity clinic = _context.Clinics.FirstOrDefault(c => c.Id == user_logged.Clinic.Id);

            List<DoctorEntity> doctors = _context.Doctors.OrderBy(d => d.Name).ToList();
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

            List<SelectListItem> list = new List<SelectListItem>();
            list = doctors_by_clinic.Select(d => new SelectListItem
            {
                Text = $"{d.Name}",
                Value = $"{d.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = string.Empty,
                Value = "0"
            });

            return list.OrderBy(l => l.Text);
        }

        public IEnumerable<SelectListItem> GetComboPsychiatristsByClinic(string idUser)
        {
            UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                   .FirstOrDefault(u => u.Id == idUser);

            ClinicEntity clinic = _context.Clinics.FirstOrDefault(c => c.Id == user_logged.Clinic.Id);

            List<PsychiatristEntity> psychiatrists = _context.Psychiatrists.OrderBy(d => d.Name).ToList();
            List<PsychiatristEntity> psychiatrists_by_clinic = new List<PsychiatristEntity>();
            UserEntity user;
            foreach (PsychiatristEntity item in psychiatrists)
            {
                user = _context.Users.FirstOrDefault(u => u.Id == item.CreatedBy);
                if (clinic.Users.Contains(user))
                {
                    psychiatrists_by_clinic.Add(item);
                }
            }

            List<SelectListItem> list = new List<SelectListItem>();
            list = psychiatrists_by_clinic.Select(p => new SelectListItem
            {
                Text = $"{p.Name}",
                Value = $"{p.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = string.Empty,
                Value = "0"
            });

            return list.OrderBy(l => l.Text);
        }

        public IEnumerable<SelectListItem> GetComboLegalGuardiansByClinic(string idUser)
        {
            UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                   .FirstOrDefault(u => u.Id == idUser);

            ClinicEntity clinic = _context.Clinics.FirstOrDefault(c => c.Id == user_logged.Clinic.Id);

            List<LegalGuardianEntity> legalGuardians = _context.LegalGuardians.OrderBy(d => d.Name).ToList();
            List<LegalGuardianEntity> legalGuardians_by_clinic = new List<LegalGuardianEntity>();
            UserEntity user;
            foreach (LegalGuardianEntity item in legalGuardians)
            {
                user = _context.Users.FirstOrDefault(u => u.Id == item.CreatedBy);
                if (clinic.Users.Contains(user))
                {
                    legalGuardians_by_clinic.Add(item);
                }
            }

            List<SelectListItem> list = new List<SelectListItem>();
            list = legalGuardians_by_clinic.Select(lg => new SelectListItem
            {
                Text = $"{lg.Name}",
                Value = $"{lg.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = string.Empty,
                Value = "0"
            });

            return list.OrderBy(l => l.Text);
        }

        public IEnumerable<SelectListItem> GetComboDiagnosticsByClinic(string idUser)
        {
            UserEntity user_logged = _context.Users.Include(u => u.Clinic)
                                                   .FirstOrDefault(u => u.Id == idUser);

            ClinicEntity clinic = _context.Clinics.FirstOrDefault(c => c.Id == user_logged.Clinic.Id);

            List<DiagnosticEntity> diagnostics = _context.Diagnostics.OrderBy(d => d.Code).ToList();
            List<DiagnosticEntity> diagnostics_by_clinic = new List<DiagnosticEntity>();
            UserEntity user;
            foreach (DiagnosticEntity item in diagnostics)
            {
                user = _context.Users.FirstOrDefault(u => u.Id == item.CreatedBy);
                if (clinic.Users.Contains(user))
                {
                    diagnostics_by_clinic.Add(item);
                }
            }

            List<SelectListItem> list = new List<SelectListItem>();
            list = diagnostics_by_clinic.Select(d => new SelectListItem
            {
                Text = $"{d.Code} {d.Description}",
                Value = $"{d.Id}"
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select diagnostic...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDocumentDescriptions()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = DocumentDescription.Psychiatrist_evaluation.ToString(), Value = "0"},
                new SelectListItem { Text = DocumentDescription.Intake.ToString(), Value = "1"},
                new SelectListItem { Text = DocumentDescription.Bio.ToString(), Value = "2"},
                new SelectListItem { Text = DocumentDescription.MTP.ToString(), Value = "3"},
                new SelectListItem { Text = DocumentDescription.Fars.ToString(), Value = "4"},
                new SelectListItem { Text = DocumentDescription.MTP_review.ToString(), Value = "5"},
                new SelectListItem { Text = DocumentDescription.Consent.ToString(), Value = "6"},
                new SelectListItem { Text = DocumentDescription.Others.ToString(), Value = "7"}
            };

            return list;
        }
    }
}
