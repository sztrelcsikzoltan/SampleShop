using SampleShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SampleShop.Core.Contracts
{
    public interface ICartService
    {
        void AddToCart(HttpContextBase httpContext, string productId);
        void RemoveFromCart(HttpContextBase httpContext, string cartItemId);
        List<CartItemViewModel> GetCartItems(HttpContextBase httpContext);
        CartSummaryViewModel GetCartSummary(HttpContextBase httpContext);
    }
}
