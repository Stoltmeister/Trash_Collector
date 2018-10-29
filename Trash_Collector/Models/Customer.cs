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
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }        
        [ForeignKey("Address")]
        public int AddressID { get; set; }
        public Address Address { get; set; }
        [Display(Name = "Total Amount Owed")]
        public double AmountOwed { get; set; }
        [Display(Name = "Weekly Pick Up Day")]
        public DayOfWeek? WeeklyPickupDay { get; set; }
        [Display(Name = "Special Pickup Day")]
        public DateTime? SpecialPickupDay { get; set; }
        [Display(Name = "Pickup Suspension Start Date")]
        public DateTime? PickupPauseDate { get; set; }
        [Display(Name = "Pickup Resuming Date")]
        public DateTime? ResumePickupDate { get; set; }
    }
}