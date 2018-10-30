using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Trash_Collector.Models;

namespace Trash_Collector.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db;
        public EmployeesController()
        {
            db = new ApplicationDbContext();
        }

        // GET: Employees
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            EmployeeCustomersViewModel employeeCustomers = new EmployeeCustomersViewModel();
            List<Customer> todaysCustomers = new List<Customer>();
            List<Customer> PaidCustomers = new List<Customer>();
            var allCustomers = db.Customers.ToList(); 

            foreach (Customer c in allCustomers)
            {       //Cant do c.Address directly?
                if (db.Addresses.Where(a => a.CustomerID == c.ID).Single().ZipCode == employee.ZipCode)
                {
                    if (c.WeeklyPickupDay == DateTime.Today.DayOfWeek && DateTime.Today.CompareTo(c.PickupPauseDate) <= 0)
                    {
                        todaysCustomers.Add(c);
                    }
                    else if (c.SpecialPickupDay == DateTime.Today)
                    {
                        //Test this to see if time messes it up, This overrides pause dates because they likely just want a special pickup but not weekly
                        todaysCustomers.Add(c);
                    }
                }                
            }
            employeeCustomers.Employee = employee;
            employeeCustomers.LocalCustomers = todaysCustomers;
            employeeCustomers.ChargedCustomers = PaidCustomers;
            return View(employeeCustomers);
            // model needs to be updated in the view
        }        

        public ActionResult Create()
        {
            Employee employee = new Employee();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Email,ZipCode")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                string userID = User.Identity.GetUserId();
                employee.Email = db.Users.Where(u => u.Id == userID).Single().Email;
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        public ActionResult WeeklyBill(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            customer.AmountOwed += 7.5;
            return RedirectToAction("Index", new { id = User.Identity.GetUserId() });
        }

        // What dis below?
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
