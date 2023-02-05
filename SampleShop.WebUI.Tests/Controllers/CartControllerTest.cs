using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleShop.Core.Contracts;
using SampleShop.Core.Models;
using SampleShop.Core.ViewModels;
using SampleShop.Services;
using SampleShop.WebUI.Controllers;
using SampleShop.WebUI.Tests.Mocks;
using System;
using System.Linq;
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

            var httpContext = new MockHttpContext();

            ICartService cartService = new CartService(products, carts);

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

            var httpContext = new MockHttpContext();

            ICartService cartService = new CartService(products, carts);
            var controller = new CartController(cartService);
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

            products.Create(new Product() { Id="1", Price = 10.00m  });
            products.Create(new Product() { Id="2", Price = 1.00m  });

            Cart cart = new Cart();
            cart.CartItems.Add( new CartItem() { ProductId ="1", Quantity = 2 });
            cart.CartItems.Add( new CartItem() { ProductId ="2", Quantity = 2 });
            carts.Create(cart);

            CartService cartService = new CartService(products, carts);

            var controller = new CartController(cartService);
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
    }
}
