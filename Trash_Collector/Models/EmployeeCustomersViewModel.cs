using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trash_Collector.Models
{
    public class EmployeeCustomersViewModel
    {
        public Employee Employee { get; set; }
        public List<Customer> LocalCustomers { get; set; }
    }
}