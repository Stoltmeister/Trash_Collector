using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Trash_Collector.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        [ForeignKey("Address")]
        public int CustomerID { get; set; }
        public Address Address { get; set; }
        public double AmountOwed { get; set; }
        public DayOfWeek WeeklyPickupDay { get; set; }
        public DateTime SpecialPickupDay { get; set; }
        public DateTime PickupPauseDate { get; set; }
        public DateTime ResumePickupDate { get; set; }
    }
}