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
    [Authorize(Roles = "Customer")]
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        public ActionResult Index(int? id)
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
            return View(customer);
        }
        
        public ActionResult Details(int? id)
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
            return View(customer);
        }

        [HttpGet]        
        public ActionResult Create()
        {
            Address address = new Address();
            Customer customer = new Customer();            
            NewCustomerViewModel viewmodel = new NewCustomerViewModel();
            viewmodel.CustomerDetails = customer;
            viewmodel.AddressInformation = address;
            viewmodel.ID = 0;
            return View(viewmodel);
        }
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewCustomerViewModel viewmodel)
        {
            string userID = User.Identity.GetUserId();
            var newCustomer = new Customer();            
            newCustomer.Email = db.Users.Where(u => u.Id == userID).Single().Email;
            newCustomer.FirstName = viewmodel.CustomerDetails.FirstName;
            newCustomer.LastName = viewmodel.CustomerDetails.LastName;            
            db.Customers.Add(newCustomer);
            db.Addresses.Add(viewmodel.AddressInformation);            
            db.SaveChanges();
            return RedirectToAction("Index", new { id = newCustomer.ID });            
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
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
            return View(customer);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,email,password,Address,AmountOwed,WeeklyPickupDay,SpecialPickupDay,PickupPauseDate,ResumePickupDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = customer.ID });
            }
            return View(customer);
        }

        public ActionResult Delete(int? id)
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
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = customer.ID });
        }

        public ActionResult WeeklyPickup(int? id)
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
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WeeklyPickup(Customer customer)
        {
            var currentCustomer = db.Customers.Where(c => c.ID == customer.ID).Single();
            currentCustomer.WeeklyPickupDay = customer.WeeklyPickupDay;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = currentCustomer.ID });
        }

        public ActionResult SpecialPickup(int? id)
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
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SpecialPickup(Customer customer)
        {
            var currentCustomer = db.Customers.Where(c => c.ID == customer.ID).Single();
            currentCustomer.SpecialPickupDay = customer.SpecialPickupDay;
            currentCustomer.AmountOwed += 5.5;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = currentCustomer.ID });
        }

        public ActionResult Billing(int? id)
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
            return View(customer);
        }

        public ActionResult PauseService(int? id)
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
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PauseService(Customer customer)
        {
            var currentCustomer = db.Customers.Where(c => c.ID == customer.ID).Single();
            currentCustomer.PickupPauseDate = customer.PickupPauseDate;
            currentCustomer.ResumePickupDate = customer.ResumePickupDate;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = customer.ID });
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
