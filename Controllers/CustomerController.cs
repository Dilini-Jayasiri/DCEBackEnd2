using DCEBackEnd.DTO;
using DCEBackEnd.Models;
using DCEBackEnd.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DCEBackEnd.Controllers
{
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        private readonly CustomerRepository customerRepository;
        private readonly OrderRepository orderRepository;

        public CustomerController(IConfiguration configuration, CustomerRepository customerRepository, OrderRepository orderRepository)
        {
            this.configuration = configuration;
            this.connectionString = this.configuration.GetConnectionString("DefaultConnection");
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
        }


        [Route("GetAllCustomers")]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                customers = customerRepository.GetAllCustomers();
                if (customers != null)
                {
                    return Ok(customers);
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [Route("CreateCustomer")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerDTO cus)
        {
            try
            {
                if (customerRepository.CreateCustomer(cus) != null)
                {
                    return Ok(cus);
                }
                else
                {
                    return StatusCode(500, "Customer not saved !!!");
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }



        [Route("UpdateCustomer")]
        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(Customer cus)
        {
            try
            {
                if (customerRepository.UpdateCustomer(cus) != true)
                {
                   return NotFound($"Customer with ID {cus.UserId} not found.");
                }
                else
                {
                    return Ok(cus);
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [Route("DeleteCustomer/{UserId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(string UserId)
        {
            try
            {
                if (customerRepository.DeleteCustomer(UserId.ToString()) != true)
                {
                    return NotFound($"Customer with ID {UserId} not found.");
                }
                else
                {
                    return Ok($"Customer with ID {UserId} has been deleted.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Get Active Orders by Customer
        [HttpGet]
        [Route("GetActiveOrders/{customerId}")]
        public async Task<ActionResult> GetOrdersByCustomer(string customerId)
        {
            List<Order> orders = new List<Order>();
            
            try
            {
                orders =  orderRepository.GetOrdersByCustomer(customerId.ToString());
                if (orders != null)
                {
                    return Ok(orders);
                }
                else
                {
                    return BadRequest();
                }
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
