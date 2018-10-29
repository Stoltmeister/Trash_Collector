using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trash_Collector.Models;

namespace Trash_Collector.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context;

        public HomeController()
        {
            context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.GetUserName();
                if (User.IsInRole("Customer"))
                {                    
                    var currentCustomer = context.Customers.Where(c => c.Email == userName).Single();
                    return RedirectToAction("Index", "Customers", currentCustomer);
                }
                else if (User.IsInRole("Employee"))
                {
                    var currentEmployee = context.Employees.Where(e => e.Email == userName).Single();
                    return RedirectToAction("Index", "Employees", currentEmployee);
                }
            }
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}