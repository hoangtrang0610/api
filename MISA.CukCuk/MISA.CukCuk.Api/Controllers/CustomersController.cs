using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Models;
using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// lấy toàn bộ khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: HTTrang(08/01/2021)
        [HttpGet]
        public IActionResult Get()
        {
            var connectionString = "User Id=nvmanh;Host=103.124.92.43;Port=3306;Password=12345678;Database=MISACukCuk_MF657_HTTRANG;";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customers = dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.Text);
            return Ok(customers);
        }

        /// <summary>
        /// lấy danh sách khách hàng theo id và tên
        /// </summary>
        /// <param name="id">id của khách hàng</param>
        /// <param name="name">tên của khách ahngf</param>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: HTTrang(08/01/2021) 
        [HttpGet("{filter}")]
        public IActionResult Get([FromHeader] int id, [FromQuery] string name)
        {
            return Ok("value");
        }

        /// <summary>
        /// thêm 1 khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: HTTrang(09/01/2021)
        [HttpPost]
        public IActionResult Post(Customer customer)
        {
            //validate dữ liệu
            //check trùng mã
            var customerCode = customer.CustomerCode;
            if (string.IsNullOrEmpty(customerCode))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "CustomerCode", msg = "Mã khách hàng không được  phép để trống" },
                    usermsg = "Mã khách hàng không được  phép để trống",
                    Code = 999,
                };
                return BadRequest(msg);
            }
            //Check trùng mã
            var connectionString = "User Id=nvmanh;Host=103.124.92.43;Port=3306;Password=12345678;Database=MISACukCuk_MF657_HTTRANG;";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var res = dbConnection.Query<Customer>("Proc_GetCustomerByCode", new { CustomerCode = customerCode }, commandType: CommandType.StoredProcedure);
            if (res.Count() > 0)
            {
                return BadRequest("Mã khách đã tồn tại");
            }
            var properties = customer.GetType().GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(customer);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    parameters.Add($"@{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    parameters.Add($"@{propertyName}", propertyValue);
                }
            }
            var rowEffects = dbConnection.Execute("Proc_InsertCustomer", parameters, commandType: CommandType.StoredProcedure);
            if (rowEffects > 0)
            {
                return Created("abc", customer);
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// cập nhật thông tin 1 khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: HTTrang(13/01/2021)
        [HttpPut]
        public IActionResult Put([FromBody] Customer value)
        {
            var connectionString = "User Id=nvmanh;Host=103.124.92.43;Port=3306;Password=12345678;Database=MISACukCuk_MF657_HTTRANG;";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var properties = value.GetType().GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(value);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    parameters.Add($"@{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    parameters.Add($"@{propertyName}", propertyValue);
                }
            }
            var res = dbConnection.QueryFirstOrDefault<Customer>("Proc_GetCustomerById", new { CustomerId = value.CustomerId.ToString() }, commandType: CommandType.StoredProcedure);
            if (res != null)
            {
                var rowEffects = dbConnection.Execute("Proc_UpdateCustomer", parameters, commandType: CommandType.StoredProcedure);
                return Ok("Cập nhật thành công");
            }
            else
            {
                return Ok("Không có khách hàng có mã trên");
            }
        }

        /// <summary>
        /// xóa 1 khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: HTTrang(13/01/2021)
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var connectionString = "User Id=nvmanh;Host=103.124.92.43;Port=3306;Password=12345678;Database=MISACukCuk_MF657_HTTRANG;";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var rowEffects = dbConnection.Execute("DELETE FROM  Customer WHERE CustomerId ="+"\""+id.ToString()+"\"", commandType: CommandType.Text);
            if (rowEffects>0)
            {
                return Ok("Xóa thành công");
            }
            else
            {
                return Ok("Không có khách hàng có mã trên");
            }
        }
    }
}

