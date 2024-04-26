using DCEBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace DCEBackEnd.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public List<Order> GetOrdersByCustomer(string customerId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("Order_By_Customer_proc", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customerId", customerId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                connection.Open();
                adapter.Fill(dataTable);
                connection.Close();

                foreach (DataRow row in dataTable.Rows)
                {
                    Order order = new Order
                    {
                        OrderId = row["OrderId"].ToString(),
                        ProductId = row["ProductId"].ToString(),
                        OrderStatus = int.Parse(row["OrderStatus"].ToString()),
                        OrderType = int.Parse(row["OrderType"].ToString()),
                        OrderBy = row["OrderBy"].ToString(),
                        OrderedOn = DateTime.Parse(row["OrderedOn"].ToString()),
                        ShippedOn = DateTime.Parse(row["ShippedOn"].ToString()),
                        IsActive = bool.Parse(row["IsActive"].ToString())
                    };
                    orders.Add(order);
                }
            }

            return orders;
        }
    }
}
