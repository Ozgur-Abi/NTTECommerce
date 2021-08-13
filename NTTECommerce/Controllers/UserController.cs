using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using NTTECommerce.Models;
using System;
using System.Threading.Tasks;

namespace NTTECommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ApiBaseController
    {
        [HttpPost("adduser")]
        public void register(string username, string password)
        {
            SqlConnection myConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "INSERT INTO Users (Username,Password) Values (@Username,@Password)";
            sqlCmd.Connection = myConnection;


            sqlCmd.Parameters.AddWithValue("@Username", username);
            sqlCmd.Parameters.AddWithValue("@Password", password);
            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }

        [HttpGet("getuserbyid")]
        public User getUserById(int id)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Select * from Users where Id=" + id + "";
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            User user = null;
            while (reader.Read()){
                user = new User();
                user.Id = Convert.ToInt32(reader.GetValue(0));
                user.Username = reader.GetValue(1).ToString();
                user.Password = reader.GetValue(2).ToString();
            }
            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> login(string username, string password)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT * FROM Users WHERE Username='" + username + "'";
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            User user = null;
            while (reader.Read())
            {
                if (!reader.GetValue(2).ToString().Equals(password)) return Unauthorized("Incorrect password");
                user = new User();
                user.Id = Convert.ToInt32(reader.GetValue(0));
                user.Username = reader.GetValue(1).ToString();
                user.Password = reader.GetValue(2).ToString();
                return user;
            }
            return Unauthorized("No such user exists");
        }
    }
}