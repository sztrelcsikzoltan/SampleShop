using SampleShop.Core.Contracts;
using SampleShop.Core.Models;
using SampleShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleShop.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = OrderContext;
        }

        public void CreateOrder(Order order, List<CartItemViewModel> cartItems)
        {
            foreach (var item in cartItems)
            {
                OrderItem orderItem = new OrderItem()
                {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                };
                order.OrderItems.Add(orderItem);
            }
            orderContext.Create(order);
            orderContext.Commit();
        }

        public List<Order> GetOrderList()
        {
            return orderContext.GetAll().ToList();
        }

        public Order GetOrder(string Id)
        {
            return orderContext.Get(Id);
        }

        public void UpdateOrder(Order order)
        {
            orderContext.Update(order);
            orderContext.Commit();
        }

    }
}
