using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Trash_Collector.Models
{
    public class NewCustomerViewModel
    {        
        public Customer CustomerDetails { get; set; }
        [Display(Name = "Address")]
        public Address AddressInformation { get; set; }
    }
}