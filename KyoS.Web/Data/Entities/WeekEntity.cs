﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KyoS.Web.Data.Entities
{
    public class WeekEntity
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]        
        public DateTime InitDate { get; set; }
        [DataType(DataType.Date)]        
        public DateTime FinalDate { get; set; }
        public IEnumerable<WorkdayEntity> Days { get; set; }
        public ClinicEntity Clinic { get; set; }
        public int WeekOfYear { get; set; }
        public string Alias
        {
            get
            {
                var month = (InitDate.Month < 10) ? $"0{InitDate.Month}" : InitDate.Month.ToString();
                var initday = (InitDate.Day < 10) ? $"0{InitDate.Day}" : InitDate.Day.ToString();
                var finalday = (FinalDate.Day < 10) ? $"0{FinalDate.Day}" : FinalDate.Day.ToString();

                return $"{month}_{initday}{finalday}_{InitDate.Year}";
            }
        }
        public int Units 
        {
            get
            {
                return Notes * 16;
            }
        }
        public int Notes
        {
            get
            {
                int count = 0;
                if(this.Days != null)
                { 
                    foreach (var item in this.Days)
                    {
                        count += item.Workdays_Clients.Where(wc => wc.Present == true).Count();
                    }
                }
                return count;
            }
        }
        public string MonthYear
        {
            get
            {
                try
                {
                    var month = (InitDate.Month == 1) ? "January" : (InitDate.Month == 2) ? "February" : (InitDate.Month == 3) ? "March" :
                            (InitDate.Month == 4) ? "April" : (InitDate.Month == 5) ? "May" : (InitDate.Month == 6) ? "June" : (InitDate.Month == 7) ? "July" :
                            (InitDate.Month == 8) ? "August" : (InitDate.Month == 9) ? "September" : (InitDate.Month == 10) ? "October" :
                            (InitDate.Month == 11) ? "November" : (InitDate.Month == 12) ? "December" : string.Empty;
                    var year = InitDate.Year.ToString().Substring(2);

                    return $"{month} - {year}";
                }
                catch (Exception)
                {

                    return string.Empty;
                }
                
            }
        }
    }
}
