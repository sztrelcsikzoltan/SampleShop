using SampleShop.Core.Contracts;
using SampleShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleShop.WebUI.Controllers
{
    public class CartController : Controller
    {
        IRepository<Customer> customers;
        ICartService cartService;
        IOrderService orderService;

        public CartController(ICartService cartService, IOrderService orderService, IRepository<Customer> customers)
        {
            this .cartService = cartService;
            this .orderService = orderService;
            this .customers = customers;
        }
        // GET: Cart
        public ActionResult Index()
        {
            var model = cartService.GetCartItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToCart(string id)
        {
            cartService.AddToCart(this.HttpContext, id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(string id)
        {
            cartService.RemoveFromCart(this.HttpContext, id);

            return RedirectToAction("Index");
        }

        public PartialViewResult CartSummary()
        {
            var cartSummary = cartService.GetCartSummary(this.HttpContext);

            return PartialView(cartSummary);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            Customer customer = customers.GetAll().FirstOrDefault(c => c.Email == User.Identity.Name);
            
            if (customer == null)
            {
                return RedirectToAction("Error");
            }
            
            Order order = new Order()
            {
                Email = customer.Email,
                City = customer.City,
                State = customer.State,
                Street = customer.Street,
                FirstName = customer.FirsName,
                LastName = customer.FirsName,
                ZipCode = customer.ZipCode
            };
            return View(order);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order)
        {
            var cartItems = cartService.GetCartItems(this.HttpContext);
            order.OrderStatus = "Order created.";
            order.Email = User.Identity.Name;

            // payment process
            order.OrderStatus = "Payment processed";
            orderService.CreateOrder(order, cartItems);
            cartService.ClearCart(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderId = order.Id });
        }

        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}