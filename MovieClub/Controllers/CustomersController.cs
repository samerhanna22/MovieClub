using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieClub.Models;
using MovieClub.ViewModels;

namespace MovieClub.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {


            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing); // 've added this although it is not there in the tutorial

        }

        public ViewResult Index()
        {
            // we are using jquery datatables instead of this:
            //var customers = _context.Customers.Include( c => c.MembershipType).ToList();
            //return View(customers);


            return View();
        }

        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).ToList().SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();

            CustomerFormViewModel viewModel = new CustomerFormViewModel()
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes
            };

            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            // let validate here (server side)
            if (!ModelState.IsValid)
            {
                CustomerFormViewModel CusVM = new CustomerFormViewModel()
                {
                    Customer = customer, // so we send back the entries that the user set
                    MembershipTypes = _context.MembershipTypes.ToList()
                };

                return View("CustomerForm", CusVM);
            }

            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);

            }
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);

                //TryUpdateModel(customerInDb); // not preferred for security issues
                //TryUpdateModel(customer, "", new string[]{ "Name", "DOB" }); // you can also specify what fields to be updated ... but still not safe
                // or you may also use Auto Mapper: http://automapper.org/
                // but you will need also to create DTO object class of customers where you limit wich fieldds to be mapped for security reasons

                // So, safest to do is to map manually the fields to update
                customerInDb.Name = customer.Name;
                customerInDb.DOB = customer.DOB;
                customerInDb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;




            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }

        public ActionResult Edit(int id)
        {

            var customer = _context.Customers.ToList().SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();
            var membershipTypes = _context.MembershipTypes.ToList();
            return View("CustomerForm", new CustomerFormViewModel() { Customer = customer, MembershipTypes = membershipTypes });
        }



    }
}