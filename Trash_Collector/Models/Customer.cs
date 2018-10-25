using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Trash_Collector.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }        
        public Address Address { get; set; }
        public double AmountOwed { get; set; }
        public DayOfWeek WeeklyPickupDay { get; set; }
        public DateTime SpecialPickupDay { get; set; }
        public DateTime PickupPauseDate { get; set; }
        public DateTime ResumePickupDate { get; set; }
    }
}