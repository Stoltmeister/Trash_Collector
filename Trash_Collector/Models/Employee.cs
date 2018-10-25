using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trash_Collector.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }
    }
}