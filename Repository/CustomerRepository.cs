using DCEBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using DCEBackEnd.DTO;

namespace DCEBackEnd.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        private readonly string getAllCustomersQuery;
        private readonly string createCustomerQuery;
        private readonly string updateCustomerQuery;
        private readonly string deleteCustomerQuery;

        public CustomerRepository(IConfiguration configuration) { 
            this.configuration = configuration;
            this.connectionString = this.configuration.GetConnectionString("DefaultConnection");
            this.getAllCustomersQuery = "SELECT * FROM CustomerTbl ORDER BY CreatedOn DESC;";
            this.createCustomerQuery = "INSERT INTO CustomerTbl (UserId, Username, Email, FirstName, LastName, CreatedOn, IsActive) " +
                          "VALUES (@UserId, @Username, @Email, @FirstName, @LastName, @CreatedOn, @IsActive);";
            this.deleteCustomerQuery = "DELETE FROM CustomerTbl WHERE UserId = @UserId;";
            this.updateCustomerQuery = "UPDATE CustomerTbl " +
                                 "SET Username = @Username, " +
                                 "    Email = @Email, " +
                                 "    FirstName = @FirstName, " +
                                 "    LastName = @LastName, " +
                                 "    IsActive = @IsActive " +
                                 "WHERE UserId = @UserId;";
        }

        // Create Customer
        public CustomerDTO CreateCustomer(CustomerDTO cus)
        {
                string guid = GenerateGuid().ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(createCustomerQuery, conn);
                    cmd.Parameters.AddWithValue("@UserId", guid);
                    cmd.Parameters.AddWithValue("@Username", cus.Username ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", cus.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FirstName", cus.FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", cus.LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now );
                    //cmd.Parameters.AddWithValue("@IsActive", cus.IsActive);

                    bool isActiveValue = cus.IsActive ?? false;
                    cmd.Parameters.AddWithValue("@IsActive", isActiveValue);

                    cmd.ExecuteNonQuery();
                }
                return cus;
        }

        // Delete Customer
        public Boolean DeleteCustomer(string userId)
        {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(deleteCustomerQuery, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return false;
                    }
                }
                return true;
        }

        // Get All Customers
        public List<Customer> GetAllCustomers()
        {
                List<Customer> customers = new List<Customer>();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(getAllCustomersQuery, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    Console.WriteLine($"Number of rows retrieved: {dataTable.Rows.Count}");

                    foreach (DataRow row in dataTable.Rows)
                    {
                        Customer customerModel = new Customer
                        {
                            UserId = row["UserId"].ToString(),
                            Username = row["Username"].ToString(),
                            Email = row["Email"].ToString(),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            CreatedOn = DateTime.Parse(row["CreatedOn"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        };
                        customers.Add(customerModel);
                    }
                }
            if (customers != null )
            {
                return customers;
            }
            else
            {
                return null;
            }
                
        }

      
        // Update Customers
        public Boolean UpdateCustomer(Customer cus)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(updateCustomerQuery, conn);
                cmd.Parameters.AddWithValue("@UserId", cus.UserId);
                cmd.Parameters.AddWithValue("@Username", cus.Username);
                cmd.Parameters.AddWithValue("@Email", cus.Email);
                cmd.Parameters.AddWithValue("@FirstName", cus.FirstName);
                cmd.Parameters.AddWithValue("@LastName", cus.LastName);

                bool isActiveValue = cus.IsActive ?? false;
                cmd.Parameters.AddWithValue("@IsActive", isActiveValue);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return false;
                }
            }

            return true;
        }


        // Generate GUID 
        public static string GenerateGuid()
        {
            // Generate a new GUID
            Guid newGuid = Guid.NewGuid();

            // Convert the GUID to a string and return it
            return newGuid.ToString();
        }

    }
}
