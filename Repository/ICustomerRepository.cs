using DCEBackEnd.DTO;
using DCEBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace DCEBackEnd.Repository
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAllCustomers();
        public CustomerDTO CreateCustomer(CustomerDTO cus);
        public Boolean UpdateCustomer(Customer cus);
        public Boolean DeleteCustomer(string userId);
    }
}
