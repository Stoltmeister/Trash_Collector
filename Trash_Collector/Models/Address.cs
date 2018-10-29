using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trash_Collector.Models
{
    public class Address
    {   
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int CustomerID { get; set; }
        [Display(Name = "Street Number")]
        public int StreetNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
    }

}