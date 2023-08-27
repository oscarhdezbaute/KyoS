﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KyoS.Common.Enums;
using KyoS.Web.Data.Contracts;

namespace KyoS.Web.Data.Entities
{
    public class CiteEntity : AuditableEntity
    {
        public int Id { get; set; }
        public ClientEntity Client { get; set; }
        public FacilitatorEntity Facilitator { get; set; }
        public ScheduleEntity Schedule { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public CiteStatus Status { get; set; }
        public string EventNote { get; set; }
        public string PatientNote { get; set; }

        public decimal Copay { get; set; }
        public Workday_Client Worday_CLient { get; set; }

        public ClinicEntity Clinic { get; set; }

    }
}
