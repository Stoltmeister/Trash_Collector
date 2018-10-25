using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Trash_Collector.Models
{
    public class Address
    {
        [Key, ForeignKey("AspNetUsers")]
        public int ID { get; set; }
        public int StreetNumber { get; set; }
        public string Street { get; set; }
        public int StateID { get; set; }
        public int ZipCode { get; set; }
    }
}