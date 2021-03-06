﻿using KyoS.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace KyoS.Web.Data.Entities
{
    public class ThemeEntity
    {
        public int Id { get; set; }

        [Display(Name = "Topic")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]        
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }

        [Display(Name = "Day of week")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public DayOfWeekType Day { get; set; }

        public ClinicEntity Clinic { get; set; }
    }
}
