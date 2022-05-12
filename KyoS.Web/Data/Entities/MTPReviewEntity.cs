﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using KyoS.Common.Enums;
using KyoS.Web.Data.Contracts;

namespace KyoS.Web.Data.Entities
{
    public class MTPReviewEntity : AuditableEntity
    {
        public int Id { get; set; }

        public MTPEntity Mtp { get; set; }

        public int MTP_FK { get; set; }

        public string ProviderNumber { get; set; }

        public string ServiceCode { get; set; }

        public int NumberUnit { get; set; }

        public string SummaryOfServices { get; set; }

        public string DescribeClient { get; set; }

        public string DescribeAnyGoals { get; set; }

        public string SpecifyChanges { get; set; }

        public string IfCurrent { get; set; }

        public DateTime ReviewedOn { get; set; }

        public bool TheConsumer { get; set; }

        public bool ACopy{ get; set; }

        public bool TheTreatmentPlan { get; set; }

        [Display(Name = "Date of Person Signature")]
        [DataType(DataType.Date)]

        public DateTime DateSignaturePerson { get; set; }

        [Display(Name = "Date of Clinical Director")]
        [DataType(DataType.Date)]

        public DateTime DateClinicalDirector { get; set; }

        [Display(Name = "Date of Therapist")]
        [DataType(DataType.Date)]

        public DateTime DateTherapist { get; set; }

        [Display(Name = "Date of Licensed Practitioner")]
        [DataType(DataType.Date)]

        public DateTime DateLicensedPractitioner { get; set; }

        public string Therapist { get; set; }

        public string LicensedPractitioner { get; set; }

        public string ClinicalDirector { get; set; }

        public bool Documents { get; set; }
    }
}
