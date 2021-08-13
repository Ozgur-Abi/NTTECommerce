using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using NTTECommerce.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace NTTECommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductController : ApiBaseController
    {
        public IStringLocalizer<ProductController> _localizer { get; }
        public ProductController(IStringLocalizer<ProductController> localizer)
        {
            _localizer = localizer;
           
        }

        [HttpPost]
        public void addProduct(string productName, string productCategory)
        {
            SqlConnection myConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "INSERT INTO Products (Name,Category) Values (@Name,@Category)";
            sqlCmd.Connection = myConnection;


            sqlCmd.Parameters.AddWithValue("@Name", productName);
            sqlCmd.Parameters.AddWithValue("@Category", productCategory);
            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(string language)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT * FROM TranslationTable WHERE ProductId = (Select * FROM Languages WHERE Name = '" + language + "'";
            sqlCmd.Connection = myConnection;

            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            List<Product> products = new List<Product>();
            Product prd = null;
            while (reader.Read())
            {
                prd = new Product();
                prd.Id = Convert.ToInt32(reader.GetValue(0));

                prd.Name = _localizer[reader.GetValue(1).ToString()];
                prd.Category = _localizer[reader.GetValue(2).ToString()];
                
                products.Add(prd);
            }
            if (products.Count == 0)
                return BadRequest("There are no products to display!");
            return products;
        }

        [HttpGet("category")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(string category, string lang)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT * FROM Products WHERE Category='" + category + "'";
            sqlCmd.Connection = myConnection;

            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            List<Product> products = new List<Product>();
            Product prd = null;
            while (reader.Read())
            {
                prd = new Product();
                prd.Id = Convert.ToInt32(reader.GetValue(0));
                prd.Name = reader.GetValue(1).ToString();
                prd.Category = reader.GetValue(2).ToString();
                products.Add(prd);
            }
            if (products.Count == 0)
                return BadRequest("There are no products of given category!");
            return products;
        }
    }
}