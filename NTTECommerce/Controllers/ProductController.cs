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

        [HttpPost("addProduct")]
        public async Task<ActionResult<bool>> addProduct(string productName, string categoryName)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT Id FROM Categories WHERE Name ='" + categoryName + "'";
            sqlCmd.Connection = myConnection;

            int categoryId = 0;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                categoryId = Convert.ToInt32(reader.GetValue(0));
            }

            reader.Close();
            if (categoryId == 0)
            {
                sqlCmd.CommandText = "INSERT INTO Categories (Name) VALUES ('" + categoryName + "')";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText = "SELECT Id FROM Categories WHERE Name='" + categoryName + "'";
                reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    categoryId = Convert.ToInt32(reader.GetValue(0));
                }
                reader.Close();
            }


            sqlCmd.CommandText = "INSERT INTO Products (Name, CategoryId) VALUES ('" + productName + "', '" + categoryId + "')";
            sqlCmd.ExecuteNonQuery();
            myConnection.Close();
            return true;
        }

        [HttpGet("getProducts")]
        public async Task<ActionResult<List<Product>>> getProducts(string language)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = myConnection;
            sqlCmd.CommandType = CommandType.Text;
            myConnection.Open();

            int languageId = 0;
            sqlCmd.CommandText = "SELECT * FROM Languages WHERE Name='" + language + "'";
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                if (language.Equals(reader.GetValue(1).ToString()))
                {
                    languageId = Convert.ToInt32(reader.GetValue(0));
                    break;
                }
                return BadRequest("No localization options avaliable for the given language");
            }
            reader.Close();

            sqlCmd.CommandText = "SELECT * FROM Products";

            List<int> productIds = new List<int>();
            List<int> categoryIds = new List<int>();
            List<string> productNames = new List<string>();
            List<Product> products = new List<Product>();

            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                int productId = Convert.ToInt32(reader.GetValue(0));
                string productName = reader.GetValue(1).ToString();
                int categoryId = Convert.ToInt32(reader.GetValue(2));

                productIds.Add(productId);
                productNames.Add(productName);
                categoryIds.Add(categoryId);
            }

            reader.Close();

            for (int i = 0; i < productIds.Count; i++)
            {
                Product product = new Product();
                string productTranslation = "";
                string categoryTranslation = "";
                sqlCmd.CommandText = "SELECT * FROM ProductTrans WHERE ProductId ='" + productIds[i] + "' AND LanguageId='" + languageId + "'";
                reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                   productTranslation = reader.GetValue(2).ToString();
                }
                reader.Close();

                if (productTranslation.Equals(""))
                {
                    productTranslation = productNames[i];
                }

                sqlCmd.CommandText = "SELECT * FROM CategoryTrans WHERE CategoryId ='" + categoryIds[i] + "' AND LanguageId='" + languageId + "'";
                reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    categoryTranslation = reader.GetValue(2).ToString();
                }
                reader.Close();

                if (categoryTranslation.Equals(""))
                {
                    sqlCmd.CommandText = "SELECT * FROM Categories WHERE Id ='" + categoryIds[i] + "'";
                    reader = sqlCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        categoryTranslation = reader.GetValue(1).ToString();
                    }
                    reader.Close();
                }

                product.Name = productTranslation;
                product.Category = categoryTranslation;
                product.Id = productIds[i];

                products.Add(product);
            }

            myConnection.Close();

            if (products.Count == 0)
                return BadRequest("There are no products!");
            return products;
        }

        [HttpGet("getProductsByCategory")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(string categoryName)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT Id FROM Categories WHERE Name='" + categoryName + "'";
            sqlCmd.Connection = myConnection;

            int categoryId = 0;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                categoryId = Convert.ToInt32(reader.GetValue(0));
            }

            reader.Close();
            if (categoryId == 0)
            {
                return BadRequest("There is no such category!");
            }

            sqlCmd.CommandText = "SELECT * FROM Products WHERE CategoryId='" + categoryId + "'";
            reader = sqlCmd.ExecuteReader();

            List<Product> products = new List<Product>();
            Product prd = null;
            while (reader.Read())
            {
                prd = new Product();
                prd.Id = Convert.ToInt32(reader.GetValue(0));
                prd.Name = reader.GetValue(1).ToString();
                prd.Category = categoryName;
                products.Add(prd);
            }

            reader.Close();
            if (products.Count == 0)
                return BadRequest("There are no products of given category!");
            return products;
        }

        [HttpPost("addProductTranslation")]
        public async Task<ActionResult<bool>> addProductTranslation(string productName, string languageName, string translationText)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlCommand sqlCmd = new SqlCommand();

            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT Id FROM Products WHERE Name='" + productName + "'";
            sqlCmd.Connection = myConnection;

            int productId = 0;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                productId = Convert.ToInt32(reader.GetValue(0));
            }

            reader.Close();
            if (productId == 0)
            {
                return BadRequest("There is no such product!");
            }


            sqlCmd.CommandText = "SELECT Id FROM Languages WHERE Name ='" + languageName + "'";

            int languageId = 0;
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                languageId = Convert.ToInt32(reader.GetValue(0));
            }

            reader.Close();
            if (languageId == 0)
            {
                sqlCmd.CommandText = "INSERT INTO Languages (Name) VALUES ('" + languageName + "')";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText = "SELECT Id FROM Languages WHERE Name='" + languageName + "'";
                reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    languageId = Convert.ToInt32(reader.GetValue(0));
                }
                reader.Close();
            }


            sqlCmd.CommandText = "INSERT INTO ProductTrans (ProductId, LanguageId, Translation) VALUES ('" + productId + "', '" + languageId + "', '" + translationText + "')";
            sqlCmd.ExecuteNonQuery();
            myConnection.Close();
            return true;
        }

        [HttpPost("addCategoryTranslation")]
        public async Task<ActionResult<bool>> addCategoryTranslation(string categoryName, string languageName, string translationText)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ecommercedat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlCommand sqlCmd = new SqlCommand();

            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT Id FROM Categories WHERE Name='" + categoryName + "'";
            sqlCmd.Connection = myConnection;

            int categoryId = 0;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                categoryId = Convert.ToInt32(reader.GetValue(0));
            }

            reader.Close();
            if (categoryId == 0)
            {
                return BadRequest("There is no such category!");
            }


            sqlCmd.CommandText = "SELECT Id FROM Languages WHERE Name ='" + languageName + "'";

            int languageId = 0;
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                languageId = Convert.ToInt32(reader.GetValue(0));
            }

            reader.Close();
            if (languageId == 0)
            {
                sqlCmd.CommandText = "INSERT INTO Languages (Name) VALUES ('" + languageName + "')";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText = "SELECT Id FROM Languages WHERE Name='" + languageName + "'";
                reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    languageId = Convert.ToInt32(reader.GetValue(0));
                }
                reader.Close();
            }


            sqlCmd.CommandText = "INSERT INTO CategoryTrans (CategoryId, LanguageId, Translation) VALUES ('" + categoryId + "', '" + languageId + "', '" + translationText + "')";
            sqlCmd.ExecuteNonQuery();
            myConnection.Close();
            return true;
        }
    }
}