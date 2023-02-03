using SampleShop.Core.Contracts;
using SampleShop.Core.Models;
using SampleShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SampleShop.Services
{
    public class CartService : ICartService
    {
        IRepository<Product> productContext;
        IRepository<Cart> cartContext;

        public const string CartSessionName = "eCommerceCart";

        public CartService(IRepository<Product> productContext, IRepository<Cart> cartContext)
        {
            this.cartContext = cartContext;
            this.productContext = productContext;
 
        }

        private Cart GetCart(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(CartSessionName);
            Cart cart = new Cart();

            if (cookie != null)
            {
                string cartId = cookie.Value;
                if (!string.IsNullOrEmpty(cartId))
                {
                    cart = cartContext.Get(cartId);
                }
                else if (createIfNull)
                {
                    cart = CreateNewCart(httpContext);
                }
            }
            else if (createIfNull)
            {
                cart = CreateNewCart(httpContext);
            }

            return cart;
        }

        private Cart CreateNewCart(HttpContextBase httpContext)
        {
            Cart cart = new Cart();
            cartContext.Create(cart);
            cartContext.Commit();

            HttpCookie cookie = new HttpCookie(CartSessionName);
            cookie.Value = cart.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return cart;
        }

        public void AddToCart(HttpContextBase httpContext, string productId)
        {
            Cart cart = GetCart(httpContext, true);
            CartItem cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem()
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            cartContext.Commit();
        }

        public void RemoveFromCart(HttpContextBase httpContext, string cartItemId)
        {
            Cart cart = GetCart(httpContext, true);
            CartItem cartItem = cart.CartItems.FirstOrDefault(i => i.Id== cartItemId);

            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                cartContext.Commit();
            }
        }

        public List<CartItemViewModel> GetCartItems(HttpContextBase httpContext)
        {
            Cart cart = GetCart(httpContext, false);

            if (cart != null)
            {
                var results = (from c in cart.CartItems
                              join p in productContext.GetAll() on c.ProductId equals p.Id
                              select new CartItemViewModel()
                              {
                                  Id = c.Id,
                                  Quality = c.Quantity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price
                              }
                              ).ToList();

                return results;
            }
            else
            {
                return new List<CartItemViewModel>();
            }
        }

        public CartSummaryViewModel GetCartSummary(HttpContextBase httpContext)
        {
            Cart cart = GetCart(httpContext, false);
            CartSummaryViewModel model = new CartSummaryViewModel(0, 0);
            if (cart != null)
            {
                int? cartCount = (from item in cart.CartItems
                                  select item.Quantity).Sum();

                decimal? cartTotal = (from item in cart.CartItems
                                     join p in productContext.GetAll() on item.ProductId equals p.Id
                                     select item.Quantity * p.Price).Sum();

                model.CartCount = cartCount ?? 0;
                model.CartTotal= cartTotal ?? decimal.Zero;

                return model;
            }
            else
            {
                return model;
            }
        }
                        
    }
}
