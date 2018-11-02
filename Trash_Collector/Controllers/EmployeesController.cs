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

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public List<Customer> GetDaysCustomers(DateTime day, int zipCode)
        {
            List<Customer> todaysCustomers = new List<Customer>();
            var allCustomers = db.Customers.ToList();

            foreach (Customer c in allCustomers)
            {
                try
                {
                    if (db.Addresses.Where(a => a.CustomerID == c.AddressID).Single().ZipCode == zipCode)
                    {
                        if (c.WeeklyPickupDay == day.DayOfWeek)
                        {
                            if (c.LastPickupDay == null || c.LastPickupDay.Value.Date < DateTime.Today.Date && c.PickupPauseDate == null || DateTime.Today.CompareTo(c.PickupPauseDate) <= 0)
                            {
                                todaysCustomers.Add(c);
                            }
                        }
                        else if (c.SpecialPickupDay == day)
                        {
                            todaysCustomers.Add(c);
                        }
                    }
                }
                catch (Exception)
                {
                    // in case of empty list so it doesnt get mad ;)
                }
            }
            return todaysCustomers;
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
            List<Customer> todaysCustomers = GetDaysCustomers(DateTime.Today, employee.ZipCode);
            employeeCustomers.Employee = employee;
            employeeCustomers.LocalCustomers = todaysCustomers;
            return View(employeeCustomers);
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

        public ActionResult DayCustomers(DayOfWeek day)
        {
            DateTime dayToCheck = GetNextWeekday(DateTime.Today, day);
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
            List<Customer> todaysCustomers = GetDaysCustomers(dayToCheck, employee.ZipCode);
            employeeCustomers.Employee = employee;
            employeeCustomers.LocalCustomers = todaysCustomers;
            return View("Index", employeeCustomers);
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

        public ActionResult GetDay()
        {
            Customer dayCustomer = new Customer();
            return View(dayCustomer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDay(Customer dayCustomer)
        {
            DayOfWeek SelectedDay = (DayOfWeek)dayCustomer.WeeklyPickupDay;
            return RedirectToAction("DayCustomers", new { day = SelectedDay });
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

        public ActionResult MapView(int? id)
        {
            NewCustomerViewModel customerViewModel = new NewCustomerViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            Address address = db.Addresses.Find(customer.AddressID);
            customerViewModel.AddressInformation = address;
            customerViewModel.CustomerDetails = customer;
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressInfo = address.StreetNumber + " " + address.Street + " " + address.ZipCode;
            ViewBag.MapCall = ApiKeys.MapCall;
            ViewBag.ApiKey = ApiKeys.GeoCodeKey;
            return View(customerViewModel);
        }

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
