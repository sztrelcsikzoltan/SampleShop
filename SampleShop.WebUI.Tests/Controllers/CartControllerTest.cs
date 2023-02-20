using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleShop.Core.Contracts;
using SampleShop.Core.Models;
using SampleShop.Core.ViewModels;
using SampleShop.Services;
using SampleShop.WebUI.Controllers;
using SampleShop.WebUI.Tests.Mocks;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;

namespace SampleShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class CartControllerTest
    {
        [TestMethod]
        public void CanAddCartItem()
        {
            // Setup            
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers= new MockContext<Customer>();

            var httpContext = new MockHttpContext();

            ICartService cartService = new CartService(products, carts);
            IOrderService orderService = new OrderService(orders);
            var controller = new CartController(cartService, orderService, customers);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            
            // Act
            cartService.AddToCart(httpContext, "1");

            Cart cart = carts.GetAll().FirstOrDefault();
            
            // Test
            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.CartItems.Count);
            Assert.AreEqual("1", cart.CartItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanAddCartItemViaController()
        {
            // Setup
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            var httpContext = new MockHttpContext();

            ICartService cartService = new CartService(products, carts);
            IOrderService orderService = new OrderService(orders);
            var controller = new CartController(cartService, orderService, customers);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            // Act
            // cartService.AddToCart(httpContext, "1");
            controller.AddToCart("1");

            Cart cart = carts.GetAll().FirstOrDefault();

            // Test
            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.CartItems.Count);
            Assert.AreEqual("1", cart.CartItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            // Setup
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            products.Create(new Product() { Id="1", Price = 10.00m  });
            products.Create(new Product() { Id="2", Price = 1.00m  });

            Cart cart = new Cart();
            cart.CartItems.Add( new CartItem() { ProductId ="1", Quantity = 2 });
            cart.CartItems.Add( new CartItem() { ProductId ="2", Quantity = 2 });
            carts.Create(cart);

            ICartService cartService = new CartService(products, carts);
            IOrderService orderService = new OrderService(orders);
            var controller = new CartController(cartService, orderService, customers);

            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add( new System.Web.HttpCookie("eCommerceCart") { Value = cart.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            // Act
            var result = controller.CartSummary() as PartialViewResult;
            var cartSummary = (CartSummaryViewModel)result.ViewData.Model;

            // Test
            Assert.AreEqual(4, cartSummary.CartCount);
            Assert.AreEqual(22.00m, cartSummary.CartTotal);
        }

        [TestMethod]
        public void CanCheckoutAndCreateOrder()
        {
            IRepository<Product> products = new MockContext<Product>();
            products.Create(new Product() { Id = "1", Price = 10.00m });
            products.Create(new Product() { Id = "2", Price = 5.00m });

            IRepository<Cart> carts = new MockContext<Cart>();
            Cart cart = new Cart();
            cart.CartItems.Add(new CartItem() { ProductId = "1", Quantity = 2, CartId = cart.Id });
            cart.CartItems.Add(new CartItem() { ProductId = "2", Quantity = 4, CartId = cart.Id });

            carts.Create(cart);

            ICartService cartService = new CartService(products, carts);

            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);
            IRepository<Customer> customers = new MockContext<Customer>();

            customers.Create(new Customer() { Id = "1", Email= "client99@gmail.com", ZipCode = "1053" });

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("client99@gmail.com", "Forms"), null);

            var controller = new CartController(cartService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.User = FakeUser;
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceCart")
            {
                Value = cart.Id
            });

            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            // Act
            Order order = new Order();
            controller.Checkout(order);

            // Test
            Assert.AreEqual(2, order.OrderItems.Count); // 2 items should be in the order
            Assert.AreEqual(0, cart.CartItems.Count); // cart should be empty after checkout

            Order orderInRepository = orders.Get(order.Id); // retrieve order from repository 
            Assert.AreEqual(2, orderInRepository.OrderItems.Count); // check that it has 2 items
        }

    }
}
