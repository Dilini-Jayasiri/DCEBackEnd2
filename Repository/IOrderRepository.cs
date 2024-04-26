using DCEBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace DCEBackEnd.Repository
{
    public interface IOrderRepository
    {
        public List<Order> GetOrdersByCustomer(string customerId);
    }
}
