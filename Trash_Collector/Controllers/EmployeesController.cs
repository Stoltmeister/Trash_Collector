using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var user = db.Users.Where(u => u.Id == userId).Single();
            int? id = db.Employees.Where(e => e.Email == user.Email).Single().ID;
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
            List<Customer> ChargedCustomers = new List<Customer>();
            var allCustomers = db.Customers.ToList(); 

            foreach (Customer c in allCustomers)
            {      
                if (db.Addresses.Where(a => a.CustomerID == c.ID).Single().ZipCode == employee.ZipCode)
                {
                    if (c.WeeklyPickupDay == DateTime.Today.DayOfWeek)
                    {
                        if (c.LastPickupDay == null || c.LastPickupDay.Value.Date < DateTime.Today.Date && c.PickupPauseDate == null || DateTime.Today.CompareTo(c.PickupPauseDate) <= 0)
                        {
                            todaysCustomers.Add(c);
                        }
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
            return View(employeeCustomers);
            // model needs to be updated in the view
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(EmployeeCustomersViewModel employeeCustomersViewModel)
        {
            foreach (Customer c in employeeCustomersViewModel.LocalCustomers)
            {
                if (c.LastPickupDay.Value.Date < DateTime.Today.Date)
                {
                    employeeCustomersViewModel.LocalCustomers.Remove(c);                   
                    db.Customers.Find(c.ID).AmountOwed += 7.5;
                    db.Customers.Find(c.ID).LastPickupDay = DateTime.Today.Date;
                }
            }
            db.SaveChanges();
            return View(employeeCustomersViewModel);
        }
        [HttpGet]
        public ActionResult Pickup(int? id)
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

            return RedirectToAction("CompletePickup", customer);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CompletePickup(Customer customer)
        {
            customer = db.Customers.Find(customer.ID);
            customer.LastPickupDay = DateTime.Today.Date;
            customer.AmountOwed += 7.5;
            db.SaveChanges();            
            return RedirectToAction("Index");
        }


        public ActionResult Create()
        {
            Employee e = new Employee();
            EmployeeCustomersViewModel ecv = new EmployeeCustomersViewModel();
            ecv.Employee = e;
            return View(ecv);
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
        
        //public ActionResult WeeklyBill(EmployeeCustomersViewModel employeeCustomersViewModel)
        //{

        //    if (employeeCustomersViewModel == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = db.Customers.Find(employeeCustomersViewModel.ChargedCustomer);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    customer.AmountOwed += 7.5;
        //    db.SaveChanges();
        //    employeeCustomersViewModel.LocalCustomers.Remove(customer);            
        //    return View("Index", employeeCustomersViewModel);
        //}

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
